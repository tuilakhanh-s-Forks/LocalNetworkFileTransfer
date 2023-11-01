namespace LocalNetworkFileTransfer.Event
{
	public class TransferEventArgs : EventArgs
	{
		public long bytesRead { get; }
		public long fileSize { get; }
		public double transferSpeed { get; }

		public TransferEventArgs(long bytesRead, long fileSize, double transferSpeed)
		{
			this.bytesRead = bytesRead;
			this.fileSize = fileSize;
			this.transferSpeed = transferSpeed;
		}
	}
}
