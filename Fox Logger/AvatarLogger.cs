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
[assembly: MelonInfo(typeof(AvatarLogger.AvatarLogger), "FoxLogger", "1.5", "By Nicky Blackburn")]
[assembly: MelonColor(System.ConsoleColor.Yellow)]
//Namespace containing all code within the mod
namespace AvatarLogger
{
    //Class containing all code relevant to the mod and its functions
    public class AvatarLogger : MelonMod
    {
        //Making strings to contain logging settings and allowences
        public static string LFAV = "False";
        public static string LOAV = "False";
        //Make string to contain friend avatars
        public static string FriendIDs = null;
        //Sets static counter values to monitor logging statistics
        public static int PC = 0;
        public static int Q = 0;
        public static int Pub = 0;
        public static int Pri = 0;
        //Void to run on application start
        public override void OnApplicationStart()
        {
            //Create a melon loader settings category
            var category = MelonPreferences.CreateCategory("Foxlogger", "Foxlogger");
            //Create values to be in settings
            var CS = category.CreateEntry("CS", "", is_hidden: true);
            var LFA = category.CreateEntry("LogFriendsAvatars", "", is_hidden: true);
            var LOA = category.CreateEntry("LogOwnAvatars", "", is_hidden: true);
            //Read and report values shown
            var CSV = CS.Value;
            //If CS (CleanStart) is empty begin first time setup
            if (CSV.Length != 1)
            {
                //Set CS to one to not have this occur from this point onwards
                CS.Value = "1";
                //Disable self logging and friend logging by default
                LFA.Value = "True";
                LOA.Value = "True";
                //Saves current state of the settings
                category.SaveToFile(true);
                //Displays info pane about the settings and how they can be changed
                MelonLogger.Msg("Default settings created!\nBy default avatars uploaded by you\nor your friends will not be logged!\nWant to change these settings? Then\nQuit the game and goto '/VRChat/UserData/MelonPreferences.cfg'\nHere you can change your logging settings!\nSide note: Setting CS to empty will reset everything\nto default settings on next boot!");
            }
            else 
                LFAV = LFA.Value;
                MelonLogger.Msg($"LogFriendsAvatars:{LFAV}");
                LOAV = LOA.Value;
                MelonLogger.Msg($"LogOwnAvatars:{LOAV}");
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
        //Wait till scene loads
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            //When scene loads fetch friends
            if (buildIndex == -1) { MelonCoroutines.Start(FetchFriends()); }
        }
        private static System.Collections.IEnumerator FetchFriends()
        {
            //Wait till world loads
            while(RoomManager.field_Internal_Static_ApiWorld_0 == null)
            {
                yield return null;
            }
            //Get friend IDs to array
            string[] pals = APIUser.CurrentUser.friendIDs.ToArray();
            //For every ID add ID to string
            foreach(string pal in pals) { FriendIDs+=$"{pal},"; }
        }
        //Method responsible for extracting data from a hast table and logging particular variables
        private static void ExecuteLog(dynamic playerHashtable)
        {
            
            
            //Locate the log file
            string AvatarFile = "FoxData\\Avatars.txt";
            //If the log file does not exist create it and append the credits of the mod
            if (!File.Exists(AvatarFile))
            { File.AppendAllText(AvatarFile, "Mod By Nicky Blackburn\n"); }
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
                    //If a pc asset URL is logged add a value to the counter
                    PC = PC + 1;
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
                    //If a quest asset URL is logged add a a value to the counter
                    Q = Q + 1;
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
                //Adjust counter values to whatever the avatrs relese status is
                string rs = playerHashtable["avatarDict"]["releaseStatus"].ToString();
                if (rs == "public") { Pub = Pub + 1; };
                if (rs == "private") { Pri = Pri + 1; };
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
                //Displays user statistics on every 10 logs
                int total = Pub + Pri;
                bool isMultiple = total % 10 == 0;
                if (isMultiple) { MelonLogger.Msg("Session Statistics:"); }
                if (isMultiple) { MelonLogger.Msg($"Total Logged:{total}|PC:{PC}|Quest:{Q}|Publics:{Pub}|Privates:{Pri}"); }
                File.AppendAllText(AvatarFile, "\n\n");
            }
        }
    }
}