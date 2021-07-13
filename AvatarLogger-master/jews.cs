using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using DSharpPlus;
using DSharpPlus.Entities;
using HarmonyLib;
using MelonLoader;
using NekroExtensions;
using Newtonsoft.Json;
using VRC.Core;
using System.Net;
using UnityEngine;

namespace AvatarLoger
{
    public class Jews : MelonMod
    {
        private const string PublicAvatarFile = "AvatarLog\\Public.txt";
        private const string PrivateAvatarFile = "AvatarLog\\Private.txt";
        private static string _avatarIDs = "";
        private static readonly Queue<ApiAvatar> AvatarToPost = new Queue<ApiAvatar>();
        private static readonly HttpClient WebHookClient = new HttpClient();
        private static readonly BoolPacking WebHookBoolBundle = new BoolPacking();

        private static readonly DiscordColor PrivateColor = new DiscordColor("#FF0000");
        private static readonly DiscordColor PublicColor = new DiscordColor("#00FF00");

        private static Config Config { get; set; }

        private static HarmonyMethod GetPatch(string name)
        {
            return new HarmonyMethod(typeof(Jews).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }

        public override void OnApplicationStart()
        {
            // create directory if it doesnt exist
            // and yes you can use this without doing Directory.Exists("AvatarLog")
            // because Directory.CreateDirectory("AvatarLog"); checks if it already exists 
            // "Creates all directories and subdirectories in the specified path unless they already exist." from https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.createdirectory?view=net-5.0
            Directory.CreateDirectory("AvatarLog");


            // create log files if they dont exist
            if (!File.Exists(PublicAvatarFile))
                File.AppendAllText(PublicAvatarFile, $"Made by KeafyIsHere{Environment.NewLine}");
            if (!File.Exists(PrivateAvatarFile))
                File.AppendAllText(PrivateAvatarFile, $"Made by KeafyIsHere{Environment.NewLine}");


            // load all ids from the the text files
            foreach (var line in File.ReadAllLines(PublicAvatarFile))
                if (line.Contains("Avatar ID"))
                    _avatarIDs += line.Replace("Avatar ID:", "");
            foreach (var line in File.ReadAllLines(PrivateAvatarFile))
                if (line.Contains("Avatar ID"))
                    _avatarIDs += line.Replace("Avatar ID:", "");


            // check config and create if needed
            if (!File.Exists("AvatarLog\\Config.json"))
            {
                // create config since its not there or user renamed it
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now}] [AvatarLogger] Config.json not found!");
                Console.WriteLine($"[{DateTime.Now}] [AvatarLogger] Config.json Generating new one please fill out");
                File.WriteAllText("AvatarLog\\Config.json", JsonConvert.SerializeObject(new Config
                {
                    CanPostSelfAvatar = false,
                    CanPostFriendsAvatar = false,
                    PrivateWebhook = "",
                    PublicWebhook = ""
                }, Formatting.Indented));
                Console.ResetColor();
            }
            else
            {
                // config exists so load it pog
                MelonLogger.Msg(ConsoleColor.Green, $"Config File Detected!");
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("AvatarLog\\Config.json"));
            }


            // check the webhook urls the user put in the config
            if (!string.IsNullOrEmpty(Config.PrivateWebhook) &&
                Config.PrivateWebhook.StartsWith("https://") &&
                Config.PrivateWebhook.Count(x => x.Equals('/')).Equals(6) &&
                Config.PrivateWebhook.Contains("discord.com/api/webhooks/")) WebHookBoolBundle[0] = true;
            if (!string.IsNullOrEmpty(Config.PublicWebhook) &&
                Config.PublicWebhook.StartsWith("https://") &&
                Config.PublicWebhook.Count(x => x.Equals('/')).Equals(6) &&
                Config.PublicWebhook.Contains("discord.com/api/webhooks/")) WebHookBoolBundle[1] = true;


            // patch methods in the AssetBundleDownloadManager to log avatars pog
            foreach (var methodInfo in typeof(AssetBundleDownloadManager).GetMethods().Where(p =>
                p.GetParameters().Length == 1 && p.GetParameters().First().ParameterType == typeof(ApiAvatar) &&
                p.ReturnType == typeof(void)))
            {
                HarmonyInstance.Patch(methodInfo, GetPatch("ApiAvatarDownloadPatch"));
            }

