
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.txtSearchTerm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.progress = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatsAmount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStats = new System.Windows.Forms.ToolStripStatusLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.flowAvatars = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtAvatarInfo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbCopy = new System.Windows.Forms.ComboBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logo = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnScan = new System.Windows.Forms.Button();
            this.nmThread = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.chkLoadImages = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nmQuestVersion = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nmPcVersion = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.cbVersionUnity = new System.Windows.Forms.ComboBox();
            this.btnBrowserView = new System.Windows.Forms.Button();
            this.btnExtractVRCA = new System.Windows.Forms.Button();
            this.btnLoadVRCA = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnHsbClean = new System.Windows.Forms.Button();
            this.cbLimit = new System.Windows.Forms.ComboBox();
            this.chkQuest = new System.Windows.Forms.CheckBox();
            this.chkPC = new System.Windows.Forms.CheckBox();
            this.chkPrivate = new System.Windows.Forms.CheckBox();
            this.chkPublic = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnApi = new System.Windows.Forms.Button();
            this.btnVrcaSearch = new System.Windows.Forms.Button();
            this.btnRepair = new System.Windows.Forms.Button();
            this.btnUnity = new System.Windows.Forms.Button();
            this.btnHotswap = new System.Windows.Forms.Button();
            this.btnStopSearch = new System.Windows.Forms.Button();
            this.lblAvatarCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearchLocal = new System.Windows.Forms.Button();
            this.cbSearchTerm = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.selectedImage = new System.Windows.Forms.PictureBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pbRipped = new System.Windows.Forms.PictureBox();
            this.btnRipped = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmThread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmQuestVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmPcVersion)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRipped)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearchTerm
            // 
            this.txtSearchTerm.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchTerm.Location = new System.Drawing.Point(197, 6);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new System.Drawing.Size(550, 38);
            this.txtSearchTerm.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
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
            this.statusStrip.Location = new System.Drawing.Point(0, 677);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip.Size = new System.Drawing.Size(1345, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // progress
            // 
            this.progress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.progress.Name = "progress";
            this.progress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progress.Size = new System.Drawing.Size(100, 16);
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
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
            // lblStatsAmount
            // 
            this.lblStatsAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblStatsAmount.Name = "lblStatsAmount";
            this.lblStatsAmount.Size = new System.Drawing.Size(13, 17);
            this.lblStatsAmount.Text = "0";
            // 
            // lblStats
            // 
            this.lblStats.BackColor = System.Drawing.Color.Transparent;
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(81, 17);
            this.lblStats.Text = ":Database Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(10, 53);
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
            this.flowAvatars.Size = new System.Drawing.Size(850, 468);
            this.flowAvatars.TabIndex = 7;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(518, 50);
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
            this.txtAvatarInfo.Size = new System.Drawing.Size(258, 409);
            this.txtAvatarInfo.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbCopy);
            this.groupBox1.Controls.Add(this.btnCopy);
            this.groupBox1.Controls.Add(this.txtAvatarInfo);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(869, 190);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 468);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Avatar/World Info";
            // 
            // cbCopy
            // 
            this.cbCopy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCopy.FormattingEnabled = true;
            this.cbCopy.Items.AddRange(new object[] {
            "Time Dectected",
            "Avatar ID",
            "Avatar Name",
            "Avatar Description",
            "Author ID",
            "Author Name",
            "PC Asset URL",
            "Quest Asset URL",
            "Image URL",
            "Thumbnail URL",
            "Unity Version",
            "Release Status",
            "Tags",
            "World ID",
            "World Name"});
            this.cbCopy.Location = new System.Drawing.Point(6, 434);
            this.cbCopy.Name = "cbCopy";
            this.cbCopy.Size = new System.Drawing.Size(167, 28);
            this.cbCopy.TabIndex = 19;
            // 
            // btnCopy
            // 
            this.btnCopy.ForeColor = System.Drawing.Color.Black;
            this.btnCopy.Location = new System.Drawing.Point(179, 434);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(85, 28);
            this.btnCopy.TabIndex = 20;
            this.btnCopy.Text = "Copy Info";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.logo);
            this.panel1.Location = new System.Drawing.Point(12, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(173, 170);
            this.panel1.TabIndex = 10;
            // 
            // logo
            // 
            this.logo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(24)))), ((int)(((byte)(47)))));
            this.logo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logo.ImageLocation = "https://github.com/Dean2k/A.R.E.S/releases/latest/download/ARESLogo.png";
            this.logo.Location = new System.Drawing.Point(0, 0);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(173, 170);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 0;
            this.logo.TabStop = false;
            this.logo.Click += new System.EventHandler(this.logo_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(24)))), ((int)(((byte)(47)))));
            this.panel2.Controls.Add(this.btnScan);
            this.panel2.Controls.Add(this.nmThread);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.chkLoadImages);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.nmQuestVersion);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.nmPcVersion);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cbVersionUnity);
            this.panel2.Controls.Add(this.btnBrowserView);
            this.panel2.Controls.Add(this.btnExtractVRCA);
            this.panel2.Controls.Add(this.btnLoadVRCA);
            this.panel2.Controls.Add(this.btnDownload);
            this.panel2.Location = new System.Drawing.Point(12, 184);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(173, 481);
            this.panel2.TabIndex = 11;
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(3, 206);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(167, 43);
            this.btnScan.TabIndex = 19;
            this.btnScan.Text = "Scan Unity Package";
            this.toolTip1.SetToolTip(this.btnScan, "Scan and remove bad files from unity packages");
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // nmThread
            // 
            this.nmThread.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmThread.Location = new System.Drawing.Point(2, 426);
            this.nmThread.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmThread.Name = "nmThread";
            this.nmThread.Size = new System.Drawing.Size(167, 26);
            this.nmThread.TabIndex = 0;
            this.toolTip1.SetToolTip(this.nmThread, "Setting this number too high can cause issues");
            this.nmThread.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(4, 403);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(166, 20);
            this.label8.TabIndex = 0;
            this.label8.Text = "Max Image Threads";
            // 
            // chkLoadImages
            // 
            this.chkLoadImages.AutoSize = true;
            this.chkLoadImages.Checked = true;
            this.chkLoadImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLoadImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLoadImages.ForeColor = System.Drawing.Color.White;
            this.chkLoadImages.Location = new System.Drawing.Point(7, 454);
            this.chkLoadImages.Name = "chkLoadImages";
            this.chkLoadImages.Size = new System.Drawing.Size(148, 28);
            this.chkLoadImages.TabIndex = 0;
            this.chkLoadImages.Text = "Load Images";
            this.toolTip1.SetToolTip(this.chkLoadImages, "This will still load image on top right even if disabled");
            this.chkLoadImages.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(5, 300);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Quest Avatar Version:";
            // 
            // nmQuestVersion
            // 
            this.nmQuestVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmQuestVersion.Location = new System.Drawing.Point(3, 319);
            this.nmQuestVersion.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nmQuestVersion.Name = "nmQuestVersion";
            this.nmQuestVersion.Size = new System.Drawing.Size(167, 26);
            this.nmQuestVersion.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(5, 252);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "PC Avatar Version:";
            // 
            // nmPcVersion
            // 
            this.nmPcVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmPcVersion.Location = new System.Drawing.Point(3, 271);
            this.nmPcVersion.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nmPcVersion.Name = "nmPcVersion";
            this.nmPcVersion.Size = new System.Drawing.Size(167, 26);
            this.nmPcVersion.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(2, 345);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Unity Version:";
            // 
            // cbVersionUnity
            // 
            this.cbVersionUnity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVersionUnity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbVersionUnity.FormattingEnabled = true;
            this.cbVersionUnity.Items.AddRange(new object[] {
            "2019",
            "2018"});
            this.cbVersionUnity.Location = new System.Drawing.Point(2, 373);
            this.cbVersionUnity.Name = "cbVersionUnity";
            this.cbVersionUnity.Size = new System.Drawing.Size(167, 28);
            this.cbVersionUnity.TabIndex = 0;
            // 
            // btnBrowserView
            // 
            this.btnBrowserView.Location = new System.Drawing.Point(3, 10);
            this.btnBrowserView.Name = "btnBrowserView";
            this.btnBrowserView.Size = new System.Drawing.Size(167, 43);
            this.btnBrowserView.TabIndex = 16;
            this.btnBrowserView.Text = "Browser View";
            this.btnBrowserView.UseVisualStyleBackColor = true;
            this.btnBrowserView.Click += new System.EventHandler(this.btnBrowserView_Click);
            // 
            // btnExtractVRCA
            // 
            this.btnExtractVRCA.Location = new System.Drawing.Point(3, 157);
            this.btnExtractVRCA.Name = "btnExtractVRCA";
            this.btnExtractVRCA.Size = new System.Drawing.Size(167, 43);
            this.btnExtractVRCA.TabIndex = 15;
            this.btnExtractVRCA.Text = "Extract VRCA/VRCW";
            this.btnExtractVRCA.UseVisualStyleBackColor = true;
            this.btnExtractVRCA.Click += new System.EventHandler(this.btnExtractVRCA_Click);
            // 
            // btnLoadVRCA
            // 
            this.btnLoadVRCA.Location = new System.Drawing.Point(3, 108);
            this.btnLoadVRCA.Name = "btnLoadVRCA";
            this.btnLoadVRCA.Size = new System.Drawing.Size(167, 43);
            this.btnLoadVRCA.TabIndex = 14;
            this.btnLoadVRCA.Text = "Load VRCA/VRCW";
            this.btnLoadVRCA.UseVisualStyleBackColor = true;
            this.btnLoadVRCA.Click += new System.EventHandler(this.btnLoadVRCA_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(3, 59);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(167, 43);
            this.btnDownload.TabIndex = 13;
            this.btnDownload.Text = "Download VRCA / VRCW";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(24)))), ((int)(((byte)(47)))));
            this.panel3.Controls.Add(this.btnRipped);
            this.panel3.Controls.Add(this.btnHsbClean);
            this.panel3.Controls.Add(this.cbLimit);
            this.panel3.Controls.Add(this.chkQuest);
            this.panel3.Controls.Add(this.chkPC);
            this.panel3.Controls.Add(this.chkPrivate);
            this.panel3.Controls.Add(this.chkPublic);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.btnApi);
            this.panel3.Controls.Add(this.btnVrcaSearch);
            this.panel3.Controls.Add(this.btnRepair);
            this.panel3.Controls.Add(this.btnUnity);
            this.panel3.Controls.Add(this.btnHotswap);
            this.panel3.Controls.Add(this.btnStopSearch);
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
            this.panel3.Location = new System.Drawing.Point(191, 8);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1142, 657);
            this.panel3.TabIndex = 12;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // btnHsbClean
            // 
            this.btnHsbClean.Location = new System.Drawing.Point(399, 139);
            this.btnHsbClean.Name = "btnHsbClean";
            this.btnHsbClean.Size = new System.Drawing.Size(113, 41);
            this.btnHsbClean.TabIndex = 28;
            this.btnHsbClean.Text = "Clean HSB";
            this.toolTip1.SetToolTip(this.btnHsbClean, "This will delete the HSB folder & HSBC.rar to fix potential issues");
            this.btnHsbClean.UseVisualStyleBackColor = true;
            this.btnHsbClean.Click += new System.EventHandler(this.btnHsbClean_Click);
            // 
            // cbLimit
            // 
            this.cbLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLimit.FormattingEnabled = true;
            this.cbLimit.Items.AddRange(new object[] {
            "Max",
            "1000",
            "500",
            "250",
            "100",
            "50",
            "10",
            "1"});
            this.cbLimit.Location = new System.Drawing.Point(753, 6);
            this.cbLimit.Name = "cbLimit";
            this.cbLimit.Size = new System.Drawing.Size(110, 39);
            this.cbLimit.TabIndex = 27;
            this.toolTip1.SetToolTip(this.cbLimit, "Limit amount of results pulled back (5000 is max for API local is unlimited)");
            // 
            // chkQuest
            // 
            this.chkQuest.AutoSize = true;
            this.chkQuest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkQuest.ForeColor = System.Drawing.Color.White;
            this.chkQuest.Location = new System.Drawing.Point(197, 115);
            this.chkQuest.Name = "chkQuest";
            this.chkQuest.Size = new System.Drawing.Size(67, 20);
            this.chkQuest.TabIndex = 26;
            this.chkQuest.Text = "Quest";
            this.chkQuest.UseVisualStyleBackColor = true;
            // 
            // chkPC
            // 
            this.chkPC.AutoSize = true;
            this.chkPC.Checked = true;
            this.chkPC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPC.ForeColor = System.Drawing.Color.White;
            this.chkPC.Location = new System.Drawing.Point(197, 94);
            this.chkPC.Name = "chkPC";
            this.chkPC.Size = new System.Drawing.Size(47, 20);
            this.chkPC.TabIndex = 25;
            this.chkPC.Text = "PC";
            this.chkPC.UseVisualStyleBackColor = true;
            // 
            // chkPrivate
            // 
            this.chkPrivate.AutoSize = true;
            this.chkPrivate.Checked = true;
            this.chkPrivate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrivate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPrivate.ForeColor = System.Drawing.Color.White;
            this.chkPrivate.Location = new System.Drawing.Point(106, 115);
            this.chkPrivate.Name = "chkPrivate";
            this.chkPrivate.Size = new System.Drawing.Size(76, 20);
            this.chkPrivate.TabIndex = 24;
            this.chkPrivate.Text = "Private";
            this.chkPrivate.UseVisualStyleBackColor = true;
            // 
            // chkPublic
            // 
            this.chkPublic.AutoSize = true;
            this.chkPublic.Checked = true;
            this.chkPublic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPublic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPublic.ForeColor = System.Drawing.Color.White;
            this.chkPublic.Location = new System.Drawing.Point(106, 94);
            this.chkPublic.Name = "chkPublic";
            this.chkPublic.Size = new System.Drawing.Size(70, 20);
            this.chkPublic.TabIndex = 23;
            this.chkPublic.Text = "Public";
            this.chkPublic.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(10, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 31);
            this.label7.TabIndex = 22;
            this.label7.Text = "Filter:";
            // 
            // btnApi
            // 
            this.btnApi.Location = new System.Drawing.Point(518, 139);
            this.btnApi.Name = "btnApi";
            this.btnApi.Size = new System.Drawing.Size(113, 41);
            this.btnApi.TabIndex = 20;
            this.btnApi.Text = "Disable API";
            this.btnApi.UseVisualStyleBackColor = true;
            this.btnApi.Click += new System.EventHandler(this.btnApi_Click);
            // 
            // btnVrcaSearch
            // 
            this.btnVrcaSearch.Location = new System.Drawing.Point(637, 139);
            this.btnVrcaSearch.Name = "btnVrcaSearch";
            this.btnVrcaSearch.Size = new System.Drawing.Size(110, 41);
            this.btnVrcaSearch.TabIndex = 19;
            this.btnVrcaSearch.Text = "Attempt VRCA Search";
            this.btnVrcaSearch.UseVisualStyleBackColor = true;
            this.btnVrcaSearch.Click += new System.EventHandler(this.btnVrcaSearch_Click);
            // 
            // btnRepair
            // 
            this.btnRepair.Location = new System.Drawing.Point(753, 139);
            this.btnRepair.Name = "btnRepair";
            this.btnRepair.Size = new System.Drawing.Size(110, 41);
            this.btnRepair.TabIndex = 18;
            this.btnRepair.Text = "Repair VRCA";
            this.btnRepair.UseVisualStyleBackColor = true;
            this.btnRepair.Click += new System.EventHandler(this.btnRepair_Click);
            // 
            // btnUnity
            // 
            this.btnUnity.Location = new System.Drawing.Point(637, 94);
            this.btnUnity.Name = "btnUnity";
            this.btnUnity.Size = new System.Drawing.Size(110, 41);
            this.btnUnity.TabIndex = 17;
            this.btnUnity.Text = "Open Unity";
            this.btnUnity.UseVisualStyleBackColor = true;
            this.btnUnity.Click += new System.EventHandler(this.btnUnity_Click);
            // 
            // btnHotswap
            // 
            this.btnHotswap.Enabled = false;
            this.btnHotswap.Location = new System.Drawing.Point(753, 94);
            this.btnHotswap.Name = "btnHotswap";
            this.btnHotswap.Size = new System.Drawing.Size(110, 41);
            this.btnHotswap.TabIndex = 16;
            this.btnHotswap.Text = "Hotswap";
            this.btnHotswap.UseVisualStyleBackColor = true;
            this.btnHotswap.Click += new System.EventHandler(this.btnHotswap_Click);
            // 
            // btnStopSearch
            // 
            this.btnStopSearch.Location = new System.Drawing.Point(753, 50);
            this.btnStopSearch.Name = "btnStopSearch";
            this.btnStopSearch.Size = new System.Drawing.Size(110, 41);
            this.btnStopSearch.TabIndex = 15;
            this.btnStopSearch.Text = "Stop Search";
            this.btnStopSearch.UseVisualStyleBackColor = true;
            this.btnStopSearch.Click += new System.EventHandler(this.btnStopSearch_Click);
            // 
            // lblAvatarCount
            // 
            this.lblAvatarCount.AutoSize = true;
            this.lblAvatarCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvatarCount.ForeColor = System.Drawing.Color.White;
            this.lblAvatarCount.Location = new System.Drawing.Point(108, 152);
            this.lblAvatarCount.Name = "lblAvatarCount";
            this.lblAvatarCount.Size = new System.Drawing.Size(30, 31);
            this.lblAvatarCount.TabIndex = 14;
            this.lblAvatarCount.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 31);
            this.label3.TabIndex = 13;
            this.label3.Text = "Count:";
            // 
            // btnSearchLocal
            // 
            this.btnSearchLocal.Location = new System.Drawing.Point(637, 50);
            this.btnSearchLocal.Name = "btnSearchLocal";
            this.btnSearchLocal.Size = new System.Drawing.Size(110, 41);
            this.btnSearchLocal.TabIndex = 12;
            this.btnSearchLocal.Text = "Search Local";
            this.btnSearchLocal.UseVisualStyleBackColor = true;
            this.btnSearchLocal.Click += new System.EventHandler(this.btnSearchLocal_Click);
            // 
            // cbSearchTerm
            // 
            this.cbSearchTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSearchTerm.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSearchTerm.FormattingEnabled = true;
            this.cbSearchTerm.Items.AddRange(new object[] {
            "Avatar Name",
            "Author Name",
            "Avatar ID",
            "Author ID",
            "World Name",
            "World ID"});
            this.cbSearchTerm.Location = new System.Drawing.Point(197, 50);
            this.cbSearchTerm.Name = "cbSearchTerm";
            this.cbSearchTerm.Size = new System.Drawing.Size(315, 39);
            this.cbSearchTerm.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.selectedImage);
            this.panel4.Location = new System.Drawing.Point(869, 6);
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
            // pbRipped
            // 
            this.pbRipped.Image = global::ARES.Properties.Resources.rippeds;
            this.pbRipped.Location = new System.Drawing.Point(1425, 147);
            this.pbRipped.Name = "pbRipped";
            this.pbRipped.Size = new System.Drawing.Size(132, 75);
            this.pbRipped.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRipped.TabIndex = 13;
            this.pbRipped.TabStop = false;
            // 
            // btnRipped
            // 
            this.btnRipped.Location = new System.Drawing.Point(518, 94);
            this.btnRipped.Name = "btnRipped";
            this.btnRipped.Size = new System.Drawing.Size(113, 41);
            this.btnRipped.TabIndex = 29;
            this.btnRipped.Text = "Search Ripped";
            this.btnRipped.UseVisualStyleBackColor = true;
            this.btnRipped.Click += new System.EventHandler(this.btnRipped_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1345, 699);
            this.Controls.Add(this.pbRipped);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "A.R.E.S";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Ares_Close);
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmThread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmQuestVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmPcVersion)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.selectedImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRipped)).EndInit();
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
        private System.Windows.Forms.ComboBox cbVersionUnity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nmPcVersion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nmQuestVersion;
        private System.Windows.Forms.Button btnStopSearch;
        private System.Windows.Forms.Button btnHotswap;
        private System.Windows.Forms.Button btnUnity;
        private System.Windows.Forms.Button btnRepair;
        private System.Windows.Forms.Button btnVrcaSearch;
        private System.Windows.Forms.ComboBox cbCopy;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnApi;
        private System.Windows.Forms.CheckBox chkQuest;
        private System.Windows.Forms.CheckBox chkPC;
        private System.Windows.Forms.CheckBox chkPrivate;
        private System.Windows.Forms.CheckBox chkPublic;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbLimit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkLoadImages;
        private System.Windows.Forms.NumericUpDown nmThread;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pbRipped;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnHsbClean;
        private System.Windows.Forms.Button btnRipped;
    }
}

