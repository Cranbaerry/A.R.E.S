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
        public static  void DecompressToFile(BundleFileInstance bundleInst, string savePath)
        {
            AssetBundleFile bundle = bundleInst.file;

            FileStream bundleStream = File.Open(savePath, FileMode.Create);

            bundle.Unpack(bundle.reader, new AssetsFileWriter(bundleStream));

            bundleStream.Position = 0;

            AssetBundleFile newBundle = new AssetBundleFile();
            newBundle.Read(new AssetsFileReader(bundleStream), false);

            bundle.reader.Close();
            bundleInst.file = newBundle;
        }
        //Creates function allowing it to be used with string imputs
        public static void DecompressToFileStr(string bundlePath, string unpackedBundlePath)
        {
            var am = new AssetsManager();
            DecompressToFile(am.LoadBundleFile(bundlePath), unpackedBundlePath);
        }



        //Creates function to compress asset bundles
        public static void CompressBundle(string file, string compFile)
        {
            var am = new AssetsManager();
            var bun = am.LoadBundleFile(file);
            using (var stream = File.OpenWrite(compFile))
            using (var writer = new AssetsFileWriter(stream))
            {
                bun.file.Pack(bun.file.reader, writer, AssetBundleCompressionType.LZMA);
            }
        }
        //creates areguments to call decompression and compression
        static void Main(string[] args)
        {
            string work = args[0];
            if (work == "d")
            {
                Console.WriteLine("Decompression prtocall launched!");
                string ABF = args[1];
                Console.WriteLine("File located! Decompressing...");
                Console.WriteLine("DevNote: On larger avatars this may take a while, there is no progress bar");
                Console.WriteLine("While you wait here are some credits:");
                Console.WriteLine("LargestBoi (HOTSWAP.exe)");
                Console.WriteLine("nesrak1 for AssetsTools.NET v2");
                DecompressToFileStr(ABF, "decompressed.vrca");
                Console.WriteLine("File decompressed!");
                Console.WriteLine("Decompression prtocall quitting...");
            }
            if (work == "c")
            {
                Console.WriteLine("Compression prtocall launched!");
                string dir = args[1];
                Console.WriteLine("File located! Compressing...");
                Console.WriteLine("DevNote: On larger avatars this may take a while, there is no progress bar");
                Console.WriteLine("While you wait here are some credits:");
                Console.WriteLine("LargestBoi (HOTSWAP.exe)");
                Console.WriteLine("nesrak1 for AssetsTools.NET v2");
                CompressBundle(dir, "compressed.vrca");
                Console.WriteLine("File compressed!");
                Console.WriteLine("Compression prtocall quitting...");
            }
        }
    }
}