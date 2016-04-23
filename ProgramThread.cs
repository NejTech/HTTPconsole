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
		public volatile List<string> stderrBuffer; // unused

		public volatile Queue<string> stdinBuffer;

		private string _filename;
		private string _args;

        public void ThreadRun ()
		{
			ProcessStartInfo psi = new ProcessStartInfo();

			psi.FileName = _filename;
			psi.Arguments = _args;

			psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			psi.RedirectStandardInput = true;

			Process p = new Process();
			p.StartInfo = psi;
			p.OutputDataReceived += (sender, e) => SaveOutput(e.Data);
			p.ErrorDataReceived += (sender, e) => SaveOutput("(stderr): " + e.Data);

			p.Start();
			p.BeginOutputReadLine();
			p.BeginErrorReadLine();

			while (!p.HasExited)
			{
				lock (stdinBuffer)
				{
					if (stdinBuffer.Count != 0)
					{
						SaveOutput("(stdin): " + stdinBuffer.Peek());
						p.StandardInput.WriteLine(stdinBuffer.Dequeue());
					}
				}
			}

			p.WaitForExit();
			SaveOutput("--- PROGRAM EXITED WITH CODE: " + p.ExitCode.ToString() + " ---");
			SaveOutput("--- CLOSE HTTPCONSOLE WINDOW TO STOP HTTP SERVER ---");
		}

		private void SaveOutput(string output)
		{
			stdoutBuffer.Add(output);
			UIForm.Instance.ConsoleBoxAppendText(output + "\r\n");
		}

		public ProgramThread(string filename, string args)
		{
			stdoutBuffer = new List<string>();
			stderrBuffer = new List<string>();
			stdinBuffer = new Queue<string>();

			_filename = filename;
			_args = args;
		}
	}
}

