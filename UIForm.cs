using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HTTPconsole
{
	public class UIForm : Form
	{
		private string _command;
		private string _args;
		private TextBox inputBox;
		private TextBox consoleBox;
		private ProgramThread pt;

		public static UIForm Instance;

		public UIForm(string command, string args)
		{
			_args = args;
			Instance = this;

			if (command == "")
			{
				using (PathDialogForm pdf = new PathDialogForm())
				{
					if (pdf.ShowDialog() == DialogResult.Cancel)
					{
						Environment.Exit(0);
					}
				}
			}
			else
			{
				_command = command;
			}

			Text = _command + " " + _args;
			Size = new Size(800, 600);
			SizeGripStyle = SizeGripStyle.Hide;
			FormBorderStyle = FormBorderStyle.Fixed3D;
			MaximizeBox = false;

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

			consoleBox = new TextBox();
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

			inputBox = new TextBox();
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
			inputButton.Click += InputButton_Click;
			inputButton.Parent = this;

			StatusBar sb = new StatusBar();
			sb.Text = "HTTP server running on port 49900, Operating system: " + Environment.OSVersion.VersionString;
			sb.Parent = this;
			sb.SizingGrip = false;

			pt = new ProgramThread(_command, _args);
			Thread program = new Thread(new ThreadStart(pt.ThreadRun));
			program.IsBackground = true;
			program.Start();

			HttpThread ht = new HttpThread(49900, pt);
			Thread http = new Thread(new ThreadStart(ht.ThreadRun));
			http.IsBackground = true;
			http.Start();
		}

		void InputButton_Click (object sender, EventArgs e)
		{
			pt.stdinBuffer.Enqueue(inputBox.Text);
			inputBox.Text = "";
		}

		public void PathDialogCallback(string command, string args)
		{
			_command = command;
			_args = args;
		}


		delegate void ConsoleBoxAppendTextDelegate(string text);

		public void ConsoleBoxAppendText(string text)
		{
			if(consoleBox.InvokeRequired)
			{
				consoleBox.Invoke(new ConsoleBoxAppendTextDelegate(this.ConsoleBoxAppendText), new object[] { text });
			}
			else
			{
				consoleBox.Text = consoleBox.Text += text;
				consoleBox.SelectionStart = consoleBox.Text.Length;
				consoleBox.ScrollToCaret();
			}
		}
	}
}

