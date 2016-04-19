using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace HTTPconsole
{
	public class HttpThread
	{
		private int _port = 49900;
		private ProgramThread _programthread;

		private string pageHtml;
		private string pageJS;
		private string pageStyle;
		private string jquery;

        public void ThreadRun()
		{
			if (!HttpListener.IsSupported)
			{
				Console.WriteLine("HTTPListener is not supported on this platform!");
				Console.WriteLine("Press any key to exit...");
				Environment.Exit(1);
			}

			HttpListener listener = new HttpListener();
			string prefix = "http://127.0.0.1:" + _port.ToString() + "/";
			listener.Prefixes.Add(prefix);
            try
            {
                listener.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Došlo k chybě při pokusu o zapnutí HTTP serveru.");
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    Console.WriteLine("K této chybě často dochází pod systémy Windows kvůli jejich");
                    Console.WriteLine("zásadám zabezpečení přístupu k službám protokolu HTTP.");
                    Console.WriteLine("Pro nápravu spusťte (jako administrátor) následující příkaz:");
                    Console.WriteLine("netsh http add urlacl url={0} user={1}", prefix,username);
                }
                Console.WriteLine("Chybová zpráva: " + e.Message);
            }

			while (true)
			{
				HttpListenerContext context = listener.GetContext();
				HttpListenerRequest request = context.Request;
				HttpListenerResponse response = context.Response;

				if (request.RawUrl == "/raw")
				{
					string stdoutf = "";
					foreach (string s in _programthread.stdoutBuffer)
					{
						stdoutf += s + "<br />";
					}

					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(stdoutf);
					response.ContentLength64 = buffer.Length;

					Stream output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);
					output.Close();
				}
				else if (request.RawUrl == "/js")
				{
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(pageJS);
					response.ContentLength64 = buffer.Length;

					Stream output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);
					output.Close();
				}
				else if (request.RawUrl == "/style")
				{
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(pageStyle);
					response.ContentLength64 = buffer.Length;

					Stream output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);
					output.Close();
				}
				else if (request.RawUrl == "/jquery")
				{
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(jquery);
					response.ContentLength64 = buffer.Length;

					Stream output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);
					output.Close();
				}
				else if (request.RawUrl == "/versioninfo")
				{
					string responsef = Environment.OSVersion.VersionString;
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responsef);
					response.ContentLength64 = buffer.Length;

					Stream output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);
					output.Close();
				}
				else if (request.RawUrl.Contains("/input?"))
				{
					string rawinput = request.RawUrl.Replace("/input?","");
					string input_b64 = WebUtility.UrlDecode(rawinput);
					string input = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(input_b64));
					_programthread.stdinBuffer.Enqueue(input);
				}
				else
				{
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(pageHtml);
					response.ContentLength64 = buffer.Length;

					Stream output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);
					output.Close();
				}
			}
		}

		public HttpThread(int port, ProgramThread programThread)
		{
			_port = port;
			_programthread = programThread;

			Assembly a = Assembly.GetExecutingAssembly();

			using (StreamReader sr1 = new StreamReader(a.GetManifestResourceStream("HTTPconsole.Resources.main.js")))
			{
				pageJS = sr1.ReadToEnd();
			}
			using (StreamReader sr2 = new StreamReader(a.GetManifestResourceStream("HTTPconsole.Resources.index.html")))
			{
				pageHtml = sr2.ReadToEnd();
			}
			using (StreamReader sr3 = new StreamReader(a.GetManifestResourceStream("HTTPconsole.Resources.style.css")))
			{
				pageStyle = sr3.ReadToEnd();
			}
			using (StreamReader sr4 = new StreamReader(a.GetManifestResourceStream("HTTPconsole.Resources.jquery.js")))
			{
				jquery = sr4.ReadToEnd();
			}
		}
	}
}

