using System.IO;
using System.Reflection;
using UnityEngine;

namespace ReuploaderMod.Components
{
    public static class Paths
    {
        public static string ReuploaderModDataPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "ReuploaderModData");

        public static string ModsPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Mods");

        public static string AssemblyNamePath = Path.Combine(ModsPath, Assembly.GetExecutingAssembly().GetName().Name + ".dll");

        public static string AvatarLogPath = Path.Combine(ReuploaderModDataPath, "avatarlog.log");

        public static string TestAvatarLogPath = Path.Combine(ReuploaderModDataPath, "testavatarlog.log");
    }
}