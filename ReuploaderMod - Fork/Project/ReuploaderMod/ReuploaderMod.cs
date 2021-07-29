using BestHTTP;
using Harmony;
using MelonLoader;
using ReuploaderMod.Components;
using System.Reflection;
using System.Runtime.InteropServices;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace ReuploaderMod
{
    public class ReuploaderMod : MelonMod
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OpenMenuDelegate(System.IntPtr self, bool @bool);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate System.IntPtr GetTimeoutDelegate(System.IntPtr self);

        public static ReuploaderMod Instance;

        private GameObject ReuploaderComponentHolder;

        public ReuploaderButtons ReuploaderScript;

        public object ReuploaderModLoaderInstance;

        public MethodInfo ReuploaderModSendNameRequestMethod;

        public override void OnApplicationStart()
        {
            ClassInjector.RegisterTypeInIl2Cpp<ReuploaderButtons>();
            ClassInjector.RegisterTypeInIl2Cpp<ApiFileUtils>();
            ClassInjector.RegisterTypeInIl2Cpp<ApiWorldReuploader>();

            Instance = this;
        }

        public override void OnLevelWasInitialized(int level)
        {
            if ((uint)level > 1u)
            {
                RegisterComponentHolder();
            }
        }


        private void RegisterComponentHolder()
        {
            if (!ReuploaderComponentHolder)
            {
                ReuploaderComponentHolder = new GameObject("Reuploader");
                UnityEngine.Object.DontDestroyOnLoad(ReuploaderComponentHolder);
                ReuploaderComponentHolder.AddComponent<ApiWorldReuploader>();
                ReuploaderScript = ReuploaderComponentHolder.AddComponent<ReuploaderButtons>();
            }
        }

        private void patches()
        {
            HarmonyInstance.Create("Nigga").Patch(AccessTools.Property(typeof(HTTPRequest), "Timeout").GetSetMethod(), new HarmonyMethod(AccessTools.Method(typeof(ReuploaderMod), "SetTimeoutPatch")));
        }

        private static bool SetTimeoutPatch(HTTPRequest __instance, ref Il2CppSystem.TimeSpan __0)
        {
            __0._ticks = Il2CppSystem.TimeSpan.FromMinutes(30.0).Ticks;
            return true;
        }
    }
}