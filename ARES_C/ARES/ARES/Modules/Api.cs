using ARES.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ARES.Modules
{
    public class Api
    {
        public List<Records> getAvatars(string query)
        {
            string url = "";
            if (!String.IsNullOrEmpty(query)) {
                url = string.Format("http://avatarlogger.tk/records/Avatars?include=TimeDetected,AvatarID,AvatarName,AvatarDescription,AuthorID,AuthorName,PCAssetURL,QUESTAssetURL,ImageURL,ThumbnailURL,UnityVersion,Releasestatus,Tags&size=500&order=TimeDetected,desc&filter=AvatarName,eq,{0}", query);
            } else
            {
                url = string.Format("http://avatarlogger.tk/records/Avatars?include=TimeDetected,AvatarID,AvatarName,AvatarDescription,AuthorID,AuthorName,PCAssetURL,QUESTAssetURL,ImageURL,ThumbnailURL,UnityVersion,Releasestatus,Tags&size=500&order=TimeDetected,desc");
            }
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url);

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            Console.WriteLine(WebResp.StatusCode);
            Console.WriteLine(WebResp.Server);

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            Avatar items = JsonConvert.DeserializeObject<Avatar>(jsonString);

            return items.records;
        }

        public Stats getStats()
        {
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://avatarlogger.tk/stats.php"));

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            Console.WriteLine(WebResp.StatusCode);
            Console.WriteLine(WebResp.Server);

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            Stats item = JsonConvert.DeserializeObject<Stats>(jsonString);

            return item;
        }
    }
}
