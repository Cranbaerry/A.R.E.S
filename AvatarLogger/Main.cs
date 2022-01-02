//Inporting all refernces used within the mod
using System;
using System.Collections.Generic;
using MelonLoader;
using VRC.Core;
using VRC.UI.Core;
using Newtonsoft.Json;
using UnityEngine;
using ComfyUtils;
using static Logging.Logging;
using static BaseFuncs.BaseFuncs;
using static Patches.Patches;
using static Buttons.Buttons;
using System.Threading;
//Melon mod information
[assembly: MelonGame("VRChat")]
[assembly: MelonInfo(typeof(AvatarLogger.Main), "A.R.E.S Logger", "3.5", "By LargestBoi & Yui")]
[assembly: MelonColor(ConsoleColor.Yellow)]

namespace AvatarLogger
{
    public class Main : MelonMod
    {
        //Makes a dictionary to store all files that mey need to be updated to ensure consistent stability of ARES
        private Dictionary<string, string> UpkeepFiles = new Dictionary<string, string>();
        //Creates string that can retrieve the current instance ID
        public static string WorldInstanceID => $"{RoomManager.field_Internal_Static_ApiWorld_0.id}:{RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId}";
        //Function that allows the world to be joined via an instanc ID
        public static void JoinInstance(string worldID, string instanceID)
        => new PortalInternal().Method_Private_Void_String_String_PDM_0(worldID, instanceID);
        //Sets static counter values to monitor logging statistics
        public static int PC = 0;
        public static int Q = 0;
        public static int Pub = 0;
        public static int Pri = 0;
        private static ConfigHelper<Config> Helper;
        public static Config Config => Helper.Config;
        //Void to run on application start
        public override void OnApplicationStart()
        {
            Helper = new ConfigHelper<Config>($"{MelonUtils.UserDataDirectory}\\ARESConfig.json", true);
            //Runs basic setup for the MelonLoader/modding system as a whole
            new Thread(() => StartupPreperation()).Start();
            //Ensures reqired upkeep files are installed and updated
            UpkeepFiles.Add($"{MelonHandler.PluginsDirectory}\\ARESPlugin.dll", "https://github.com/LargestBoi/A.R.E.S/releases/latest/download/ARESPlugin.dll");
            HandleQueue(UpkeepFiles);
            try
            {
                MelonLogger.Msg("Applying patches...");
                AllowAvatarCopyingPatch();
                MelonLogger.Msg("Avatar cloning patched, force clone enabled!");
                OnEventPatch();
                MelonLogger.Msg("OnEvent patch applied (1/2)");
                MelonCoroutines.Start(OnNetworkManagerInit());
                MelonLogger.Msg("Network manager patched (2/2)");
                MelonLogger.Msg("Avatars can now be logged!");
            }
            catch
            {
                MelonLogger.Msg("Failed patches!");
            }
            //Starts lister to await Ui
            MelonCoroutines.Start(FindUI());
            MelonLogger.Msg("Listening for Ui...");
        }
        //Wait till scene loads to being getting updated friends list
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            //When scene loads fetch friends
            if (buildIndex == -1) { MelonCoroutines.Start(FetchFriends()); }
        }
        //On network manager run a command thatll log all users in a room
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
        //Locates the Ui in preperation for button creation
        private static System.Collections.IEnumerator FindUI()
        {
            while (UIManager.prop_UIManager_0 == null) yield return null;
            while (UnityEngine.Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
            while (GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)") == null) yield return null;
            //Once the Ui has launched get variables and begin Ui creation
            OnUiManagerInit();
        }
    }
}
