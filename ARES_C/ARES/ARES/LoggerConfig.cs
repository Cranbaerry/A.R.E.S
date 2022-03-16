using ARES.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARES
{
    public partial class LoggerConfig : Form
    {
        private AresConfig config;
        private string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"\GUI", @"\UserData") + @"\ARESConfig.json";
        private bool loading = true;
        public LoggerConfig()
        {
            InitializeComponent();
        }

        private void LoggerConfig_Load(object sender, EventArgs e)
        {

            LoadConfig();
            if (config != null)
            {
                SetCheckBoxes();
            }
            loading = false;
        }

        private void LoadConfig()
        {
            try
            {
                string json = File.ReadAllText(fileLocation);
                config = JsonConvert.DeserializeObject<AresConfig>(json);
            }
            catch { }
        }

        private void SetCheckBoxes()
        {
            cbConsoleError.Checked = config.ConsoleError;
            cbStealth.Checked = config.Stealth;
            cbHWIDSpoof.Checked = config.HWIDSpoof;
            cbLogAvatars.Checked = config.LogAvatars;
            cbLogFriendsAvatars.Checked = config.LogFriendsAvatars;
            cbLogOwnAvatars.Checked = config.LogOwnAvatars;
            cbLogPrivateAvatars.Checked = config.LogPrivateAvatars;
            cbLogPublicAvatars.Checked = config.LogPublicAvatars;
            cbLogToConsole.Checked = config.LogToConsole;
            cbLogWorlds.Checked = config.LogWorlds;
            cbUnlimitedFavorites.Checked = config.UnlimitedFavorites;
        }

        private void WriteConfig()
        {
            if (!loading)
            {
                string json = JsonConvert.SerializeObject(config);
                File.WriteAllText(fileLocation, json);
            }
        }

        private void cbUnlimitedFavorites_CheckedChanged(object sender, EventArgs e)
        {
            config.UnlimitedFavorites = cbUnlimitedFavorites.Checked;
            WriteConfig();
        }

        private void cbStealth_CheckedChanged(object sender, EventArgs e)
        {
            config.Stealth = cbStealth.Checked;
            WriteConfig();
        }

        private void cbLogAvatars_CheckedChanged(object sender, EventArgs e)
        {
            config.LogAvatars = cbLogAvatars.Checked;
            WriteConfig();
        }

        private void cbLogWorlds_CheckedChanged(object sender, EventArgs e)
        {
            config.LogWorlds = cbLogWorlds.Checked;
            WriteConfig();
        }

        private void cbLogFriendsAvatars_CheckedChanged(object sender, EventArgs e)
        {
            config.LogFriendsAvatars = cbLogFriendsAvatars.Checked;
            WriteConfig();
        }

        private void cbLogOwnAvatars_CheckedChanged(object sender, EventArgs e)
        {
            config.LogOwnAvatars = cbLogOwnAvatars.Checked;
            WriteConfig();
        }

        private void cbLogPublicAvatars_CheckedChanged(object sender, EventArgs e)
        {
            config.LogPublicAvatars = cbLogPublicAvatars.Checked;
            WriteConfig();
        }

        private void cbLogPrivateAvatars_CheckedChanged(object sender, EventArgs e)
        {
            config.LogPrivateAvatars = cbLogPrivateAvatars.Checked;
            WriteConfig();
        }

        private void cbLogToConsole_CheckedChanged(object sender, EventArgs e)
        {
            config.LogToConsole = cbLogToConsole.Checked;
            WriteConfig();
        }

        private void cbConsoleError_CheckedChanged(object sender, EventArgs e)
        {
            config.ConsoleError = cbConsoleError.Checked;
            WriteConfig();
        }

        private void cbHWIDSpoof_CheckedChanged(object sender, EventArgs e)
        {
            config.HWIDSpoof = cbHWIDSpoof.Checked;
            WriteConfig();
        }
    }
}
