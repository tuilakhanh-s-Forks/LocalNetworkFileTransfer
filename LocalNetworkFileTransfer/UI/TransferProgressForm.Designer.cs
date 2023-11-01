namespace LocalNetworkFileTransfer.UI
{
	partial class TransferProgressForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			progressBar = new ProgressBar();
			labelFileSize = new Label();
			labelSpeed = new Label();
			SuspendLayout();
			// 
			// progressBar
			// 
			progressBar.Location = new Point(12, 22);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(494, 33);
			progressBar.TabIndex = 0;
			// 
			// labelFileSize
			// 
			labelFileSize.AutoSize = true;
			labelFileSize.Font = new Font("Open Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			labelFileSize.Location = new Point(12, 68);
			labelFileSize.Name = "labelFileSize";
			labelFileSize.Size = new Size(31, 19);
			labelFileSize.TabIndex = 1;
			labelFileSize.Text = "size";
			// 
			// labelSpeed
			// 
			labelSpeed.AutoSize = true;
			labelSpeed.Font = new Font("Open Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			labelSpeed.Location = new Point(385, 68);
			labelSpeed.Name = "labelSpeed";
			labelSpeed.Size = new Size(45, 19);
			labelSpeed.TabIndex = 2;
			labelSpeed.Text = "speed";
			// 
			// TransferProgressForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(518, 96);
			Controls.Add(labelSpeed);
			Controls.Add(labelFileSize);
			Controls.Add(progressBar);
			Name = "TransferProgressForm";
			Text = "Transfer Progress";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private ProgressBar progressBar;
		private Label labelFileSize;
		private Label labelSpeed;
	}
}