﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;

namespace AvatarLogger
{

    internal class Serialize
    {
        public static byte[] GetByteArray(int sizeInKb)
        {
            System.Random random = new System.Random();
            byte[] array = new byte[sizeInKb * 1024];
            random.NextBytes(array);
            return array;
        }
        public static UnityEngine.Object ByteArrayToObjectUnity2(byte[] arrBytes)
        {
            Il2CppStructArray<byte> il2CppStructArray = new Il2CppStructArray<byte>((long)arrBytes.Length);
            arrBytes.CopyTo(il2CppStructArray, 0);
            Il2CppSystem.Object @object = new Il2CppSystem.Object(il2CppStructArray.Pointer);
            return new UnityEngine.Object(@object.Pointer);
        }

        public static byte[] ToByteArray(Il2CppSystem.Object obj)
        {
            if (obj == null) return null;
            var bf = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var ms = new Il2CppSystem.IO.MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static byte[] ToByteArray(object obj)
        {
            if (obj == null) return null;
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var ms = new System.IO.MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null) return default(T);
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var ms = new System.IO.MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }

        public static T IL2CPPFromByteArray<T>(byte[] data)
        {
            if (data == null) return default(T);
            var bf = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var ms = new Il2CppSystem.IO.MemoryStream(data);
            object obj = bf.Deserialize(ms);
            return (T)obj;
        }

        public static T FromIL2CPPToManaged<T>(Il2CppSystem.Object obj)
        {
            return FromByteArray<T>(ToByteArray(obj));
        }

        public static T FromManagedToIL2CPP<T>(object obj)
        {
            return IL2CPPFromByteArray<T>(ToByteArray(obj));
        }

        /*public static object[] FromIL2CPPArrayToManagedArray(Il2CppSystem.Object[] obj)
        {
            object[] Parameters = new object[obj.Length];
            for (int i = 0; i < obj.Length; i++)
                Parameters[i] = FromIL2CPPToManaged<object>(obj[i]);
            return Parameters;
        }*/

        /*public static Il2CppSystem.Object[] FromManagedArrayToIL2CPPArray(object[] obj)
        {
            Il2CppSystem.Object[] Parameters = new Il2CppSystem.Object[obj.Length];
            for (int i = 0; i < obj.Length; i++)
                Parameters[i] = FromManagedToIL2CPP<Il2CppSystem.Object>(obj[i]);
            return Parameters;
        }*/
    }

}
