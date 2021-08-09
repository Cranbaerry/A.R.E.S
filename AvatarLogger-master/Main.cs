﻿using System;
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

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.Main), "Avatar Logger", "3", "KeafyIsHere, LargestBoi & cassell1337")]

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
        private static string tagsstr = "None";
        private static Config Config { get; set; }
        private static HarmonyMethod GetPatch<T>(string name) where T : class
         => new HarmonyMethod(typeof(T).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        public override void OnApplicationStart()
        {
            Directory.CreateDirectory("AvatarLog");
            if (!File.Exists(AvatarFile))
            { File.AppendAllText(AvatarFile, $"Original Mod by KeafyIsHere and Maintained by LargestBoi & cassell1337\n"); }
            foreach (string line in File.ReadAllLines(AvatarFile)) { AvatarIDs.Add(AvatarRegex.Match(line).Value); }
            if (!File.Exists(ConfigFile))
            {
                File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(new Config
                {
                    LogOwnAvatars = true,
                    LogFriendsAvatars = true,
                    LogToConsole = true,
                    ALLOW_API_UPLOAD = false,
                }, Formatting.Indented));
            }
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("AvatarLog\\Config.json"));
            foreach (MethodInfo method in typeof(AssetBundleDownloadManager).GetMethods().Where(m =>
            m.GetParameters().Length == 1
            && m.GetParameters().First().ParameterType == typeof(ApiAvatar)
            && m.ReturnType == typeof(void)))
            { HarmonyInstance.Patch(method, GetPatch<Main>("OnAvatarDownloaded")); }
            
            if (Config.ALLOW_API_UPLOAD)
            {
                try
                {
                    Process cmd = new Process();
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();

                    cmd.StandardInput.WriteLine("wmic baseboard get serialnumber");
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();
                    string HWID = cmd.StandardOutput.ReadToEnd().Replace("SerialNumber", "").Replace("\n", "").Replace("\r", "");
                    MelonLogger.Msg(HWID);
                    Leaf.xNet.HttpRequest request = new Leaf.xNet.HttpRequest();
                    request.Get("http://api.avataruploader.tk/checkin/" + HWID).ToString();
                    MelonLogger.Msg("Connected to API, UserID: " + HWID);
                }
                catch (Exception ex) { MelonLogger.Msg("Failed To Connect To API | " + ex.Message); }
            }
        }
        private static void APICall(string Avatar1)
        {
            try
            {
                Leaf.xNet.HttpRequest request = new Leaf.xNet.HttpRequest();
                //string HWID = UHWIDEngine.SimpleUid;
                //request.AddHeader(HttpHeader.UserAgent, HWID);
                request.Post("http://api.avataruploader.tk/upload", Avatar1, "application/json").ToString();
                MelonLogger.Msg("Avatar Logged To API");
            }
            catch (Exception ex) { MelonLogger.Msg("API Down | " + ex.Message); }
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
                else if (__0.releaseStatus == "private")
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
                    if (Config.LogToConsole) { MelonLogger.Msg($"[Avatar Logged] {__0.name} [Private]"); }
                    if (Config.ALLOW_API_UPLOAD)
                    {
                        string AvatarJson = "{\"TimeDetected\":\"" + UT + "\",\"AvatarID\":\"" + __0.id + "\",\"AvatarName\":\"" + __0.name + "\",\"AvatarDescription\":\"" + __0.description + "\",\"AuthorID\":\"" + __0.authorId + "\",\"AuthorName\":\"" + __0.authorName + "\",\"AssetURL\":\"" + __0.assetUrl + "\",\"ImageURL\":\"" + __0.imageUrl + "\",\"ThumbnailURL\":\"" + __0.thumbnailImageUrl + "\",\"ReleaseStatus\":\"" + __0.releaseStatus + "\",\"UnityVersion\":\"" + __0.unityVersion + "\",\"Platform\":\"" + __0.platform + "\",\"APIVersion\":\"" + __0.apiVersion + "\",\"Version\":\"" + __0.version + "\",\"Tags\":\"" + tagsstr + "\"}";
                        System.Threading.Thread thread = new System.Threading.Thread(() => APICall(AvatarJson));
                        thread.Start();
                    }
                }
            }
            return true;
        }
    }
}