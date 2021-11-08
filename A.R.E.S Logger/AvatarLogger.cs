//Inporting all refernces used within the mod
using ExitGames.Client.Photon;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Reflection;
using VRC.Core;
using System.IO;
using System.Text;
//Declaring the assembly/melon mod information
[assembly: MelonGame("VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.AvatarLogger), "A.R.E.S Logger", "V1", "By LargestBoi & Yui")]
[assembly: MelonColor(System.ConsoleColor.Yellow)]
//Namespace containing all code within the mod
namespace AvatarLogger
{
    //Class containing all code relevant to the mod and its functions
    public class AvatarLogger : MelonMod
    {
        //Void to run on application start
        public override void OnApplicationStart()
        {
            //Informs user that this is a beta build and not an offically supported release 
            MelonLogger.Msg("Note: This is a beta version in heavy development!\nDon't use this as a main mod version\nit will only lead to your pain!");
            try
            {
                //Attempts to use harmony to patch/hook into the VRChat networking client
                HarmonyInstance.Patch(
                typeof(VRCNetworkingClient)
                .GetMethod("OnEvent")
                , new HarmonyLib.HarmonyMethod(typeof(AvatarLogger)
                .GetMethod(nameof(AvatarLogger.Detour), BindingFlags.NonPublic | BindingFlags.Static)), null, null, null, null);
                //Informs the end user if the patch was successfully engaged
                MelonLogger.Msg("Hook Successful :)");
            }
            catch
            {
                //If for whatever reason the hook/patch had failed inform the user of the error
                MelonLogger.Msg("Hook failed :(");
            }
            //Begins attachment to network manager
            MelonCoroutines.Start(OnNetworkManagerInit());
            base.OnApplicationStart();
        }
        //Logs whenever a player joins
        internal static System.Collections.IEnumerator OnNetworkManagerInit()
        {
            //Obtains the player hash table and sends it to the method responsible for logging information from the table
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
        //Logs when an avatar is changed
        private static bool Detour(ref EventData __0)
        {
            try
            {
                //If an avatar is changed obtian the new hash table and parse it in to the method responsible for logging information from the table
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
        //Method responsible for extracting data from a hast table and logging particular variables
        private static void ExecuteLog(dynamic playerHashtable)
        {
            //Locate the log file
            string AvatarFile = "GUI\\Log.txt";
            //If the log file does not exist create it and append the credits of the mod
            if (!File.Exists(AvatarFile))
            { File.AppendAllText(AvatarFile, "Mod By LargestBoi & Yui\n"); }
            //Read the entire contents of the log file
            string contents = File.ReadAllText(AvatarFile);
            //If the hash table passed into the method contains a new avatar ID that is not already present within the log file
            if (!contents.Contains(playerHashtable["avatarDict"]["id"].ToString()))
            {
                //Log the following variables to the log file
                File.AppendAllLines(AvatarFile, new string[]
                {
                    //Obtains the cuttent system date/time in unix and logs it as the time the avatar was detected
                    $"Time Detected:{((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString()}",
                    //Continues to extract more data from the hash table and write it to the log file such as:
                    //Avatar ID, Name, Description, Author ID, Author Name and the PC Asset URL
                    $"Avatar ID:{playerHashtable["avatarDict"]["id"]}",
                    $"Avatar Name:{playerHashtable["avatarDict"]["name"]}",
                    $"Avatar Description:{playerHashtable["avatarDict"]["description"]}",
                    $"Author ID:{playerHashtable["avatarDict"]["authorId"]}",
                    $"Author Name:{playerHashtable["avatarDict"]["authorName"]}",
                });
                //If an avatar is not pc compatable the value on the hash table does not exist, if it an attempt to pull this information is made it will fail,
                //We used a try/catch statement to detect the existance of an asset url
                try
                {
                    //Will attempt to write the pc url to the log file, if it is present this will complete without error, logging the pc asset url
                    File.AppendAllText(AvatarFile, $"PC Asset URL:{playerHashtable["avatarDict"]["unityPackages"][0]["assetUrl"]}\n");
                }
                catch
                {
                    //If it fails to retreive the pc asset url the default text "PC Asset URL:None" is written into the log and a new line is formed "\n"
                    File.AppendAllText(AvatarFile, $"PC Asset URL:None\n");
                }
                //If an avatar is not quest compatable the value on the hash table does not exist, if it an attempt to pull this information is made it will fail,
                //We used a try/catch statement to detect the existance of an asset url
                try
                {
                    //Will attempt to write the quest url to the log file, if it is present this will complete without error, logging the quest asset url
                    File.AppendAllText(AvatarFile, $"Quest Asset URL:{playerHashtable["avatarDict"]["unityPackages"][1]["assetUrl"]}\n");
                }
                catch
                {
                    //If it fails to retreive the quest asset url the default text "Quest Asset URL:None" is written into the log and a new line is formed "\n"
                    File.AppendAllText(AvatarFile, $"Quest Asset URL:None\n");
                }
                File.AppendAllLines(AvatarFile, new string[]
                {
                    //Continues to extract more data from the hash table and write it to the log file such as:
                    //Image URL, Thumbnail URL, Unity Version and the Release Status of the avatar (Public or Private)
                    $"Image URL:{playerHashtable["avatarDict"]["imageUrl"]}",
                    $"Thumbnail URL:{playerHashtable["avatarDict"]["thumbnailImageUrl"]}",
                    $"Unity Version:{playerHashtable["avatarDict"]["unityPackages"][0]["unityVersion"]}",
                    $"Release Status:{playerHashtable["avatarDict"]["releaseStatus"]}",
                });
                //The last variables extracted are the tags of the avatar, these are added by the avatar uploader or by VRChat administrators/developers,
                //they are initally stored as an array, if no tags are set the if statemnt will just continue with its else
                if (playerHashtable["avatarDict"]["tags"].Count > 0)
                {
                    //Prepares to create a string from the array of tags
                    StringBuilder builder = new StringBuilder();
                    //Adds the text "Tags: " to the string being created as an identifer
                    builder.Append("Tags: ");
                    //For every value in the tags array add it to the string being created
                    foreach (string tag in playerHashtable["avatarDict"]["tags"]) { builder.Append($"{tag},"); }
                    //Write the final created string into the log file containing all extracted and sorted tags
                    File.AppendAllText(AvatarFile, builder.ToString().Remove(builder.ToString().LastIndexOf(",")));
                }
                //If there are no tags present the default text "Tags: None" is written into the log file
                else { File.AppendAllText(AvatarFile, "Tags: None"); }
                //Inform the user of the successful log
                MelonLogger.Msg($"Logged: {playerHashtable["avatarDict"]["name"]}|{playerHashtable["avatarDict"]["releaseStatus"]}");
                //Append two new lines to the log file in preperation for another entry
                File.AppendAllText(AvatarFile, "\n\n");
            }
        }
    }
}