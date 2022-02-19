using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARES.Modules
{
    public static class HotSwap
    {
        public static void DecompressToFile(BundleFileInstance bundleInst, string savePath)
        {
            AssetBundleFile bundle = bundleInst.file;
            Console.WriteLine($"22.2% Bundle file assigned!");
            FileStream bundleStream = File.Open(savePath, FileMode.Create);
            Console.WriteLine($"33.3% Loaded file to bundle stream!");
            var progressBar = new SZProgress();
            bundle.Unpack(bundle.reader, new AssetsFileWriter(bundleStream), progressBar);
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
                var progressBar = new SZProgress();
                bun.file.Pack(bun.file.reader, writer, AssetBundleCompressionType.LZMA, progressBar);

                Console.WriteLine($"100% Compressed file packing complete!");
            }
        }
    }
}
