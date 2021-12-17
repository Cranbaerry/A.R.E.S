﻿//Inporting all refernces used within the mod
using ExitGames.Client.Photon;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Reflection;
using VRC.Core;
using VRC.UI.Core;
using System.IO;
using System.Text;
using UnityEngine;
using ARES.Utils.API.QM;
using ARES.Utils.API;
using LoadSprite;
using System.Windows.Forms;
//Declaring the assembly/melon mod information
[assembly: MelonGame("VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.AvatarLogger), "A.R.E.S Logger", "2.2", "By LargestBoi & Yui")]
[assembly: MelonColor(System.ConsoleColor.Yellow)]
//Namespace containing all code within the mod
namespace AvatarLogger
{
    //Class containing all code relevant to the mod and its functions
    public class AvatarLogger : MelonMod
    {
        //Creates varaible for buttons
        internal static Sprite ButtonImage = null;
        internal static MenuPage mainPage;
        public static string WorldInstanceID => $"{RoomManager.field_Internal_Static_ApiWorld_0.id}:{RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId}";
        //Making strings to contain logging settings and allowences
        public static string LFAV = "False";
        public static string LOAV = "False";
        public static string LTCV = "True";
        public static string CEV = "False";
        public static bool LFAVB = false;
        public static bool LOAVB = false;
        public static bool LTCVB = false;
        public static bool CEVB = false;
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
            var category = MelonPreferences.CreateCategory("ARES", "ARES");
            //Create values to be in settings
            var CS = category.CreateEntry("CS", "", is_hidden: true);
            var LFA = category.CreateEntry("LogFriendsAvatars", "", is_hidden: true);
            var LOA = category.CreateEntry("LogOwnAvatars", "", is_hidden: true);
            var LTC = category.CreateEntry("LogToConsole", "", is_hidden: true);
            var CE = category.CreateEntry("ConsoleError", "", is_hidden: true);
            //Read and report values shown
            var CSV = CS.Value;
            //If CS (CleanStart) is empty begin first time setup
            if (CSV.Length != 1)
            {
                //Set CS to one to not have this occur from this point onwards
                CS.Value = "1";
                //Disable self logging and friend logging by default
                LFA.Value = "False";
                LFAVB = false;
                LOA.Value = "False";
                LOAVB = false;
                LTC.Value = "True";
                LTCVB = true;
                CE.Value = "False";
                CEVB = false;
                MelonLogger.Msg($"LogFriendsAvatars:{LFAV}");
                MelonLogger.Msg($"LogOwnAvatars:{LOAV}");
                MelonLogger.Msg($"LogToConsole:{LTCV}");
                MelonLogger.Msg($"LogErrorToConsole:{CEV}");
                //Saves current state of the settings
                category.SaveToFile(true);
                //Displays info pane about the settings and how they can be changed
                MelonLogger.Msg("Default settings created!");
            }
            //Loads values into strings and bools, then reporting them to the user
            else
            {
                LFAV = LFA.Value;
                if (LFAV == "True") { LFAVB = true; }
                if (LFAV == "False") { LFAVB = false; }
                MelonLogger.Msg($"LogFriendsAvatars:{LFAV}");
                LOAV = LOA.Value;
                if (LOAV == "True") { LOAVB = true; }
                if (LOAV == "False") { LOAVB = false; }
                MelonLogger.Msg($"LogOwnAvatars:{LOAV}");
                LTCV = LTC.Value;
                if (LTCV == "True") { LTCVB = true; }
                if (LTCV == "False") { LTCVB = false; }
                MelonLogger.Msg($"LogToConsole:{LTCV}");
                CEV = CE.Value;
                if (CEV == "True") { CEVB = true; }
                if (CEV == "False") { CEVB = false; }
                MelonLogger.Msg($"LogErrorToConsole:{CEV}");
            }
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
            ButtonImage = (Environment.CurrentDirectory + "\\GUI\\ARESLogo.png").LoadSpriteFromDisk();
            MelonCoroutines.Start(FindUI());
            base.OnApplicationStart();
        }
        //Code to force join a world provided an instance and world ID
        public static void JoinInstance(string worldID, string instanceID)
        => new PortalInternal().Method_Private_Void_String_String_PDM_0(worldID, instanceID);

        //Button API code waiting for the Ui to launch
        private static System.Collections.IEnumerator FindUI()
        {
            while (UIManager.prop_UIManager_0 == null) yield return null;
            while (UnityEngine.Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
            while (GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)") == null) yield return null;
            //Once the Ui has launched get variables and begin Ui creation
            APIStuff.Left.Setup(GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Left").transform);
            APIStuff.Right.Setup(GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Right").transform);
            APIStuff.Left.WingOpen.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(new Action(() => APIStuff.Init_L()));
            APIStuff.Right.WingOpen.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(new Action(() => APIStuff.Init_R()));
            OnUiManagerInit();
        }
        //Creation of buttons within the Ui
        private static void OnUiManagerInit()
        {
            MelonLogger.Msg("Ui initiating...");
            mainPage = new MenuPage("ARES_MainMenu", "ARES", true, false, false, delegate { }, "Access ARES settings!", ButtonImage);
            new Tab(APIStuff.GetMenuTabTemplate().transform.parent, "ARES_MainMenu", "ARES Logging Mod", ButtonImage);
            var ToggleGroup = new ButtonGroup(mainPage.menuContents, "Toggles");
            new ToggleButton(ToggleGroup, "Log Own Avatars", delegate { LOAC(true); }, delegate { LOAC(false); }, "Toggles the ability to log avatars uploaded to your account!", LOAVB);
            new ToggleButton(ToggleGroup, "Log Friends Avatars", delegate { LFAC(true); }, delegate { LFAC(false); }, "Toggles the ability to log avatars uploaded to your friends accounts!", LFAVB);
            new ToggleButton(ToggleGroup, "Log To Console", delegate { LTCC(true); }, delegate { LTCC(false); }, "Toggles the ability display logged avatars in console!", LTCVB);
            new ToggleButton(ToggleGroup, "Log Errors To Console", delegate { LTCC(true); }, delegate { LTCC(false); }, "Toggles the ability display why particular avatars aren't logged in console!", CEVB);
            var InstanceGroup = new ButtonGroup(mainPage.menuContents, "Instance Management");
            new SimpleSingleButton(InstanceGroup, "Copy Current Instance ID", delegate { Clipboard.SetText(WorldInstanceID); }, "Copies the current instance ID to your clipboard!");
            new SimpleSingleButton(InstanceGroup, "Join By Clipboard Instance", JoinInstanceByID, "Joins the instance currently within your clipboard!");
            MelonLogger.Msg("Ui ready!");
        }
        //Functions that run when a toggle is set or a button is pressed causing the settings to be changed, saved and take effect!
        private static void LOAC(bool value)
        {
            if (value == true)
            {
                MelonPreferences.SetEntryValue("ARES", "LogOwnAvatars", "True");
                LOAVB = true;
                MelonLogger.Msg("Logging of own avatars enabled!");
            }
            if (value == false)
            {
                MelonPreferences.SetEntryValue("ARES", "LogOwnAvatars", "False");
                LOAVB = false;
                MelonLogger.Msg("Logging of own avatars disabled!");
            }
            MelonPreferences.Save();
        }
        private static void LFAC(bool value)
        {
            if (value == true)
            {
                MelonPreferences.SetEntryValue("ARES", "LogFriendsAvatars", "True");
                LFAVB = true;
                MelonLogger.Msg("Logging of friends avatars enabled!");
            }
            if (value == false)
            {
                MelonPreferences.SetEntryValue("ARES", "LogFriendsAvatars", "False");
                LFAVB = false;
                MelonLogger.Msg("Logging of friends avatars disabled!");
            }
            MelonPreferences.Save();
        }
        private static void LTCC(bool value)
        {
            if (value == true)
            {
                MelonPreferences.SetEntryValue("ARES", "LogToConsole", "True");
                LTCVB = true;
                MelonLogger.Msg("Logging to console enabled!");
            }
            if (value == false)
            {
                MelonPreferences.SetEntryValue("ARES", "LogToConsole", "False");
                LTCVB = false;
                MelonLogger.Msg("Logging to console disabled!");
            }
            MelonPreferences.Save();
        }
        private static void CEC(bool value)
        {
            if (value == true)
            {
                MelonPreferences.SetEntryValue("ARES", "ConsoleError", "True");
                CEVB = true;
                MelonLogger.Msg("Log errors to console enabled!");
            }
            if (value == false)
            {
                MelonPreferences.SetEntryValue("ARES", "ConsoleError", "False");
                CEVB = false;
                MelonLogger.Msg("Log errors to console disabled!");
            }
            MelonPreferences.Save();
        }
        //Takes the insatnce ID attached to the clipboard and joins the world!
        private static void JoinInstanceByID()
        {
            string[] ID = Clipboard.GetText().Split(':');
            if (Clipboard.GetText().Contains("wrld"))
            {
                JoinInstance(ID[0], ID[1]);
                MelonLogger.Msg($"Instance joined: {Clipboard.GetText()}");
            }
            else
            {
                MelonLogger.Msg($"Invalid instance ID!");
            }
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
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null) { yield return null; }
            //Get friend IDs to array
            string[] pals = APIUser.CurrentUser.friendIDs.ToArray();
            //For every ID add ID to string
            foreach (string pal in pals) { FriendIDs += $"{pal},"; }
        }
        //Method responsible for extracting data from a hast table and logging particular variables
        private static void ExecuteLog(dynamic playerHashtable)
        {
            //If logging of friends avatars is disabled
            if (!LFAVB)
            {
                //Check if the avatar about to be logged is uploaded by a friend
                if (FriendIDs.Contains(playerHashtable["avatarDict"]["authorId"].ToString()))
                {
                    //If the user is a friend inform the user the log has not occurred and why so
                    if (CEVB) { MelonLogger.Msg($"{playerHashtable["avatarDict"]["authorName"].ToString()}'s avatar {playerHashtable["avatarDict"]["name"].ToString()} was not logged, they are a friend!"); }
                    return;
                }
            }
            //If logging own avatars is disabled
            if (!LOAVB)
            {
                //Check if the avatar about to be uploaded belongs to the user and was uploaded from their account
                if (APIUser.CurrentUser.id == playerHashtable["avatarDict"]["authorId"].ToString())
                {
                    //If the avatar was uploaded by the user inform them the avatr was not logged and why it was not logged
                    if (CEVB) { MelonLogger.Msg($"Your avatar {playerHashtable["avatarDict"]["name"].ToString()} was not logged, you have log own avatars disabled!"); }
                    return;
                }
            }
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
                //New optimised Quest/PC asset URL logging 
                string pcasset = "None";
                string qasset = "None";
                foreach (dynamic unitypackage in playerHashtable["avatarDict"]["unityPackages"])
                {
                    try
                    {
                        switch (unitypackage["platform"].ToString())
                        {
                            //Checks for avi version and logs accordingly for Quest and PC
                            case "standalonewindows":
                                if (pcasset != "None")
                                {
                                    pcasset = unitypackage["assetUrl"].ToString();
                                    PC = +1;
                                }
                                break;
                            case "android":
                                if (qasset != "None")
                                {
                                    qasset = unitypackage["assetUrl"].ToString();
                                    Q = +1;
                                }
                                break;
                        }
                    }
                    catch { }
                }
                File.AppendAllLines(AvatarFile, new string[]
                {
                    $"PC Asset URL:{pcasset}",
                    $"Quest Asset URL:{qasset}",
                    $"Image URL:{playerHashtable["avatarDict"]["imageUrl"]}",
                    $"Thumbnail URL:{playerHashtable["avatarDict"]["thumbnailImageUrl"]}",
                    $"Unity Version:{playerHashtable["avatarDict"]["unityPackages"][0]["unityVersion"]}",
                    $"Release Status:{playerHashtable["avatarDict"]["releaseStatus"]}",
                });
                //Adjust counter values to whatever the avatrs relese status is
                string rs = playerHashtable["avatarDict"]["releaseStatus"].ToString();
                if (rs == "public") { Pub =+1; };
                if (rs == "private") { Pri =+1; };
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
                //Update in-game session statistic menu!
                int totallogged = Pub + Pri;
                float privatepercentage = 0;
                if (Pri > 0)
                {
                    if (totallogged>0)
                    {
                        privatepercentage = ((float)Pri / totallogged) * 100;
                    }
                }
                //Inform the user of the successful log
                if (LTCVB) { MelonLogger.Msg($"Logged: {playerHashtable["avatarDict"]["name"]}|{playerHashtable["avatarDict"]["releaseStatus"]}!"); }
                File.AppendAllText(AvatarFile, "\n\n");
            }
        }
    }
}