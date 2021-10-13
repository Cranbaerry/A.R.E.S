using System;
using System.IO;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace HOTSWAP
{
    class Program
    {
        //Creates funtion to decompress asset bundles
        public static  void DecompressToFile(BundleFileInstance bundleInst, string savePath)
        {
            DateTime start1 = DateTime.UtcNow;
            AssetBundleFile bundle = bundleInst.file;
            DateTime end1 = DateTime.UtcNow;
            TimeSpan timeDiff1 = end1 - start1;
            Console.WriteLine($"22.2% Bundle file assigned! Time taken: {Convert.ToInt32(timeDiff1.TotalMilliseconds / 1000).ToString()} seconds");
            DateTime start2 = DateTime.UtcNow;
            FileStream bundleStream = File.Open(savePath, FileMode.Create);
            DateTime end2 = DateTime.UtcNow;
            TimeSpan timeDiff2 = end2 - start2;
            Console.WriteLine($"33.3% Loaded file to bundle stream! Time taken: {Convert.ToInt32(timeDiff2.TotalMilliseconds / 1000).ToString()} seconds");
            DateTime start3 = DateTime.UtcNow;
            bundle.Unpack(bundle.reader, new AssetsFileWriter(bundleStream));
            DateTime end3 = DateTime.UtcNow;
            TimeSpan timeDiff3 = end3 - start3;
            Console.WriteLine($"44.4% Unpack stream complete! Time taken: {Convert.ToInt32(timeDiff3.TotalMilliseconds / 1000).ToString()} seconds");
            DateTime start4 = DateTime.UtcNow;
            bundleStream.Position = 0;
            DateTime end4 = DateTime.UtcNow;
            TimeSpan timeDiff4 = end4 - start4;
            Console.WriteLine($"55.5% Bundle stream position assigned! Time taken: {Convert.ToInt32(timeDiff4.TotalMilliseconds / 1000).ToString()} seconds");
            DateTime start5 = DateTime.UtcNow;
            AssetBundleFile newBundle = new AssetBundleFile();
            DateTime end5 = DateTime.UtcNow;
            TimeSpan timeDiff5 = end5 - start5;
            Console.WriteLine($"66.6% Created new asset bundle file! Time taken: {Convert.ToInt32(timeDiff5.TotalMilliseconds / 1000).ToString()} seconds");
            DateTime start6 = DateTime.UtcNow;
            newBundle.Read(new AssetsFileReader(bundleStream), false);
            DateTime end6 = DateTime.UtcNow;
            TimeSpan timeDiff6 = end6 - start6;
            Console.WriteLine($"77.7% Bundle written to file! Time taken: {Convert.ToInt32(timeDiff6.TotalMilliseconds / 1000).ToString()} seconds");
            DateTime start7 = DateTime.UtcNow;
            bundle.reader.Close();
            DateTime end7 = DateTime.UtcNow;
            TimeSpan timeDiff7 = end7 - start7;
            Console.WriteLine($"88.8% Bundle closed! Time taken: {Convert.ToInt32(timeDiff7.TotalMilliseconds / 1000).ToString()} seconds");
            DateTime start8 = DateTime.UtcNow;
            bundleInst.file = newBundle;
            DateTime end8 = DateTime.UtcNow;
            TimeSpan timeDiff8 = end8 - start8;
            Console.WriteLine($"100% Bundle instance cleaned! Time taken: {Convert.ToInt32(timeDiff8.TotalMilliseconds / 1000).ToString()} seconds");
            TimeSpan timeDiffTotal = end8 - start1;
            Console.WriteLine($"Total time taken: {Convert.ToInt32(timeDiffTotal.TotalMilliseconds / 1000).ToString()} seconds");
        }
        //Creates function allowing it to be used with string imputs
        public static void DecompressToFileStr(string bundlePath, string unpackedBundlePath)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            DateTime start = DateTime.UtcNow;
            var am = new AssetsManager();
            DateTime end = DateTime.UtcNow;
            TimeSpan timeDiff = end - start;
            Console.WriteLine($"11.1% Declared new asset manager! Time taken: {Convert.ToInt32(timeDiff.TotalMilliseconds / 1000).ToString()} seconds");
            DecompressToFile(am.LoadBundleFile(bundlePath), unpackedBundlePath);
        }



        //Creates function to compress asset bundles
        public static void CompressBundle(string file, string compFile)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            DateTime start = DateTime.UtcNow;
            var am = new AssetsManager();
            DateTime end = DateTime.UtcNow;
            TimeSpan timeDiff = end - start;
            Console.WriteLine($"25% Declared new asset manager! Time taken: {Convert.ToInt32(timeDiff.TotalMilliseconds / 1000).ToString()} seconds");
            DateTime start1 = DateTime.UtcNow;
            var bun = am.LoadBundleFile(file);
            DateTime end1 = DateTime.UtcNow;
            TimeSpan timeDiff1 = end1 - start1;
            Console.WriteLine($"50% Bundle file initialized! Time taken: {Convert.ToInt32(timeDiff1.TotalMilliseconds / 1000).ToString()} seconds");
            DateTime start2 = DateTime.UtcNow;
            using (var stream = File.OpenWrite(compFile))
            using (var writer = new AssetsFileWriter(stream))
            {
                DateTime end2 = DateTime.UtcNow;
                TimeSpan timeDiff2 = end2 - start2;
                Console.WriteLine($"75% File compression stream ready! Time taken: {Convert.ToInt32(timeDiff2.TotalMilliseconds / 1000).ToString()} seconds");
                DateTime start3 = DateTime.UtcNow;
                bun.file.Pack(bun.file.reader, writer, AssetBundleCompressionType.LZMA);
                DateTime end3 = DateTime.UtcNow;
                TimeSpan timeDiff3 = end3 - start3;
                Console.WriteLine($"100% Compressed file packing complete! Time taken: {Convert.ToInt32(timeDiff3.TotalMilliseconds / 1000).ToString()} seconds");
                TimeSpan timeDiffTotal = end3 - start;
                Console.WriteLine($"Total time taken: {Convert.ToInt32(timeDiffTotal.TotalMilliseconds / 1000).ToString()} seconds");
            }
        }
        //creates areguments to call decompression and compression
        static void Main(string[] args)
        {
            string work = args[0];
            if (work == "d")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Decompression protocol launched!");
                string ABF = args[1];
                Console.WriteLine("File located! Decompressing...");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("DevNote: This may take a while on large avatars,\nthe percentages arent even, they are\njust displayed based on the event currently\noccuring!");
                Console.WriteLine("While you wait here are some credits:");
                Console.WriteLine("LargestBoi (HOTSWAP.exe)");
                Console.WriteLine("nesrak1 for AssetsTools.NET v2");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===================================================");
                Console.ForegroundColor = ConsoleColor.White;
                DecompressToFileStr(ABF, "decompressed.vrca");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("File decompressed!");
                Console.WriteLine("Decompression prtocall quitting...");
            }
            if (work == "c")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Compression protocol launched!");
                string dir = args[1];
                Console.WriteLine("File located! Compressing...");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("DevNote: This may take a while on large avatars,\nthe percentages arent even, they are\njust displayed based on the event currently\noccuring!");
                Console.WriteLine("While you wait here are some credits:");
                Console.WriteLine("LargestBoi (HOTSWAP.exe)");
                Console.WriteLine("nesrak1 for AssetsTools.NET v2");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===================================================");
                Console.ForegroundColor = ConsoleColor.White;
                CompressBundle(dir, "compressed.vrca");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("File compressed!");
                Console.WriteLine("Compression prtocall quitting...");
            }
        }
    }
}