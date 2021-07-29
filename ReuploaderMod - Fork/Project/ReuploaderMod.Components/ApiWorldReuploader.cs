using Il2CppSystem.Collections.Generic;
using MelonLoader;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC.Core;

namespace ReuploaderMod.Components
{
    public class ApiWorldReuploader : MonoBehaviour
    {
        public Delegate ReferencedDelegate;

        public IntPtr MethodInfo;

        public List<MonoBehaviour> AntiGcList;

        public static bool bool_0;

        public ApiWorldReuploader(IntPtr intptr_1)
            : base(intptr_1)
        {
            AntiGcList = new List<MonoBehaviour>(1);
            AntiGcList.Add(this);
        }

        public ApiWorldReuploader(Delegate delegate_1, IntPtr intptr_1)
            : base(ClassInjector.DerivedConstructorPointer<ApiWorldReuploader>())
        {
            ClassInjector.DerivedConstructorBody(this);
            ReferencedDelegate = delegate_1;
            MethodInfo = intptr_1;
        }

        ~ApiWorldReuploader()
        {
            Marshal.FreeHGlobal(MethodInfo);
            MethodInfo = IntPtr.Zero;
            ReferencedDelegate = null;
            AntiGcList.Remove(this);
            AntiGcList = null;
        }

        private void Start()
        {
        }

        private static IEnumerator smethod_0()
        {
            ApiWorld.FetchList((Action<IEnumerable<ApiWorld>>)delegate (IEnumerable<ApiWorld> ienumerable_0)
            {
                try
                {
                    List<ApiWorld> list = ienumerable_0.Cast<List<ApiWorld>>();
                    if (list != null)
                    {
                        List<ApiWorld>.Enumerator enumerator = list.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            MelonLogger.Log(enumerator.Current.id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.LogError(ex.ToString());
                }
            }, (Action<string>)delegate (string string_0)
            {
                MelonLogger.LogError(string_0);
            }, ApiWorld.SortHeading.Order, ApiWorld.SortOwnership.Any, ApiWorld.SortOrder.Descending, 0, 50, string.Empty, null, null, null, null, ApiWorld.ReleaseStatus.Public, null, null, disableCache: true);
            yield return null;
        }
    }
}