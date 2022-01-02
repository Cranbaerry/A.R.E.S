using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarLogger
{
    public class Config
    {
        public bool LogFriendsAvatars { get; set; } = false;
        public bool LogOwnAvatars { get; set; } = false;
        public bool LogToConsole { get; set; } = true;
        public bool ConsoleError { get; set; } = true;
    }
}
