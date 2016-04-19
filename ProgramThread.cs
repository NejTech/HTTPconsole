using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace HTTPconsole
{
	public class ProgramThread
	{
		public volatile List<string> stdoutBuffer;
		public volatile List<string> stderrBuffer;

		public volatile Queue<string> stdinBuffer;

		private UserInputThread _uithread;

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
			p.OutputDataReceived += (sender, e) => SaveOutput(e.Data, p.HasExited);
			p.ErrorDataReceived += (sender, e) => SaveOutput(e.Data, p.HasExited);

			p.Start();
			p.BeginOutputReadLine();
			p.BeginErrorReadLine();

			while (!p.HasExited)
			{
				lock (stdinBuffer)
				{
					RedrawPromptUnix();
					if (_uithread.commands.Count != 0)
					{
						p.StandardInput.WriteLine(_uithread.commands.Dequeue());
					}
					if (stdinBuffer.Count != 0)
					{
						p.StandardInput.WriteLine(stdinBuffer.Dequeue());
					}
				}
			}

			p.WaitForExit();
			SaveOutput("----- PROGRAM EXITED WITH CODE: " + p.ExitCode.ToString() + " ------", p.HasExited);
			SaveOutput("--- PRESS CTRL-C TO STOP HTTP SERVER ---", p.HasExited
			);
		}

		public ProgramThread(string filename, string args)
		{
			_filename = filename;
			_args = args;

			stdoutBuffer = new List<string>();
			stderrBuffer = new List<string>();
			stdinBuffer = new Queue<string>(); // used for HTTP input

			_uithread = new UserInputThread();
			Thread input = new Thread(new ThreadStart(_uithread.ThreadRun));
			input.Start();
		}

		private void SaveOutput(string line, bool showPrompt)
		{
			if (line == null)
				return;

			if (line == "")
				line = " ";

			stdoutBuffer.Add(line);

			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				Console.SetCursorPosition(0, Console.WindowHeight - 1);

				if (line.Length < Console.BufferWidth)
				{
					Console.Write(line);
					Console.WriteLine(new String(' ', Console.WindowWidth - line.Length));
				}
				else if (line.Length > Console.BufferWidth)
				{
					Console.WriteLine(line.Remove(Console.WindowWidth - 1));
				}
				else
				{
					Console.WriteLine(line);
				}
			}
			else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				Console.SetCursorPosition(0, stdoutBuffer.Count);

				if (line.Length < Console.BufferWidth)
				{
					Console.Write(line);
					Console.WriteLine(new String(' ', Console.WindowWidth - line.Length));
				}
				else if (line.Length > Console.BufferWidth)
				{
					Console.WriteLine(line.Remove(Console.WindowWidth - 1));
				}
				else
				{
					Console.WriteLine(line);
				}
                RedrawPromptWindows();
			}
			else
			{
				return;
			}
		}

		private void RedrawPromptUnix()
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				Console.SetCursorPosition(0, Console.WindowHeight - 1);
				Console.Write("> ");
				Console.Write(_uithread.currentCommand);
				Console.Write(new String(' ', Console.WindowWidth - _uithread.currentCommand.Length - 2));
			}
			else
			{
				return;
			}
		}
        private void RedrawPromptWindows()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Console.SetCursorPosition(0, stdoutBuffer.Count + 1);
                Console.Write("> ");
                Console.Write(_uithread.currentCommand);
                Console.Write(new String(' ', Console.WindowWidth - _uithread.currentCommand.Length - 3));
            }
            else
            {
                return;
            }
        }
	}
}

