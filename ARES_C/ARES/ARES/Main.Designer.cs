
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
            this.txtSearchTerm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbAvatarName = new System.Windows.Forms.RadioButton();
            this.rbAvatarAuthor = new System.Windows.Forms.RadioButton();
            this.rbAvatarAutherId = new System.Windows.Forms.RadioButton();
            this.cbAvatarId = new System.Windows.Forms.RadioButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.flowAvatars = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtAvatarInfo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearchTerm
            // 
            this.txtSearchTerm.Location = new System.Drawing.Point(90, 12);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new System.Drawing.Size(458, 20);
            this.txtSearchTerm.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search Term:";
            // 
            // rbAvatarName
            // 
            this.rbAvatarName.AutoSize = true;
            this.rbAvatarName.Location = new System.Drawing.Point(90, 38);
            this.rbAvatarName.Name = "rbAvatarName";
            this.rbAvatarName.Size = new System.Drawing.Size(87, 17);
            this.rbAvatarName.TabIndex = 2;
            this.rbAvatarName.TabStop = true;
            this.rbAvatarName.Text = "Avatar Name";
            this.rbAvatarName.UseVisualStyleBackColor = true;
            // 
            // rbAvatarAuthor
            // 
            this.rbAvatarAuthor.AutoSize = true;
            this.rbAvatarAuthor.Location = new System.Drawing.Point(183, 38);
            this.rbAvatarAuthor.Name = "rbAvatarAuthor";
            this.rbAvatarAuthor.Size = new System.Drawing.Size(90, 17);
            this.rbAvatarAuthor.TabIndex = 3;
            this.rbAvatarAuthor.TabStop = true;
            this.rbAvatarAuthor.Text = "Avatar Author";
            this.rbAvatarAuthor.UseVisualStyleBackColor = true;
            // 
            // rbAvatarAutherId
            // 
            this.rbAvatarAutherId.AutoSize = true;
            this.rbAvatarAutherId.Location = new System.Drawing.Point(279, 38);
            this.rbAvatarAutherId.Name = "rbAvatarAutherId";
            this.rbAvatarAutherId.Size = new System.Drawing.Size(102, 17);
            this.rbAvatarAutherId.TabIndex = 4;
            this.rbAvatarAutherId.TabStop = true;
            this.rbAvatarAutherId.Text = "Avatar Author Id";
            this.rbAvatarAutherId.UseVisualStyleBackColor = true;
            // 
            // cbAvatarId
            // 
            this.cbAvatarId.AutoSize = true;
            this.cbAvatarId.Location = new System.Drawing.Point(387, 38);
            this.cbAvatarId.Name = "cbAvatarId";
            this.cbAvatarId.Size = new System.Drawing.Size(68, 17);
            this.cbAvatarId.TabIndex = 5;
            this.cbAvatarId.TabStop = true;
            this.cbAvatarId.Text = "Avatar Id";
            this.cbAvatarId.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip.Location = new System.Drawing.Point(0, 491);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1279, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(64, 17);
            this.statusLabel.Text = "Status: Idle";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(90, 17);
            this.toolStripStatusLabel2.Text = "Database Size: 0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Search Type:";
            // 
            // flowAvatars
            // 
            this.flowAvatars.AutoScroll = true;
            this.flowAvatars.Location = new System.Drawing.Point(16, 61);
            this.flowAvatars.Name = "flowAvatars";
            this.flowAvatars.Padding = new System.Windows.Forms.Padding(10);
            this.flowAvatars.Size = new System.Drawing.Size(812, 394);
            this.flowAvatars.TabIndex = 7;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(564, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(71, 20);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
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
            this.txtAvatarInfo.Size = new System.Drawing.Size(421, 369);
            this.txtAvatarInfo.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAvatarInfo);
            this.groupBox1.Location = new System.Drawing.Point(834, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 402);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Avatar Info";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 513);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.flowAvatars);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbAvatarAuthor);
            this.Controls.Add(this.cbAvatarId);
            this.Controls.Add(this.txtSearchTerm);
            this.Controls.Add(this.rbAvatarAutherId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbAvatarName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "A.R.E.S V11";
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton cbAvatarId;
        private System.Windows.Forms.RadioButton rbAvatarAutherId;
        private System.Windows.Forms.RadioButton rbAvatarAuthor;
        private System.Windows.Forms.RadioButton rbAvatarName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchTerm;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowAvatars;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtAvatarInfo;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

