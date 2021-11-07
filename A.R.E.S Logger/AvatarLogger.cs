using ExitGames.Client.Photon;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnhollowerBaseLib;
using VRC.Core;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[assembly: MelonGame("VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.AvatarLogger), "Avatar Logger", "Beta_0.5", "By LargestBoi & Yui")]
[assembly: MelonColor(System.ConsoleColor.Yellow)]

namespace AvatarLogger
{
    public class AvatarLogger : MelonMod
    {
        public override void OnApplicationStart()
        {
            try
            {
                MelonLogger.Msg("Note: This is a beta version in heavy development!\nDon't use this as a main mod version\nit will only lead to your pain!");
                HarmonyInstance.Patch(
                typeof(VRCNetworkingClient)
                .GetMethod("OnEvent")
                , new HarmonyLib.HarmonyMethod(typeof(AvatarLogger)
                .GetMethod(nameof(AvatarLogger.Detour), BindingFlags.NonPublic | BindingFlags.Static)), null, null, null, null);
                MelonLogger.Msg("Hook Successful :)");
            }
            catch
            {
                MelonLogger.Msg("Hook failed :(");
            }
            base.OnApplicationStart();
        }
        private static bool Detour(ref EventData __0)
        {
            try
            {
                string AvatarFile = "AvatarLog\\Log.txt";
                if (!File.Exists(AvatarFile))
                { File.AppendAllText(AvatarFile, "Mod By LargestBoi & Yui\n"); }
                if (__0.Code == 253)
                {
                    string customProps = JsonConvert.SerializeObject(Serialize.FromIL2CPPToManaged<object>(__0.Parameters));
                    ///MelonLogger.Msg(customProps);
                    dynamic playerHashtable = JsonConvert.DeserializeObject(customProps);
                    string contents = File.ReadAllText(AvatarFile);

                    if (!contents.Contains(playerHashtable["251"]["avatarDict"]["id"].ToString()))
                    {

                        File.AppendAllLines(AvatarFile, new string[]
                        {
                        $"Time Detected:{((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString()}",
                        $"Avatar ID:{playerHashtable["251"]["avatarDict"]["id"]}",
                        $"Avatar Name:{playerHashtable["251"]["avatarDict"]["name"]}",
                        $"Avatar Description:{playerHashtable["251"]["avatarDict"]["description"]}",
                        $"Author ID:{playerHashtable["251"]["avatarDict"]["authorId"]}",
                        $"Author Name:{playerHashtable["251"]["avatarDict"]["authorName"]}",
                        $"PC Asset URL:{playerHashtable["251"]["avatarDict"]["unityPackages"][0]["assetUrl"]}",
                        });
                        try
                        {
                            string quest = playerHashtable["251"]["avatarDict"]["unityPackages"][1]["assetUrl"];
                            File.AppendAllText(AvatarFile, $"Quest Asset URL:{quest}\n");
                        }
                        catch
                        {
                            string quest = "None";
                            File.AppendAllText(AvatarFile, $"Quest Asset URL:{quest}\n");
                        }
                        File.AppendAllLines(AvatarFile, new string[]
                        {
                        $"Image URL:{playerHashtable["251"]["avatarDict"]["imageUrl"]}",
                        $"Thumbnail URL:{playerHashtable["251"]["avatarDict"]["thumbnailImageUrl"]}",
                        $"Release Status:{playerHashtable["251"]["avatarDict"]["releaseStatus"]}",
                        });
                        if (playerHashtable["251"]["avatarDict"]["tags"].Count > 0)
                        {
                            StringBuilder builder = new StringBuilder();
                            builder.Append("Tags: ");
                            foreach (string tag in playerHashtable["251"]["avatarDict"]["tags"]) { builder.Append($"{tag},"); }
                            string tagsstr = builder.ToString().Remove(builder.ToString().LastIndexOf(","));
                            File.AppendAllText(AvatarFile, builder.ToString().Remove(builder.ToString().LastIndexOf(",")));
                        }
                        else { File.AppendAllText(AvatarFile, "Tags: None"); }
                        MelonLogger.Msg($"Logged: {playerHashtable["251"]["avatarDict"]["name"]}|{playerHashtable["251"]["avatarDict"]["releaseStatus"]}");
                        File.AppendAllText(AvatarFile, "\n\n");
                    }
                }

            }
            catch
            {

            }
            return true;
        }
    }
}