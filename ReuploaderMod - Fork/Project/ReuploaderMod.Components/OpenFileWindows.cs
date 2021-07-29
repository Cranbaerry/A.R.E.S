using ReuploaderMod.Utilities;
using System.Runtime.InteropServices;

namespace ReuploaderMod.Components
{
    public static class OpenFileWindows
    {
        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetOpenFileName(ref OpenFileName openFileName_0);

        public static string OpenfileDialog()
        {
            OpenFileName openFileName_ = default(OpenFileName);
            openFileName_.lStructSize = Marshal.SizeOf<OpenFileName>();
            openFileName_.lpstrFilter = "All files(*.*)\0\0";
            openFileName_.lpstrFile = new string(new char[256]);
            openFileName_.nMaxFile = openFileName_.lpstrFile.Length;
            openFileName_.lpstrFileTitle = new string(new char[64]);
            openFileName_.nMaxFileTitle = openFileName_.lpstrFileTitle.Length;
            openFileName_.lpstrTitle = "Open File Dialog...";
            if (!GetOpenFileName(ref openFileName_))
            {
                return string.Empty;
            }
            return openFileName_.lpstrFile;
        }
    }
}