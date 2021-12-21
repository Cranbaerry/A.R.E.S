using MelonLoader;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;

[assembly: MelonInfo(typeof(ARESPlugin.Updater), "ARES Mod Updater", "1.1", "LargestBoi")]
[assembly: MelonColor(ConsoleColor.Yellow)]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ARESPlugin
{
    public class Updater : MelonPlugin
    {
        private string Mod => $"{MelonHandler.ModsDirectory}\\AvatarLogger.dll";

        public override void OnApplicationStart()
        {
            if (File.Exists(Mod))
            {
                var OldHash = SHA256CheckSum(Mod);
                MelonLogger.Msg($"Avatar Logger Found: {OldHash}");
                DownloadMod();
                if (SHA256CheckSum(Mod) != OldHash)
                {
                    MelonLogger.Msg($"Updated: AvatarLogger.dll!");
                }
            }
            else
            {
                MelonLogger.Msg($"Avatar Logger Not Found! Downloading...");
                DownloadMod();
                MelonLogger.Msg($"Avatar Logger Installed!");
            }
        }

        private void DownloadMod() => new WebClient().DownloadFile("https://github.com/LargestBoi/A.R.E.S/releases/latest/download/AvatarLogger.dll", Mod);

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