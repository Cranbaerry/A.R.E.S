﻿using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARES.Modules
{
    public static class HotSwap
    {
        public static void DecompressToFile(BundleFileInstance bundleInst, string savePath, HotswapConsole hotSwap)
        {
            AssetBundleFile bundle = bundleInst.file;
            safeWrite(hotSwap.txtStatusText,$"22.2% Bundle file assigned!" + Environment.NewLine);
            safeProgress(hotSwap.pbProgress, 22);
            FileStream bundleStream = File.Open(savePath, FileMode.Create);
            safeWrite(hotSwap.txtStatusText, $"33.3% Loaded file to bundle stream!" + Environment.NewLine);
            safeProgress(hotSwap.pbProgress, 33);
            var progressBar = new SZProgress(hotSwap);
            bundle.Unpack(bundle.reader, new AssetsFileWriter(bundleStream), progressBar);
            safeWrite(hotSwap.txtStatusText, $"44.4% Unpack stream complete!" + Environment.NewLine);
            safeProgress(hotSwap.pbProgress, 44);
            bundleStream.Position = 0;
            safeWrite(hotSwap.txtStatusText, $"55.5% Bundle stream position assigned!" + Environment.NewLine);
            safeProgress(hotSwap.pbProgress, 55);
            AssetBundleFile newBundle = new AssetBundleFile();
            safeWrite(hotSwap.txtStatusText, $"66.6% Created new asset bundle file!" + Environment.NewLine);
            safeProgress(hotSwap.pbProgress, 66);
            newBundle.Read(new AssetsFileReader(bundleStream), false);
            safeWrite(hotSwap.txtStatusText, $"77.7% Bundle written to file!" + Environment.NewLine);
            safeProgress(hotSwap.pbProgress, 77);
            bundle.reader.Close();
            safeWrite(hotSwap.txtStatusText, $"88.8% Bundle closed!" + Environment.NewLine);
            safeProgress(hotSwap.pbProgress, 88);
            bundleInst.file = newBundle;
            bundleStream.Flush();
            bundleStream.Close();
            safeWrite(hotSwap.txtStatusText, $"100% Bundle instance cleaned!" + Environment.NewLine);
            safeProgress(hotSwap.pbProgress, 100);
        }
        //Creates function allowing it to be used with string imputs
        public static void DecompressToFileStr(string bundlePath, string unpackedBundlePath, HotswapConsole hotSwap)
        {
            var am = new AssetsManager();
            safeWrite(hotSwap.txtStatusText, "11.1% Declared new asset manager!" + Environment.NewLine);
            safeProgress(hotSwap.pbProgress, 11);
            DecompressToFile(am.LoadBundleFile(bundlePath), unpackedBundlePath,  hotSwap);
        }

        private static void safeWrite(TextBox text, string textWrite)
        {
            if (text.InvokeRequired)
            {
                text.Invoke((MethodInvoker)delegate
                {
                    text.Text += textWrite;
                });
            }
        }

        private static void safeProgress(ProgressBar progress, int value)
        {
            if (progress.InvokeRequired)
            {
                progress.Invoke((MethodInvoker)delegate
                {
                    progress.Value = value;
                });
            }
        }



        //Creates function to compress asset bundles
        public static void CompressBundle(string file, string compFile, HotswapConsole hotSwap)
        {
            var am = new AssetsManager();
            safeWrite(hotSwap.txtStatusText, $"25% Declared new asset manager!" + Environment.NewLine);
            safeWrite(hotSwap.txtStatusText, $"25% Declared new asset manager!" + Environment.NewLine);
            var bun = am.LoadBundleFile(file);
            safeWrite(hotSwap.txtStatusText, $"50% Bundle file initialized!" + Environment.NewLine);
            using (var stream = File.OpenWrite(compFile))
            {
                using (var writer = new AssetsFileWriter(stream))
                {
                    safeWrite(hotSwap.txtStatusText, $"75% File compression stream ready!" + Environment.NewLine);
                    var progressBar = new SZProgress(hotSwap);
                    bun.file.Pack(bun.file.reader, writer, AssetBundleCompressionType.LZMA, progressBar);

                    safeWrite(hotSwap.txtStatusText, $"100% Compressed file packing complete!" + Environment.NewLine);
                }
            }
            am.UnloadAll();
            bun = null;
        }
    }
}
