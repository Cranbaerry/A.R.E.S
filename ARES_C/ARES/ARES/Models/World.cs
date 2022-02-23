using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARES.Models
{
    public class WorldClass
    {
        public string TimeDetected { get; set; }
        public string WorldID { get; set; }
        public string WorldName { get; set; }
        public string WorldDescription { get; set; }
        public string AuthorName { get; set; }

        public string AuthorID { get; set; }
        public string PCAssetURL { get; set; }
        public string ImageURL { get; set; }
        public string ThumbnailURL { get; set; }
        public string UnityVersion { get; set; }
        public string Releasestatus { get; set; }
        public string Tags { get; set; }
    }

    public class Worlds
    {
        public List<WorldClass> records { get; set; }
    }
}
