using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MelonLoader;
using HarmonyLib;
using Newtonsoft.Json;
using VRC.Core;
using VRC;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.Main), "Avatar Logger", "1.5", "KeafyIsHere & LargestBoi")]

#pragma warning disable IDE0044
#pragma warning disable IDE0051

namespace AvatarLogger
{
    public class Main : MelonMod
    {
        private const string ConfigFile = "AvatarLog\\Config.json";
        private const string PublicAvatarFile = "AvatarLog\\Public.txt";
        private const string PrivateAvatarFile = "AvatarLog\\Private.txt";

        private static List<string> AvatarIDs = new List<string>();
        private static Regex AvatarRegex = new Regex("avtr_[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}");

        private static Config Config { get; set; }

        private static HarmonyMethod GetPatch<T>(string name) where T : class
         => new HarmonyMethod(typeof(T).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));

        public override void OnApplicationStart()
        {
            Directory.CreateDirectory("AvatarLog");

            if (!File.Exists(PublicAvatarFile))
            { File.AppendAllText(PublicAvatarFile, $"Original Mod by KeafyIsHere and Maintained by LargestBoi\n"); }
            if (!File.Exists(PrivateAvatarFile))
            { File.AppendAllText(PrivateAvatarFile, $"Original Mod by KeafyIsHere and Maintained by LargestBoi\n"); }

            foreach (string line in File.ReadAllLines(PublicAvatarFile)) { AvatarIDs.Add(AvatarRegex.Match(line).Value); }
            foreach (string line in File.ReadAllLines(PrivateAvatarFile)) { AvatarIDs.Add(AvatarRegex.Match(line).Value); }

            if (!File.Exists(ConfigFile))
            {
                File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(new Config
                {
                    LogOwnAvatars = true,
                    LogFriendsAvatars = true,
                    LogToConsole = true,
                }, Formatting.Indented));
            }
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("AvatarLog\\Config.json"));

            foreach (MethodInfo method in typeof(AssetBundleDownloadManager).GetMethods().Where(m =>
            m.GetParameters().Length == 1
            && m.GetParameters().First().ParameterType == typeof(ApiAvatar)
            && m.ReturnType == typeof(void)))
            { HarmonyInstance.Patch(method, GetPatch<Main>("OnAvatarDownloaded")); }
        }

        private static bool OnAvatarDownloaded(ApiAvatar __0)
        {
            if (__0.authorId == APIUser.CurrentUser.id && !Config.LogOwnAvatars) { return true; }
            if (APIUser.CurrentUser.friendIDs.Contains(__0.authorId) && !Config.LogFriendsAvatars) { return true; }
            if (!AvatarIDs.Contains(__0.id))
            {
                if (__0.releaseStatus == "public")
                {
                    AvatarIDs.Add(__0.id);
                    File.AppendAllLines(PublicAvatarFile, new string[]
                    {
                        $"Time Detected:{DateTime.Now}",
                        $"Avatar ID:{__0.id}",
                        $"Avatar Name:{__0.name}",
                        $"Avatar Description:{__0.description}",
                        $"Author ID:{__0.authorId}",
                        $"Author Name:{__0.authorName}",
                        $"Asset URL:{__0.assetUrl}",
                        $"Image URL:{__0.imageUrl}",
                        $"Thumbnail URL:{__0.thumbnailImageUrl}",
                        $"Release Status:{__0.releaseStatus}",
                        $"Version:{__0.version}\n"
                    });
                    if (Config.LogToConsole) { MelonLogger.Msg($"[Avatar Logged] {__0.name} [Public]"); }
                }
                else if (__0.releaseStatus == "private")
                {
                    AvatarIDs.Add(__0.id);
                    File.AppendAllLines(PrivateAvatarFile, new string[]
                    {
                        $"Time Detected:{DateTime.Now}",
                        $"Avatar ID:{__0.id}",
                        $"Avatar Name:{__0.name}",
                        $"Avatar Description:{__0.description}",
                        $"Author ID:{__0.authorId}",
                        $"Author Name:{__0.authorName}",
                        $"Asset URL:{__0.assetUrl}",
                        $"Image URL:{__0.imageUrl}",
                        $"Thumbnail URL:{__0.thumbnailImageUrl}",
                        $"Release Status:{__0.releaseStatus}",
                        $"Version:{__0.version}\n"
                    });
                    if (Config.LogToConsole) { MelonLogger.Msg($"[Avatar Logged] {__0.name} [Private]"); }
                }
            }
            return true;
        }
    }
}