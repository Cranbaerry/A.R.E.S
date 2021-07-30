using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Text.RegularExpressions;
using System.Diagnostics;
using VRC.Core;
using librsync.net;

namespace HOTSWAP
{
    class Program
    {
        public static AssetBundleFile DecompressBundle(string file, string decompFile)
        {
            var bun = new AssetBundleFile();

            var fs = (Stream)File.OpenRead(file);
            var reader = new AssetsFileReader(fs);

            bun.Read(reader, true);
            if (bun.bundleHeader6.GetCompressionType() != 0)
            {
                Stream nfs = decompFile switch
                {
                    null => new MemoryStream(),
                    _ => File.Open(decompFile, FileMode.Create, FileAccess.ReadWrite)
                };
                var writer = new AssetsFileWriter(nfs);
                bun.Unpack(reader, writer);

                nfs.Position = 0;
                fs.Close();

                fs = nfs;
                reader = new AssetsFileReader(fs);

                bun = new AssetBundleFile();
                bun.Read(reader);
                bun.Close();
            }

            return bun;
        }
        public static void CompressBundle(string file, string compFile)
        {
            var bun = DecompressBundle(file, null);
            var fs = File.OpenWrite(compFile);
            using var writer = new AssetsFileWriter(fs);
            bun.Pack(bun.reader, writer, AssetBundleCompressionType.LZMA);
        }

        static void Main(string[] args)
        {
        string work = args[0];
        if (work == "d")
        {
            string dir = args[1];
            DecompressBundle(dir, "decompressedfile");
        }
        if (work == "c")
        {
            CompressBundle("decompressedfile1", "custom.vrca");
        }
        if (work == "mID")
        {
            Console.WriteLine("avtr_" + Guid.NewGuid().ToString());
        }
        if (work == "gSIG")
        {
            Stream inStream = null;
            FileStream outStream = null;
            byte[] buf = new byte[64 * 1024];
            IAsyncResult asyncRead = null;
            IAsyncResult asyncWrite = null;
            int read = 0;
            inStream = Librsync.ComputeSignature(File.OpenRead("AvatarC.vrca"));
            outStream = File.Open("Signature.sig", FileMode.Create, FileAccess.Write);
            asyncRead = inStream.BeginRead(buf, 0, buf.Length, null, null);
            read = inStream.EndRead(asyncRead);
            asyncWrite = outStream.BeginWrite(buf, 0, read, null, null);
            outStream.EndWrite(asyncWrite);
            inStream.Close();
            //outStream.Close();

            string signatureFilename = "Signature.sig";
            bool wait = true;
            bool wasError = false;
            bool worthRetry = false;
            string errorStr = "";
            string sigMD5Base64 = "";
            wait = true;
            errorStr = "";
                VRC.Tools.FileMD5(signatureFilename, md5Bytes);
                {
                    sigMD5Base64 = Convert.ToBase64String(md5Bytes);
                }
            );
            }
        if (work == "hSIG")
        {
            string signatureFilename = "Signature.sig";
            bool wait = true;
            bool wasError = false;
            bool worthRetry = false;
            string errorStr = "";
            string sigMD5Base64 = "";
            wait = true;
            errorStr = "";
            VRC.Tools.FileMD5(signatureFilename,
                delegate (byte[] md5Bytes)
                {
                    sigMD5Base64 = Convert.ToBase64String(md5Bytes);
                    wait = false;
                }
            );
        }
}
}