using Il2CppSystem.Collections;
using MelonLoader;
using System;
using System.Linq;
using System.Reflection;
using VRC;

namespace ReuploaderMod
{
    public static class PlayerExtensions
    {
        private sealed class Class32
        {

		internal bool method_0(PropertyInfo propertyInfo_0)
            {
                if (propertyInfo_0.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Count((PropertyInfo propertyInfo_0) => propertyInfo_0.PropertyType == typeof(Hashtable)) == 1)
                {
                    return propertyInfo_0.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Count((PropertyInfo propertyInfo_0) => propertyInfo_0.PropertyType == typeof(int)) == 2;
                }
                return false;
            }

            internal bool method_1(PropertyInfo propertyInfo_0)
            {
                return propertyInfo_0.PropertyType == typeof(Hashtable);
            }

            internal bool method_2(PropertyInfo propertyInfo_0)
            {
                return propertyInfo_0.PropertyType == typeof(int);
            }
        }

        private static PropertyInfo propertyInfo_0;

        private static MethodInfo methodInfo_0;

        static PlayerExtensions()
        {
            try
            {
                propertyInfo_0 = typeof(Player).GetProperties(BindingFlags.Instance | BindingFlags.Public).First((PropertyInfo propertyInfo_0) => propertyInfo_0.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Count((PropertyInfo propertyInfo_0) => propertyInfo_0.PropertyType == typeof(Hashtable)) == 1 && propertyInfo_0.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Count((PropertyInfo propertyInfo_0) => propertyInfo_0.PropertyType == typeof(int)) == 2);
                if (propertyInfo_0 != null)
                {
                    methodInfo_0 = propertyInfo_0.GetGetMethod();
                }
            }
            catch (Exception ex)
            {
                MelonLogger.LogError(ex.ToString() + "PLAYER PHOTONPLAYER");
            }
        }

        public static object GetPhotonPlayer(this Player player)
        {
            return methodInfo_0.Invoke(player, null);
        }




    }
}