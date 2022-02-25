using ARES.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ARES.Modules
{
    public class CoreFunctions
    {
        public string ErrorImage = "https://image.freepik.com/free-vector/glitch-error-404-page_23-2148105404.jpg";

        public string SetAvatarInfo(Records avatar)
        {
            string avatarString = string.Format("Time Dectected: {0} {13}Avatar ID: {1} {13}Avatar Name: {2} {13}Avatar Description {3} {13}Author ID: {4} {13}Author Name: {5} {13}PC Asset URL: {6} {13}Quest Asset URL: {7} {13}Image URL: {8} {13}Thumbnail URL: {9} {13}Unity Version: {10} {13}Release Status: {11} {13}Tags: {12}", GetDate(Convert.ToDouble(avatar.TimeDetected)), avatar.AvatarID, avatar.AvatarName, avatar.AvatarDescription, avatar.AuthorID, avatar.AuthorName, avatar.PCAssetURL, avatar.QUESTAssetURL, avatar.ImageURL, avatar.ThumbnailURL, avatar.UnityVersion, avatar.Releasestatus, avatar.Tags, Environment.NewLine);
            return avatarString;
        }

        public string SetWorldInfo(WorldClass avatar)
        {
            string avatarString = string.Format("Time Dectected: {0} {12}World ID: {1} {12}World Name: {2} {12}World Description {3} {12}Author ID: {4} {12}Author Name: {5} {12}PC Asset URL: {6} {12}Image URL: {7} {12}Thumbnail URL: {8} {12}Unity Version: {9} {12}Release Status: {10} {12}Tags: {11}", GetDate(Convert.ToDouble(avatar.TimeDetected)), avatar.WorldID, avatar.WorldName, avatar.WorldDescription, avatar.AuthorID, avatar.AuthorName, avatar.PCAssetURL, avatar.ImageURL, avatar.ThumbnailURL, avatar.UnityVersion, avatar.Releasestatus, avatar.Tags, Environment.NewLine);
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
            if (File.Exists("Log.txt"))
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
                        PCAssetURL = string.Join("", lineItem[6].Split(':').Skip(1)).Replace("\r", "").Replace("https", "https:"),
                        QUESTAssetURL = string.Join("", lineItem[7].Split(':').Skip(1)).Replace("\r", "").Replace("https", "https:"),
                        ImageURL = string.Join("", lineItem[8].Split(':').Skip(1)).Replace("\r", "").Replace("https", "https:"),
                        ThumbnailURL = string.Join("", lineItem[9].Split(':').Skip(1)).Replace("\r", "").Replace("https", "https:"),
                        UnityVersion = lineItem[10].Split(':')[1].Replace("\r", ""),
                        Releasestatus = lineItem[11].Split(':')[1].Replace("\r", ""),
                        Tags = lineItem[12].Split(':')[1].Replace("\r", "")
                    };
                    list.Add(records);
                }
                WriteLog(string.Format("Loaded Local Avatars"));
                return list;
            }
            return new List<Records>();
        }

        public List<WorldClass> getLocalWorlds()
        {
            if (File.Exists("LogWorld.txt"))
            {
                List<WorldClass> list = new List<WorldClass>();
                string contents = File.ReadAllText(@"LogWorld.txt");
                string pattern = "Time Detected:(.*)\r\nWorld ID:(.*)\r\nWorld Name:(.*)\r\nWorld Description:(.*)\r\nAuthor ID:(.*)\r\nAuthor Name:(.*)\r\nPC Asset URL:(.*)\r\nImage URL:(.*)\r\nThumbnail URL:(.*)\r\nUnity Version:(.*)\r\nRelease Status:(.*)\r\nTags:(.*)";
                string[] logRecords = Regex.Matches(contents, pattern).Cast<Match>().Select(m => m.Value).ToArray();


                foreach (var item in logRecords)
                {
                    string[] lineItem = item.Split('\n');
                    WorldClass records = new WorldClass
                    {
                        TimeDetected = lineItem[0].Split(':')[1].Replace("\r", ""),
                        WorldID = lineItem[1].Split(':')[1].Replace("\r", ""),
                        WorldName = lineItem[2].Split(':')[1].Replace("\r", ""),
                        WorldDescription = lineItem[3].Split(':')[1].Replace("\r", ""),
                        AuthorID = lineItem[4].Split(':')[1].Replace("\r", ""),
                        AuthorName = lineItem[5].Split(':')[1].Replace("\r", ""),
                        PCAssetURL = string.Join("", lineItem[6].Split(':').Skip(1)).Replace("\r", "").Replace("https", "https:"),
                        ImageURL = string.Join("", lineItem[7].Split(':').Skip(1)).Replace("\r", "").Replace("https", "https:"),
                        ThumbnailURL = string.Join("", lineItem[8].Split(':').Skip(1)).Replace("\r", "").Replace("https", "https:"),
                        UnityVersion = lineItem[9].Split(':')[1].Replace("\r", ""),
                        Releasestatus = lineItem[10].Split(':')[1].Replace("\r", ""),
                        Tags = lineItem[11].Split(':')[1].Replace("\r", "")
                    };
                    list.Add(records);
                }
                WriteLog(string.Format("Loaded Local Worlds"));
                return list;
            }
            return new List<WorldClass>();
        }

        public (bool, bool) setupHSB()
        {
            if (File.Exists("HSBC.rar"))
            {
                return (true, true);
            }
            try
            {
                tryDelete();
                string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string commands = string.Format("/C UnRAR.exe x HSB.rar HSB -id[c,d,n,p,q] -O+");

                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "CMD.EXE",
                    Arguments = commands,
                    WorkingDirectory = filePath
                };
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit();
                return (true, false);
            }
            catch
            {
                return (false, false);
            }
        }

        public bool setupUnity(string unityPath)
        {
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                string commands = string.Format("/C \"{0}\" -ProjectPath HSB", unityPath);

                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "CMD.EXE",
                    Arguments = commands,
                    WorkingDirectory = filePath
                };
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit();

            }
            catch { return false; }


            try
            {

                string commands = string.Format("/C Rar.exe a HSBC.rar HSB");
                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "CMD.EXE",
                    Arguments = commands,
                    WorkingDirectory = filePath
                };
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit();

                tryDelete();
            }
            catch (Exception  ex) {
                Console.WriteLine(ex.Message);
                return false; 
            }
            return true;
        }

        private void tryDelete()
        {
            try
            {
                killProcess("Unity Hub.exe");
                killProcess("Unity.exe");
                if (Directory.Exists("HSB"))
                {
                    Directory.Delete("HSB", true);
                }
                Directory.CreateDirectory("HSB");
            }
            catch { }
        }

        public bool openUnityPreSetup(string unityPath)
        {

            tryDelete();
            try
            {
                string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string commands = string.Format("/C UnRAR.exe x HSBC.rar HSB -id[c,d,n,p,q] -O+");

                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "CMD.EXE",
                    Arguments = commands,
                    WorkingDirectory = filePath
                };
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit();

                if (Directory.Exists("HSB/HSB"))
                {
                    commands = string.Format("/C \"{0}\" -ProjectPath HSB/HSB", unityPath);

                    p = new Process();
                    psi = new ProcessStartInfo
                    {
                        FileName = "CMD.EXE",
                        Arguments = commands,
                        WorkingDirectory = filePath
                    };
                    p.StartInfo = psi;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.Start();
                    //p.WaitForExit();
                }
                return false;
            }
            catch { return false; }
        }

        private void killProcess(string processName)
        {
            try
            {
                Process.Start("taskkill", "/F /IM \"" + processName + "\"");
                Console.WriteLine("Killed Process: " + processName);
                WriteLog(string.Format("Killed Process", processName));
            }
            catch { }
        }

        public void uploadToApi(List<Records> avatars)
        {
           
            string uploadedFile = "AvatarUploaded.txt";
            if (!File.Exists(uploadedFile))
            {
                File.Create(uploadedFile);
            }
            Thread.Sleep(500);
            foreach (var item in avatars)
            {
                if (!HasAvatarId(uploadedFile, item.AvatarID))
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://avatarlogger.tk/records/Avatars");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    string jsonPost = JsonConvert.SerializeObject(item);
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(jsonPost);
                    }
                    try
                    {
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                        }
                        File.AppendAllText(uploadedFile, item.AvatarID + Environment.NewLine);
                        WriteLog(string.Format("Avatar: {0} uploaded to API", item.AvatarID));
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("(409) Conflict"))
                        {
                            File.AppendAllText(uploadedFile, item.AvatarID + Environment.NewLine);
                            WriteLog(string.Format("Avatar: {0} already on API", item.AvatarID));
                        }
                    }
                    Console.WriteLine(item.AvatarID);
                }
            }
        }

        public void uploadToApiWorld(List<WorldClass> worlds)
        {

            string uploadedFile = "WorldUploaded.txt";
            if (!File.Exists(uploadedFile))
            {
                var myFile = File.Create(uploadedFile);
                myFile.Close();
            }
            Thread.Sleep(500);
            foreach (var item in worlds)
            {
                if (!HasAvatarId(uploadedFile, item.WorldID))
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://avatarlogger.tk/records/Worlds");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    string jsonPost = JsonConvert.SerializeObject(item);
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(jsonPost);
                    }
                    try
                    {
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                        }
                        File.AppendAllText(uploadedFile, item.WorldID + Environment.NewLine);
                        WriteLog(string.Format("World: {0} Uploaded to API", item.WorldID));
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("(409) Conflict"))
                        {
                            File.AppendAllText(uploadedFile, item.WorldID + Environment.NewLine);
                            WriteLog(string.Format("World: {0} already on API", item.WorldID));
                        }
                    }
                    Console.WriteLine(item.WorldID);
                }
            }
        }

        public bool HasAvatarId(string avatarFile, string avatarId)
        {
            var lines = File.ReadLines(avatarFile);
            foreach (var line in lines)
            {
                if (line.Contains(avatarId))
                {
                    return true;
                }
            }

            return false;
        }

        public void WriteLog(string logText)
        {
            string logBuilder = string.Format("{0:yy/MM/dd H:mm:ss} | {1} \n", DateTime.Now,logText);
            File.AppendAllText("LatestLog.txt", logBuilder);
        }
    }
}
