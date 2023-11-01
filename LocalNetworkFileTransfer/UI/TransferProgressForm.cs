namespace LocalNetworkFileTransfer.UI
{
	public partial class TransferProgressForm : Form
	{
		public TransferProgressForm(string caption)
		{
			InitializeComponent();
			this.Text = caption;
		}

		public void UpdateProgress(long bytesRead, long fileSize, double transferSpeed)
		{
			if (progressBar.InvokeRequired)
			{
				progressBar.Invoke(new Action(() => progressBar.Maximum = (int)fileSize));
				progressBar.Invoke(new Action(() => progressBar.Value = (int)(fileSize - bytesRead)));
				labelFileSize.Invoke(new Action(() => labelFileSize.Text = $"File size: {fileSize / 1024:F2} KB"));
				labelSpeed.Invoke(new Action(() => labelSpeed.Text = $"Speed: {transferSpeed:F2} MB/s"));
			}
			else
			{
				progressBar.Maximum = (int)fileSize;
				progressBar.Value = (int)(fileSize - bytesRead);
				labelFileSize.Text = $"File size: {fileSize / 1024:F2} KB";
				labelSpeed.Text = $"Speed: {transferSpeed:F2} MB/s";
			}
		}
	}
}
