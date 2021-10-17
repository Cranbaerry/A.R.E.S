using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MelonLoader;
using HarmonyLib;
using ComfyUtils;
using VRC.Core;
using UnityEngine;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.Main), "A.R.E.S", "0.2B", "LargestBoi")]

#pragma warning disable IDE0044
#pragma warning disable IDE0051
#pragma warning disable IDE0059

namespace AvatarLogger
{
    //Creates avatar class to hold infos
    internal class Avatar
    {
        public string TimeDetected { get; set; }
        public string AvatarID { get; set; }
        public string AvatarName { get; set; }
        public string AvatarDescription { get; set; }
        public string AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AssetURL { get; set; }
        public string QuestAsstURL { get; set; }
        public string ImageURL { get; set; }
        public string ThumbnailURL { get; set; }
        public string ReleaseStatus { get; set; }
        public string UnityVersion { get; set; }
        public string Platforms { get; set; }
        public int APIVersion { get; set; }
        public int Version { get; set; }
        public string Tags { get; set; }
    }
    public class Main : MelonMod
    {
        //Log creation and reading
        private const string ConfigFile = "AvatarLog\\Config.json";
        private const string AvatarFile = "AvatarLog\\Log.txt";
        private static List<string> AvatarIDs = new List<string>();
        //Sets avatar id regex
        private static Regex AvatarRegex = new Regex("avtr_[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}");
        private static string tagsstr = "None";
        //Uses the Comfy config helper (Thx Boppr)
        private static ConfigHelper<Config> Helper;
        private static Config Config;
        private static HarmonyMethod GetPatch<T>(string name) where T : class
         => new HarmonyMethod(typeof(T).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        public override void OnApplicationStart()
        {
            Directory.CreateDirectory("AvatarLog");
            Helper = new ConfigHelper<Config>(ConfigFile);
            Config = Helper.Config;
            //If config changes inform user in console
            Helper.OnConfigUpdated += new Action(delegate () { Config = Helper.Config; MelonLogger.Msg("[Config Updated]"); });
            //Creation of the initial log file
            if (!File.Exists(AvatarFile))
            { File.AppendAllText(AvatarFile, "Mod re-written by LargestBoi\n"); }
            foreach (string line in File.ReadAllLines(AvatarFile)) { AvatarIDs.Add(AvatarRegex.Match(line).Value); }
            foreach (MethodInfo method in typeof(AssetBundleDownloadManager).GetMethods().Where(m =>
            m.GetParameters().Length == 1
            && m.GetParameters().First().ParameterType == typeof(ApiAvatar)
            && m.ReturnType == typeof(void)))
            { HarmonyInstance.Patch(method, GetPatch<Main>("OnAvatarDownloaded")); }
        }
        private static bool OnAvatarDownloaded(ApiAvatar __0)
        {
            ApiAvatar avatar = __0;
            if (!Config.LogAvatars) { return true; }
            if (avatar.authorId == APIUser.CurrentUser.id && !Config.LogOwnAvatars) { return true; }
            if (APIUser.CurrentUser.friendIDs.Contains(avatar.authorId) && !Config.LogFriendsAvatars) { return true; }
            if (!AvatarIDs.Contains(avatar.id)) { LogAvatar(avatar); }
            return true;
        }

        private static void LogAvatar(ApiAvatar avatar)
        {
            AvatarIDs.Add(avatar.id);



            string UT = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString();
            string tagstr = string.Join(",", avatar.tags);
            File.AppendAllLines(AvatarFile, new string[]
            {
                    $"Time Detected:{UT}",
                    $"Avatar ID:{avatar.id}",
                    $"Avatar Name:{avatar.name}",
                    $"Avatar Description:{avatar.description}",
                    $"Author ID:{avatar.authorId}",
                    $"Author Name:{avatar.authorName}",
                    $"PC Asset URL:{avatar.assetUrl}",
                    $"Image URL:{avatar.imageUrl}",
                    $"Thumbnail URL:{avatar.thumbnailImageUrl}",
                    $"Release Status:{avatar.releaseStatus}",
                    $"Unity Version:{avatar.unityVersion}",
                    $"Platforms:{avatar.supportedPlatforms}",
                    $"API Version:{avatar.apiVersion}",
                    $"Version:{avatar.version}",
            });
            //Converts tags to string to be logged
            if (avatar.tags.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Tags: ");
                foreach (string tag in avatar.tags) { builder.Append($"{tag},"); }
                File.AppendAllText(AvatarFile, builder.ToString().Remove(builder.ToString().LastIndexOf(",")));
                string tagsstr = builder.ToString().Remove(builder.ToString().LastIndexOf(","));
            }
            else { File.AppendAllText(AvatarFile, "Tags: None"); }
            File.AppendAllText(AvatarFile, "\n\n");
            if (Config.LogToConsole) { MelonLogger.Msg($"AvatarLogged: {avatar.name} | {avatar.releaseStatus}"); }
        }
    }
}