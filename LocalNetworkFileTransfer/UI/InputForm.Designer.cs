namespace LocalNetworkFileTransfer.UI
{
	partial class InputForm
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
			textBoxInput = new TextBox();
			buttonOk = new Button();
			SuspendLayout();
			// 
			// textBoxInput
			// 
			textBoxInput.Font = new Font("Open Sans", 12F, FontStyle.Regular, GraphicsUnit.Point);
			textBoxInput.Location = new Point(12, 12);
			textBoxInput.Name = "textBoxInput";
			textBoxInput.Size = new Size(382, 29);
			textBoxInput.TabIndex = 0;
			// 
			// buttonOk
			// 
			buttonOk.Font = new Font("Open Sans", 12F, FontStyle.Bold, GraphicsUnit.Point);
			buttonOk.Location = new Point(156, 47);
			buttonOk.Name = "buttonOk";
			buttonOk.Size = new Size(81, 32);
			buttonOk.TabIndex = 1;
			buttonOk.Text = "OK";
			buttonOk.UseVisualStyleBackColor = true;
			buttonOk.Click += buttonOk_Click;
			// 
			// InputForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(406, 85);
			Controls.Add(buttonOk);
			Controls.Add(textBoxInput);
			Name = "InputForm";
			Text = "Enter IPv4";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox textBoxInput;
		private Button buttonOk;
	}
}