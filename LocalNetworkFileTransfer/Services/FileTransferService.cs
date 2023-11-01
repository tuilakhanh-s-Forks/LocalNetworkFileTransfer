using LocalNetworkFileTransfer.Crypto;
using LocalNetworkFileTransfer.FileTransfer;
using LocalNetworkFileTransfer.Models;
using LocalNetworkFileTransfer.UI;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace LocalNetworkFileTransfer.Services
{
	public class FileTransferService
	{
		private readonly TcpListener _listener;
		private readonly int _port = 9009;
		private DiffieHellman _diffieHellman;
		private FileSender _sender;
		private FileReceiver _receiver;

		public FileTransferService()
		{
			_listener = new TcpListener(IPAddress.Any, _port);
			_diffieHellman = new DiffieHellman();
		}

		public async Task StartSendFile(string filePath)
		{
			_listener.Start();
			TcpClient client = await _listener.AcceptTcpClientAsync();
			NetworkStream stream = client.GetStream();
			DHKeyExchange? remoteKey = KeyExchange(stream);
			DialogResult result = MessageBox.Show($"Do you want transfer file to ", "Comfirm Transfer File", MessageBoxButtons.YesNo);
			if (result == DialogResult.No) {
				return;
			}
			_sender = new FileSender(stream, _diffieHellman, remoteKey);
			TransferProgressForm progressForm = new TransferProgressForm("Send Progress");
			_sender.TransferProgress += (sender, e) =>
			{
				progressForm.UpdateProgress(e.bytesRead, e.fileSize, e.transferSpeed);
			};
			_ = Task.Run(() => progressForm.ShowDialog());
			await _sender.SendFileAsync(filePath);
			progressForm.Invoke(new Action(() => progressForm.Close()));
		}

		public async Task StartRecieveFile(IPAddress hostIP, string savePath)
		{
			TcpClient client = new TcpClient();
			try
			{
				await client.ConnectAsync(new IPEndPoint(hostIP, _port));
				NetworkStream stream = client.GetStream();
				DHKeyExchange? remoteKey = KeyExchange(stream);
				_receiver = new FileReceiver(stream, _diffieHellman, remoteKey);
				TransferProgressForm progressForm = new TransferProgressForm("Receive Progress");
				_receiver.TransferProgress += (sender, e) =>
				{
					progressForm.UpdateProgress(e.bytesRead, e.fileSize, e.transferSpeed);
				};
				_ = Task.Run(() => progressForm.ShowDialog());
				await _receiver.ReceiveFileAsync(savePath);
				progressForm.Invoke(new Action(() => progressForm.Close()));
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				client.Close();
			}
		}

		private DHKeyExchange? KeyExchange(NetworkStream stream)
		{
			string serverKeyJson = JsonConvert.SerializeObject(new DHKeyExchange(_diffieHellman.PublicKey, _diffieHellman.AesIV));
			byte[] serverKeyBytes = Encoding.UTF8.GetBytes(serverKeyJson);
			stream.Write(serverKeyBytes, 0, serverKeyBytes.Length);
				
			byte[] remotekeyBytes = new byte[serverKeyBytes.Length];

			stream.Read(remotekeyBytes, 0, remotekeyBytes.Length);

			string remotekeyJson = Encoding.UTF8.GetString(remotekeyBytes);

			return JsonConvert.DeserializeObject<DHKeyExchange>(remotekeyJson);
		}
	}
}
