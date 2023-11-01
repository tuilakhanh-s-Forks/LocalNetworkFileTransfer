using System.Security.Cryptography;

namespace LocalNetworkFileTransfer.Crypto
{
	public class DiffieHellman : IDisposable
	{
		#region " Private Fields "
		private AES AES;
		private ECDiffieHellmanCng DiffieHellmanCng;
		private byte[] _PrivateKey;
		#endregion

		#region " Properties "
		public byte[] PublicKey => DiffieHellmanCng.PublicKey.ToByteArray();
		public byte[] PrivateKey
		{
			get => _PrivateKey ?? DiffieHellmanCng.Key.Export(CngKeyBlobFormat.EccPrivateBlob);
			set
			{
				_PrivateKey = value;
				DiffieHellmanCng = new ECDiffieHellmanCng(CngKey.Import(_PrivateKey, CngKeyBlobFormat.EccPrivateBlob, CngProvider.MicrosoftSoftwareKeyStorageProvider))
				{
					KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
				};
			}
		}
		public byte[] AesIV { get => AES.IV; set => AES.IV = value; }
		#endregion

		#region " Constructor "
		public DiffieHellman()
		{
			_PrivateKey = null;
			AES = new AES();
			DiffieHellmanCng = new ECDiffieHellmanCng(CngKey.Create(CngAlgorithm.ECDiffieHellmanP256, null, new CngKeyCreationParameters
			{
				ExportPolicy = CngExportPolicies.AllowPlaintextExport,
				KeyCreationOptions = CngKeyCreationOptions.MachineKey,
				KeyUsage = CngKeyUsages.AllUsages,
				Provider = CngProvider.MicrosoftSoftwareKeyStorageProvider,
				UIPolicy = new CngUIPolicy(CngUIProtectionLevels.None)
			}))
			{
				KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
			};
		}
		#endregion
		#region " Methods "
		public async Task<byte[]> EncryptAsync(byte[] publicKey, byte[] data)
		{
			using (var key = CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob))
			{
				var derivedKey = DiffieHellmanCng.DeriveKeyMaterial(key);
				AES.Key = derivedKey;
				return await AES.ProcessAsync(data, AES.Cryptor.Encrypt);
			}
		}

		public async Task<byte[]> DecryptAsync(byte[] publicKey, byte[] data, byte[] IV)
		{
			using (var key = CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob))
			{
				var derivedKey = DiffieHellmanCng.DeriveKeyMaterial(key);
				AES.Key = derivedKey;
				AES.IV = IV;
				return await AES.ProcessAsync(data, AES.Cryptor.Decrypt);
			}
		}

		public void Dispose()
		{
			DiffieHellmanCng.Dispose();
			AES.Dispose();
			DiffieHellmanCng = null;
			AES = null;
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
