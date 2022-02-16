using ARES.Models;
using ARES.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
        public bool locked;
        public Thread imageThread;
        public int avatarCount;

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
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!locked)
            {
                flowAvatars.Controls.Clear();
                statusLabel.Text = "Status: Loading API";
                List<Records> avatars = ApiGrab.getAvatars(txtSearchTerm.Text);
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
        }

        public void GetImages()
        {
            try
            {
                foreach (var item in AvatarList)
                {
                    PictureBox avatarImage = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(148, 146) };
                    Bitmap bitmap; bitmap = CoreFunctions.loadImage(item.ThumbnailURL);

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
                        
                    } else
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
                        progress.GetCurrentParent().Invoke(new MethodInvoker(delegate { progress.Value ++; }));
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
            Records avatar = AvatarList.Find(x => x.AvatarID == img.Name);
            txtAvatarInfo.Text = CoreFunctions.SetAvatarInfo(avatar);


            Bitmap bitmap; bitmap = CoreFunctions.loadImage(avatar.ThumbnailURL);

            if (bitmap != null)
            {
                selectedImage.Image = bitmap;
            }
        }
    }
}
