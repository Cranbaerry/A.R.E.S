using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReuploaderMod.Components
{
    public static class Extensions
    {
        public static Il2CppSystem.Collections.Generic.List<string> ConvertToIl2cpp(this System.Collections.Generic.List<string> originallist)
        {
            Il2CppSystem.Collections.Generic.List<string> il2cpplist = new Il2CppSystem.Collections.Generic.List<string>();
            foreach (string item in originallist)
            {
                il2cpplist.Add(item);
            }
            return il2cpplist;
        }
    }
}
