using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARES.Models
{
    public class AresConfig
    {
        public bool UnlimitedFavorites { get; set; }
        public bool Stealth { get; set; }
        public bool LogAvatars { get; set; }
        public bool LogWorlds { get; set; }
        public bool LogFriendsAvatars { get; set; }
        public bool LogOwnAvatars { get; set; }
        public bool LogPublicAvatars { get; set; }
        public bool LogPrivateAvatars { get; set; }
        public bool LogToConsole { get; set; }
        public bool ConsoleError { get; set; }
        public bool HWIDSpoof { get; set; }
        public bool AutoUpdate { get; set; }
    }
}
