using MelonLoader;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;

[assembly: MelonInfo(typeof(ARESPlugin.Updater), "ARES Mod Updater", "1.0", "LargestBoi")]
[assembly: MelonColor(System.ConsoleColor.Yellow)]
[assembly: MelonGame]

namespace ARESPlugin
{
    public class Updater : MelonPlugin
    {
        private WebClient client = new WebClient();
        public static bool AvatarLogger = false;
        public static bool ButtonAPI = false;

        public override void OnApplicationStart()
        {
            var ModFiles = Directory.GetFiles(Environment.CurrentDirectory + "\\Mods", "*.dll").ToList();
            foreach (var Mod in ModFiles)
            {
                if(Mod.ToString().Contains("AvatarLogger.dll"))
                {
                    AvatarLogger = true;
                    var OldHash = SHA256CheckSum(Mod);
                    MelonLogger.Msg($"Avatar Logger Found: {OldHash}");
                    client.DownloadFile("https://github.com/LargestBoi/A.R.E.S/releases/latest/download/AvatarLogger.dll", Mod);
                    if (SHA256CheckSum(Mod) != OldHash)
                    {
                        MelonLogger.Msg($"Updated: AvatarLogger.dll!");
                    }
                }
                if (Mod.ToString().Contains("PlagueButtonAPI.dll"))
                {
                    ButtonAPI = true;
                    var OldHash = SHA256CheckSum(Mod);
                    MelonLogger.Msg($"Plague Button API Found: {OldHash}");
                    client.DownloadFile("https://github.com/LargestBoi/A.R.E.S/releases/latest/download/PlagueButtonAPI.dll", Mod);
                    if (SHA256CheckSum(Mod) != OldHash)
                    {
                        MelonLogger.Msg($"Updated: PlagueButtonAPI.dll!");
                    }
                }
            }
            if (!AvatarLogger)
            {
                MelonLogger.Msg($"Avatar Logger Not Found! Downloading...");
                client.DownloadFile("https://github.com/LargestBoi/A.R.E.S/releases/latest/download/AvatarLogger.dll", Environment.CurrentDirectory + "\\Mods\\AvatarLogger.dll");
                MelonLogger.Msg($"Avatar Logger Installed!");
            }
            if (!ButtonAPI)
            {
                MelonLogger.Msg($"ButtonAPI Not Found! Downloading...");
                client.DownloadFile("https://github.com/LargestBoi/A.R.E.S/releases/latest/download/PlagueButtonAPI.dll", Environment.CurrentDirectory + "\\Mods\\PlagueButtonAPI.dll");
                MelonLogger.Msg($"ButtonAPI Installed!");
            }
        }

        private string SHA256CheckSum(string filePath)
        {
            using (var hash = SHA256.Create())
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    return Convert.ToBase64String(hash.ComputeHash(fileStream));
                }
            }
        }
    }
}