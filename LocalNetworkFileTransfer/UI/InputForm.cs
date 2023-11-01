namespace LocalNetworkFileTransfer.UI
{
	public partial class InputForm : Form
	{
		public InputForm(string caption)
		{
			InitializeComponent();
			this.Text = caption;
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		public string InputText
		{
			get { return textBoxInput.Text; }
		}
	}
}
