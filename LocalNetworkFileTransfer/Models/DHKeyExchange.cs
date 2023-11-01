namespace LocalNetworkFileTransfer.Models
{
	public class DHKeyExchange
	{
		public byte[] _publicKey;
		public byte[] _aesIV;

		 public DHKeyExchange(byte[] publicKey, byte[] aesIV)
		{
			_publicKey = publicKey;
			_aesIV = aesIV;
		}
	}
}
