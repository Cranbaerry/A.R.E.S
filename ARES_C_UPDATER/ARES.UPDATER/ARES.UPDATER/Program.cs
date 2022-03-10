using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ARES.UPDATER
{
    class Program
    {
        static string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static string guiLocation = fileLocation + @"\GUI\ARES.exe";
        static bool guiDownloaded;
        static int timeout = 7200000;
        static void Main(string[] args)
        {
            if (File.Exists("VRChat.exe"))
            {
                Console.WriteLine("This updater is about to close any running instances of\nARES, Unity, Unity Hub, Hotswaps and AssetRipper sessions!\nPlease save your work in your unity projects if they are open before\npressing enter to continue");
                while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                killProcess("ARES.exe");
                killProcess("HOTSWAP.exe");
                killProcess("Unity Hub.exe");
                killProcess("Unity.exe");
                killProcess("AssetRipperConsole.exe");
                if (!Directory.Exists(fileLocation + @"\GUI"))
                {
                    Directory.CreateDirectory(fileLocation + @"\GUI");
                    Console.WriteLine("Updating ARES");
                    startGuiDownload();
                }
                if (!File.Exists(fileLocation + @"\UnRAR.exe"))
                {
                    startUnRarDownload();
                }

                if (guiDownloaded)
                {
                    extractGUI();
                    return;
                }


                string application = SHA256CheckSum(guiLocation);
                string latestString = GetHashLatestAsync("https://raw.githubusercontent.com/Dean2k/A.R.E.S/main/VersionHashes/ARESGUI.txt");

                if (application.ToLower() != latestString.ToLower())
                {
                    Console.WriteLine("Updating ARES");
                    startGuiDownload();
                    extractGUI();
                }
                startARES();
            }
            else
            {
                Console.WriteLine("The updater is not currently in the VRChat folder, please place it\nalongside your 'VRChat.exe' file for optimal preformance!\nYou can just hit enter to close meh!");
                while (Console.ReadKey().Key != ConsoleKey.Enter) { }
            }

        }

        static void startGuiDownload()
        {
            FileDownloader fileDownloader = new FileDownloader("https://github.com/Dean2k/A.R.E.S/releases/latest/download/GUI.rar", fileLocation + @"\GUI.rar");
            fileDownloader.StartDownload(timeout, "GUI.rar");
            guiDownloaded = true;
        }

        static void startUnRarDownload()
        {
            FileDownloader fileDownloader = new FileDownloader("https://github.com/Dean2k/A.R.E.S/releases/latest/download/UnRAR.exe", fileLocation + @"\UnRAR.exe");
            fileDownloader.StartDownload(timeout, "UnRAR.rar");
        }

        static void extractGUI()
        {
            string fileExtractLocation = string.Format("x -id[c,d,n,p,q] -O+ -y {0}\\GUI.rar \"{0}{1}\"", fileLocation, @"\GUI\");
            try
            {
                string commands = string.Format("/C UnRAR.exe {0}", fileExtractLocation);

                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "CMD.EXE",
                    Arguments = commands,
                    WorkingDirectory = fileLocation
                };
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit();

            }
            catch { }
        }

        static void startARES()
        {
            try
            {
                string commands = string.Format("/C ARES.exe");

                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "CMD.EXE",
                    Arguments = commands,
                    WorkingDirectory = fileLocation + @"\GUI\",
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                };
                p.StartInfo = psi;
                p.Start();
            }
            catch { }
        }

        static void killProcess(string processName)
        {
            try
            {
                Process.Start("taskkill", "/F /IM \"" + processName + "\"");
                Console.WriteLine("Killed Process: " + processName);
            }
            catch { }
        }

        static string SHA256CheckSum(string filePath)
        {
            using (SHA256 SHA256 = SHA256Managed.Create())
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                    return BitConverter.ToString(SHA256.ComputeHash(fileStream)).Replace("-", "");
            }
        }

        static string GetHashLatestAsync(string url)
        {
            string result;
            using (HttpClient client = new HttpClient())
            {
                result = client.GetStringAsync(url).Result;
            }
            return result;
        }
    }
}
