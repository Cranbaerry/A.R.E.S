using System;
using System.IO;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace HOTSWAP
{
    class Program
    {
        //Creates funtion to decompress asset bundles
        public static void DecompressToFile(BundleFileInstance bundleInst, string savePath)
        {
            AssetBundleFile bundle = bundleInst.file;
            Console.WriteLine($"22.2% Bundle file assigned!");
            FileStream bundleStream = File.Open(savePath, FileMode.Create);
            Console.WriteLine($"33.3% Loaded file to bundle stream!");
            bundle.Unpack(bundle.reader, new AssetsFileWriter(bundleStream));
            Console.WriteLine($"44.4% Unpack stream complete!");
            bundleStream.Position = 0;
            Console.WriteLine($"55.5% Bundle stream position assigned!");
            AssetBundleFile newBundle = new AssetBundleFile();
            Console.WriteLine($"66.6% Created new asset bundle file!");
            newBundle.Read(new AssetsFileReader(bundleStream), false);
            Console.WriteLine($"77.7% Bundle written to file!");
            bundle.reader.Close();
            Console.WriteLine($"88.8% Bundle closed!");
            bundleInst.file = newBundle;
            bundleStream.Flush();
            bundleStream.Close();
            Console.WriteLine($"100% Bundle instance cleaned!");
        }
        //Creates function allowing it to be used with string imputs
        public static void DecompressToFileStr(string bundlePath, string unpackedBundlePath)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var am = new AssetsManager();
            Console.WriteLine($"11.1% Declared new asset manager!");
            DecompressToFile(am.LoadBundleFile(bundlePath), unpackedBundlePath);
        }



        //Creates function to compress asset bundles
        public static void CompressBundle(string file, string compFile)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            var am = new AssetsManager();
            Console.WriteLine($"25% Declared new asset manager!");
            var bun = am.LoadBundleFile(file);
            Console.WriteLine($"50% Bundle file initialized!");
            using (var stream = File.OpenWrite(compFile))
            using (var writer = new AssetsFileWriter(stream))
            {
                Console.WriteLine($"75% File compression stream ready!");
                bun.file.Pack(bun.file.reader, writer, AssetBundleCompressionType.LZMA);
                Console.WriteLine($"100% Compressed file packing complete!");
            }
        }
        //creates areguments to call decompression and compression
        static void Main(string[] args)
        {
            string work = args[0];
            if (work == "d")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("A.R.E.S");
                Console.WriteLine("Avatar Ripping/Extraction System");
                Console.WriteLine("Decompression protocol launched!");
                string ABF = args[1];
                string DFN = args[2];
                Console.WriteLine("File located! Decompressing...");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("DevNote: This may take a while on large avatars,\nthe percentages arent even, they are\njust displayed based on the event currently\noccuring!");
                Console.WriteLine("While you wait here are some credits:");
                Console.WriteLine("LargestBoi (HOTSWAP.exe)");
                Console.WriteLine("nesrak1 for AssetsTools.NET v2");
                Console.WriteLine("This was modified into AssetsTools.NET v3");
                Console.WriteLine("by LargestBoi making compression ~3X faster!");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===================================================");
                Console.ForegroundColor = ConsoleColor.White;
                DecompressToFileStr(ABF, DFN);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("File decompressed!");
                Console.WriteLine("Decompression prtocall quitting...");
            }
            if (work == "c")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("A.R.E.S");
                Console.WriteLine("Avatar Ripping/Extraction System");
                Console.WriteLine("Compression protocol launched!");
                string ABF = args[1];
                string TFP = args[2];
                Console.WriteLine("File located! Compressing...");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("DevNote: This may take a while on large avatars,\nthe percentages arent even, they are\njust displayed based on the event currently\noccuring!");
                Console.WriteLine("While you wait here are some credits:");
                Console.WriteLine("LargestBoi (HOTSWAP.exe)");
                Console.WriteLine("nesrak1 for AssetsTools.NET v2");
                Console.WriteLine("This was modified into AssetsTools.NET v3");
                Console.WriteLine("by LargestBoi making compression ~3X faster!");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===================================================");
                Console.ForegroundColor = ConsoleColor.White;
                CompressBundle(ABF, TFP);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("File compressed!");
                Console.WriteLine("Compression prtocall quitting...");
            }
        }
    }
}