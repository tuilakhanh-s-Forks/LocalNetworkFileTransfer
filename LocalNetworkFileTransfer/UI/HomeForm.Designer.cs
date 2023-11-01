namespace LocalNetworkFileTransfer
{
	partial class HomeForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			buttonSend = new Button();
			buttonRecieve = new Button();
			SuspendLayout();
			// 
			// buttonSend
			// 
			buttonSend.Font = new Font("Open Sans", 12F, FontStyle.Bold, GraphicsUnit.Point);
			buttonSend.Location = new Point(38, 75);
			buttonSend.Name = "buttonSend";
			buttonSend.Size = new Size(112, 43);
			buttonSend.TabIndex = 0;
			buttonSend.Text = "Send File";
			buttonSend.UseVisualStyleBackColor = true;
			buttonSend.Click += buttonSend_Click;
			// 
			// buttonRecieve
			// 
			buttonRecieve.Font = new Font("Open Sans", 12F, FontStyle.Bold, GraphicsUnit.Point);
			buttonRecieve.Location = new Point(203, 75);
			buttonRecieve.Name = "buttonRecieve";
			buttonRecieve.Size = new Size(112, 43);
			buttonRecieve.TabIndex = 1;
			buttonRecieve.Text = "Recieve File";
			buttonRecieve.UseVisualStyleBackColor = true;
			buttonRecieve.Click += buttonRecieve_Click;
			// 
			// Home
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(362, 193);
			Controls.Add(buttonRecieve);
			Controls.Add(buttonSend);
			Name = "Home";
			Text = "File Transfer";
			ResumeLayout(false);
		}

		#endregion

		private Button buttonSend;
		private Button buttonRecieve;
	}
}