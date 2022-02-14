using ARES.Models;
using ARES.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARES
{
    public partial class Main : Form
    {
        public Api ApiGrab;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ApiGrab = new Api();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<Records> avatars = ApiGrab.getAvatars(txtSearchTerm.Text);

            foreach (var item in avatars)
            {
                PictureBox avatarImage = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(148, 146) };
                try
                {
                    
                } catch
                {

                }
                flowAvatars.Controls.Add(avatarImage);
            }
        }
    }
}
