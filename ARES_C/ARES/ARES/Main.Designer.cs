﻿
namespace ARES
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.txtSearchTerm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStats = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatsAmount = new System.Windows.Forms.ToolStripStatusLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.flowAvatars = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtAvatarInfo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logo = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBrowserView = new System.Windows.Forms.Button();
            this.btnExtractVRCA = new System.Windows.Forms.Button();
            this.btnLoadVRCA = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnLoadAvatars = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearchLocal = new System.Windows.Forms.Button();
            this.cbSearchTerm = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.selectedImage = new System.Windows.Forms.PictureBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAvatarCount = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearchTerm
            // 
            this.txtSearchTerm.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchTerm.Location = new System.Drawing.Point(197, 6);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new System.Drawing.Size(490, 38);
            this.txtSearchTerm.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search Term:";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progress,
            this.statusLabel,
            this.toolStripStatusLabel1,
            this.lblStatsAmount,
            this.lblStats});
            this.statusStrip.Location = new System.Drawing.Point(0, 646);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip.Size = new System.Drawing.Size(1175, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(64, 17);
            this.statusLabel.Text = "Status: Idle";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // lblStats
            // 
            this.lblStats.BackColor = System.Drawing.Color.Transparent;
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(81, 17);
            this.lblStats.Text = ":Database Size";
            // 
            // lblStatsAmount
            // 
            this.lblStatsAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblStatsAmount.Name = "lblStatsAmount";
            this.lblStatsAmount.Size = new System.Drawing.Size(13, 17);
            this.lblStatsAmount.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 31);
            this.label2.TabIndex = 6;
            this.label2.Text = "Search Type:";
            // 
            // flowAvatars
            // 
            this.flowAvatars.AutoScroll = true;
            this.flowAvatars.Location = new System.Drawing.Point(13, 186);
            this.flowAvatars.Name = "flowAvatars";
            this.flowAvatars.Padding = new System.Windows.Forms.Padding(10);
            this.flowAvatars.Size = new System.Drawing.Size(674, 435);
            this.flowAvatars.TabIndex = 7;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(458, 50);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(113, 41);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search Api";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtAvatarInfo
            // 
            this.txtAvatarInfo.Location = new System.Drawing.Point(6, 19);
            this.txtAvatarInfo.Multiline = true;
            this.txtAvatarInfo.Name = "txtAvatarInfo";
            this.txtAvatarInfo.ReadOnly = true;
            this.txtAvatarInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAvatarInfo.Size = new System.Drawing.Size(258, 410);
            this.txtAvatarInfo.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAvatarInfo);
            this.groupBox1.Location = new System.Drawing.Point(693, 186);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 435);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Avatar Info";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.logo);
            this.panel1.Location = new System.Drawing.Point(12, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(173, 170);
            this.panel1.TabIndex = 10;
            // 
            // logo
            // 
            this.logo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logo.ImageLocation = "https://github.com/Dean2k/A.R.E.S/releases/latest/download/ARESLogo.png";
            this.logo.Location = new System.Drawing.Point(0, 0);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(173, 170);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 0;
            this.logo.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnBrowserView);
            this.panel2.Controls.Add(this.btnExtractVRCA);
            this.panel2.Controls.Add(this.btnLoadVRCA);
            this.panel2.Controls.Add(this.btnDownload);
            this.panel2.Controls.Add(this.btnLoadAvatars);
            this.panel2.Location = new System.Drawing.Point(12, 182);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(173, 448);
            this.panel2.TabIndex = 11;
            // 
            // btnBrowserView
            // 
            this.btnBrowserView.Location = new System.Drawing.Point(3, 206);
            this.btnBrowserView.Name = "btnBrowserView";
            this.btnBrowserView.Size = new System.Drawing.Size(167, 43);
            this.btnBrowserView.TabIndex = 16;
            this.btnBrowserView.Text = "Brower View";
            this.btnBrowserView.UseVisualStyleBackColor = true;
            // 
            // btnExtractVRCA
            // 
            this.btnExtractVRCA.Location = new System.Drawing.Point(3, 157);
            this.btnExtractVRCA.Name = "btnExtractVRCA";
            this.btnExtractVRCA.Size = new System.Drawing.Size(167, 43);
            this.btnExtractVRCA.TabIndex = 15;
            this.btnExtractVRCA.Text = "Extract VRCA";
            this.btnExtractVRCA.UseVisualStyleBackColor = true;
            // 
            // btnLoadVRCA
            // 
            this.btnLoadVRCA.Location = new System.Drawing.Point(3, 108);
            this.btnLoadVRCA.Name = "btnLoadVRCA";
            this.btnLoadVRCA.Size = new System.Drawing.Size(167, 43);
            this.btnLoadVRCA.TabIndex = 14;
            this.btnLoadVRCA.Text = "Load VRCA";
            this.btnLoadVRCA.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(3, 59);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(167, 43);
            this.btnDownload.TabIndex = 13;
            this.btnDownload.Text = "Download VRCA";
            this.btnDownload.UseVisualStyleBackColor = true;
            // 
            // btnLoadAvatars
            // 
            this.btnLoadAvatars.Location = new System.Drawing.Point(3, 10);
            this.btnLoadAvatars.Name = "btnLoadAvatars";
            this.btnLoadAvatars.Size = new System.Drawing.Size(167, 43);
            this.btnLoadAvatars.TabIndex = 11;
            this.btnLoadAvatars.Text = "Load Avatars";
            this.btnLoadAvatars.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblAvatarCount);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.btnSearchLocal);
            this.panel3.Controls.Add(this.cbSearchTerm);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.txtSearchTerm);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.flowAvatars);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(191, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(972, 624);
            this.panel3.TabIndex = 12;
            // 
            // btnSearchLocal
            // 
            this.btnSearchLocal.Location = new System.Drawing.Point(577, 50);
            this.btnSearchLocal.Name = "btnSearchLocal";
            this.btnSearchLocal.Size = new System.Drawing.Size(110, 41);
            this.btnSearchLocal.TabIndex = 12;
            this.btnSearchLocal.Text = "Search Local";
            this.btnSearchLocal.UseVisualStyleBackColor = true;
            // 
            // cbSearchTerm
            // 
            this.cbSearchTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSearchTerm.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSearchTerm.FormattingEnabled = true;
            this.cbSearchTerm.Items.AddRange(new object[] {
            "Author ID",
            "Author Name",
            "Avatar ID",
            "Avatar Name"});
            this.cbSearchTerm.Location = new System.Drawing.Point(197, 50);
            this.cbSearchTerm.Name = "cbSearchTerm";
            this.cbSearchTerm.Size = new System.Drawing.Size(255, 39);
            this.cbSearchTerm.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.selectedImage);
            this.panel4.Location = new System.Drawing.Point(693, 9);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(270, 171);
            this.panel4.TabIndex = 10;
            // 
            // selectedImage
            // 
            this.selectedImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedImage.Location = new System.Drawing.Point(0, 0);
            this.selectedImage.Name = "selectedImage";
            this.selectedImage.Size = new System.Drawing.Size(270, 171);
            this.selectedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.selectedImage.TabIndex = 0;
            this.selectedImage.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 31);
            this.label3.TabIndex = 13;
            this.label3.Text = "Avatar Count:";
            // 
            // lblAvatarCount
            // 
            this.lblAvatarCount.AutoSize = true;
            this.lblAvatarCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvatarCount.Location = new System.Drawing.Point(200, 152);
            this.lblAvatarCount.Name = "lblAvatarCount";
            this.lblAvatarCount.Size = new System.Drawing.Size(30, 31);
            this.lblAvatarCount.TabIndex = 14;
            this.lblAvatarCount.Text = "0";
            // 
            // progress
            // 
            this.progress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.progress.Name = "progress";
            this.progress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progress.Size = new System.Drawing.Size(100, 16);
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(54)))), ((int)(((byte)(135)))));
            this.ClientSize = new System.Drawing.Size(1175, 668);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "A.R.E.S V11";
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.selectedImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchTerm;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowAvatars;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblStats;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtAvatarInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox selectedImage;
        private System.Windows.Forms.ToolStripStatusLabel lblStatsAmount;
        private System.Windows.Forms.Button btnLoadAvatars;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ComboBox cbSearchTerm;
        private System.Windows.Forms.Button btnBrowserView;
        private System.Windows.Forms.Button btnExtractVRCA;
        private System.Windows.Forms.Button btnLoadVRCA;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnSearchLocal;
        private System.Windows.Forms.Label lblAvatarCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripProgressBar progress;
        public System.Windows.Forms.StatusStrip statusStrip;
    }
}
