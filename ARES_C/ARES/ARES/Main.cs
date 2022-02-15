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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARES
{
    public partial class Main : Form
    {
        public Api ApiGrab;
        public CoreFunctions CoreFunctions;
        private List<Records> AvatarList;

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
            flowAvatars.Controls.Clear();
            List<Records> avatars = ApiGrab.getAvatars(txtSearchTerm.Text);

            foreach (var item in avatars)
            {
                PictureBox avatarImage = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(148, 146) };
                Bitmap bitmap; bitmap = CoreFunctions.loadImage(item.ThumbnailURL);

                if (bitmap != null)
                {
                    avatarImage.Image = bitmap;
                    avatarImage.Name = item.AvatarID;
                    avatarImage.Click += LoadInfo;
                    flowAvatars.Controls.Add(avatarImage);
                }            
            }
            AvatarList = avatars;
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
