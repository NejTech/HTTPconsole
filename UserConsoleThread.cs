/*

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace HTTPconsole
{
	public class UserConsoleThread
	{
        private ProgramThread _programthread;
        private UserInputThread _uithread;

        public void ThreadRun()
        {
            string commandName = Path.GetFileName(_programthread._filename);
            string commandArgs = _programthread._args;
            _uithread = new UserInputThread();
            Thread input = new Thread(new ThreadStart(_uithread.ThreadRun));
            input.Start();
        }

		public UserConsoleThread (ProgramThread programThread)
		{
            _programthread = programThread;
        }

		public static void AddLine(string line)
        {

			Console.ResetColor();
            ConsoleColor oForegroud = Console.ForegroundColor;
            ConsoleColor oBackground = Console.BackgroundColor;

			Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 2);
			Console.WriteLine();
			Console.WriteLine(line);


			Console.ForegroundColor = oBackground;
			Console.BackgroundColor = oForeground;
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("> ");
            Console.Write(_uithread.currentCommand);
            Console.Write(new string(' ', (Console.WindowWidth - (_uithread.currentCommand.Length + 2))));

			Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = oForegroud;
            Console.BackgroundColor = oBackground;

            int availableLines = Console.WindowHeight - 2;
            
			List<string> lines;
			lock(buffer) lines = buffer.Reverse<string>().Take(availableLines).Reverse<string>().ToList();
			// K proměnné buffer přistupuje ProgramThread, a pokud ji změní během enumerace, tak dojde k výjimce
			// Proto si připravíme pomocný list posledních n linek, které se vejdou do konzole, a až ty poté enumerujeme pomocí foreach
			// Jelikož Take(int x) bere prvních x linek, musíme List před a po otočit pomocí reverse, abychom dostali posledních x linek


			foreach (string line in lines)
			{
				if (line == null)
					break;

				if (line.Length < Console.WindowWidth)
				{
					Console.Write(line);
					Console.Write(new String(' ', (Console.WindowWidth - line.Length)));
				}
				else
				if (line.Length == Console.WindowWidth)
				{
					Console.Write(line);
				}
				else
				{
					Console.Write(line.Remove(Console.WindowWidth - 1));
				}
			}
        }
    }
}

*/