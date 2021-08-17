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
using Leaf.xNet;
using static System.Net.WebRequest;
using System.Threading.Tasks;
using System.Diagnostics;
using RubyButtonAPI;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.Main), "Avatar Logger", "V3.2(Beta)", "KeafyIsHere, LargestBoi & cassell1337")]

#pragma warning disable IDE0044
#pragma warning disable IDE0051

namespace AvatarLogger
{
    public class Main : MelonMod
    {
        private const string ConfigFile = "AvatarLog\\Config.json";
        private const string AvatarFile = "AvatarLog\\Log.txt";
        private const string ErrorLogFile = "AvatarLog\\ErrorLog.txt";
        private static List<string> AvatarIDs = new List<string>();
        private static Regex AvatarRegex = new Regex("avtr_[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}");
        private static string tagsstr = "None";
        private static Config Config { get; set; }
        private static HarmonyMethod GetPatch<T>(string name) where T : class
         => new HarmonyMethod(typeof(T).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        public override void OnApplicationStart()
        {
            MelonCoroutines.Start(Buddon());
            Directory.CreateDirectory("AvatarLog");
            if (!File.Exists(AvatarFile))
            { File.AppendAllText(AvatarFile, $"Original Mod by KeafyIsHere and Maintained by LargestBoi & cassell1337\n"); }
            foreach (string line in File.ReadAllLines(AvatarFile)) { AvatarIDs.Add(AvatarRegex.Match(line).Value); }
            if (!File.Exists(ConfigFile))
            {
                File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
            }
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("AvatarLog\\Config.json"));
            SaveConfig();
            foreach (MethodInfo method in typeof(AssetBundleDownloadManager).GetMethods().Where(m =>
            m.GetParameters().Length == 1
            && m.GetParameters().First().ParameterType == typeof(ApiAvatar)
            && m.ReturnType == typeof(void)))
            { HarmonyInstance.Patch(method, GetPatch<Main>("OnAvatarDownloaded")); }

            if (Config.ALLOW_API_UPLOAD)
            {
                System.Threading.Thread thread = new System.Threading.Thread(() => updateuser());
            }
        }
        private static void updateuser()
        {
            try
            {
                string HWID = Config.Username;
                MelonLogger.Msg("Logged in user: " + HWID);
                Leaf.xNet.HttpRequest request = new Leaf.xNet.HttpRequest();
                request.ConnectTimeout = 25000;
                request.Get("http://api.avataruploader.tk/checkin/" + HWID).ToString();
                //MelonLogger.Msg("Connected to API, UserID: " + HWID);
            }
            catch (Exception ex)
            {
                string gg = "";
                //File.AppendAllText(ErrorLogFile, ex.Message);
                //MelonLogger.Msg("Failed To Connect To API | " + ex.Message + "\n");
            }
        }
        private static void APICall(string Avatar1)
        {
            try
            {
                Leaf.xNet.HttpRequest request = new Leaf.xNet.HttpRequest();
                string HWID = UnityEngine.SystemInfo.GetDeviceUniqueIdentifier();
                request.ConnectTimeout = 25000;
                request.AddHeader(HttpHeader.UserAgent, HWID);
                request.Post("https://api.avataruploader.tk/upload", Avatar1, "application/json").ToString();
                //MelonLogger.Msg("Avatar Logged To API:");
            }
            catch (Exception ex)
            {
                string gg = "";
                //File.AppendAllText(ErrorLogFile, ex.Message);
                //MelonLogger.Msg("Failed To Connect To API | " + ex.Message + "\n");
            }
        }
        private static bool OnAvatarDownloaded(ApiAvatar __0)
        {
            if (!Config.LogAvatars) { return true; }
            if (__0.authorId == APIUser.CurrentUser.id && !Config.LogOwnAvatars) { return true; }
            if (APIUser.CurrentUser.friendIDs.Contains(__0.authorId) && !Config.LogFriendsAvatars) { return true; }
            if (!AvatarIDs.Contains(__0.id))
            {
                AvatarIDs.Add(__0.id);
                DateTime foo = DateTime.Now;
                long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                string tagstr = string.Join(",", __0.tags);
                string UT = unixTime.ToString();
                File.AppendAllLines(AvatarFile, new string[]
                {
                    $"Time Detected:{UT}",
                    $"Avatar ID:{__0.id}",
                    $"Avatar Name:{__0.name}",
                    $"Avatar Description:{__0.description}",
                    $"Author ID:{__0.authorId}",
                    $"Author Name:{__0.authorName}",
                    $"Asset URL:{__0.assetUrl}",
                    $"Image URL:{__0.imageUrl}",
                    $"Thumbnail URL:{__0.thumbnailImageUrl}",
                    $"Release Status:{__0.releaseStatus}",
                    $"Unity Version:{__0.unityVersion}",
                    $"Platform:{__0.platform}",
                    $"API Version:{__0.apiVersion}",
                    $"Version:{__0.version}",
                });
                if (__0.tags.Count > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("Tags: ");
                    foreach (string tag in __0.tags) { builder.Append($"{tag},"); }
                    File.AppendAllText(AvatarFile, builder.ToString().Remove(builder.ToString().LastIndexOf(",")));
                    string tagsstr = builder.ToString().Remove(builder.ToString().LastIndexOf(","));
                }
                else
                {
                    File.AppendAllText(AvatarFile, "Tags: None");
                    var tagsstr = "";
                }
                File.AppendAllText(AvatarFile, "\n\n");
                if (Config.LogToConsole) { MelonLogger.Msg($"[Avatar Logged] {__0.name} [Public]"); }
                if (Config.ALLOW_API_UPLOAD)
                {
                    string AvatarJson = "{\"TimeDetected\":\"" + UT + "\",\"AvatarID\":\"" + __0.id + "\",\"AvatarName\":\"" + __0.name + "\",\"AvatarDescription\":\"" + __0.description + "\",\"AuthorID\":\"" + __0.authorId + "\",\"AuthorName\":\"" + __0.authorName + "\",\"AssetURL\":\"" + __0.assetUrl + "\",\"ImageURL\":\"" + __0.imageUrl + "\",\"ThumbnailURL\":\"" + __0.thumbnailImageUrl + "\",\"ReleaseStatus\":\"" + __0.releaseStatus + "\",\"UnityVersion\":\"" + __0.unityVersion + "\",\"Platform\":\"" + __0.platform + "\",\"APIVersion\":\"" + __0.apiVersion + "\",\"Version\":\"" + __0.version + "\",\"Tags\":\"" + tagsstr + "\"}";
                    System.Threading.Thread thread = new System.Threading.Thread(() => APICall(AvatarJson));
                    thread.Start();
                }
            }
            return true;
        }
        private void SaveConfig() => File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(Config, Formatting.Indented));
        private System.Collections.IEnumerator Buddon()
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null) { yield return null; }
            QMNestedButton MMButton = new QMNestedButton(
                "ShortcutMenu",
                0,
                0,
                "Avatar Logger\nSettings",
                "Settings for the avatar logger"
                );
            QMToggleButton LOAButton = new QMToggleButton(
                MMButton,
                1,
                0,
                "Log Own\nAvatars",
                delegate () { Config.LogOwnAvatars = true; SaveConfig(); },
                "OFF",
                delegate () { Config.LogOwnAvatars = false; SaveConfig(); },
                "Toggles logging of own avatars");
            LOAButton.setToggleState(Config.LogOwnAvatars);
            QMToggleButton LFAButton = new QMToggleButton(
                MMButton,
                2,
                0,
                "Log Friends\nAvatars",
                delegate () { Config.LogFriendsAvatars = true; SaveConfig(); },
                "OFF",
                delegate () { Config.LogFriendsAvatars = false; SaveConfig(); },
                "Toggles logging of friends avatars");
            LFAButton.setToggleState(Config.LogFriendsAvatars);
            QMToggleButton LTCButton = new QMToggleButton(
                MMButton,
                3,
                0,
                "Log To\nConsole",
                delegate () { Config.LogToConsole = true; SaveConfig(); },
                "OFF",
                delegate () { Config.LogToConsole = false; SaveConfig(); },
                "Toggles logging of to console");
            LTCButton.setToggleState(Config.LogToConsole);
            QMToggleButton LAButton = new QMToggleButton(
                MMButton,
                4,
                0,
                "Log Avatars",
                delegate () { Config.LogAvatars = true; SaveConfig(); },
                "OFF",
                delegate () { Config.LogAvatars = false; SaveConfig(); },
                "Toggles logging avatars");
            LAButton.setToggleState(Config.LogAvatars);
            QMToggleButton LTAButton = new QMToggleButton(
                MMButton,
                1,
                1,
                "Allow API",
                delegate () { Config.ALLOW_API_UPLOAD = true; SaveConfig(); },
                "OFF",
                delegate () { Config.ALLOW_API_UPLOAD = false; SaveConfig(); },
                "Toggles logging avatars to the API");
            LTAButton.setToggleState(Config.ALLOW_API_UPLOAD);
        }
    }
}