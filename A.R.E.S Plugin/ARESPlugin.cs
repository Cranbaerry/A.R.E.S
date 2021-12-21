using MelonLoader;
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Security.Cryptography;

[assembly: MelonInfo(typeof(ARESPlugin.Updater), "ARES Mod Updater", "1.1", "LargestBoi")]
[assembly: MelonColor(ConsoleColor.Yellow)]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ARESPlugin
{
    public class Updater : MelonPlugin
    {
        private Dictionary<string, string> Files = new Dictionary<string, string>();

        public override void OnApplicationStart()
        {
            Files.Add($"{MelonHandler.ModsDirectory}\\AvatarLogger.dll", "https://github.com/LargestBoi/A.R.E.S/releases/latest/download/AvatarLogger.dll");
            Files.Add($"{MelonUtils.GameDirectory}\\ReMod.Core.dll", "https://github.com/RequiDev/ReModCE/releases/latest/download/ReMod.Core.dll");
            Files.Add($"{MelonUtils.GameDirectory}\\ARESLogo.png", "https://github.com/LargestBoi/A.R.E.S/releases/latest/download/ARESLogo.png");

            foreach (KeyValuePair<string, string> pair in Files)
            {
                string name = pair.Key.Substring(pair.Key.LastIndexOf('\\') + 1);
                if (File.Exists(pair.Key))
                {
                    var OldHash = SHA256CheckSum(pair.Key);
                    MelonLogger.Msg($"{name} Found: {OldHash}");
                    DownloadMod(pair);
                    if (SHA256CheckSum(pair.Key) != OldHash)
                    {
                        MelonLogger.Msg($"Updated: {name}!");
                    }
                }
                else
                {
                    MelonLogger.Msg($"{name} Not Found! Downloading...");
                    DownloadMod(pair);
                    MelonLogger.Msg($"{name} Installed!");
                }
            }
        }

        private void DownloadMod(KeyValuePair<string, string> pair) => new WebClient().DownloadFile(pair.Value, pair.Key);

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