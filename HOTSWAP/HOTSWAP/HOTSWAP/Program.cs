using System;
using System.Collections;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using UnhollowerRuntimeLib;
using UnityEngine;
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
        //Creates funtion to decompress asset bundles
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
        //Creates function to compress asset bundles
        public static void CompressBundle(string file, string compFile)
        {
            var bun = DecompressBundle(file, null);
            var fs = File.OpenWrite(compFile);
            using var writer = new AssetsFileWriter(fs);
            bun.Pack(bun.reader, writer, AssetsBundleCompressionType.LZMA);
        }
        //creates areguments to call decompression and compression
        static void Main(string[] args)
        {
            string work = args[0];
            if (work == "d")
            {
                string dir = args[1];
                DecompressBundle(dir, "decompressed.vrca");
            }
            if (work == "c")
            {
                string dir = args[1];
                CompressBundle(dir, "compressed.vrca");
            }
            //(NOT IN USE) can be used to create new avtr id
            if (work == "mID")
            {
                Console.WriteLine("avtr_" + Guid.NewGuid().ToString());
            }
        }
    }
}