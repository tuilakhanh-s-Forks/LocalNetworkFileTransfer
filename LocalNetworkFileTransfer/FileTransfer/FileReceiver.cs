using LocalNetworkFileTransfer.Crypto;
using LocalNetworkFileTransfer.Event;
using LocalNetworkFileTransfer.Models;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace LocalNetworkFileTransfer.FileTransfer
{
	public class FileReceiver
	{
		private readonly NetworkStream _stream;
		private readonly DiffieHellman _difieHellman;
		private readonly DHKeyExchange _remoteKey;
		private readonly int bufferSize = 32768;

		public event EventHandler<TransferEventArgs> TransferProgress;

		public FileReceiver(NetworkStream stream, DiffieHellman difieHellman, DHKeyExchange remoteKey)
		{
			_stream = stream;
			_difieHellman = difieHellman;
			_remoteKey = remoteKey;
		}

		public async Task ReceiveFileAsync(string savePath)
		{
			try
			{
				byte[] fileSizeEncrypted = new byte[16];
				await _stream.ReadAsync(fileSizeEncrypted, 0, fileSizeEncrypted.Length);
				byte[] fileSizeBytes = await _difieHellman.DecryptAsync(_remoteKey._publicKey, fileSizeEncrypted, _remoteKey._aesIV);
				long fileSize = BitConverter.ToInt64(fileSizeBytes);

				byte[] fileNameLengthEncryptedBytes = new byte[16];
				await _stream.ReadAsync(fileNameLengthEncryptedBytes, 0, fileNameLengthEncryptedBytes.Length);
				byte[] fileNameLengthBytes = await _difieHellman.DecryptAsync(_remoteKey._publicKey, fileNameLengthEncryptedBytes, _remoteKey._aesIV);
				long fileNameLength = BitConverter.ToInt64(fileNameLengthBytes);

				byte[] fileNameEncryptedBytes = new byte[fileNameLength];
				await _stream.ReadAsync(fileNameEncryptedBytes, 0, fileNameEncryptedBytes.Length);
				byte[] fileNameBytes = await _difieHellman.DecryptAsync(_remoteKey._publicKey, fileNameEncryptedBytes, _remoteKey._aesIV);
				string fileName = Encoding.UTF8.GetString(fileNameBytes);

				Stopwatch stopWatch = new Stopwatch();
				stopWatch.Start();

				using (FileStream fileStream = new FileStream($"{savePath}\\{fileName}", FileMode.Create, FileAccess.Write))
				{
					byte[] buffer = new byte[CalculateSizeWithPadding(bufferSize)];
					long bytesLeft = fileSize;
					while (bytesLeft > 0)
					{
						long nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;
						if (nextPacketSize != bufferSize)
						{
							buffer = new byte[CalculateSizeWithPadding((int)nextPacketSize)];
						}
						await _stream.ReadAsync(buffer, 0, buffer.Length);
						byte[] decryptedData = await _difieHellman.DecryptAsync(_remoteKey._publicKey, buffer, _remoteKey._aesIV);
						await fileStream.WriteAsync(decryptedData, 0, decryptedData.Length);
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
		}

		private int CalculateSizeWithPadding(int dataLength)
		{
			int blockSize = 128;
			int paddingLength = blockSize - ((dataLength * 8) % blockSize);
			if (paddingLength == 0)
			{
				paddingLength = blockSize;
			}
			return (paddingLength / 8) + dataLength;
		}
	}
}
