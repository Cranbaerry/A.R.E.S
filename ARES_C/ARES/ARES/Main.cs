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
        public GenerateHtml generateHtml;
        private List<Records> AvatarList;
        private List<WorldClass> worldList;
        private List<Records> localAvatars;
        private List<WorldClass> localWorlds;
        public bool locked;
        public Thread imageThread;
        public Thread vrcaThread;
        public int avatarCount;
        public int worldCount;
        public Records selectedAvatar;
        public WorldClass selectedWorld;
        public string unityPath;
        public HotswapConsole hotswapConsole;
        public bool isAvatar;
        public bool apiEnabled;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ApiGrab = new Api();
            CoreFunctions = new CoreFunctions();
            iniFile = new IniFile();
            generateHtml = new GenerateHtml();

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            if (File.Exists("LatestLog.txt"))
            {
                File.Move("LatestLog.txt", string.Format("Logs\\{0}.txt", string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", DateTime.Now)));
                Thread.Sleep(500);
                var myFile = File.Create("LatestLog.txt");
                myFile.Close();
            }
            else
            {
                var myFile = File.Create("LatestLog.txt");
                myFile.Close();
            }

            if (!iniFile.KeyExists("apiEnabled"))
            {
                DialogResult dlgResult = MessageBox.Show("Enable API support?", "API", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dlgResult == DialogResult.No)
                {
                    apiEnabled = false;
                    iniFile.Write("apiEnabled", "false");
                }
                else if (dlgResult == DialogResult.Yes)
                {
                    apiEnabled = true;
                    iniFile.Write("apiEnabled", "true");
                }
                else
                {
                    apiEnabled = true;
                    iniFile.Write("apiEnabled", "true");
                }
            }
            else
            {
                apiEnabled = Convert.ToBoolean(iniFile.Read("apiEnabled"));
            }

            try
            {
                lblStatsAmount.Text = ApiGrab.getStats().Total_database_size;
            }
            catch
            {
                CoreFunctions.WriteLog("Error getting API stats.");
            }
            cbSearchTerm.SelectedIndex = 0;
            cbVersionUnity.SelectedIndex = 0;
            MessageBoxManager.Yes = "PC";
            MessageBoxManager.No = "Quest";
            MessageBoxManager.Register();
            if (!iniFile.KeyExists("unity"))
            {
                MessageBox.Show("Please select unity.exe");
                selectFile();
            }
            else
            {
                unityPath = iniFile.Read("unity");
            }

            string pluginCheck = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("GUI", "");
            if (!File.Exists(pluginCheck + @"\Plugins\ARESPlugin.dll") && apiEnabled)
            {
                btnSearch.Enabled = false;
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
            if (localAvatars.Count > 0 && apiEnabled)
            {
                CoreFunctions.uploadToApi(localAvatars);
            }

            localWorlds = CoreFunctions.getLocalWorlds();
            if (localWorlds.Count > 0 && apiEnabled)
            {
                CoreFunctions.uploadToApiWorld(localWorlds);
            }

        }

        private void selectFile()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Unity (Unity.exe)|Unity.exe";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Select Unity exe";

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
                if (!cbSearchTerm.Text.Contains("World"))
                {
                    List<Records> avatars = ApiGrab.getAvatars(txtSearchTerm.Text, cbSearchTerm.Text);
                    AvatarList = avatars;
                    if (chkPC.Checked)
                    {
                        AvatarList = AvatarList.Where(x => x.PCAssetURL.Trim().ToLower() != "none").ToList();
                    }
                    if (chkQuest.Checked)
                    {
                        AvatarList = AvatarList.Where(x => x.QUESTAssetURL.Trim().ToLower() != "none").ToList();
                    }
                    if (chkPublic.Checked == true && chkPrivate.Checked == false)
                    {
                        AvatarList = AvatarList.Where(x => x.Releasestatus.ToLower().Trim() == "public").ToList();
                    }
                    if (chkPublic.Checked == false && chkPrivate.Checked == true)
                    {
                        AvatarList = AvatarList.Where(x => x.Releasestatus.ToLower().Trim() == "private").ToList();
                    }
                    avatarCount = AvatarList.Count();
                    lblAvatarCount.Text = avatarCount.ToString();
                    progress.Maximum = avatarCount;
                    progress.Value = 0;
                    locked = true;
                    isAvatar = true;
                    statusLabel.Text = "Status: Loading Avatar Images";
                    imageThread = new Thread(new ThreadStart(GetImages));
                    imageThread.Start();
                }
                else
                {
                    List<WorldClass> worlds = ApiGrab.getWorlds(txtSearchTerm.Text, cbSearchTerm.Text);
                    worldList = worlds;
                    worldCount = worlds.Count();
                    lblAvatarCount.Text = worldCount.ToString();
                    progress.Maximum = worldCount;
                    progress.Value = 0;
                    locked = true;
                    isAvatar = false;
                    statusLabel.Text = "Status: Loading World Images";
                    imageThread = new Thread(new ThreadStart(GetImagesWorld));
                    imageThread.Start();
                }
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

        public void GetImagesWorld()
        {
            try
            {
                foreach (var item in worldList)
                {
                    Panel groupBox = new Panel { Size = new Size(150, 150), BackColor = Color.Transparent };
                    PictureBox avatarImage = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(148, 146) };
                    Label label = new Label { Text = "World Name: " + item.WorldName, BackColor = Color.Transparent, ForeColor = Color.Red, Size = new Size(148, 146) };
                    Bitmap bitmap = CoreFunctions.loadImage(item.ThumbnailURL);

                    if (bitmap != null)
                    {
                        avatarImage.Image = bitmap;
                        label.Name = item.WorldID;
                        //avatarImage.Click += LoadInfo;
                        label.Click += LoadInfoWorld;
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
                        worldCount--;
                        if (lblAvatarCount.InvokeRequired)
                        {
                            lblAvatarCount.Invoke((MethodInvoker)delegate
                            {
                                lblAvatarCount.Text = worldCount.ToString();
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

        private void LoadInfoWorld(object sender, EventArgs e)
        {
            var img = (Label)sender;
            selectedWorld = worldList.Find(x => x.WorldID == img.Name);
            txtAvatarInfo.Text = CoreFunctions.SetWorldInfo(selectedWorld);

            Bitmap bitmap; bitmap = CoreFunctions.loadImage(selectedWorld.ImageURL);

            if (bitmap != null)
            {
                selectedImage.Image = bitmap;
            }
            if (selectedWorld.PCAssetURL != "None")
            {
                string[] version = selectedWorld.PCAssetURL.Split('/');
                nmPcVersion.Value = Convert.ToInt32(version[7]);
            }
            else
            {
                nmPcVersion.Value = 0;
            }
            nmQuestVersion.Value = 0;

        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAvatarInfo.Text))
            {
                if (selectedAvatar != null)
                {
                    if (txtAvatarInfo.Text.Contains("avtr_") && selectedAvatar.AvatarID.Contains("avtr_"))
                    {

                        SaveFileDialog savefile = new SaveFileDialog();
                        string fileName = "custom.vrca";
                        savefile.Filter = "VRCA files (*.vrca)|*.vrca";
                        savefile.FileName = fileName;


                        if (savefile.ShowDialog() == DialogResult.OK)
                        {
                            fileName = savefile.FileName;
                        }
                        if (!downloadVRCA(fileName))
                        {
                            return;
                        }
                    }
                }

                if (selectedWorld != null)
                {
                    if (txtAvatarInfo.Text.Contains("wrld_") && selectedWorld.WorldID.Contains("wrld_"))
                    {

                        SaveFileDialog savefile = new SaveFileDialog();
                        string fileName = "custom.VRCW";
                        savefile.Filter = "VRCW files (*.VRCW)|*.VRCW";
                        savefile.FileName = fileName;


                        if (savefile.ShowDialog() == DialogResult.OK)
                        {
                            fileName = savefile.FileName;
                        }
                        if (!downloadVRCW(fileName))
                        {
                            return;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an avatar or world first.");
            }

        }

        private bool downloadVRCW(string fileName = "custom.vrcw")
        {
            string[] version = selectedWorld.PCAssetURL.Split('/');
            version[7] = nmPcVersion.Value.ToString();
            downloadFile(string.Join("/", version), fileName);
            return true;
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
                    string commands = string.Format("/C AssetRipperConsole.exe \"{2}\" \"{3}\\AssetRipperConsole_win64\\{0}\" -o \"{1}\" -q ", unityVersion, folderDlg.SelectedPath, filePath + @"\custom.vrca", filePath);

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

                    tryDeleteDirectory(folderDlg.SelectedPath + @"\AssetRipper\GameAssemblies");
                    tryDeleteDirectory(folderDlg.SelectedPath + @"\Assets\Scripts");
                    try
                    {
                        Directory.Move(folderDlg.SelectedPath + @"\Assets\Shader", folderDlg.SelectedPath + @"\Assets\.Shader");
                    }
                    catch { }

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
                    if (chkPC.Checked)
                    {
                        avatars = localAvatars.Where(x => x.PCAssetURL.Trim().ToLower() != "none").ToList();
                    }
                    if (chkQuest.Checked)
                    {
                        avatars = localAvatars.Where(x => x.QUESTAssetURL.Trim().ToLower() != "none").ToList();
                    }
                    if (chkPublic.Checked == true && chkPrivate.Checked == false)
                    {
                        avatars = localAvatars.Where(x => x.Releasestatus.ToLower().Trim() == "public").ToList();
                    }
                    if (chkPublic.Checked == false && chkPrivate.Checked == true)
                    {
                        avatars = localAvatars.Where(x => x.Releasestatus.ToLower().Trim() == "private").ToList();
                    }
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

            tryDelete(fileDecompressed);
            tryDelete(fileDecompressed2);
            tryDelete(fileDummy);
            tryDelete(fileTarget);

            try
            {
                File.Copy(unityVRCA, fileDummy);
            }
            catch { }

            try
            {
                HotSwap.DecompressToFileStr(fileDummy, fileDecompressed, hotswapConsole);
            }
            catch (Exception ex)
            {
                CoreFunctions.WriteLog(string.Format("{0}", ex.Message));
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
            catch (Exception ex)
            {
                CoreFunctions.WriteLog(string.Format("{0}", ex.Message));
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
            catch (Exception ex)
            {
                CoreFunctions.WriteLog(string.Format("{0}", ex.Message));
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
            CoreFunctions.WriteLog(string.Format("Successfully hotswapped avatar"));
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

            tryDelete(fileDecompressed2);

            try
            {
                HotSwap.DecompressToFileStr("custom.vrca", fileDecompressed2, hotswapConsole);
            }
            catch (Exception ex)
            {
                CoreFunctions.WriteLog(string.Format("{0}", ex.Message));
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
            CoreFunctions.WriteLog(string.Format("Repaired VRCA file"));
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
                if (File.Exists(location))
                {
                    File.Delete(location);
                    CoreFunctions.WriteLog(string.Format("Deleted file {0}", location));
                }
            }
            catch (Exception ex)
            {
                CoreFunctions.WriteLog(string.Format("{0}", ex.Message));
            }
        }

        private void tryDeleteDirectory(string location)
        {
            try
            {
                Directory.Delete(location, true);
                CoreFunctions.WriteLog(string.Format("Deleted file {0}", location));
            }
            catch (Exception ex)
            {
                CoreFunctions.WriteLog(string.Format("{0}", ex.Message));
            }
        }

        private void btnUnity_Click(object sender, EventArgs e)
        {
            string tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\Roaming", "");
            string unityTemp = "\\Local\\Temp\\DefaultCompany\\HSB";
            string unityTemp2 = "\\LocalLow\\Temp\\DefaultCompany\\HSB";

            tryDeleteDirectory(tempFolder + unityTemp);
            tryDeleteDirectory(tempFolder + unityTemp2);

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

        private void btnBrowserView_Click(object sender, EventArgs e)
        {
            if (AvatarList != null)
            {
                generateHtml.GenerateHtmlPage(AvatarList);
                System.Diagnostics.Process.Start("avatars.html");
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (isAvatar)
            {
                if (cbCopy.Text == "Time Dectected")
                {
                    Clipboard.SetText(selectedAvatar.TimeDetected);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Avatar ID")
                {
                    Clipboard.SetText(selectedAvatar.AvatarID);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Avatar Name")
                {
                    Clipboard.SetText(selectedAvatar.AvatarName);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Avatar Description")
                {
                    Clipboard.SetText(selectedAvatar.AvatarDescription);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Author ID")
                {
                    Clipboard.SetText(selectedAvatar.AuthorID);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Author Name")
                {
                    Clipboard.SetText(selectedAvatar.AuthorName);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "PC Asset URL")
                {
                    Clipboard.SetText(selectedAvatar.PCAssetURL);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Quest Asset URL")
                {
                    Clipboard.SetText(selectedAvatar.QUESTAssetURL);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Image URL")
                {
                    Clipboard.SetText(selectedAvatar.ImageURL);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Thumbnail URL")
                {
                    Clipboard.SetText(selectedAvatar.ThumbnailURL);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Unity Version")
                {
                    Clipboard.SetText(selectedAvatar.UnityVersion);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Release Status")
                {
                    Clipboard.SetText(selectedAvatar.Releasestatus);
                    MessageBox.Show("information copied to clipboard.");
                }
                if (cbCopy.Text == "Tags")
                {
                    Clipboard.SetText(selectedAvatar.Tags);
                    MessageBox.Show("information copied to clipboard.");
                }
            }
            else
            {
                MessageBox.Show("Only works for avatars atm.");
            }
        }

        private void btnApi_Click(object sender, EventArgs e)
        {
            if (apiEnabled)
            {
                btnSearch.Enabled = false;
                apiEnabled = false;
                btnApi.Text = "Enable API";
                iniFile.Write("apiEnabled", "false");
            }
            else if (!apiEnabled)
            {
                btnSearch.Enabled = true;
                apiEnabled = true;
                btnApi.Text = "Disable API";
                iniFile.Write("apiEnabled", "true");
            }
        }
    }
}