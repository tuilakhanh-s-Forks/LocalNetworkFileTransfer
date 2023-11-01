using LocalNetworkFileTransfer.Services;
using LocalNetworkFileTransfer.UI;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace LocalNetworkFileTransfer
{
	public partial class HomeForm : Form
	{
		private FileTransferService fileTransferService;

		public HomeForm()
		{
			InitializeComponent();
			fileTransferService = new FileTransferService();
		}

		private async void buttonSend_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog()
			{
				Title = "Select a file to transfer",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
			};
			if (ofd.ShowDialog() != DialogResult.OK)
			{
				MessageBox.Show("Please sellect file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			this.Enabled = false;
			await fileTransferService.StartSendFile(ofd.FileName);
			this.Enabled = true;
		}

		private async void buttonRecieve_Click(object sender, EventArgs e)
		{
			var form = new InputForm("Enter sender IP");
			if (form.ShowDialog() != DialogResult.OK || form.InputText.Equals(""))
			{
				MessageBox.Show("Please input IPv4", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			IPAddress hostIP = ParseIPv4(form.InputText);
			if (hostIP == null)
			{
				MessageBox.Show("Please input valid IPv4", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			FolderBrowserDialog fbd = new FolderBrowserDialog()
			{
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
			};
			if (fbd.ShowDialog() != DialogResult.OK)
			{
				MessageBox.Show("Please sellect save folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			this.Enabled = false;
			await fileTransferService.StartRecieveFile(hostIP, fbd.SelectedPath);
			this.Enabled = true;
		}

		private IPAddress ParseIPv4(string input)
		{
			try
			{
				if (IPAddress.TryParse(input, out IPAddress ipAddress) && ipAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					return ipAddress;
				}
			}
			catch (Exception)
			{
				return null;
			}
			return null;
		}

	}
}