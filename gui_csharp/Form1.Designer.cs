namespace AndroidToolsGUI
{
    partial class Form1
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
            this.tabTools = new System.Windows.Forms.TabControl();
            this.tabFastboot = new System.Windows.Forms.TabPage();
            this.grpFastboot = new System.Windows.Forms.GroupBox();
            this.lblFastbootDesc = new System.Windows.Forms.Label();
            this.btnStopFastboot = new System.Windows.Forms.Button();
            this.btnStartFastboot = new System.Windows.Forms.Button();
            this.tabMtk = new System.Windows.Forms.TabPage();
            this.grpMtk = new System.Windows.Forms.GroupBox();
            this.lblMtkDesc = new System.Windows.Forms.Label();
            this.btnStopMtk = new System.Windows.Forms.Button();
            this.btnStartMtk = new System.Windows.Forms.Button();
            this.tabExtractor = new System.Windows.Forms.TabPage();
            this.grpExtractor = new System.Windows.Forms.GroupBox();
            this.lblFirmwarePath = new System.Windows.Forms.Label();
            this.btnExtract = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFirmwarePath = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.lblLogTitle = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.tabTools.SuspendLayout();
            this.tabFastboot.SuspendLayout();
            this.grpFastboot.SuspendLayout();
            this.tabMtk.SuspendLayout();
            this.grpMtk.SuspendLayout();
            this.tabExtractor.SuspendLayout();
            this.grpExtractor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // tabTools
            // 
            this.tabTools.Controls.Add(this.tabFastboot);
            this.tabTools.Controls.Add(this.tabMtk);
            this.tabTools.Controls.Add(this.tabExtractor);
            this.tabTools.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTools.Location = new System.Drawing.Point(12, 70);
            this.tabTools.Name = "tabTools";
            this.tabTools.SelectedIndex = 0;
            this.tabTools.Size = new System.Drawing.Size(360, 379);
            this.tabTools.TabIndex = 0;
            // 
            // tabFastboot
            // 
            this.tabFastboot.BackColor = System.Drawing.SystemColors.Control;
            this.tabFastboot.Controls.Add(this.grpFastboot);
            this.tabFastboot.Location = new System.Drawing.Point(4, 22);
            this.tabFastboot.Name = "tabFastboot";
            this.tabFastboot.Padding = new System.Windows.Forms.Padding(3);
            this.tabFastboot.Size = new System.Drawing.Size(352, 411);
            this.tabFastboot.TabIndex = 0;
            this.tabFastboot.Text = "Fastboot";
            // 
            // grpFastboot
            // 
            this.grpFastboot.Controls.Add(this.lblFastbootDesc);
            this.grpFastboot.Controls.Add(this.btnStopFastboot);
            this.grpFastboot.Controls.Add(this.btnStartFastboot);
            this.grpFastboot.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFastboot.Location = new System.Drawing.Point(6, 6);
            this.grpFastboot.Name = "grpFastboot";
            this.grpFastboot.Size = new System.Drawing.Size(340, 120);
            this.grpFastboot.TabIndex = 0;
            this.grpFastboot.TabStop = false;
            this.grpFastboot.Text = "Fastboot Monitor Settings";
            // 
            // lblFastbootDesc
            // 
            this.lblFastbootDesc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFastbootDesc.Location = new System.Drawing.Point(15, 23);
            this.lblFastbootDesc.Name = "lblFastbootDesc";
            this.lblFastbootDesc.Size = new System.Drawing.Size(310, 32);
            this.lblFastbootDesc.TabIndex = 2;
            this.lblFastbootDesc.Text = "Start background monitoring for Android devices connected via Fastboot mode.";
            // 
            // btnStopFastboot
            // 
            this.btnStopFastboot.Enabled = false;
            this.btnStopFastboot.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopFastboot.Location = new System.Drawing.Point(125, 68);
            this.btnStopFastboot.Name = "btnStopFastboot";
            this.btnStopFastboot.Size = new System.Drawing.Size(100, 25);
            this.btnStopFastboot.TabIndex = 1;
            this.btnStopFastboot.Text = "Stop Monitor";
            this.btnStopFastboot.UseVisualStyleBackColor = true;
            this.btnStopFastboot.Click += new System.EventHandler(this.btnStopFastboot_Click);
            // 
            // btnStartFastboot
            // 
            this.btnStartFastboot.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartFastboot.Location = new System.Drawing.Point(18, 68);
            this.btnStartFastboot.Name = "btnStartFastboot";
            this.btnStartFastboot.Size = new System.Drawing.Size(100, 25);
            this.btnStartFastboot.TabIndex = 0;
            this.btnStartFastboot.Text = "Start Monitor";
            this.btnStartFastboot.UseVisualStyleBackColor = true;
            this.btnStartFastboot.Click += new System.EventHandler(this.btnStartFastboot_Click);
            // 
            // tabMtk
            // 
            this.tabMtk.BackColor = System.Drawing.SystemColors.Control;
            this.tabMtk.Controls.Add(this.grpMtk);
            this.tabMtk.Location = new System.Drawing.Point(4, 22);
            this.tabMtk.Name = "tabMtk";
            this.tabMtk.Padding = new System.Windows.Forms.Padding(3);
            this.tabMtk.Size = new System.Drawing.Size(352, 411);
            this.tabMtk.TabIndex = 1;
            this.tabMtk.Text = "MediaTek (MTK)";
            // 
            // grpMtk
            // 
            this.grpMtk.Controls.Add(this.lblMtkDesc);
            this.grpMtk.Controls.Add(this.btnStopMtk);
            this.grpMtk.Controls.Add(this.btnStartMtk);
            this.grpMtk.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMtk.Location = new System.Drawing.Point(6, 6);
            this.grpMtk.Name = "grpMtk";
            this.grpMtk.Size = new System.Drawing.Size(340, 120);
            this.grpMtk.TabIndex = 1;
            this.grpMtk.TabStop = false;
            this.grpMtk.Text = "MTK BROM / Preloader Settings";
            // 
            // lblMtkDesc
            // 
            this.lblMtkDesc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMtkDesc.Location = new System.Drawing.Point(15, 23);
            this.lblMtkDesc.Name = "lblMtkDesc";
            this.lblMtkDesc.Size = new System.Drawing.Size(310, 32);
            this.lblMtkDesc.TabIndex = 2;
            this.lblMtkDesc.Text = "Start monitoring for MediaTek devices in Preloader or BROM mode on COM ports.";
            // 
            // btnStopMtk
            // 
            this.btnStopMtk.Enabled = false;
            this.btnStopMtk.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopMtk.Location = new System.Drawing.Point(125, 68);
            this.btnStopMtk.Name = "btnStopMtk";
            this.btnStopMtk.Size = new System.Drawing.Size(100, 25);
            this.btnStopMtk.TabIndex = 1;
            this.btnStopMtk.Text = "Stop Monitor";
            this.btnStopMtk.UseVisualStyleBackColor = true;
            this.btnStopMtk.Click += new System.EventHandler(this.btnStopMtk_Click);
            // 
            // btnStartMtk
            // 
            this.btnStartMtk.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartMtk.Location = new System.Drawing.Point(18, 68);
            this.btnStartMtk.Name = "btnStartMtk";
            this.btnStartMtk.Size = new System.Drawing.Size(100, 25);
            this.btnStartMtk.TabIndex = 0;
            this.btnStartMtk.Text = "Start Monitor";
            this.btnStartMtk.UseVisualStyleBackColor = true;
            this.btnStartMtk.Click += new System.EventHandler(this.btnStartMtk_Click);
            // 
            // tabExtractor
            // 
            this.tabExtractor.BackColor = System.Drawing.SystemColors.Control;
            this.tabExtractor.Controls.Add(this.grpExtractor);
            this.tabExtractor.Location = new System.Drawing.Point(4, 22);
            this.tabExtractor.Name = "tabExtractor";
            this.tabExtractor.Size = new System.Drawing.Size(352, 411);
            this.tabExtractor.TabIndex = 2;
            this.tabExtractor.Text = "Extractor";
            // 
            // grpExtractor
            // 
            this.grpExtractor.Controls.Add(this.lblFirmwarePath);
            this.grpExtractor.Controls.Add(this.btnExtract);
            this.grpExtractor.Controls.Add(this.btnBrowse);
            this.grpExtractor.Controls.Add(this.txtFirmwarePath);
            this.grpExtractor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpExtractor.Location = new System.Drawing.Point(6, 6);
            this.grpExtractor.Name = "grpExtractor";
            this.grpExtractor.Size = new System.Drawing.Size(340, 150);
            this.grpExtractor.TabIndex = 0;
            this.grpExtractor.TabStop = false;
            this.grpExtractor.Text = "Firmware Archive Extractor";
            // 
            // lblFirmwarePath
            // 
            this.lblFirmwarePath.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirmwarePath.Location = new System.Drawing.Point(15, 25);
            this.lblFirmwarePath.Name = "lblFirmwarePath";
            this.lblFirmwarePath.Size = new System.Drawing.Size(200, 15);
            this.lblFirmwarePath.TabIndex = 3;
            this.lblFirmwarePath.Text = "Firmware Path (Absolute Path):";
            // 
            // btnExtract
            // 
            this.btnExtract.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExtract.Location = new System.Drawing.Point(18, 97);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(100, 25);
            this.btnExtract.TabIndex = 2;
            this.btnExtract.Text = "Extract Now";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(245, 43);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFirmwarePath
            // 
            this.txtFirmwarePath.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirmwarePath.Location = new System.Drawing.Point(18, 45);
            this.txtFirmwarePath.Name = "txtFirmwarePath";
            this.txtFirmwarePath.Size = new System.Drawing.Size(220, 21);
            this.txtFirmwarePath.TabIndex = 0;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.Black;
            this.txtLog.Location = new System.Drawing.Point(388, 34);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(434, 411);
            this.txtLog.TabIndex = 1;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearLog.Location = new System.Drawing.Point(747, 8);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 2;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // 
            // lblLogTitle
            // 
            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogTitle.Location = new System.Drawing.Point(385, 12);
            this.lblLogTitle.Name = "lblLogTitle";
            this.lblLogTitle.Size = new System.Drawing.Size(117, 13);
            this.lblLogTitle.TabIndex = 3;
            this.lblLogTitle.Text = "Output Log Window";
            // 
            // picLogo
            // 
            this.picLogo.Location = new System.Drawing.Point(12, 12);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(150, 50);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 4;
            this.picLogo.TabStop = false;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderTitle.Location = new System.Drawing.Point(170, 16);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(193, 23);
            this.lblHeaderTitle.TabIndex = 5;
            this.lblHeaderTitle.Text = "Android Multi Tools";
            // 
            // lblHeaderSubtitle
            // 
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderSubtitle.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(172, 42);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(154, 13);
            this.lblHeaderSubtitle.TabIndex = 6;
            this.lblHeaderSubtitle.Text = "Classic Win32 Form Version 1.0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 461);
            this.Controls.Add(this.lblHeaderSubtitle);
            this.Controls.Add(this.lblHeaderTitle);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.lblLogTitle);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.tabTools);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Android Tools Billy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabTools.ResumeLayout(false);
            this.tabFastboot.ResumeLayout(false);
            this.grpFastboot.ResumeLayout(false);
            this.tabMtk.ResumeLayout(false);
            this.grpMtk.ResumeLayout(false);
            this.tabExtractor.ResumeLayout(false);
            this.grpExtractor.ResumeLayout(false);
            this.grpExtractor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabTools;
        private System.Windows.Forms.TabPage tabFastboot;
        private System.Windows.Forms.TabPage tabMtk;
        private System.Windows.Forms.TabPage tabExtractor;
        private System.Windows.Forms.GroupBox grpFastboot;
        private System.Windows.Forms.Label lblFastbootDesc;
        private System.Windows.Forms.Button btnStopFastboot;
        private System.Windows.Forms.Button btnStartFastboot;
        private System.Windows.Forms.GroupBox grpMtk;
        private System.Windows.Forms.Label lblMtkDesc;
        private System.Windows.Forms.Button btnStopMtk;
        private System.Windows.Forms.Button btnStartMtk;
        private System.Windows.Forms.GroupBox grpExtractor;
        private System.Windows.Forms.Label lblFirmwarePath;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFirmwarePath;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Label lblLogTitle;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
    }
}
