//Importing reqired modules
using ExitGames.Client.Photon;
using Harmony;
using MelonLoader;
using Newtonsoft.Json;
using Photon.Realtime;
using System;
using System.Reflection;
using VRC.Core;
using static BaseFuncs.BaseFuncs;
using static Logging.Logging;

//Contains all patches ARES makes to VRChat
namespace Patches
{
    internal static class Patches
    {
        //Creates new instance to patch on
        private static HarmonyLib.Harmony Instance = new HarmonyLib.Harmony("ARES");

        //Enables avatar cloning regadless of what the person has their clone setting on
        public static void AllowAvatarCopyingPatch()
        {
            Instance.Patch(typeof(APIUser).GetProperty(nameof(APIUser.allowAvatarCopying)).GetSetMethod(), new HarmonyLib.HarmonyMethod(typeof(Patches).GetMethod(nameof(Patches.ForceClone), BindingFlags.NonPublic | BindingFlags.Static)));
        }

        private static void ForceClone(ref bool __0) => __0 = true;

        //All the possible routes leading to an avatar being logged
        public static void OnEventPatch()
        {
            Instance.Patch(typeof(VRCNetworkingClient).GetMethod("OnEvent"), new HarmonyLib.HarmonyMethod(typeof(Patches).GetMethod(nameof(Patches.Detour), BindingFlags.NonPublic | BindingFlags.Static)), null, null, null, null);
            //Instance.Patch(typeof(LoadBalancingClient).GetMethod("OnEvent"), new HarmonyLib.HarmonyMethod(typeof(Patches).GetMethod(nameof(Patches.OnEventLBC), BindingFlags.Static | BindingFlags.NonPublic)), null, null, null, null);
        }

        private static bool OnEventLBC(EventData __0)
        {
            var eventCode = __0.Code;
            switch (eventCode)
            {
                case 1:
                case 3:
                    return true;

                case 4:
                    return true;

                case 6:
                    return true;

                case 7:
                    return true;

                case 9:
                    return true;

                case 33:
                    return true;

                case 209:
                    return true;

                case 210:
                    return true;

                case 253:
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
                        catch (Exception ex) { MelonLogger.Msg(ex.Message); }
                        return true;
                    };
                default:
                    break;
            }
            return true;
        }

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
    }
}