using System.Diagnostics;
using System.Security.Cryptography;

namespace LocalNetworkFileTransfer.Crypto
{
	public class AES : IDisposable
	{
		public enum Cryptor
		{
			Encrypt, Decrypt
		}

		#region " Properties "
		public int FileReadChunkSize { get; set; }
		public int KeySize { get => SymmetricKey.KeySize; set => SymmetricKey.KeySize = value; }
		public int BlockSize { get => SymmetricKey.BlockSize; set => SymmetricKey.BlockSize = value; }
		public CipherMode Mode { get => SymmetricKey.Mode; set => SymmetricKey.Mode = value; }
		public PaddingMode Padding { get => SymmetricKey.Padding; set => SymmetricKey.Padding = value; }
		public byte[] Key { get => SymmetricKey.Key; set => SymmetricKey.Key = value; }
		public byte[] IV { get => SymmetricKey.IV; set => SymmetricKey.IV = value; }
		#endregion

		#region " Fields "
		private Aes SymmetricKey;
		#endregion

		#region " Constructor "
		public AES()
		{
			FileReadChunkSize = 32768;
			SymmetricKey = Aes.Create();
			SymmetricKey.Padding = PaddingMode.PKCS7;
			GenerateKeyIV();
		}

		#endregion
		#region " Methods "
		public void GenerateKeyIV()
		{
			SymmetricKey.GenerateKey();
			SymmetricKey.GenerateIV();
		}

		public async Task<byte[]> ProcessAsync(byte[] data, Cryptor cryptor)
		{
			using (var output = new MemoryStream())
			using (var createCryptor = cryptor == Cryptor.Encrypt ? SymmetricKey.CreateEncryptor() : SymmetricKey.CreateDecryptor())
			using (CryptoStream cs = new CryptoStream(output, createCryptor, CryptoStreamMode.Write))
			{
				await cs.WriteAsync(data, 0, data.Length);
				if (!cs.HasFlushedFinalBlock)
					cs.FlushFinalBlock();
				return output.ToArray();
			}
		}

		public void Dispose()
		{
			SymmetricKey.Dispose();
			SymmetricKey = null;
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
