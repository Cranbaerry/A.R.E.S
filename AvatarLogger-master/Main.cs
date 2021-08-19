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
using Leaf.xNet;
using RubyButtonAPI;
using ComfyUtils;
using VRC;
using VRC.UI;
using VRC.Core;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.Main), "Avatar Logger", "V3.2", "KeafyIsHere, LargestBoi, cassell1337, MonkeyBoi(Boppr)")]

#pragma warning disable IDE0044
#pragma warning disable IDE0051
#pragma warning disable IDE0059
#pragma warning disable CS0168

namespace AvatarLogger
{
    internal class Avatar
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
        private const string ConfigFile = "AvatarLog\\Config.json";
        private const string AvatarFile = "AvatarLog\\Log.txt";
        private const string ErrorLogFile = "AvatarLog\\ErrorLog.txt";
        private static List<string> AvatarIDs = new List<string>();
        private static Regex AvatarRegex = new Regex("avtr_[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}");
        private static string tagsstr = "None";
        private static ConfigHelper<Config> Helper;
        private static Config Config;
        private static HarmonyMethod GetPatch<T>(string name) where T : class
         => new HarmonyMethod(typeof(T).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        public override void OnApplicationStart()
        {
            MelonCoroutines.Start(Buddon());
            Directory.CreateDirectory("AvatarLog");
            Helper = new ConfigHelper<Config>(ConfigFile);
            Config = Helper.Config;
            Helper.OnConfigUpdated += new Action(delegate () { Config = Helper.Config; MelonLogger.Msg("[Config Updated]"); });
            if (!File.Exists(AvatarFile))
            { File.AppendAllText(AvatarFile, "Original Mod by KeafyIsHere and Maintained by LargestBoi & cassell1337\n"); }
            foreach (string line in File.ReadAllLines(AvatarFile)) { AvatarIDs.Add(AvatarRegex.Match(line).Value); }
            foreach (MethodInfo method in typeof(AssetBundleDownloadManager).GetMethods().Where(m =>
            m.GetParameters().Length == 1
            && m.GetParameters().First().ParameterType == typeof(ApiAvatar)
            && m.ReturnType == typeof(void)))
            { HarmonyInstance.Patch(method, GetPatch<Main>("OnAvatarDownloaded")); }
            if (Config.SendToAPI) { Thread thread = new Thread(() => UpdateUser()); }
        }
        private static void UpdateUser()
        {
            try
            {
                string HWID = Config.Username;
                MelonLogger.Msg("Logged in user: " + HWID);
                Leaf.xNet.HttpRequest request = new Leaf.xNet.HttpRequest { ConnectTimeout = 25000 };
                request.Get("http://api.avataruploader.tk/checkin/" + HWID).ToString();
                //MelonLogger.Msg("Connected to API, UserID: " + HWID);
            }
            catch (Exception ex)
            {
                //File.AppendAllText(ErrorLogFile, ex.Message);
                //MelonLogger.Msg("Failed To Connect To API | " + ex.Message + "\n");
            }
        }
        private static void APICall(Avatar avatar)
        {
            try
            {
                Leaf.xNet.HttpRequest request = new Leaf.xNet.HttpRequest();
                string HWID = UnityEngine.SystemInfo.GetDeviceUniqueIdentifier();
                request.ConnectTimeout = 25000;
                request.AddHeader(HttpHeader.UserAgent, HWID);
                request.Post("https://api.avataruploader.tk/upload", JsonConvert.SerializeObject(avatar), "application/json");
                //MelonLogger.Msg("Avatar Logged To API:");
            }
            catch (Exception ex)
            {
                //File.AppendAllText(ErrorLogFile, ex.Message);
                //MelonLogger.Msg("Failed To Connect To API | " + ex.Message + "\n");
            }
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
                    $"Asset URL:{avatar.assetUrl}",
                    $"Image URL:{avatar.imageUrl}",
                    $"Thumbnail URL:{avatar.thumbnailImageUrl}",
                    $"Release Status:{avatar.releaseStatus}",
                    $"Unity Version:{avatar.unityVersion}",
                    $"Platform:{avatar.platform}",
                    $"API Version:{avatar.apiVersion}",
                    $"Version:{avatar.version}",
            });
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
            if (Config.LogToConsole) { MelonLogger.Msg($"[Avatar Logged] {avatar.name} [Public]"); }
            if (Config.SendToAPI)
            {
                Avatar AvatarToSend = new Avatar
                {
                    TimeDetected = UT,
                    AvatarID = avatar.id,
                    AvatarName = avatar.name,
                    AvatarDescription = avatar.description,
                    AuthorID = avatar.authorId,
                    AuthorName = avatar.authorName,
                    AssetURL = avatar.assetUrl,
                    ImageURL = avatar.imageUrl,
                    ThumbnailURL = avatar.thumbnailImageUrl,
                    ReleaseStatus = avatar.releaseStatus,
                    UnityVersion = avatar.unityVersion,
                    Platform = avatar.platform,
                    APIVersion = avatar.apiVersion,
                    Version = avatar.version,
                    Tags = tagsstr
                };
                new Thread(() => APICall(AvatarToSend)).Start();
            }
        }
        private IEnumerator Buddon()
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null) { yield return null; }
            QMNestedButton MMButton = new QMNestedButton(
                "ShortcutMenu",
                0, 0,
                "Avatar Logger\nSettings",
                "Settings for the avatar logger"
                );
            QMToggleButton LOAButton = new QMToggleButton(
                MMButton,
                1, 0,
                "Log Own\nAvatars On",
                delegate ()
                {
                    Config.LogOwnAvatars = true;
                    RelogWorld();
                    Helper.SaveConfig(Config);
                },
                "Log Own\nAvatars Off",
                delegate ()
                {
                    Config.LogOwnAvatars = false;
                    Helper.SaveConfig(Config);
                },
                "Toggles logging of own avatars"
                );
            LOAButton.setToggleState(Config.LogOwnAvatars);
            QMToggleButton LFAButton = new QMToggleButton(
                MMButton,
                2, 0,
                "Log Friends\nAvatars On",
                delegate ()
                {
                    Config.LogFriendsAvatars = true;
                    RelogWorld();
                    Helper.SaveConfig(Config);
                },
                "Log Friends\nAvatars Off",
                delegate ()
                {
                    Config.LogFriendsAvatars = false;
                    Helper.SaveConfig(Config);
                },
                "Toggles logging of friends avatars"
                );
            LFAButton.setToggleState(Config.LogFriendsAvatars);
            QMToggleButton LTCButton = new QMToggleButton(
                MMButton,
                3, 0,
                "Log To\nConsole On",
                delegate ()
                {
                    Config.LogToConsole = true;
                    Helper.SaveConfig(Config);
                },
                "Log To\nConsole Off",
                delegate ()
                {
                    Config.LogToConsole = false;
                    Helper.SaveConfig(Config);
                },
                "Toggles logging of to console"
                );
            LTCButton.setToggleState(Config.LogToConsole);
            QMToggleButton LAButton = new QMToggleButton(
                MMButton,
                4, 0,
                "Log Avatars\nOn",
                delegate ()
                {
                    Config.LogAvatars = true;
                    RelogWorld();
                    Helper.SaveConfig(Config);
                },
                "Log Avatars\nOff",
                delegate ()
                {
                    Config.LogAvatars = false;
                    Helper.SaveConfig(Config);
                },
                "Toggles logging avatars"
                );
            LAButton.setToggleState(Config.LogAvatars);
            QMToggleButton LTAButton = new QMToggleButton(
                MMButton,
                1, 1,
                "Send To\nAPI On",
                delegate ()
                {
                    Config.SendToAPI = true;
                    RelogWorld();
                    Helper.SaveConfig(Config);
                },
                "Send To\nAPI Off",
                delegate ()
                {
                    Config.SendToAPI = false;
                    Helper.SaveConfig(Config);
                },
                "Toggles logging avatars to the API"
                );
            LTAButton.setToggleState(Config.SendToAPI);
            GameObject.Destroy(GameObject.Find("UserInterface/QuickMenu/UserInteractMenu/ReportAbuseButton"));
            new QMSingleButton(
                "UserInteractMenu",
                4, 1,
                "Log User's\nAvatar",
                delegate ()
                { LogAvatar(QuickMenu.prop_QuickMenu_0.field_Private_Player_0.prop_ApiAvatar_0); },
                "Logs selected user's avatar"
                );
            CreateLPA();
        }
        private async void CreateLPA()
        {
            GameObject LPAOriginal = GameObject.Find("UserInterface/MenuContent/Screens/Avatar/Favorite Button");
            GameObject LPA = GameObject.Instantiate(LPAOriginal, LPAOriginal.transform.parent);
            await Task.Delay(10);
            LPA.name = "LogPreviewAvatar";
            LPA.transform.localPosition = new Vector3(-241f, 375f, LPA.transform.position.z);
            Transform child = LPA.transform.FindChild("Horizontal");
            for (int i = 0; i < child.childCount; i++)
            {
                GameObject child0 = child.GetChild(i).gameObject;
                if (child0.name != "FavoriteActionText") { GameObject.Destroy(child0); }
            }
            child.GetComponentInChildren<Text>().text = "Log Avatar";
            LPA.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            LPA.GetComponent<Button>().onClick.AddListener((UnityAction)delegate ()
            {
                new Thread(() =>
                {
                    while (LPA.transform.parent.GetComponent<PageAvatar>().field_Private_GameObject_0 == null) { Task.Delay(1); }
                    string avatarID = AvatarRegex.Match(LPA.transform.parent.GetComponent<PageAvatar>().field_Private_GameObject_0.name).Value;
                    API.Fetch<ApiAvatar>(avatarID, onSuccess: new Action<ApiContainer>(container =>
                    { LogAvatar(container.Model.Cast<ApiAvatar>()); }));
                }).Start();
            });
            LPA.SetActive(true);
        }
        private void RelogWorld()
        {
            foreach (Player player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            { OnAvatarDownloaded(player.prop_ApiAvatar_0); }
        }
    }
}