using ARES.Models;
using ARES.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARES
{
    public partial class Main : Form
    {
        public Api ApiGrab;
        public CoreFunctions CoreFunctions;
        private List<Records> AvatarList;
        private List<Records> localAvatars;
        public bool locked;
        public Thread imageThread;
        public int avatarCount;
        public Records selectedAvatar;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ApiGrab = new Api();
            CoreFunctions = new CoreFunctions();
            lblStatsAmount.Text = ApiGrab.getStats().Total_database_size;
            cbSearchTerm.SelectedIndex = 3;
            cbVersionUnity.SelectedIndex = 0;
            MessageBoxManager.Yes = "Quest";
            MessageBoxManager.No = "PC";
            MessageBoxManager.Register();
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
            } else
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
                    PictureBox avatarImage = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(148, 146) };
                    Bitmap bitmap = CoreFunctions.loadImage(item.ThumbnailURL);

                    if (bitmap != null)
                    {
                        avatarImage.Image = bitmap;
                        avatarImage.Name = item.AvatarID;
                        avatarImage.Click += LoadInfo;
                        if (flowAvatars.InvokeRequired)
                        {
                            flowAvatars.Invoke((MethodInvoker)delegate
                            {
                                flowAvatars.Controls.Add(avatarImage);
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
            var img = (PictureBox)sender;
            selectedAvatar = AvatarList.Find(x => x.AvatarID == img.Name);
            txtAvatarInfo.Text = CoreFunctions.SetAvatarInfo(selectedAvatar);


            Bitmap bitmap; bitmap = CoreFunctions.loadImage(selectedAvatar.ThumbnailURL);

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
            if (selectedAvatar.QuestAssetURL != "None")
            {
                string[] version = selectedAvatar.QuestAssetURL.Split('/');
                nmQuestVersion.Value = Convert.ToInt32(version[7]);
            } else
            {
                nmQuestVersion.Value = 0;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (selectedAvatar != null)
            {

                DialogResult dlgResult = MessageBox.Show("Select which version to download", "VRCA Select", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dlgResult == DialogResult.Yes)
                {
                    if (selectedAvatar.QuestAssetURL != "None")
                    {
                        string[] version = selectedAvatar.QuestAssetURL.Split('/');
                        version[7] = nmQuestVersion.Value.ToString();
                        downloadFile(string.Join("/", version), "custom.vrca");
                    }
                    else
                    {
                        MessageBox.Show("Quest version doesn't exist");
                        return;
                    }
                }
                else if (dlgResult == DialogResult.No)
                {
                    if (selectedAvatar.PCAssetURL != "None")
                    {
                        string[] version = selectedAvatar.PCAssetURL.Split('/');
                        version[7] = nmPcVersion.Value.ToString();
                        downloadFile(string.Join("/", version), "custom.vrca");
                    }
                    else
                    {
                        MessageBox.Show("PC version doesn't exist");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an avatar first.");
            }
        }



        private void btnExtractVRCA_Click(object sender, EventArgs e)
        {
            if (selectedAvatar != null)
            {
                
                DialogResult dlgResult = MessageBox.Show("Select which version to download", "VRCA Select", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dlgResult == DialogResult.Yes)
                {
                    if (selectedAvatar.QuestAssetURL != "None")
                    {
                        string[] version = selectedAvatar.QuestAssetURL.Split('/');
                        version[7] = nmQuestVersion.Value.ToString();
                        downloadFile(string.Join("/", version), "custom.vrca");
                    } else
                    {
                        MessageBox.Show("Quest version doesn't exist");
                        return;
                    }
                }
                else if (dlgResult == DialogResult.No)
                {
                    if (selectedAvatar.PCAssetURL != "None")
                    {
                        string[] version = selectedAvatar.PCAssetURL.Split('/');
                        version[7] = nmPcVersion.Value.ToString();
                        downloadFile(string.Join("/", version), "custom.vrca");
                    }
                    else
                    {
                        MessageBox.Show("PC version doesn't exist");
                        return;
                    }
                }
                else
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
                   if(ex.Message == "(404) Not Found")
                    {
                        MessageBox.Show("Version doesn't exist or file has been deleted from VRChat servers");
                    }
                }
            }
        }

        private void btnStopSearch_Click(object sender, EventArgs e)
        {
            imageThread.Abort();
        }

        private void btnLoadAvatars_Click(object sender, EventArgs e)
        {
            localAvatars = CoreFunctions.getLocalAvatars();
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
    }
}
