using ARES.Models;
using ARES.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ARES
{
    public partial class Main : Form
    {
        public Api ApiGrab;
        public CoreFunctions CoreFunctions;
        public IniFile iniFile;
        private List<Records> AvatarList;
        private List<Records> localAvatars;
        public bool locked;
        public Thread imageThread;
        public Thread vrcaThread;
        public int avatarCount;
        public Records selectedAvatar;
        public string unityPath;
        public HotswapConsole hotswapConsole;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ApiGrab = new Api();
            CoreFunctions = new CoreFunctions();
            iniFile = new IniFile();
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            if (File.Exists("LatestLog.txt"))
            {
                File.Move("LatestLog.txt", string.Format("Logs\\{0}.txt", string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", DateTime.Now)));
                File.Create("LatestLog.txt");
            } else
            {
                File.Create("LatestLog.txt");
            }

            

            lblStatsAmount.Text = ApiGrab.getStats().Total_database_size;
            cbSearchTerm.SelectedIndex = 0;
            cbVersionUnity.SelectedIndex = 0;
            MessageBoxManager.Yes = "PC";
            MessageBoxManager.No = "Quest";
            MessageBoxManager.Register();
            if (!iniFile.KeyExists("unity"))
            {
                selectFile();
            }
            else
            {
                unityPath = iniFile.Read("unity");
            }

            string pluginCheck = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("GUI", "");
            if (!File.Exists(pluginCheck + @"\Plugins\ARESPlugin.dll"))
            {
               // btnSearch.Enabled = false;
            }

            if (!string.IsNullOrEmpty(unityPath))
            {
                var unitySetup = CoreFunctions.setupHSB();
                if (unitySetup == (true, false))
                {
                    CoreFunctions.setupUnity(unityPath);
                }
            }

            localAvatars = CoreFunctions.getLocalAvatars();
            if (localAvatars.Count > 0)
            {
                CoreFunctions.uploadToApi(localAvatars);
            }

        }

        private void selectFile()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "exe files (*.exe)|*.exe";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            unityPath = filePath;
            iniFile.Write("unity", filePath);
        }

        private string selectFileVrca()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "vrca files (*.vrca)|*.vrca";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            return filePath;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!locked)
            {
                flowAvatars.Controls.Clear();

                statusLabel.Text = "Status: Loading API";
                List<Records> avatars = ApiGrab.getAvatars(txtSearchTerm.Text, cbSearchTerm.Text);
                AvatarList = avatars;
                avatarCount = avatars.Count();
                lblAvatarCount.Text = avatarCount.ToString();
                progress.Maximum = avatarCount;
                progress.Value = 0;
                locked = true;
                statusLabel.Text = "Status: Loading Avatar Images";
                imageThread = new Thread(new ThreadStart(GetImages));
                imageThread.Start();
            }
            else
            {
                MessageBox.Show("Still loading last search");
            }
        }


        public void GetImages()
        {
            try
            {
                foreach (var item in AvatarList)
                {
                    Panel groupBox = new Panel { Size = new Size(150, 150), BackColor = Color.Transparent };
                    PictureBox avatarImage = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(148, 146) };
                    Label label = new Label { Text = "Avatar Name: " + item.AvatarName, BackColor = Color.Transparent, ForeColor = Color.Red, Size = new Size(148, 146) };
                    Bitmap bitmap = CoreFunctions.loadImage(item.ThumbnailURL);

                    if (bitmap != null)
                    {
                        avatarImage.Image = bitmap;
                        label.Name = item.AvatarID;
                        //avatarImage.Click += LoadInfo;
                        label.Click += LoadInfo;
                        groupBox.Controls.Add(avatarImage);
                        groupBox.Controls.Add(label);
                        label.Parent = avatarImage;
                        if (flowAvatars.InvokeRequired)
                        {
                            flowAvatars.Invoke((MethodInvoker)delegate
                            {
                                flowAvatars.Controls.Add(groupBox);
                            });
                        }
                    }
                    else
                    {
                        avatarCount--;
                        if (lblAvatarCount.InvokeRequired)
                        {
                            lblAvatarCount.Invoke((MethodInvoker)delegate
                            {
                                lblAvatarCount.Text = avatarCount.ToString();
                            });
                        }
                    }
                    if (progress.GetCurrentParent().InvokeRequired)
                    {
                        progress.GetCurrentParent().Invoke(new MethodInvoker(delegate { progress.Value++; }));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            locked = false;
            if (statusLabel.GetCurrentParent().InvokeRequired)
            {
                statusLabel.GetCurrentParent().Invoke((MethodInvoker)delegate
                {
                    statusLabel.Text = "Status: Idle";
                });
            }
        }

        private void LoadInfo(object sender, EventArgs e)
        {
            var img = (Label)sender;
            selectedAvatar = AvatarList.Find(x => x.AvatarID == img.Name);
            txtAvatarInfo.Text = CoreFunctions.SetAvatarInfo(selectedAvatar);

            Bitmap bitmap; bitmap = CoreFunctions.loadImage(selectedAvatar.ImageURL);

            if (bitmap != null)
            {
                selectedImage.Image = bitmap;
            }
            if (selectedAvatar.PCAssetURL != "None")
            {
                string[] version = selectedAvatar.PCAssetURL.Split('/');
                nmPcVersion.Value = Convert.ToInt32(version[7]);
            }
            else
            {
                nmPcVersion.Value = 0;
            }
            if (selectedAvatar.QUESTAssetURL != "None")
            {
                string[] version = selectedAvatar.QUESTAssetURL.Split('/');
                nmQuestVersion.Value = Convert.ToInt32(version[7]);
            }
            else
            {
                nmQuestVersion.Value = 0;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (selectedAvatar != null)
            {
                string fileName = "custom.vrca";
                SaveFileDialog savefile = new SaveFileDialog();
                // set a default file name
                savefile.FileName = "custom.vrca";
                // set filters - this can be done in properties as well
                savefile.Filter = "VRCA files (*.vrca)|*.vrca";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    fileName = savefile.FileName;
                }
                if (!downloadVRCA(fileName))
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please select an avatar first.");
            }
        }

        private bool downloadVRCA(string fileName = "custom.vrca")
        {
            if (selectedAvatar.AuthorName != "VRCA")
            {
                if (selectedAvatar.PCAssetURL != "None" && selectedAvatar.QUESTAssetURL != "None")
                {
                    DialogResult dlgResult = MessageBox.Show("Select which version to download", "VRCA Select", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dlgResult == DialogResult.No)
                    {
                        if (selectedAvatar.QUESTAssetURL != "None")
                        {

                            string[] version = selectedAvatar.QUESTAssetURL.Split('/');
                            version[7] = nmQuestVersion.Value.ToString();
                            downloadFile(string.Join("/", version), fileName);

                        }
                        else
                        {
                            MessageBox.Show("Quest version doesn't exist");
                            return false;
                        }
                    }
                    else if (dlgResult == DialogResult.Yes)
                    {
                        if (selectedAvatar.PCAssetURL != "None")
                        {

                            string[] version = selectedAvatar.PCAssetURL.Split('/');
                            version[7] = nmPcVersion.Value.ToString();
                            downloadFile(string.Join("/", version), fileName);

                        }
                        else
                        {
                            MessageBox.Show("PC version doesn't exist");
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (selectedAvatar.PCAssetURL != "None")
                {

                    string[] version = selectedAvatar.PCAssetURL.Split('/');
                    version[7] = nmPcVersion.Value.ToString();
                    downloadFile(string.Join("/", version), fileName);

                }
                else if (selectedAvatar.QUESTAssetURL != "None")
                {

                    string[] version = selectedAvatar.QUESTAssetURL.Split('/');
                    version[7] = nmQuestVersion.Value.ToString();
                    downloadFile(string.Join("/", version), fileName);

                }
                else { return false; }

            }
            else
            {
                downloadFile(selectedAvatar.PCAssetURL, fileName);
            }
            
            return true;
        }

        private void btnExtractVRCA_Click(object sender, EventArgs e)
        {
            if (selectedAvatar != null)
            {
                if (!downloadVRCA())
                {
                    return;
                }

                FolderBrowserDialog folderDlg = new FolderBrowserDialog
                {
                    ShowNewFolderButton = true
                };
                // Show the FolderBrowserDialog.
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string unityVersion = cbVersionUnity.Text + "DLL";
                    string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string commands = string.Format("/C AssetRipperConsole.exe {2} {3}\\AssetRipperConsole_win64\\{0} -o \"{1}\" -q ", unityVersion, folderDlg.SelectedPath, filePath + @"\custom.vrca", filePath);

                    Process p = new Process();
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "CMD.EXE",
                        Arguments = commands,
                        WorkingDirectory = filePath + @"\AssetRipperConsole_win64"
                    };
                    p.StartInfo = psi;
                    p.Start();
                    p.WaitForExit();
                }
            }
            else
            {
                MessageBox.Show("Please select an avatar first.");
            }
        }

        private void downloadFile(string url, string saveName)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.Headers.Add("user-agent", "VRCX");
                    client.DownloadFile(url, saveName);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "(404) Not Found")
                    {
                        MessageBox.Show("Version doesn't exist or file has been deleted from VRChat servers");
                    }
                }
            }
        }

        private void btnStopSearch_Click(object sender, EventArgs e)
        {
            if (imageThread != null)
            {
                imageThread.Abort();
                locked = false;
            }
        }

        private void btnSearchLocal_Click(object sender, EventArgs e)
        {
            if (!locked)
            {
                flowAvatars.Controls.Clear();
                statusLabel.Text = "Status: Loading Local";
                List<Records> avatars = null;
                if (txtSearchTerm.Text == "")
                {
                    avatars = localAvatars;
                }
                else
                {
                    if (cbSearchTerm.Text == "Avatar Name")
                    {
                        avatars = localAvatars.Where(x => String.Equals(x.AvatarName, txtSearchTerm.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    }
                    if (cbSearchTerm.Text == "Avatar ID")
                    {
                        avatars = localAvatars.Where(x => String.Equals(x.AvatarID, txtSearchTerm.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    }
                    if (cbSearchTerm.Text == "Author Name")
                    {
                        avatars = localAvatars.Where(x => String.Equals(x.AuthorName, txtSearchTerm.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    }
                    if (cbSearchTerm.Text == "Author ID")
                    {
                        avatars = localAvatars.Where(x => String.Equals(x.AuthorID, txtSearchTerm.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    }
                }

                AvatarList = avatars;
                avatarCount = avatars.Count();
                lblAvatarCount.Text = avatarCount.ToString();
                progress.Maximum = avatarCount;
                progress.Value = 0;
                locked = true;
                statusLabel.Text = "Status: Loading Avatar Images";
                imageThread = new Thread(new ThreadStart(GetImages));
                imageThread.Start();
            }
            else
            {
                MessageBox.Show("Still loading last search");
            }
        }

        private void btnHotswap_Click(object sender, EventArgs e)
        {
            if (vrcaThread != null)
            {
                if (vrcaThread.IsAlive)
                {
                    MessageBox.Show("Hotswap is still busy with previous request");
                    return;
                }
            }
            if (selectedAvatar != null)
            {
                if (!downloadVRCA())
                {
                    return;
                }
                hotswapConsole = new HotswapConsole();
                hotswapConsole.Show();
                vrcaThread = new Thread(new ThreadStart(hotswap));
                vrcaThread.Start();
            }
            else
            {
                MessageBox.Show("Please select an avatar first.");
            }
        }

        private void hotswap()
        {
            string fileDecompressed = "decompressed.vrca";
            string fileDecompressed2 = "decompressed1.vrca";
            string fileDummy = "dummy.vrca";
            string fileTarget = "target.vrca";
            string tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\Roaming", "");
            string unityVRCA = tempFolder + "\\Local\\Temp\\DefaultCompany\\HSB\\custom.vrca";

            File.Delete(fileDecompressed);
            File.Delete(fileDecompressed2);
            File.Delete(fileDummy);
            File.Delete(fileTarget);

            try
            {
                File.Copy(unityVRCA, fileDummy);
            }
            catch { }

            try
            {
                HotSwap.DecompressToFileStr(fileDummy, fileDecompressed, hotswapConsole);
            }
            catch
            {
                MessageBox.Show("Error decompressing VRCA file");
                if (hotswapConsole.InvokeRequired)
                {
                    hotswapConsole.Invoke((MethodInvoker)delegate
                    {
                        hotswapConsole.Close();
                    });
                }
                return;
            }
            string newId = getFileString(fileDecompressed, @"(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})");
            string newCab = getFileString(fileDecompressed, @"(CAB-[\w\d]{32})");

            try
            {
                HotSwap.DecompressToFileStr("custom.vrca", fileDecompressed2, hotswapConsole);
            }
            catch
            {
                MessageBox.Show("Error decompressing VRCA file");
                if (hotswapConsole.InvokeRequired)
                {
                    hotswapConsole.Invoke((MethodInvoker)delegate
                    {
                        hotswapConsole.Close();
                    });
                }
                return;
            }

            string oldId = getFileString(fileDecompressed2, @"(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})");
            string oldCab = getFileString(fileDecompressed2, @"(CAB-[\w\d]{32})");

            var enc = Encoding.GetEncoding(28603);

            string text = File.ReadAllText(fileDecompressed2, enc);
            text = text.Replace(oldId, newId).Replace(oldCab, newCab);
            File.WriteAllText(fileDecompressed2, text, enc);

            text = null;

            try
            {
                HotSwap.CompressBundle(fileDecompressed2, fileTarget, hotswapConsole);
            }
            catch
            {
                MessageBox.Show("Error compressing VRCA file");
                if (hotswapConsole.InvokeRequired)
                {
                    hotswapConsole.Invoke((MethodInvoker)delegate
                    {
                        hotswapConsole.Close();
                    });
                }
                return;
            }
            try
            {
                File.Copy(fileTarget, unityVRCA, true);
            }
            catch { }

            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = new FileInfo(fileTarget).Length;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string compressedSize = string.Format("{0:0.##} {1}", len, sizes[order]);

            len = new FileInfo(fileDecompressed2).Length;
            order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            string uncompressedSize = string.Format("{0:0.##} {1}", len, sizes[order]);

            selectedImage.Image.Save(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\HSB\HSB\Assets\ARES SMART\Resources\ARESLogoTex.png", ImageFormat.Png);

            tryDelete(fileDecompressed);
            tryDelete(fileDecompressed2);
            tryDelete(fileDummy);
            tryDelete(fileTarget);

            if (hotswapConsole.InvokeRequired)
            {
                hotswapConsole.Invoke((MethodInvoker)delegate
                {
                    hotswapConsole.Close();
                });
            }

            MessageBox.Show(string.Format("Got file sizes, comp:{0}, decomp:{1}", compressedSize, uncompressedSize));
        }


        private void hotswapRepair()
        {
            string fileDecompressed2 = "decompressed1.vrca";

            File.Delete(fileDecompressed2);

            try
            {
                HotSwap.DecompressToFileStr("custom.vrca", fileDecompressed2, hotswapConsole);
            }
            catch
            {
                MessageBox.Show("Error decompressing VRCA file");
                return;
            }

            string oldId = getFileString(fileDecompressed2, @"(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})");
            string oldCab = getFileString(fileDecompressed2, @"(CAB-[\w\d]{32})");

            hotswapConsole.Close();
           
            txtSearchTerm.Text = oldId;
            cbSearchTerm.SelectedIndex = 2;
            btnSearch.PerformClick();

            txtAvatarInfo.Text += Environment.NewLine + "Avatar Id from VRCA: " + oldId + Environment.NewLine + "CAB Id from VRCA: " + oldCab;
        }

        private string getFileString(string file, string searchRegexString)
        {
            string line;
            string lineReturn = null;

            System.IO.StreamReader fileOpen =
                new System.IO.StreamReader(file);

            while ((line = fileOpen.ReadLine()) != null)
            {
                lineReturn = Regex.Match(line, searchRegexString).Value;
                if (!string.IsNullOrEmpty(lineReturn))
                {
                    break;
                }
            }

            fileOpen.Close();

            return lineReturn;
        }

        private void tryDelete(string location)
        {
            try
            {
                Directory.Delete(location, true);
            }
            catch { }
        }

        private void btnUnity_Click(object sender, EventArgs e)
        {
            string tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\Roaming", "");
            string unityTemp = "\\Local\\Temp\\DefaultCompany\\HSB";
            string unityTemp2 = "\\LocalLow\\Temp\\DefaultCompany\\HSB";

            tryDelete(tempFolder + unityTemp);
            tryDelete(tempFolder + unityTemp2);

            var unitySetup = CoreFunctions.setupHSB();
            if (unitySetup == (true, false))
            {
                CoreFunctions.setupUnity(unityPath);
                CoreFunctions.openUnityPreSetup(unityPath);
            }
            else if (unitySetup == (true, true))
            {
                CoreFunctions.openUnityPreSetup(unityPath);
            }
            btnHotswap.Enabled = true;
        }

        private void btnLoadVRCA_Click(object sender, EventArgs e)
        {
            selectedImage.ImageLocation = "https://github.com/Dean2k/A.R.E.S/releases/latest/download/ARESLogo.png";
            string file = selectFileVrca();
            selectedAvatar = new Records
            {
                AuthorID = "VRCA",
                AuthorName = "VRCA",
                AvatarDescription = "VRCA",
                ImageURL = "VRCA",
                ThumbnailURL = "VRCA",
                PCAssetURL = file,
                QUESTAssetURL = file,
                Tags = "VRCA",
                TimeDetected = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString(),
                UnityVersion = "VRCA",
                AvatarID = "VRCA",
                AvatarName = "VRCA",
                Releasestatus = "VRCA"
            };
            txtAvatarInfo.Text = CoreFunctions.SetAvatarInfo(selectedAvatar);
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            if (vrcaThread != null)
            {
                if (vrcaThread.IsAlive)
                {
                    MessageBox.Show("Hotswap is still busy with previous request");
                    return;
                }
            }
            if (selectedAvatar != null)
            {
                if (selectedAvatar.AuthorName == "VRCA")
                {
                    if (!downloadVRCA())
                    {
                        return;
                    }
                    hotswapConsole = new HotswapConsole();
                    hotswapConsole.Show();
                    vrcaThread = new Thread(new ThreadStart(hotswapRepair));
                    vrcaThread.Start();
                }
                else
                {
                    MessageBox.Show("Please load a VRCA file first");
                }
            }
            else
            {
                MessageBox.Show("Please select an avatar first.");
            }
        }

        private void btnVrcaSearch_Click(object sender, EventArgs e)
        {
            if (vrcaThread != null)
            {
                if (vrcaThread.IsAlive)
                {
                    MessageBox.Show("VRCA search (Hotswap) is still busy with previous request");
                    return;
                }
            }
            if (selectedAvatar != null)
            {
                if (selectedAvatar.AuthorName == "VRCA")
                {
                    if (!downloadVRCA())
                    {
                        return;
                    }
                    hotswapConsole = new HotswapConsole();
                    hotswapConsole.Show();
                    hotswapRepair();
                }
                else
                {
                    MessageBox.Show("Please load a VRCA file first");
                }
            }
            else
            {
                MessageBox.Show("Please select an avatar first.");
            }
        }
    }
}