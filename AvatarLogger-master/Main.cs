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
using System.Text;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.Main), "Avatar Logger", "2.5", "KeafyIsHere & LargestBoi")]

#pragma warning disable IDE0044
#pragma warning disable IDE0051

namespace AvatarLogger
{
    public class Main : MelonMod
    {
        private const string ConfigFile = "AvatarLog\\Config.json";
        private const string AvatarFile = "AvatarLog\\Log.txt";

        private static List<string> AvatarIDs = new List<string>();
        private static Regex AvatarRegex = new Regex("avtr_[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}");

        private static Config Config { get; set; }

        private static HarmonyMethod GetPatch<T>(string name) where T : class
         => new HarmonyMethod(typeof(T).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));

        public override void OnApplicationStart()
        {
            Directory.CreateDirectory("AvatarLog");

            if (!File.Exists(AvatarFile))
            { File.AppendAllText(AvatarFile, $"Original Mod by KeafyIsHere and Maintained by LargestBoi\n"); }

            foreach (string line in File.ReadAllLines(AvatarFile)) { AvatarIDs.Add(AvatarRegex.Match(line).Value); }

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
                    DateTime foo = DateTime.Now;
                    long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                    string tagstr = string.Join(",", __0.tags);
                    File.AppendAllLines(AvatarFile, new string[]
                    {
                        $"Time Detected:{unixTime}",
                        $"Avatar ID:{__0.id}",
                        $"Avatar Name:{__0.name}",
                        $"Avatar Description:{__0.description}",
                        $"Author ID:{__0.authorId}",
                        $"Author Name:{__0.authorName}",
                        $"Asset URL:{__0.assetUrl}",
                        $"Image URL:{__0.imageUrl}",
                        $"Thumbnail URL:{__0.thumbnailImageUrl}",
                        $"Release Status:{__0.releaseStatus}",
                        $"Version:{__0.version}",
                    });
                    if (__0.tags.Count > 0)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append("Tags: ");
                        foreach (string tag in __0.tags) { builder.Append($"{tag},"); }
                        File.AppendAllText(AvatarFile, builder.ToString().Remove(builder.ToString().LastIndexOf(",")));
                    }
                    else
                    {
                        File.AppendAllText(AvatarFile, "Tags: None");
                    }
                    File.AppendAllText(AvatarFile, "\n\n");
                    if (Config.LogToConsole) { MelonLogger.Msg($"[Avatar Logged] {__0.name} [Public]"); }
                }
                else if (__0.releaseStatus == "private")
                {
                    AvatarIDs.Add(__0.id);
                    DateTime foo = DateTime.Now;
                    long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                    string tagstr = string.Join(",", __0.tags);
                    File.AppendAllLines(AvatarFile, new string[]
                    {
                        $"Time Detected:{unixTime}",
                        $"Avatar ID:{__0.id}",
                        $"Avatar Name:{__0.name}",
                        $"Avatar Description:{__0.description}",
                        $"Author ID:{__0.authorId}",
                        $"Author Name:{__0.authorName}",
                        $"Asset URL:{__0.assetUrl}",
                        $"Image URL:{__0.imageUrl}",
                        $"Thumbnail URL:{__0.thumbnailImageUrl}",
                        $"Release Status:{__0.releaseStatus}",
                        $"Version:{__0.version}",
                    });
                    if (__0.tags.Count > 0)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append("Tags: ");
                        foreach (string tag in __0.tags) { builder.Append($"{tag},"); }
                        File.AppendAllText(AvatarFile, builder.ToString().Remove(builder.ToString().LastIndexOf(",")));
                    }
                    else
                    {
                        File.AppendAllText(AvatarFile, "Tags: None");
                    }
                    File.AppendAllText(AvatarFile, "\n\n");
                    if (Config.LogToConsole) { MelonLogger.Msg($"[Avatar Logged] {__0.name} [Private]"); }
                }
            }
            return true;
        }
    }
}