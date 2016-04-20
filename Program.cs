using System;
using System.Drawing;
using System.Windows.Forms;

namespace HTTPconsole
{
	class MainClass
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (args.Length == 0)
			{
				Application.Run(new UIForm("", ""));
			}
			else
			{
				string command = args[0];
				string arguments = "";

				if (args.Length > 1)
				{
					for (int i = 1; i < args.Length; i++)
					{
						arguments += args[i] + " ";
					}
				}
				Application.Run(new UIForm(command, arguments));
			}
		}
	}
}
