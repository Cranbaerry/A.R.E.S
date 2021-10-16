using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IEnumerator = System.Collections.IEnumerator;
using Newtonsoft.Json;
using MelonLoader;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using RubyButtonAPI;
using VRC;
using VRC.Core;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(A.R.E.S_Logger.Main), "A.R.E.S_Logger", "1", "LargestBoi")]

namespace A.R.E.S_Logger
{
    internal class AvatarData
    {
        public string TimeDetected { get; set; }
        public string AvatarID { get; set; }
        public string AvatarName { get; set; }
        public string AvatarDescription { get; set; }
        public string AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AssetURL { get; set; }
        public string ImageURL { get; set; }
        public string ThumbnailURL { get; set; }
        public string ReleaseStatus { get; set; }
        public string UnityVersion { get; set; }
        public string Platform { get; set; }
        public int APIVersion { get; set; }
        public int Version { get; set; }
        public string Tags { get; set; }
    }
    public class Main : MelonMod
    {
        private const string LogFile = "AvatarLog\\Log.txt";
        private const string ConfigFile = "AvatarLog\\Config.json";

        private static List<string> AvatarIDs = new List<string>();
        private static Regex AvatarRegex = new Regex("avtr_[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}");
        private static string tagsstr = "None";
        private static HarmonyMethod GetPatch<T>(string name) where T : class
         => new HarmonyMethod(typeof(T).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        public override void OnApplicationStart()
        {
            Directory.CreateDirectory("AvatarLog");
            if (!File.Exists(LogFile))
            { File.AppendAllText(LogFile, "Mod by LargestBoi\n"); }
            foreach (string line in File.ReadAllLines(LogFile)) { AvatarIDs.Add(AvatarRegex.Match(line).Value); }
            foreach (MethodInfo method in typeof(AssetBundleDownloadManager).GetMethods().Where(m =>
            m.GetParameters().Length == 1
            && m.GetParameters().First().ParameterType == typeof(ApiAvatar)
            && m.ReturnType == typeof(void)))
            { HarmonyInstance.Patch(method, GetPatch<Main>("OnAvatarDownloaded")); }
        }
    }
}
