using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HTTPconsole
{
	public class PathDialogForm : Form
	{
		private TextBox commandBox;
		private TextBox argsBox;
		private UIForm owner;

		public PathDialogForm()
		{
			Text = "HTTPconsole";
			Size = new Size(330, 185);
			SizeGripStyle = SizeGripStyle.Hide;
			FormBorderStyle = FormBorderStyle.Fixed3D;
			MaximizeBox = false;

			owner = UIForm.Instance;

			Label titleLabel = new Label();
			titleLabel.Width = 400;
			titleLabel.Font = new Font("Segoe UI", 11.0f);
			titleLabel.Text = "Please enter the command to be run.";
			titleLabel.Location = new Point(10, 10);
			titleLabel.Parent = this;

			Label commandBoxLabel = new Label();
			commandBoxLabel.Font = new Font("Segoe UI", 11.0f);
			commandBoxLabel.Text = "Command:";
			commandBoxLabel.Location = new Point(10, 40);
			commandBoxLabel.Parent = this;

			commandBox = new TextBox();
			commandBox.Font = new Font("Segoe UI", 11.0f);
			commandBox.Width = 200;
			commandBox.Location = new Point(110, 38);
			commandBox.Parent = this;

			Label argsBoxLabel = new Label();
			argsBoxLabel.Font = new Font("Segoe UI", 11.0f);
			argsBoxLabel.Text = "Arguments:";
			argsBoxLabel.Location = new Point(10, 80);
			argsBoxLabel.Parent = this;

			argsBox = new TextBox();
			argsBox.Font = new Font("Segoe UI", 11.0f);
			argsBox.Width = 200;
			argsBox.Location = new Point(110, 78);
			argsBox.Parent = this;

			Button okButton = new Button();
			okButton.Text = "OK";
			okButton.Font = new Font("Segoe UI", 11.0f);
			okButton.Location = new Point(235, 118);
			okButton.Click += OkButton_Click;
			okButton.Parent = this;

			Button cancelButton = new Button();
			cancelButton.Text = "Cancel";
			cancelButton.Font = new Font("Segoe UI", 11.0f);
			cancelButton.Location = new Point(145, 118);
			cancelButton.Click += CancelButton_Click;
			cancelButton.Parent = this;
		}

		public void OkButton_Click(object sender, EventArgs e)
		{
			owner.PathDialogCallback(commandBox.Text, argsBox.Text);
			DialogResult = DialogResult.OK;
		}

		public void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}

