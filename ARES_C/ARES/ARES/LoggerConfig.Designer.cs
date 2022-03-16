
namespace ARES
{
    partial class LoggerConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbUnlimitedFavorites = new System.Windows.Forms.CheckBox();
            this.cbStealth = new System.Windows.Forms.CheckBox();
            this.cbLogAvatars = new System.Windows.Forms.CheckBox();
            this.cbLogWorlds = new System.Windows.Forms.CheckBox();
            this.cbLogFriendsAvatars = new System.Windows.Forms.CheckBox();
            this.cbLogOwnAvatars = new System.Windows.Forms.CheckBox();
            this.cbLogPublicAvatars = new System.Windows.Forms.CheckBox();
            this.cbLogPrivateAvatars = new System.Windows.Forms.CheckBox();
            this.cbLogToConsole = new System.Windows.Forms.CheckBox();
            this.cbConsoleError = new System.Windows.Forms.CheckBox();
            this.cbHWIDSpoof = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbUnlimitedFavorites
            // 
            this.cbUnlimitedFavorites.AutoSize = true;
            this.cbUnlimitedFavorites.Location = new System.Drawing.Point(30, 24);
            this.cbUnlimitedFavorites.Name = "cbUnlimitedFavorites";
            this.cbUnlimitedFavorites.Size = new System.Drawing.Size(115, 17);
            this.cbUnlimitedFavorites.TabIndex = 0;
            this.cbUnlimitedFavorites.Text = "Unlimited Favorites";
            this.cbUnlimitedFavorites.UseVisualStyleBackColor = true;
            this.cbUnlimitedFavorites.CheckedChanged += new System.EventHandler(this.cbUnlimitedFavorites_CheckedChanged);
            // 
            // cbStealth
            // 
            this.cbStealth.AutoSize = true;
            this.cbStealth.Location = new System.Drawing.Point(30, 47);
            this.cbStealth.Name = "cbStealth";
            this.cbStealth.Size = new System.Drawing.Size(59, 17);
            this.cbStealth.TabIndex = 1;
            this.cbStealth.Text = "Stealth";
            this.cbStealth.UseVisualStyleBackColor = true;
            this.cbStealth.CheckedChanged += new System.EventHandler(this.cbStealth_CheckedChanged);
            // 
            // cbLogAvatars
            // 
            this.cbLogAvatars.AutoSize = true;
            this.cbLogAvatars.Location = new System.Drawing.Point(30, 70);
            this.cbLogAvatars.Name = "cbLogAvatars";
            this.cbLogAvatars.Size = new System.Drawing.Size(83, 17);
            this.cbLogAvatars.TabIndex = 2;
            this.cbLogAvatars.Text = "Log Avatars";
            this.cbLogAvatars.UseVisualStyleBackColor = true;
            this.cbLogAvatars.CheckedChanged += new System.EventHandler(this.cbLogAvatars_CheckedChanged);
            // 
            // cbLogWorlds
            // 
            this.cbLogWorlds.AutoSize = true;
            this.cbLogWorlds.Location = new System.Drawing.Point(30, 93);
            this.cbLogWorlds.Name = "cbLogWorlds";
            this.cbLogWorlds.Size = new System.Drawing.Size(80, 17);
            this.cbLogWorlds.TabIndex = 3;
            this.cbLogWorlds.Text = "Log Worlds";
            this.cbLogWorlds.UseVisualStyleBackColor = true;
            this.cbLogWorlds.CheckedChanged += new System.EventHandler(this.cbLogWorlds_CheckedChanged);
            // 
            // cbLogFriendsAvatars
            // 
            this.cbLogFriendsAvatars.AutoSize = true;
            this.cbLogFriendsAvatars.Location = new System.Drawing.Point(30, 116);
            this.cbLogFriendsAvatars.Name = "cbLogFriendsAvatars";
            this.cbLogFriendsAvatars.Size = new System.Drawing.Size(120, 17);
            this.cbLogFriendsAvatars.TabIndex = 4;
            this.cbLogFriendsAvatars.Text = "Log Friends Avatars";
            this.cbLogFriendsAvatars.UseVisualStyleBackColor = true;
            this.cbLogFriendsAvatars.CheckedChanged += new System.EventHandler(this.cbLogFriendsAvatars_CheckedChanged);
            // 
            // cbLogOwnAvatars
            // 
            this.cbLogOwnAvatars.AutoSize = true;
            this.cbLogOwnAvatars.Location = new System.Drawing.Point(30, 139);
            this.cbLogOwnAvatars.Name = "cbLogOwnAvatars";
            this.cbLogOwnAvatars.Size = new System.Drawing.Size(108, 17);
            this.cbLogOwnAvatars.TabIndex = 5;
            this.cbLogOwnAvatars.Text = "Log Own Avatars";
            this.cbLogOwnAvatars.UseVisualStyleBackColor = true;
            this.cbLogOwnAvatars.CheckedChanged += new System.EventHandler(this.cbLogOwnAvatars_CheckedChanged);
            // 
            // cbLogPublicAvatars
            // 
            this.cbLogPublicAvatars.AutoSize = true;
            this.cbLogPublicAvatars.Location = new System.Drawing.Point(30, 162);
            this.cbLogPublicAvatars.Name = "cbLogPublicAvatars";
            this.cbLogPublicAvatars.Size = new System.Drawing.Size(115, 17);
            this.cbLogPublicAvatars.TabIndex = 6;
            this.cbLogPublicAvatars.Text = "Log Public Avatars";
            this.cbLogPublicAvatars.UseVisualStyleBackColor = true;
            this.cbLogPublicAvatars.CheckedChanged += new System.EventHandler(this.cbLogPublicAvatars_CheckedChanged);
            // 
            // cbLogPrivateAvatars
            // 
            this.cbLogPrivateAvatars.AutoSize = true;
            this.cbLogPrivateAvatars.Location = new System.Drawing.Point(30, 185);
            this.cbLogPrivateAvatars.Name = "cbLogPrivateAvatars";
            this.cbLogPrivateAvatars.Size = new System.Drawing.Size(119, 17);
            this.cbLogPrivateAvatars.TabIndex = 7;
            this.cbLogPrivateAvatars.Text = "Log Private Avatars";
            this.cbLogPrivateAvatars.UseVisualStyleBackColor = true;
            this.cbLogPrivateAvatars.CheckedChanged += new System.EventHandler(this.cbLogPrivateAvatars_CheckedChanged);
            // 
            // cbLogToConsole
            // 
            this.cbLogToConsole.AutoSize = true;
            this.cbLogToConsole.Location = new System.Drawing.Point(30, 208);
            this.cbLogToConsole.Name = "cbLogToConsole";
            this.cbLogToConsole.Size = new System.Drawing.Size(101, 17);
            this.cbLogToConsole.TabIndex = 8;
            this.cbLogToConsole.Text = "Log To Console";
            this.cbLogToConsole.UseVisualStyleBackColor = true;
            this.cbLogToConsole.CheckedChanged += new System.EventHandler(this.cbLogToConsole_CheckedChanged);
            // 
            // cbConsoleError
            // 
            this.cbConsoleError.AutoSize = true;
            this.cbConsoleError.Location = new System.Drawing.Point(30, 231);
            this.cbConsoleError.Name = "cbConsoleError";
            this.cbConsoleError.Size = new System.Drawing.Size(89, 17);
            this.cbConsoleError.TabIndex = 9;
            this.cbConsoleError.Text = "Console Error";
            this.cbConsoleError.UseVisualStyleBackColor = true;
            this.cbConsoleError.CheckedChanged += new System.EventHandler(this.cbConsoleError_CheckedChanged);
            // 
            // cbHWIDSpoof
            // 
            this.cbHWIDSpoof.AutoSize = true;
            this.cbHWIDSpoof.Location = new System.Drawing.Point(30, 254);
            this.cbHWIDSpoof.Name = "cbHWIDSpoof";
            this.cbHWIDSpoof.Size = new System.Drawing.Size(87, 17);
            this.cbHWIDSpoof.TabIndex = 10;
            this.cbHWIDSpoof.Text = "HWID Spoof";
            this.cbHWIDSpoof.UseVisualStyleBackColor = true;
            this.cbHWIDSpoof.CheckedChanged += new System.EventHandler(this.cbHWIDSpoof_CheckedChanged);
            // 
            // LoggerConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 298);
            this.Controls.Add(this.cbHWIDSpoof);
            this.Controls.Add(this.cbConsoleError);
            this.Controls.Add(this.cbLogToConsole);
            this.Controls.Add(this.cbLogPrivateAvatars);
            this.Controls.Add(this.cbLogPublicAvatars);
            this.Controls.Add(this.cbLogOwnAvatars);
            this.Controls.Add(this.cbLogFriendsAvatars);
            this.Controls.Add(this.cbLogWorlds);
            this.Controls.Add(this.cbLogAvatars);
            this.Controls.Add(this.cbStealth);
            this.Controls.Add(this.cbUnlimitedFavorites);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LoggerConfig";
            this.Text = "Logger Config";
            this.Load += new System.EventHandler(this.LoggerConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbUnlimitedFavorites;
        private System.Windows.Forms.CheckBox cbStealth;
        private System.Windows.Forms.CheckBox cbLogAvatars;
        private System.Windows.Forms.CheckBox cbLogWorlds;
        private System.Windows.Forms.CheckBox cbLogFriendsAvatars;
        private System.Windows.Forms.CheckBox cbLogOwnAvatars;
        private System.Windows.Forms.CheckBox cbLogPublicAvatars;
        private System.Windows.Forms.CheckBox cbLogPrivateAvatars;
        private System.Windows.Forms.CheckBox cbLogToConsole;
        private System.Windows.Forms.CheckBox cbConsoleError;
        private System.Windows.Forms.CheckBox cbHWIDSpoof;
    }
}