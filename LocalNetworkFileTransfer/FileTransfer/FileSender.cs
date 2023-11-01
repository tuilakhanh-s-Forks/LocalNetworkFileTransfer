using LocalNetworkFileTransfer.Crypto;
using LocalNetworkFileTransfer.Event;
using LocalNetworkFileTransfer.Models;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace LocalNetworkFileTransfer.FileTransfer
{
	public class FileSender
	{
		private readonly NetworkStream _stream;
		private readonly DiffieHellman _difieHellman;
		private readonly DHKeyExchange _remoteKey;
		private readonly int bufferSize = 32768;

		public event EventHandler<TransferEventArgs> TransferProgress;

		public FileSender(NetworkStream stream, DiffieHellman difieHellman, DHKeyExchange remoteKey)
		{
			_stream = stream;
			_difieHellman = difieHellman;
			_remoteKey = remoteKey;
		}

		public event Action<double> TransferSpeedLogged;

		public async Task SendFileAsync(string filePath)
		{
			try
			{
				long fileSize = new FileInfo(filePath).Length;
				byte[] fileSizeEncryptedBytes = await _difieHellman.EncryptAsync(_remoteKey._publicKey, BitConverter.GetBytes(fileSize));
				await _stream.WriteAsync(fileSizeEncryptedBytes);

				string fileName = Path.GetFileName(filePath);
				byte[] fileNameEncryptedBytes = await _difieHellman.EncryptAsync(_remoteKey._publicKey, Encoding.UTF8.GetBytes(fileName));
				long fileNameLength = fileNameEncryptedBytes.Length;
				byte[] fileNameLengthEncryptedBytes = await _difieHellman.EncryptAsync(_remoteKey._publicKey, BitConverter.GetBytes(fileNameLength));

				await _stream.WriteAsync(fileNameLengthEncryptedBytes);
				await _stream.WriteAsync(fileNameEncryptedBytes);

				Stopwatch stopWatch = new Stopwatch();
				stopWatch.Start();

				using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				{
					byte[] buffer = new byte[bufferSize];
					long bytesLeft = fileSize;
					while (bytesLeft > 0)
					{
						long nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;
						if (nextPacketSize != bufferSize)
						{
							buffer = new byte[nextPacketSize];
						}
						await fileStream.ReadAsync(buffer, 0, (int)nextPacketSize);
						byte[] encryptedData = await _difieHellman.EncryptAsync(_remoteKey._publicKey, buffer);
						await _stream.WriteAsync(encryptedData, 0, encryptedData.Length);
						bytesLeft -= nextPacketSize;
						double transferSpeed = ((fileSize - bytesLeft) / (1024.0 * 1024.0)) / stopWatch.Elapsed.Seconds;
						TransferProgress?.Invoke(this, new TransferEventArgs(bytesLeft, fileSize, transferSpeed));
					}
				}
			} 
			catch
			{
				throw;
			}
			finally
			{
			}
			
		}
	}
}
