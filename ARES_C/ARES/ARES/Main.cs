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

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ApiGrab = new Api();
            CoreFunctions = new CoreFunctions();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<Records> avatars = ApiGrab.getAvatars(txtSearchTerm.Text);

            foreach (var item in avatars)
            {
                PictureBox avatarImage = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(148, 146) };

                using (WebClient webClient = new WebClient())
                {

                    webClient.Headers.Add("user-agent", "VRCX");
                    try
                    {
                        Stream stream = webClient.OpenRead(item.ThumbnailURL);
                        Bitmap bitmap; bitmap = new Bitmap(stream);

                        if (bitmap != null)
                        {
                            avatarImage.Image = bitmap;
                        }
                    }
                    catch (WebException ex)
                    {
                        avatarImage.Load("https://image.freepik.com/free-vector/glitch-error-404-page_23-2148105404.jpg");
                    }

                    txtAvatarInfo.Text = CoreFunctions.SetAvatarInfo(item);
                    //byte[] data = webClient.DownloadData(item.ThumbnailURL);
                    //    using (MemoryStream mem = new MemoryStream(data))
                    //    {
                    //        using (var yourImage = Image.FromStream(mem))
                    //        {
                    //            avatarImage.Image = yourImage.;
                    //        }
                    //    }
                    //}

                    flowAvatars.Controls.Add(avatarImage);
                }
            }
        }
    }
}
