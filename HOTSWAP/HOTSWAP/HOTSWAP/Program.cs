using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Text.RegularExpressions;

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
            string compressedfile = args[0];
            string avtrid = args[1];
            DecompressBundle(compressedfile, "decompressedfile");
            string readFile  = System.IO.File.ReadAllText("decompressedfile", Encoding.Default);
            Regex rx = new Regex(@"avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12}");
            MatchCollection matches = rx.Matches(readFile);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Console.WriteLine("'{0}' repeated at positions {1} and {2}",
                                  groups["word"].Value,
                                  groups[0].Index,
                                  groups[1].Index);
            }
            //Console.WriteLine(readFile);
            Console.Read();
        }
    }
}
