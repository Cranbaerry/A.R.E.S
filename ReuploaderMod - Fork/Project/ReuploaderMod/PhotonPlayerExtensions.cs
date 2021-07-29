using Il2CppSystem.Collections;
using MelonLoader;
using System;
using System.Linq;
using System.Reflection;
using VRC;

namespace ReuploaderMod
{
    public static class PhotonPlayerExtensions
    {
        private sealed class Class33
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

            internal bool method_3(PropertyInfo propertyInfo_0)
            {
                if (propertyInfo_0.PropertyType == typeof(int))
                {
                    return !propertyInfo_0.Name.ToLower().Contains("private");
                }
                return false;
            }
        }

        private static Type type_0;

        private static PropertyInfo propertyInfo_0;

        private static MethodInfo methodInfo_0;

        static PhotonPlayerExtensions()
        {
            try
            {
                type_0 = typeof(Player).GetProperties(BindingFlags.Instance | BindingFlags.Public).First((PropertyInfo propertyInfo_0) => propertyInfo_0.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Count((PropertyInfo propertyInfo_0) => propertyInfo_0.PropertyType == typeof(Hashtable)) == 1 && propertyInfo_0.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Count((PropertyInfo propertyInfo_0) => propertyInfo_0.PropertyType == typeof(int)) == 2).PropertyType;
                if (type_0 != null)
                {
                    propertyInfo_0 = type_0.GetProperties(BindingFlags.Instance | BindingFlags.Public).First((PropertyInfo propertyInfo_0) => propertyInfo_0.PropertyType == typeof(int) && !propertyInfo_0.Name.ToLower().Contains("private"));
                    if (propertyInfo_0 != null)
                    {
                        methodInfo_0 = propertyInfo_0.GetGetMethod();
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.LogError(ex.ToString() + "PHOTONPLAYER");
            }
        }

        public static int GetPhotonId(this object photonPlayer)
        {
            return (int)methodInfo_0.Invoke(photonPlayer, null);
        }


  


    }
}