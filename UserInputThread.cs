using System;
using System.Collections.Generic;

namespace HTTPconsole
{
	public class UserInputThread
	{
        public Queue<string> commands;
        public string currentCommand { get; private set; }

        public void ThreadRun()
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();

            while (true)
            {
                while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                {
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if (currentCommand.Length > 0)
                        {
                            currentCommand = currentCommand.Remove(currentCommand.Length - 1);
                        }
                    }
                    else
                    {
                        currentCommand += key.KeyChar;
                    }
                }
                commands.Enqueue(currentCommand);
                currentCommand = "";
            }
        }

		public UserInputThread ()
		{
            commands = new Queue<string>();
            currentCommand = "";
		}
	}
}

