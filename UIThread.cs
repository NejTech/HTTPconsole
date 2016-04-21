using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HTTPconsole
{
	public class UIThread
	{
		private ProgramThread _programThread;
		private TextBox _consoleBox;
		private StatusBar _statusBar;

		public UIThread(ProgramThread programThread, TextBox consoleBox, StatusBar statusBar)
		{
			_programThread = programThread;
			_consoleBox = consoleBox;
			_statusBar = statusBar;
		}

		public void ThreadRun()
		{
			_statusBar.Text = "HTTP server running at port 49900.";

			while (true)
			{
				string[] lines = _programThread.stdoutBuffer.ToArray();
				_consoleBox.Text = String.Join("\r\n", lines);
				_consoleBox.SelectionStart = _consoleBox.Text.Length;
				_consoleBox.ScrollToCaret();

				Thread.Sleep(50);
			}
		}
	}
}

