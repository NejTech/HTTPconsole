using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HTTPconsole
{
	public class ProgramThread
	{
		public volatile List<string> stdoutBuffer;
		public volatile List<string> stderrBuffer;

		public volatile Queue<string> stdinBuffer;

		private string _filename;
		private string _args;

        public void ThreadRun ()
		{
			ProcessStartInfo psi = new ProcessStartInfo();

			psi.FileName = _filename;
			psi.Arguments = _args;

			psi.UseShellExecute = false;
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			psi.RedirectStandardInput = true;

			Process p = new Process();
			p.StartInfo = psi;
			p.OutputDataReceived += (sender, e) => stdoutBuffer.Add(e.Data);
			p.ErrorDataReceived += (sender, e) => stdoutBuffer.Add(e.Data);

			p.Start();
			p.BeginOutputReadLine();
			p.BeginErrorReadLine();

			while (!p.HasExited)
			{
				lock (stdinBuffer)
				{
					if (stdinBuffer.Count != 0)
					{
						stdoutBuffer.Add("(INPUT): " + stdinBuffer.Peek());
						p.StandardInput.WriteLine(stdinBuffer.Dequeue());
					}
				}
			}

			p.WaitForExit();
			stdoutBuffer.Add("----- PROGRAM EXITED WITH CODE: " + p.ExitCode.ToString() + " ------");
			stdoutBuffer.Add("--- PRESS CTRL-C TO STOP HTTP SERVER ---");
		}

		public ProgramThread(string filename, string args)
		{
			if (filename == "")
			{
				using (CommandForm cf = new CommandForm()) //!!! FIXME Move to UIForm and get WinForms code out of here
				{
					if (cf.ShowDialog() == DialogResult.Cancel)
					{
						Environment.Exit(0);
					}
				}
			}
			else
			{
				_filename = filename;
				_args = args;
			}

			stdoutBuffer = new List<string>();
			stderrBuffer = new List<string>();
			stdinBuffer = new Queue<string>();

		}

		public void CommandFormReportBack(string filename, string args)
		{
			_filename = filename;
			_args = args;
		}
	}
}

