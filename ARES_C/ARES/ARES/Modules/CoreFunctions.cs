using ARES.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ARES.Modules
{
    public class CoreFunctions
    {
        public string ErrorImage = "https://image.freepik.com/free-vector/glitch-error-404-page_23-2148105404.jpg";

        public string SetAvatarInfo(Records avatar)
        {
            string avatarString = string.Format("Time Dectected: {0} {13}Avatar ID: {1} {13}Avatar Name: {2} {13}Avatar Description {3} {13}Author ID: {4} {13}Author Name: {5} {13}PC Asset URL: {6} {13}Quest Asset URL: {7} {13}Image URL: {8} {13}Thumbnail URL: {9} {13}Unity Version: {10} {13}Release Status: {11} {13}Tags: {12}", GetDate(Convert.ToDouble(avatar.TimeDetected)), avatar.AvatarID, avatar.AvatarName, avatar.AvatarDescription, avatar.AuthorID, avatar.AuthorName, avatar.PCAssetURL, avatar.QuestAssetURL, avatar.ImageURL, avatar.ThumbnailURL, avatar.UnityVersion, avatar.ReleaseStatus, avatar.Tags, Environment.NewLine);
            return avatarString;
        }

        public Bitmap loadImage(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                //Needs a useragent to be able to view images.
                webClient.Headers.Add("user-agent", "VRCX");
                try
                {
                    Stream stream = webClient.OpenRead(url);
                    Bitmap bitmap; bitmap = new Bitmap(stream);

                    if (bitmap != null)
                    {
                        return bitmap;
                    }
                }
                catch
                {
                    return null;
                    //skip as its likely avatar is been yeeted from VRC servers
                    //avatarImage.Load(CoreFunctions.ErrorImage);
                }
                return null;

            }
        }

        public string GetDate(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime.ToString();
        }

        public List<Records> getLocalAvatars()
        {
            List<Records> list = new List<Records>();
            string contents = File.ReadAllText(@"Log.txt");
            string pattern = "Time Detected:(.*)\r\nAvatar ID:(.*)\r\nAvatar Name:(.*)\r\nAvatar Description:(.*)\r\nAuthor ID:(.*)\r\nAuthor Name:(.*)\r\nPC Asset URL:(.*)\r\nQuest Asset URL:(.*)\r\nImage URL:(.*)\r\nThumbnail URL:(.*)\r\nUnity Version:(.*)\r\nRelease Status:(.*)\r\nTags:(.*)";
            string[] logRecords = Regex.Matches(contents, pattern).Cast<Match>().Select(m => m.Value).ToArray();


            foreach (var item in logRecords)
            {
                string[] lineItem = item.Split('\n');
                Records records = new Records
                {
                    TimeDetected = lineItem[0].Split(':')[1].Replace("\r", ""),
                    AvatarID = lineItem[1].Split(':')[1].Replace("\r", ""),
                    AvatarName = lineItem[2].Split(':')[1].Replace("\r", ""),
                    AvatarDescription = lineItem[3].Split(':')[1].Replace("\r", ""),
                    AuthorID = lineItem[4].Split(':')[1].Replace("\r", ""),
                    AuthorName = lineItem[5].Split(':')[1].Replace("\r", ""),
                    PCAssetURL = string.Join("", lineItem[6].Split(':').Skip(1)).Replace("\r", "").Replace("https","https:"),
                    QuestAssetURL = string.Join("", lineItem[7].Split(':').Skip(1)).Replace("\r", "").Replace("https", "https:"),
                    ImageURL = string.Join("", lineItem[8].Split(':').Skip(1)).Replace("\r", "").Replace("https", "https:"),
                    ThumbnailURL = string.Join("", lineItem[9].Split(':').Skip(1)).Replace("\r", "").Replace("https", "https:"),
                    UnityVersion = lineItem[10].Split(':')[1].Replace("\r", ""),
                    ReleaseStatus = lineItem[11].Split(':')[1].Replace("\r", ""),
                    Tags = lineItem[12].Split(':')[1].Replace("\r", "")
                };
                list.Add(records);
            }

            return list;
        }
    }
}
