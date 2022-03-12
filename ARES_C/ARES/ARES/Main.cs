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
        public Thread uploadThread;
        public Thread browserThread;
        public int threadCount;
        public int maxThreads = 12;
        public volatile int currentThreads = 0;
        public volatile int avatarCount;
        public int worldCount;
        public Records selectedAvatar;
        public WorldClass selectedWorld;
        public string unityPath;
        public HotswapConsole hotswapConsole;
        public bool isAvatar;
        public bool apiEnabled;
        public bool loadImages;
        public List<string> rippedList;

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
            try
            {
                nmThread.Value = Environment.ProcessorCount;
            }
            catch { }
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fileVersionInfo.ProductVersion;
                this.Text = "ARES V" + version;
            }
            catch { }
            cbLimit.SelectedIndex = 0;
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!Directory.Exists(filePath + @"\Logs"))
            {
                Directory.CreateDirectory(filePath + @"\Logs");
            }

            if (File.Exists(filePath + @"\LatestLog.txt"))
            {
                File.Move(filePath + @"\LatestLog.txt", string.Format(filePath + "\\Logs\\{0}.txt", string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", DateTime.Now)));
                Thread.Sleep(500);
                var myFile = File.Create(filePath + @"\LatestLog.txt");
                myFile.Close();
            }
            else
            {
                var myFile = File.Create(filePath + @"\LatestLog.txt");
                myFile.Close();
            }

            if (!File.Exists(filePath + @"\Ripped.txt"))
            {
                var myFile = File.Create(filePath + @"\Ripped.txt");
                myFile.Close();
            }
            else
            {
                rippedList = new List<string>();
                foreach (string line in System.IO.File.ReadLines(filePath + @"\Ripped.txt"))
                {
                    rippedList.Add(line);
                }
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
                MessageBox.Show("Please select unity.exe, after doing this leave the command window open it will close by itself after setup is complete");
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
#if DEBUG
                btnSearch.Enabled = true;
                btnHotswap.Enabled = true;
#endif
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
                uploadThread = new Thread(() => CoreFunctions.uploadToApi(localAvatars));
                uploadThread.Start();
            }

            localWorlds = CoreFunctions.getLocalWorlds();
            if (localWorlds.Count > 0 && apiEnabled)
            {
                CoreFunctions.uploadToApiWorld(localWorlds);
            }
            try
            {
                ScanPackage.DownloadOnlineSourcesOnStartup();
            }
            catch { }
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
                openFileDialog.Filter = "vrc* files (*.vrc*)|*.vrc*";
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
                maxThreads = Convert.ToInt32(nmThread.Value);
                loadImages = chkLoadImages.Checked;
                flowAvatars.Controls.Clear();

                statusLabel.Text = "Status: Loading API";
                if (!cbSearchTerm.Text.Contains("World"))
                {
                    List<Records> avatars = ApiGrab.getAvatars(txtSearchTerm.Text, cbSearchTerm.Text, cbLimit.Text);
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
                    while (currentThreads >= maxThreads)
                    {
                        Thread.Sleep(50);
                    }
                    currentThreads++;
                    var t = new Thread(() => MultiGetImages(item));
                    t.Start();
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

        public void MultiGetImages(Records item)
        {
            try
            {
                Panel groupBox = new Panel { Size = new Size(150, 150), BackColor = Color.Transparent };
                PictureBox avatarImage = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(148, 146) };
                PictureBox ripped = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(148, 146) };
                string questPc = "";
                if (item.PCAssetURL.ToLower() != "none")
                {
                    questPc += "{PC}";
                }
                if (item.QUESTAssetURL.ToLower() != "none")
                {
                    questPc += "{Quest}";
                }
                Label label = new Label { Text = "Avatar Name: " + item.AvatarName + " [" + questPc + "]", BackColor = Color.Transparent, ForeColor = Color.Red, Size = new Size(148, 146) };
                Bitmap bitmap = null;
                if (loadImages)
                {
                    bitmap = CoreFunctions.loadImage(item.ThumbnailURL);
                }

                if (bitmap != null || !loadImages)
                {
                    avatarImage.Image = bitmap;
                    label.Name = item.AvatarID;
                    label.Click += LoadInfo;
                    groupBox.Controls.Add(avatarImage);
                    if (rippedList.Contains(item.AvatarID))
                    {
                        ripped.Image = pbRipped.Image;

                        groupBox.Controls.Add(ripped);
                        groupBox.Controls.Add(label);
                        ripped.Parent = avatarImage;
                        label.Parent = ripped;
                    }
                    else
                    {
                        groupBox.Controls.Add(label);
                        label.Parent = avatarImage;
                    }

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
                currentThreads--;
            }
            catch
            {
                currentThreads--;
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
                    Bitmap bitmap = null;
                    if (loadImages)
                    {
                        bitmap = CoreFunctions.loadImage(item.ThumbnailURL);
                    }

                    if (bitmap != null || !loadImages)
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
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (fileName == "custom.vrcw")
            {
                fileName = filePath + @"\custom.vrcw";
            }

            string[] version = selectedWorld.PCAssetURL.Split('/');
            version[7] = nmPcVersion.Value.ToString();

            downloadFile(string.Join("/", version), fileName);
            return true;
        }

        private bool downloadVRCA(string fileName = "custom.vrca")
        {
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (fileName == "custom.vrca")
            {
                fileName = filePath + @"\custom.vrca";
            }
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
            if (selectedAvatar != null && isAvatar)
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
                    string avatarName = Encoding.ASCII.GetString(
                    Encoding.Convert(
                        Encoding.UTF8,
                        Encoding.GetEncoding(
                            Encoding.ASCII.EncodingName,
                            new EncoderReplacementFallback(string.Empty),
                            new DecoderExceptionFallback()
                            ),
                        Encoding.UTF8.GetBytes(selectedAvatar.AvatarName)
                        )
                    );
                    avatarName += "-ARES";
                    char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
                    string folderExtractLocation = folderDlg.SelectedPath + @"\" + new string(avatarName.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
                    if (!Directory.Exists(folderExtractLocation))
                    {
                        Directory.CreateDirectory(folderExtractLocation);
                    }
                    string commands = string.Format("/C AssetRipperConsole.exe \"{2}\" \"{3}\\AssetRipperConsole_win64\\{0}\" -o \"{1}\" -q ", unityVersion, folderExtractLocation, filePath + @"\custom.vrca", filePath);

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

                    tryDeleteDirectory(folderExtractLocation + @"\AssetRipper\GameAssemblies");
                    tryDeleteDirectory(folderExtractLocation + @"\Assets\Scripts");
                    try
                    {
                        Directory.Move(folderExtractLocation + @"\Assets\Shader", folderExtractLocation + @"\Assets\.Shader");
                    }
                    catch { }
                    if (selectedAvatar.AvatarID != "VRCA")
                    {
                        File.AppendAllText(filePath + @"\Ripped.txt", selectedAvatar.AvatarID + "\n");
                        rippedList.Add(selectedAvatar.AvatarID);
                    }
                }
            }
            else if (selectedWorld != null && !isAvatar)
            {
                if (!downloadVRCW())
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
                    string worldName = Encoding.ASCII.GetString(
                    Encoding.Convert(
                        Encoding.UTF8,
                        Encoding.GetEncoding(
                            Encoding.ASCII.EncodingName,
                            new EncoderReplacementFallback(string.Empty),
                            new DecoderExceptionFallback()
                            ),
                        Encoding.UTF8.GetBytes(selectedWorld.WorldName)
                        )
                    );
                    worldName += "-ARES";
                    char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
                    string folderExtractLocation = folderDlg.SelectedPath + @"\" + new string(worldName.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
                    if (!Directory.Exists(folderExtractLocation))
                    {
                        Directory.CreateDirectory(folderExtractLocation);
                    }
                    string commands = string.Format("/C AssetRipperConsole.exe \"{2}\" \"{3}\\AssetRipperConsole_win64\\{0}\" -o \"{1}\" -q ", unityVersion, folderExtractLocation, filePath + @"\custom.vrcw", filePath);

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

                    tryDeleteDirectory(folderExtractLocation + @"\AssetRipper\GameAssemblies");
                    tryDeleteDirectory(folderExtractLocation + @"\Assets\Scripts");
                    try
                    {
                        Directory.Move(folderExtractLocation + @"\Assets\Shader", folderExtractLocation + @"\Assets\.Shader");
                    }
                    catch { }
                }
            }
            else
            {
                MessageBox.Show("Please select an avatar or world first.");
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
            List<Records> localRecords = localAvatars;
            if (!locked)
            {
                loadImages = chkLoadImages.Checked;
                flowAvatars.Controls.Clear();
                statusLabel.Text = "Status: Loading Local";
                List<Records> avatars = null;

                if (chkPC.Checked)
                {
                    localRecords = localRecords.Where(x => x.PCAssetURL.Trim().ToLower() != "none").ToList();
                }
                if (chkQuest.Checked)
                {
                    localRecords = localRecords.Where(x => x.QUESTAssetURL.Trim().ToLower() != "none").ToList();
                }
                if (chkPublic.Checked == true && chkPrivate.Checked == false)
                {
                    localRecords = localRecords.Where(x => x.Releasestatus.ToLower().Trim() == "public").ToList();
                }
                if (chkPublic.Checked == false && chkPrivate.Checked == true)
                {
                    localRecords = localRecords.Where(x => x.Releasestatus.ToLower().Trim() == "private").ToList();
                }
                if (cbSearchTerm.Text == "Avatar Name" && txtSearchTerm.Text != "")
                {
                    localRecords = localRecords.Where(x => x.AvatarName.Contains(txtSearchTerm.Text)).ToList();
                }
                if (cbSearchTerm.Text == "Avatar ID" && txtSearchTerm.Text != "")
                {
                    localRecords = localRecords.Where(x => String.Equals(x.AvatarID, txtSearchTerm.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
                if (cbSearchTerm.Text == "Author Name" && txtSearchTerm.Text != "")
                {
                    localRecords = localRecords.Where(x => x.AuthorName.Contains(txtSearchTerm.Text)).ToList();
                }
                if (cbSearchTerm.Text == "Author ID" && txtSearchTerm.Text != "")
                {
                    localRecords = localRecords.Where(x => String.Equals(x.AuthorID, txtSearchTerm.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
                avatars = localRecords;

                avatars = avatars.OrderByDescending(x => x.TimeDetected).ToList();

                if (cbLimit.Text != "Max")
                {
                    avatars = avatars.Take(Convert.ToInt32(cbLimit.Text)).ToList();
                }

                AvatarList = avatars;
                avatarCount = avatars.Count();
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
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileDecompressed = filePath + @"\decompressed.vrca";
            string fileDecompressed2 = filePath + @"\decompressed1.vrca";
            string fileDecompressedFinal = filePath + @"\finalDecompressed.vrca";
            string fileDummy = filePath + @"\dummy.vrca";
            string fileTarget = filePath + @"\target.vrca";
            string tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\Roaming", "");
            string unityVRCA = tempFolder + "\\Local\\Temp\\DefaultCompany\\HSB\\custom.vrca";
            string regexId = @"(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})";
            string regexPrefabId = @"(prefab-id-v1_avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12}_[\d]{10})";
            string regexCab = @"(CAB-[\w\d]{32})";
            string regexUnity = @"20[\d]{2}.[\d]{1}.[\w\d]{4}";
            string regexUnityOlder = @"5.[\d]{1}.[\w\d]{3}";
            Regex AvatarIdRegex = new Regex(regexId);
            Regex AvatarPrefabIdRegex = new Regex(regexPrefabId);
            Regex AvatarCabRegex = new Regex(regexCab);
            Regex UnityRegex = new Regex(regexUnity);
            Regex UnityRegexOlder = new Regex(regexUnityOlder);

            tryDelete(fileDecompressed);
            tryDelete(fileDecompressed2);
            tryDelete(fileDecompressedFinal);
            tryDelete(fileDummy);
            tryDelete(fileTarget);

            try
            {
                File.Copy(unityVRCA, fileDummy);
            }
            catch
            {
                MessageBox.Show("Make sure you've started the build and publish on unity");
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
            MatchModel matchModelNew = getMatches(fileDecompressed, AvatarIdRegex, AvatarCabRegex, UnityRegex, UnityRegexOlder, AvatarPrefabIdRegex);

            try
            {
                HotSwap.DecompressToFileStr(filePath + @"\custom.vrca", fileDecompressed2, hotswapConsole);
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

            MatchModel matchModelOld = getMatches(fileDecompressed2, AvatarIdRegex, AvatarCabRegex, UnityRegex, UnityRegexOlder, AvatarPrefabIdRegex);

            getReadyForCompress(fileDecompressed2, fileDecompressedFinal, matchModelOld, matchModelNew);

            try
            {
                HotSwap.CompressBundle(fileDecompressedFinal, fileTarget, hotswapConsole);
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
            tryDelete(fileDecompressedFinal);
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
            File.AppendAllText(filePath + @"\Ripped.txt", matchModelOld.AvatarId + "\n");
            rippedList.Add(matchModelOld.AvatarId);
        }

        private MatchModel getMatches(string file, Regex avatarId, Regex avatarCab, Regex unityVersion, Regex unityVersionOld, Regex avatarAssetId)
        {
            MatchCollection avatarIdMatch = null;
            MatchCollection avatarAssetIdMatch = null;
            MatchCollection avatarCabMatch = null;
            MatchCollection unityMatch = null;
            int unityCount = 0;

            foreach (string line in System.IO.File.ReadLines(file))
            {
                var tempId = avatarId.Matches(line);
                var tempAssetId = avatarAssetId.Matches(line);
                var tempCab = avatarCab.Matches(line);
                var tempUnity = unityVersion.Matches(line);
                if (tempAssetId.Count > 0)
                {
                    avatarAssetIdMatch = tempAssetId;
                }
                if (tempId.Count > 0)
                {
                    avatarIdMatch = tempId;
                }
                if (tempCab.Count > 0)
                {
                    avatarCabMatch = tempCab;
                }
                if (tempUnity.Count > 0)
                {
                    unityMatch = tempUnity;
                    unityCount++;
                }
            }

            if(unityCount == 0)
            {
                foreach (string line in System.IO.File.ReadLines(file))
                {
                    var tempUnity = unityVersionOld.Matches(line);
                    if (tempUnity.Count > 0)
                    {
                        unityMatch = tempUnity;
                        break;
                    }
                }
            }

            return new MatchModel
            {
                AvatarId = avatarIdMatch[0].Value,
                AvatarCab = avatarCabMatch[0].Value,
                UnityVersion = unityMatch[0].Value,
                AvatarAssetId = avatarAssetIdMatch[0].Value
            };
        }

        private void getReadyForCompress(string oldFile, string newFile, MatchModel old, MatchModel newModel)
        {
            var enc = Encoding.GetEncoding(28592);
            using (StreamReaderOver vReader = new StreamReaderOver(oldFile, enc))
            {
                using (StreamWriter vWriter = new StreamWriter(newFile, false, enc))
                {
                    while (!vReader.EndOfStream)
                    {
                            string vLine = vReader.ReadLine();
                            string replace = checkAndReplaceLine(vLine, old, newModel);
                            vWriter.Write(replace);                       
                    }
                }
            }
        }

        private string checkAndReplaceLine(string line, MatchModel old, MatchModel newModel)
        {
            string edited = line;
            if (edited.Contains(old.AvatarAssetId))
            {
                edited = edited.Replace(old.AvatarAssetId, newModel.AvatarAssetId);
            }
            if (edited.Contains(old.AvatarId))
            {
                edited = edited.Replace(old.AvatarId, newModel.AvatarId);
            }
            if (edited.Contains(old.AvatarCab))
            {
                edited = edited.Replace(old.AvatarCab, newModel.AvatarCab);
            }
            if (edited.Contains(old.UnityVersion))
            {
                edited = edited.Replace(old.UnityVersion, newModel.UnityVersion);
            }
            return edited;
        }

        private void hotswapRepair()
        {
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileDecompressed2 = filePath + @"\decompressed1.vrca";

            tryDelete(fileDecompressed2);

            try
            {
                HotSwap.DecompressToFileStr(filePath + @"\custom.vrca", fileDecompressed2, hotswapConsole);
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
            if (Path.GetExtension(file).ToLower() == ".vrca")
            {
                isAvatar = true;
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
            else
            {
                isAvatar = false;
                selectedWorld = new WorldClass
                {
                    AuthorID = "VRCW",
                    AuthorName = "VRCW",
                    WorldDescription = "VRCW",
                    ImageURL = "VRCW",
                    ThumbnailURL = "VRCW",
                    PCAssetURL = file,
                    Tags = "VRCW",
                    TimeDetected = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString(),
                    UnityVersion = "VRCW",
                    WorldID = "VRCW",
                    WorldName = "VRCW",
                    Releasestatus = "VRCW"
                };
                txtAvatarInfo.Text = CoreFunctions.SetWorldInfo(selectedWorld);
            }
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
                Process.Start("avatars.html");
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

        private void Ares_Close(object sender, FormClosedEventArgs e)
        {
            if (imageThread != null)
            {
                imageThread.Abort();
            }
            if (vrcaThread != null)
            {
                vrcaThread.Abort();
            }
            if (uploadThread != null)
            {
                uploadThread.Abort();
            }
            Thread.Sleep(2000);
        }

        private void logo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ARES is an avatar recovery tool! It is only for educational uses! We do not condone theft of avatars,\nthe tool soley exists to recover avatars from within VRChat back onto new accounts and into their unity packages keeping as much of the avatar in-tact as possible!\n\nCurrently Developed by: \nShrekamusChrist\n\nPrevious Developers: \nLargestBoi\nCass_Dev");
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(ScanPackage.UnityTemp))
            {
                tryDeleteDirectory(ScanPackage.UnityTemp);
            }

            Directory.CreateDirectory(ScanPackage.UnityTemp);

            string packageSelected = selectPackage();
            string outpath = "";

            if (!string.IsNullOrEmpty(packageSelected))
            {
                outpath = PackageExtractor.ExtractPackage(packageSelected, ScanPackage.UnityTemp);
            }

            (int, int, int) scanCount = ScanPackage.CheckFiles();

            if (scanCount.Item3 > 0)
            {
                MessageBox.Show("Bad files were detected please select a new location for cleaned UnityPackage");
                string fileLocation = createPackage();
                var blank = new string[0];
                var rootDir = "Assets/";
                var pack = Package.FromDirectory(outpath, fileLocation, true, blank, blank);
                pack.GeneratePackage(rootDir);
            }

            MessageBox.Show(string.Format("Bad files detected {0}, Safe files detected {1}, Unknown files detected {2}", scanCount.Item3, scanCount.Item1, scanCount.Item2));


        }

        private string selectPackage()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = ".unitypackage files (*.unitypackage)|*.unitypackage";
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

        private string createPackage()
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = ".unitypackage files (*.unitypackage)|*.unitypackage";
                sfd.FilterIndex = 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    return sfd.FileName;
                }
            }
            return null;
        }
    }
}