            // start thread to 
            MelonCoroutines.Start(DoCheck());
        }

        // ReSharper disable once UnusedMember.Local
        private static bool ApiAvatarDownloadPatch(ApiAvatar __0)
        {
            if (!_avatarIDs.Contains(__0.id))
            {
                if (__0.releaseStatus == "public")
                {
                    _avatarIDs += __0.id;
                    var sb = new StringBuilder();
                    sb.AppendLine($"Time detected:{DateTime.Now}");
                    sb.AppendLine($"Avatar ID:{__0.id}");
                    sb.AppendLine($"Avatar Name:{__0.name}");
                    sb.AppendLine($"Avatar Description:{__0.description}");
                    sb.AppendLine($"Avatar Author ID:{__0.authorId}");
                    sb.AppendLine($"Avatar Author Name:{__0.authorName}");
                    sb.AppendLine($"Avatar Asset URL:{__0.assetUrl}");
                    sb.AppendLine($"Avatar Image URL:{__0.imageUrl}");
                    sb.AppendLine($"Avatar Thumbnail Image URL:{__0.thumbnailImageUrl}");
                    sb.AppendLine($"Avatar Release Status:{__0.releaseStatus}");
                    sb.AppendLine($"Avatar Version:{__0.version}");
                    sb.AppendLine(Environment.NewLine);
                    File.AppendAllText(PublicAvatarFile, sb.ToString());
                    sb.Clear();
                    if (WebHookBoolBundle[1] && CanPost(__0.authorId))
                        AvatarToPost.Enqueue(__0);
                }
                else
                {
                    _avatarIDs += __0.id;
                    var sb = new StringBuilder();
                    sb.AppendLine($"Time detected:{DateTime.Now}");
                    sb.AppendLine($"Avatar ID:{__0.id}");
                    sb.AppendLine($"Avatar Name:{__0.name}");
                    sb.AppendLine($"Avatar Description:{__0.description}");
                    sb.AppendLine($"Avatar Author ID:{__0.authorId}");
                    sb.AppendLine($"Avatar Author Name:{__0.authorName}");
                    sb.AppendLine($"Avatar Asset URL:{__0.assetUrl}");
                    sb.AppendLine($"Avatar Image URL:{__0.imageUrl}");
                    sb.AppendLine($"Avatar Thumbnail Image URL:{__0.thumbnailImageUrl}");
                    sb.AppendLine($"Avatar Release Status:{__0.releaseStatus}");
                    sb.AppendLine($"Avatar Version:{__0.version}");
                    sb.AppendLine(Environment.NewLine);
                    File.AppendAllText(PrivateAvatarFile, sb.ToString());
                    sb.Clear();
                    if (WebHookBoolBundle[0] && CanPost(__0.authorId))
                        AvatarToPost.Enqueue(__0);
                }
            }

            return true;
        }

        private static bool CanPost(string id)
        {
            if (!Config.CanPostSelfAvatar && APIUser.CurrentUser.id.Equals(id))
                return false;
            if (Config.CanPostFriendsAvatar)
                return true;
            return !APIUser.CurrentUser.friendIDs.Contains(id);
        }

        private static System.Collections.IEnumerator DoCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                while (AvatarToPost.Count > 0)
                {
                    try
                    {
                        var avatar = AvatarToPost.Dequeue();
                        HttpWebRequest request = WebRequest.CreateHttp(avatar.releaseStatus == "public" ? Config.PublicWebhook : Config.PrivateWebhook);
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        Stream requestStream = request.GetRequestStream();
                        byte[] data = Encoding.UTF8.GetBytes($"{{\"content\":\"Avatar ID: {avatar.id}\\nAvatar Name: {avatar.name}\\nAvatar Description: {avatar.description}\\nAvatar Author ID: {avatar.authorId}\\nAvatar Author Name: {avatar.authorName}\\nAvatar Version: {avatar.version}\\nAvatar Release Status: {avatar.releaseStatus}\\nAvatar Asset URL: {avatar.assetUrl}\\nAvatar Image URL: {avatar.imageUrl}\\n\"}}");
                        requestStream.Write(data, 0, data.Length);
                        request.GetResponse();
                    }
                    catch (Exception ex) { MelonLogger.Error(ex.Message); }
                    yield return new WaitForSeconds(new System.Random().Next(1, 3));
                }
        }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}