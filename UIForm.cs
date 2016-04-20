using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HTTPconsole
{
	public class UIForm : Form
	{
		public UIForm(string command, string args)
		{
			ProgramThread pt = new ProgramThread(command, args);
			Thread program = new Thread(new ThreadStart(pt.ThreadRun));
			program.Start();

			HttpThread ht = new HttpThread(49900, pt);
			Thread http = new Thread(new ThreadStart(ht.ThreadRun));
			http.Start();

			Text = "HTTPconsole v0.3";
			Size = new Size(800, 600);

			//this.FormBorderStyle = FormBorderStyle.None;

			Label titleLabel = new Label();
			titleLabel.Text = "HTTPconsole";
			titleLabel.Width = this.Width;
			titleLabel.Height = 60;
			titleLabel.Location = new Point(0, 0);
			titleLabel.BackColor = Color.FromArgb(0x4c, 0x07, 0x62);
			titleLabel.ForeColor = Color.FromArgb(0xf2, 0xf2, 0xf2);
			titleLabel.Font = new Font("Segoe UI", 32.0f);
			titleLabel.TextAlign = ContentAlignment.MiddleCenter;
			titleLabel.Parent = this;

			/*
			Label infoLabel = new Label();
			infoLabel.Text = Environment.OSVersion.VersionString;
			infoLabel.Width = 200;
			infoLabel.Height = 20;
			infoLabel.Location = new Point(10, 550);
			infoLabel.TextAlign = ContentAlignment.MiddleLeft;
			infoLabel.Parent = this;
			*/

			TextBox consoleBox = new TextBox();
			consoleBox.BackColor = Color.FromArgb(0x5f, 0x5f, 0x5f);
			consoleBox.ForeColor = Color.FromArgb(0x73, 0xad, 0x21);
			consoleBox.Multiline = true;
			consoleBox.ReadOnly = true;
			consoleBox.Font = new Font("Monaco", 11.0f);
			consoleBox.Width = 728;
			consoleBox.Height = 408;
			consoleBox.Location = new Point(32, 70);
			consoleBox.Parent = this;

			Label inputBoxLabel = new Label();
			inputBoxLabel.Text = "Input:";
			inputBoxLabel.Width = 40;
			inputBoxLabel.Height = 20;
			inputBoxLabel.Location = new Point(33,495);
			inputBoxLabel.Font = new Font(inputBoxLabel.Font.FontFamily, 11.0f);
			inputBoxLabel.Parent = this;

			TextBox inputBox = new TextBox();
			inputBox.Width = 600;
			inputBox.Font = new Font("Monaco", 10.0f);
			inputBox.Location = new Point(80, 490);
			inputBox.Parent = this;

			Button inputButton = new Button();
			inputButton.Text = "Send";
			inputButton.Height = inputBox.Height;
			inputButton.Width = 70;
			inputButton.Location = new Point(688, 490);
			inputButton.Font = new Font(inputButton.Font.FontFamily, 11.0f);
			inputButton.Parent = this;

			StatusBar sb = new StatusBar();
			sb.Text = Environment.OSVersion.VersionString;
			sb.Parent = this;
		}
	}
}

