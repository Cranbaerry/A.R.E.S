using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

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
            //string file = @"C:\Users\adamc\Desktop\VRC\Avis\Bumblebee.vrca";
            //string decompFile = @"C:\Users\adamc\Desktop\VRC\Avis\Bumblebee.D";
            //DecompressBundle(file, decompFile);
            //string file = @"C:\Users\adamc\Desktop\VRC\Avis\Bumblebee.D";
            //string compFile = @"C:\Users\adamc\Desktop\VRC\Avis\Bumblebee2.vrca";
            //CompressBundle(file, compFile);
        }
    }
}
