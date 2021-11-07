using ExitGames.Client.Photon;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Reflection;
using VRC.Core;
using System.IO;
using System.Text;

[assembly: MelonGame("VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.AvatarLogger), "Avatar Logger", "Beta_0.7", "By LargestBoi & Yui")]
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
            MelonCoroutines.Start(OnNetworkManagerInit());
            base.OnApplicationStart();
        }
        //for logging on player joined
        internal static System.Collections.IEnumerator OnNetworkManagerInit()
        {
            while (NetworkManager.field_Internal_Static_NetworkManager_0 == null) yield return new UnityEngine.WaitForSecondsRealtime(2f);
            if (NetworkManager.field_Internal_Static_NetworkManager_0 != null) new Action(() =>
            {
                NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<VRC.Player>((obj) =>
                {
                    if (obj.field_Private_APIUser_0.id != APIUser.CurrentUser.id)
                    {
                        var ht = obj.field_Private_Player_0.field_Private_Hashtable_0;
                        dynamic playerHashtable = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(Serialize.FromIL2CPPToManaged<object>(ht)));
                        ExecuteLog(playerHashtable);
                    }
                }));
            })();
        }
        //for logging on avatar changed
        private static bool Detour(ref EventData __0)
        {
            try
            {
                if (__0.Code == 253)
                {
                    string customProps = JsonConvert.SerializeObject(Serialize.FromIL2CPPToManaged<object>(__0.Parameters));
                    dynamic playerHashtable = JsonConvert.DeserializeObject(customProps);
                    ExecuteLog(playerHashtable["251"]);
                }
            }
            catch { }
            return true;
        }
        //log method
        private static void ExecuteLog(dynamic playerHashtable)
        {
            string AvatarFile = "AvatarLog\\Log.txt";
            if (!File.Exists(AvatarFile))
            { File.AppendAllText(AvatarFile, "Mod By LargestBoi & Yui\n"); }
            string contents = File.ReadAllText(AvatarFile);
            if (!contents.Contains(playerHashtable["avatarDict"]["id"].ToString()))
            {

                File.AppendAllLines(AvatarFile, new string[]
                {
                        $"Time Detected:{((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString()}",
                        $"Avatar ID:{playerHashtable["avatarDict"]["id"]}",
                        $"Avatar Name:{playerHashtable["avatarDict"]["name"]}",
                        $"Avatar Description:{playerHashtable["avatarDict"]["description"]}",
                        $"Author ID:{playerHashtable["avatarDict"]["authorId"]}",
                        $"Author Name:{playerHashtable["avatarDict"]["authorName"]}",
                        $"PC Asset URL:{playerHashtable["avatarDict"]["unityPackages"][0]["assetUrl"]}",
                });
                try
                {
                    File.AppendAllText(AvatarFile, $"Quest Asset URL:{playerHashtable["avatarDict"]["unityPackages"][1]["assetUrl"]}\n");
                }
                catch
                {
                    File.AppendAllText(AvatarFile, $"Quest Asset URL:None\n");
                }
                File.AppendAllLines(AvatarFile, new string[]
                {
                        $"Image URL:{playerHashtable["avatarDict"]["imageUrl"]}",
                        $"Thumbnail URL:{playerHashtable["avatarDict"]["thumbnailImageUrl"]}",
                        $"Unity Version:{playerHashtable["avatarDict"]["unityPackages"][0]["unityVersion"]}",
                        $"Release Status:{playerHashtable["avatarDict"]["releaseStatus"]}",
                });
                if (playerHashtable["avatarDict"]["tags"].Count > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("Tags: ");
                    foreach (string tag in playerHashtable["avatarDict"]["tags"]) { builder.Append($"{tag},"); }
                    string tagsstr = builder.ToString().Remove(builder.ToString().LastIndexOf(","));
                    File.AppendAllText(AvatarFile, builder.ToString().Remove(builder.ToString().LastIndexOf(",")));
                }
                else { File.AppendAllText(AvatarFile, "Tags: None"); }
                MelonLogger.Msg($"Logged: {playerHashtable["avatarDict"]["name"]}|{playerHashtable["avatarDict"]["releaseStatus"]}");
                File.AppendAllText(AvatarFile, "\n\n");
            }
        }
    }
}