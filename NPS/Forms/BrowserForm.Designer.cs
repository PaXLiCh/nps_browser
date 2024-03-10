namespace NPS
{
    partial class BrowserForm
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
            System.Windows.Forms.StatusStrip statusStrip;
            PresentationControls.CheckBoxProperties checkBoxProperties1 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties2 = new PresentationControls.CheckBoxProperties();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_changeLog = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDescriptionPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.libraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstTitles = new System.Windows.Forms.ListView();
            this.colTitleID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRegion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDLCs = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLastModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstTitlesMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.downloadAndUnpackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTitleDlcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadAllDlcsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadAllWithPatchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForPatchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleDownloadedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCompPack = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.rbnGames = new System.Windows.Forms.RadioButton();
            this.rbnDLC = new System.Windows.Forms.RadioButton();
            this.LblContentTypeSelect = new System.Windows.Forms.Label();
            this.ptbCover = new System.Windows.Forms.PictureBox();
            this.txtPkgInfo = new System.Windows.Forms.Label();
            this.lstDownloadStatusMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.retryUnpackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCompletedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.lnkSearchAboutTitle = new System.Windows.Forms.LinkLabel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.splMain = new System.Windows.Forms.SplitContainer();
            this.splList = new System.Windows.Forms.SplitContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cmbType = new PresentationControls.CheckBoxComboBox();
            this.cmbRegion = new PresentationControls.CheckBoxComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rbnAvatars = new System.Windows.Forms.RadioButton();
            this.rbnThemes = new System.Windows.Forms.RadioButton();
            this.rbnUpdates = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCount = new System.Windows.Forms.Label();
            this.rbnDownloaded = new System.Windows.Forms.RadioButton();
            this.rbnUndownloaded = new System.Windows.Forms.RadioButton();
            this.rbnAll = new System.Windows.Forms.RadioButton();
            this.chkUnless = new System.Windows.Forms.CheckBox();
            this.chkHideDownloaded = new System.Windows.Forms.CheckBox();
            this.lblPs3LicenseType = new System.Windows.Forms.Label();
            this.flowLayoutPanelDownloads = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDownloadResume = new System.Windows.Forms.Button();
            this.btnDownloadPause = new System.Windows.Forms.Button();
            this.btnDownloadCancel = new System.Windows.Forms.Button();
            this.btnDownloadClear = new System.Windows.Forms.Button();
            this.btnDownloadPauseAll = new System.Windows.Forms.Button();
            this.btnOpenDirectory = new System.Windows.Forms.Button();
            this.btnDownloadResumeAll = new System.Windows.Forms.Button();
            this.lstDownloadStatus = new ListViewEmbeddedControls.ListViewEx();
            this.colDownloadTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDownloadSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDownloadStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDownloadProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            statusStrip = new System.Windows.Forms.StatusStrip();
            statusStrip.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.lstTitlesMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbCover)).BeginInit();
            this.lstDownloadStatusMenuStrip.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).BeginInit();
            this.splMain.Panel1.SuspendLayout();
            this.splMain.Panel2.SuspendLayout();
            this.splMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splList)).BeginInit();
            this.splList.Panel1.SuspendLayout();
            this.splList.Panel2.SuspendLayout();
            this.splList.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanelDownloads.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.AutoSize = false;
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripProgressBar});
            statusStrip.Location = new System.Drawing.Point(0, 701);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1085, 22);
            statusStrip.TabIndex = 21;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(918, 17);
            this.toolStripStatusLabel.Spring = true;
            this.toolStripStatusLabel.Text = "Ready";
            this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(150, 16);
            this.toolStripProgressBar.Value = 25;
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.downloadUpdateToolStripMenuItem,
            this.showDescriptionPanelToolStripMenuItem,
            this.libraryToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1085, 24);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.reloadToolStripMenuItem,
            this.ts_changeLog,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_options;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_reload;
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.reloadToolStripMenuItem.Text = "Sync cache";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.Sync);
            // 
            // ts_changeLog
            // 
            this.ts_changeLog.Name = "ts_changeLog";
            this.ts_changeLog.Size = new System.Drawing.Size(133, 22);
            this.ts_changeLog.Text = "Changelog";
            this.ts_changeLog.Click += new System.EventHandler(this.ts_changeLog_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(130, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.exitToolStripMenuItem.Tag = "Exit";
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // downloadUpdateToolStripMenuItem
            // 
            this.downloadUpdateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadToolStripMenuItem,
            this.changelogToolStripMenuItem});
            this.downloadUpdateToolStripMenuItem.Name = "downloadUpdateToolStripMenuItem";
            this.downloadUpdateToolStripMenuItem.Size = new System.Drawing.Size(113, 20);
            this.downloadUpdateToolStripMenuItem.Text = "Download update";
            this.downloadUpdateToolStripMenuItem.Visible = false;
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.downloadToolStripMenuItem.Text = "Download";
            this.downloadToolStripMenuItem.Click += new System.EventHandler(this.downloadUpdateToolStripMenuItem_Click);
            // 
            // changelogToolStripMenuItem
            // 
            this.changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
            this.changelogToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.changelogToolStripMenuItem.Text = "Changelog";
            this.changelogToolStripMenuItem.Click += new System.EventHandler(this.changelogToolStripMenuItem_Click);
            // 
            // showDescriptionPanelToolStripMenuItem
            // 
            this.showDescriptionPanelToolStripMenuItem.Name = "showDescriptionPanelToolStripMenuItem";
            this.showDescriptionPanelToolStripMenuItem.Size = new System.Drawing.Size(111, 20);
            this.showDescriptionPanelToolStripMenuItem.Text = "Description panel";
            this.showDescriptionPanelToolStripMenuItem.Click += new System.EventHandler(this.ShowDescriptionPanel);
            // 
            // libraryToolStripMenuItem
            // 
            this.libraryToolStripMenuItem.Name = "libraryToolStripMenuItem";
            this.libraryToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.libraryToolStripMenuItem.Text = "Library";
            this.libraryToolStripMenuItem.Click += new System.EventHandler(this.libraryToolStripMenuItem_Click);
            // 
            // lstTitles
            // 
            this.lstTitles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTitleID,
            this.colRegion,
            this.colTitle,
            this.colType,
            this.colDLCs,
            this.colLastModified});
            this.lstTitles.ContextMenuStrip = this.lstTitlesMenuStrip;
            this.lstTitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTitles.FullRowSelect = true;
            this.lstTitles.HideSelection = false;
            this.lstTitles.Location = new System.Drawing.Point(0, 52);
            this.lstTitles.Name = "lstTitles";
            this.lstTitles.Size = new System.Drawing.Size(851, 398);
            this.lstTitles.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstTitles.TabIndex = 1;
            this.lstTitles.UseCompatibleStateImageBehavior = false;
            this.lstTitles.View = System.Windows.Forms.View.Details;
            this.lstTitles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstTitles_ColumnClick);
            this.lstTitles.SelectedIndexChanged += new System.EventHandler(this.lstTitles_SelectedIndexChanged);
            this.lstTitles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstTitles_KeyDown);
            this.lstTitles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstTitles_MouseClick);
            // 
            // colTitleID
            // 
            this.colTitleID.Text = "Title ID";
            this.colTitleID.Width = 80;
            // 
            // colRegion
            // 
            this.colRegion.Text = "Region";
            this.colRegion.Width = 50;
            // 
            // colTitle
            // 
            this.colTitle.Text = "Title";
            this.colTitle.Width = 393;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            // 
            // colDLCs
            // 
            this.colDLCs.Text = "DLCs";
            // 
            // colLastModified
            // 
            this.colLastModified.Text = "Last Modified";
            this.colLastModified.Width = 144;
            // 
            // lstTitlesMenuStrip
            // 
            this.lstTitlesMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadAndUnpackToolStripMenuItem,
            this.showTitleDlcToolStripMenuItem,
            this.downloadAllDlcsToolStripMenuItem,
            this.downloadAllToolStripMenuItem,
            this.downloadAllWithPatchesToolStripMenuItem,
            this.checkForPatchesToolStripMenuItem,
            this.toggleDownloadedToolStripMenuItem,
            this.toolStripMenuItemCompPack});
            this.lstTitlesMenuStrip.Name = "contextMenuStrip2";
            this.lstTitlesMenuStrip.Size = new System.Drawing.Size(207, 180);
            // 
            // downloadAndUnpackToolStripMenuItem
            // 
            this.downloadAndUnpackToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_unpack;
            this.downloadAndUnpackToolStripMenuItem.Name = "downloadAndUnpackToolStripMenuItem";
            this.downloadAndUnpackToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.downloadAndUnpackToolStripMenuItem.Text = "Download and Unpack";
            this.downloadAndUnpackToolStripMenuItem.Click += new System.EventHandler(this.BtnDownloadClick);
            // 
            // showTitleDlcToolStripMenuItem
            // 
            this.showTitleDlcToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_show_dlc;
            this.showTitleDlcToolStripMenuItem.Name = "showTitleDlcToolStripMenuItem";
            this.showTitleDlcToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.showTitleDlcToolStripMenuItem.Text = "Show Title DLCs";
            this.showTitleDlcToolStripMenuItem.Click += new System.EventHandler(this.showTitleDlcToolStripMenuItem_Click);
            // 
            // downloadAllDlcsToolStripMenuItem
            // 
            this.downloadAllDlcsToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_download_dlc;
            this.downloadAllDlcsToolStripMenuItem.Name = "downloadAllDlcsToolStripMenuItem";
            this.downloadAllDlcsToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.downloadAllDlcsToolStripMenuItem.Text = "Download All DLCs";
            this.downloadAllDlcsToolStripMenuItem.Click += new System.EventHandler(this.downloadAllDlcsToolStripMenuItem_Click);
            // 
            // downloadAllToolStripMenuItem
            // 
            this.downloadAllToolStripMenuItem.Name = "downloadAllToolStripMenuItem";
            this.downloadAllToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.downloadAllToolStripMenuItem.Text = "Download All";
            this.downloadAllToolStripMenuItem.Click += new System.EventHandler(this.downloadAllToolStripMenuItem_Click);
            // 
            // downloadAllWithPatchesToolStripMenuItem
            // 
            this.downloadAllWithPatchesToolStripMenuItem.Name = "downloadAllWithPatchesToolStripMenuItem";
            this.downloadAllWithPatchesToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.downloadAllWithPatchesToolStripMenuItem.Text = "Download All w/ Patches";
            this.downloadAllWithPatchesToolStripMenuItem.Click += new System.EventHandler(this.downloadAllWithPatchesToolStripMenuItem_Click);
            // 
            // checkForPatchesToolStripMenuItem
            // 
            this.checkForPatchesToolStripMenuItem.Name = "checkForPatchesToolStripMenuItem";
            this.checkForPatchesToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.checkForPatchesToolStripMenuItem.Text = "Check for patches";
            this.checkForPatchesToolStripMenuItem.Click += new System.EventHandler(this.checkForPatchesToolStripMenuItem_Click);
            // 
            // toggleDownloadedToolStripMenuItem
            // 
            this.toggleDownloadedToolStripMenuItem.Name = "toggleDownloadedToolStripMenuItem";
            this.toggleDownloadedToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.toggleDownloadedToolStripMenuItem.Text = "Toggle Download Mark";
            this.toggleDownloadedToolStripMenuItem.Click += new System.EventHandler(this.toggleDownloadedToolStripMenuItem_Click);
            // 
            // toolStripMenuItemCompPack
            // 
            this.toolStripMenuItemCompPack.Name = "toolStripMenuItemCompPack";
            this.toolStripMenuItemCompPack.Size = new System.Drawing.Size(206, 22);
            this.toolStripMenuItemCompPack.Text = "Download CompPack";
            this.toolStripMenuItemCompPack.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(540, 20);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnDownload
            // 
            this.btnDownload.Image = global::NPS.Properties.Resources.menu_download;
            this.btnDownload.Location = new System.Drawing.Point(0, 95);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(0);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(218, 23);
            this.btnDownload.TabIndex = 3;
            this.btnDownload.Text = "Download and Unpack";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.BtnDownloadClick);
            // 
            // rbnGames
            // 
            this.rbnGames.AutoSize = true;
            this.rbnGames.Checked = true;
            this.rbnGames.Enabled = false;
            this.rbnGames.Location = new System.Drawing.Point(69, 3);
            this.rbnGames.Name = "rbnGames";
            this.rbnGames.Size = new System.Drawing.Size(58, 17);
            this.rbnGames.TabIndex = 8;
            this.rbnGames.TabStop = true;
            this.rbnGames.Text = "Games";
            this.rbnGames.UseVisualStyleBackColor = true;
            this.rbnGames.CheckedChanged += new System.EventHandler(this.rbnGames_CheckedChanged);
            // 
            // rbnDLC
            // 
            this.rbnDLC.AutoSize = true;
            this.rbnDLC.Enabled = false;
            this.rbnDLC.Location = new System.Drawing.Point(200, 3);
            this.rbnDLC.Name = "rbnDLC";
            this.rbnDLC.Size = new System.Drawing.Size(46, 17);
            this.rbnDLC.TabIndex = 9;
            this.rbnDLC.Text = "DLC";
            this.rbnDLC.UseVisualStyleBackColor = true;
            this.rbnDLC.CheckedChanged += new System.EventHandler(this.rbnDLC_CheckedChanged);
            // 
            // LblContentTypeSelect
            // 
            this.LblContentTypeSelect.Location = new System.Drawing.Point(3, 3);
            this.LblContentTypeSelect.Margin = new System.Windows.Forms.Padding(3);
            this.LblContentTypeSelect.Name = "LblContentTypeSelect";
            this.LblContentTypeSelect.Size = new System.Drawing.Size(60, 17);
            this.LblContentTypeSelect.TabIndex = 11;
            this.LblContentTypeSelect.Text = "Browse for:";
            this.LblContentTypeSelect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ptbCover
            // 
            this.ptbCover.ImageLocation = "";
            this.ptbCover.Location = new System.Drawing.Point(2, 121);
            this.ptbCover.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.ptbCover.Name = "ptbCover";
            this.ptbCover.Size = new System.Drawing.Size(211, 211);
            this.ptbCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbCover.TabIndex = 12;
            this.ptbCover.TabStop = false;
            // 
            // txtPkgInfo
            // 
            this.txtPkgInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPkgInfo.Location = new System.Drawing.Point(3, 335);
            this.txtPkgInfo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.txtPkgInfo.Name = "txtPkgInfo";
            this.txtPkgInfo.Size = new System.Drawing.Size(209, 65);
            this.txtPkgInfo.TabIndex = 13;
            // 
            // lstDownloadStatusMenuStrip
            // 
            this.lstDownloadStatusMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pauseToolStripMenuItem,
            this.resumeToolStripMenuItem,
            this.cancelToolStripMenuItem,
            this.retryUnpackToolStripMenuItem,
            this.clearCompletedToolStripMenuItem});
            this.lstDownloadStatusMenuStrip.Name = "contextMenuStrip1";
            this.lstDownloadStatusMenuStrip.Size = new System.Drawing.Size(164, 116);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_pause;
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // resumeToolStripMenuItem
            // 
            this.resumeToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_resume;
            this.resumeToolStripMenuItem.Margin = new System.Windows.Forms.Padding(1);
            this.resumeToolStripMenuItem.Name = "resumeToolStripMenuItem";
            this.resumeToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.resumeToolStripMenuItem.Text = "Resume";
            this.resumeToolStripMenuItem.Click += new System.EventHandler(this.resumeToolStripMenuItem_Click);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_cancel;
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.cancelToolStripMenuItem.Text = "Cancel";
            this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
            // 
            // retryUnpackToolStripMenuItem
            // 
            this.retryUnpackToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_retry;
            this.retryUnpackToolStripMenuItem.Name = "retryUnpackToolStripMenuItem";
            this.retryUnpackToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.retryUnpackToolStripMenuItem.Text = "Retry Unpack";
            this.retryUnpackToolStripMenuItem.Click += new System.EventHandler(this.retryUnpackToolStripMenuItem_Click);
            // 
            // clearCompletedToolStripMenuItem
            // 
            this.clearCompletedToolStripMenuItem.Image = global::NPS.Properties.Resources.menu_clear;
            this.clearCompletedToolStripMenuItem.Name = "clearCompletedToolStripMenuItem";
            this.clearCompletedToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.clearCompletedToolStripMenuItem.Text = "Clear Completed";
            this.clearCompletedToolStripMenuItem.Click += new System.EventHandler(this.clearCompletedToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // lnkSearchAboutTitle
            // 
            this.lnkSearchAboutTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkSearchAboutTitle.Location = new System.Drawing.Point(2, 403);
            this.lnkSearchAboutTitle.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.lnkSearchAboutTitle.Name = "lnkSearchAboutTitle";
            this.lnkSearchAboutTitle.Size = new System.Drawing.Size(212, 17);
            this.lnkSearchAboutTitle.TabIndex = 17;
            this.lnkSearchAboutTitle.TabStop = true;
            this.lnkSearchAboutTitle.Text = "Open Google (screens)";
            this.lnkSearchAboutTitle.Visible = false;
            this.lnkSearchAboutTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenRenaScene_LinkClicked);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.splMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 24);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(6);
            this.pnlMain.Size = new System.Drawing.Size(1085, 677);
            this.pnlMain.TabIndex = 18;
            // 
            // splMain
            // 
            this.splMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splMain.Location = new System.Drawing.Point(6, 6);
            this.splMain.Name = "splMain";
            this.splMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splMain.Panel1
            // 
            this.splMain.Panel1.Controls.Add(this.splList);
            // 
            // splMain.Panel2
            // 
            this.splMain.Panel2.Controls.Add(this.flowLayoutPanelDownloads);
            this.splMain.Panel2.Controls.Add(this.lstDownloadStatus);
            this.splMain.Size = new System.Drawing.Size(1073, 665);
            this.splMain.SplitterDistance = 450;
            this.splMain.TabIndex = 18;
            // 
            // splList
            // 
            this.splList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splList.Location = new System.Drawing.Point(0, 0);
            this.splList.Name = "splList";
            // 
            // splList.Panel1
            // 
            this.splList.Panel1.Controls.Add(this.lstTitles);
            this.splList.Panel1.Controls.Add(this.pnlSearch);
            // 
            // splList.Panel2
            // 
            this.splList.Panel2.Controls.Add(this.rbnDownloaded);
            this.splList.Panel2.Controls.Add(this.rbnUndownloaded);
            this.splList.Panel2.Controls.Add(this.rbnAll);
            this.splList.Panel2.Controls.Add(this.chkUnless);
            this.splList.Panel2.Controls.Add(this.chkHideDownloaded);
            this.splList.Panel2.Controls.Add(this.lblPs3LicenseType);
            this.splList.Panel2.Controls.Add(this.btnDownload);
            this.splList.Panel2.Controls.Add(this.ptbCover);
            this.splList.Panel2.Controls.Add(this.lnkSearchAboutTitle);
            this.splList.Panel2.Controls.Add(this.txtPkgInfo);
            this.splList.Size = new System.Drawing.Size(1073, 450);
            this.splList.SplitterDistance = 851;
            this.splList.TabIndex = 18;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.cmbType);
            this.pnlSearch.Controls.Add(this.cmbRegion);
            this.pnlSearch.Controls.Add(this.flowLayoutPanel1);
            this.pnlSearch.Controls.Add(this.flowLayoutPanel3);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(851, 52);
            this.pnlSearch.TabIndex = 17;
            // 
            // cmbType
            // 
            this.cmbType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            checkBoxProperties1.AutoSize = true;
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbType.CheckBoxProperties = checkBoxProperties1;
            this.cmbType.DisplayMemberSingleItem = "";
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(549, 3);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(152, 21);
            this.cmbType.TabIndex = 20;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // cmbRegion
            // 
            this.cmbRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            checkBoxProperties2.AutoSize = true;
            checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbRegion.CheckBoxProperties = checkBoxProperties2;
            this.cmbRegion.DisplayMemberSingleItem = "";
            this.cmbRegion.FormattingEnabled = true;
            this.cmbRegion.Location = new System.Drawing.Point(707, 3);
            this.cmbRegion.MaxDropDownItems = 5;
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.Size = new System.Drawing.Size(138, 21);
            this.cmbRegion.TabIndex = 19;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.LblContentTypeSelect);
            this.flowLayoutPanel1.Controls.Add(this.rbnGames);
            this.flowLayoutPanel1.Controls.Add(this.rbnAvatars);
            this.flowLayoutPanel1.Controls.Add(this.rbnDLC);
            this.flowLayoutPanel1.Controls.Add(this.rbnThemes);
            this.flowLayoutPanel1.Controls.Add(this.rbnUpdates);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 26);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(686, 23);
            this.flowLayoutPanel1.TabIndex = 17;
            // 
            // rbnAvatars
            // 
            this.rbnAvatars.AutoSize = true;
            this.rbnAvatars.Enabled = false;
            this.rbnAvatars.Location = new System.Drawing.Point(133, 3);
            this.rbnAvatars.Name = "rbnAvatars";
            this.rbnAvatars.Size = new System.Drawing.Size(61, 17);
            this.rbnAvatars.TabIndex = 13;
            this.rbnAvatars.Text = "Avatars";
            this.rbnAvatars.UseVisualStyleBackColor = true;
            this.rbnAvatars.Visible = false;
            this.rbnAvatars.CheckedChanged += new System.EventHandler(this.rbnAvatars_CheckedChanged);
            // 
            // rbnThemes
            // 
            this.rbnThemes.AutoSize = true;
            this.rbnThemes.Enabled = false;
            this.rbnThemes.Location = new System.Drawing.Point(252, 3);
            this.rbnThemes.Name = "rbnThemes";
            this.rbnThemes.Size = new System.Drawing.Size(63, 17);
            this.rbnThemes.TabIndex = 10;
            this.rbnThemes.Text = "Themes";
            this.rbnThemes.UseVisualStyleBackColor = true;
            this.rbnThemes.CheckedChanged += new System.EventHandler(this.rbnThemes_CheckedChanged);
            // 
            // rbnUpdates
            // 
            this.rbnUpdates.AutoSize = true;
            this.rbnUpdates.Enabled = false;
            this.rbnUpdates.Location = new System.Drawing.Point(321, 3);
            this.rbnUpdates.Name = "rbnUpdates";
            this.rbnUpdates.Size = new System.Drawing.Size(65, 17);
            this.rbnUpdates.TabIndex = 12;
            this.rbnUpdates.Text = "Updates";
            this.rbnUpdates.UseVisualStyleBackColor = true;
            this.rbnUpdates.Visible = false;
            this.rbnUpdates.CheckedChanged += new System.EventHandler(this.rbnUpdates_CheckedChanged);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel3.Controls.Add(this.lblCount);
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(692, 26);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(156, 23);
            this.flowLayoutPanel3.TabIndex = 18;
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(3, 3);
            this.lblCount.Margin = new System.Windows.Forms.Padding(3);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(150, 17);
            this.lblCount.TabIndex = 11;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbnDownloaded
            // 
            this.rbnDownloaded.AutoSize = true;
            this.rbnDownloaded.Location = new System.Drawing.Point(133, 0);
            this.rbnDownloaded.Name = "rbnDownloaded";
            this.rbnDownloaded.Size = new System.Drawing.Size(85, 17);
            this.rbnDownloaded.TabIndex = 24;
            this.rbnDownloaded.Text = "Downloaded";
            this.rbnDownloaded.UseVisualStyleBackColor = true;
            this.rbnDownloaded.CheckedChanged += new System.EventHandler(this.rbnDownloaded_CheckedChanged);
            // 
            // rbnUndownloaded
            // 
            this.rbnUndownloaded.AutoSize = true;
            this.rbnUndownloaded.Location = new System.Drawing.Point(35, 0);
            this.rbnUndownloaded.Name = "rbnUndownloaded";
            this.rbnUndownloaded.Size = new System.Drawing.Size(97, 17);
            this.rbnUndownloaded.TabIndex = 23;
            this.rbnUndownloaded.Text = "Undownloaded";
            this.rbnUndownloaded.UseVisualStyleBackColor = true;
            this.rbnUndownloaded.CheckedChanged += new System.EventHandler(this.rbnUndownloaded_CheckedChanged);
            // 
            // rbnAll
            // 
            this.rbnAll.AutoSize = true;
            this.rbnAll.Checked = true;
            this.rbnAll.Location = new System.Drawing.Point(0, 0);
            this.rbnAll.Name = "rbnAll";
            this.rbnAll.Size = new System.Drawing.Size(36, 17);
            this.rbnAll.TabIndex = 22;
            this.rbnAll.TabStop = true;
            this.rbnAll.Text = "All";
            this.rbnAll.UseVisualStyleBackColor = true;
            this.rbnAll.CheckedChanged += new System.EventHandler(this.rbnAll_CheckedChanged);
            // 
            // chkUnless
            // 
            this.chkUnless.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkUnless.AutoSize = true;
            this.chkUnless.Enabled = false;
            this.chkUnless.Location = new System.Drawing.Point(0, 23);
            this.chkUnless.Name = "chkUnless";
            this.chkUnless.Size = new System.Drawing.Size(105, 17);
            this.chkUnless.TabIndex = 21;
            this.chkUnless.Text = "Unless new DLC";
            this.chkUnless.UseVisualStyleBackColor = true;
            this.chkUnless.CheckedChanged += new System.EventHandler(this.chkUnless_CheckedChanged);
            // 
            // chkHideDownloaded
            // 
            this.chkHideDownloaded.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkHideDownloaded.AutoSize = true;
            this.chkHideDownloaded.Location = new System.Drawing.Point(106, 23);
            this.chkHideDownloaded.Name = "chkHideDownloaded";
            this.chkHideDownloaded.Size = new System.Drawing.Size(109, 17);
            this.chkHideDownloaded.TabIndex = 20;
            this.chkHideDownloaded.Text = "Hide downloaded";
            this.chkHideDownloaded.UseVisualStyleBackColor = true;
            this.chkHideDownloaded.Visible = false;
            this.chkHideDownloaded.CheckedChanged += new System.EventHandler(this.chkHideDownloaded_CheckedChanged);
            // 
            // lblPs3LicenseType
            // 
            this.lblPs3LicenseType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPs3LicenseType.Location = new System.Drawing.Point(0, 43);
            this.lblPs3LicenseType.Name = "lblPs3LicenseType";
            this.lblPs3LicenseType.Size = new System.Drawing.Size(215, 30);
            this.lblPs3LicenseType.TabIndex = 19;
            this.lblPs3LicenseType.Text = "PS3LicenseType";
            this.lblPs3LicenseType.Visible = false;
            // 
            // flowLayoutPanelDownloads
            // 
            this.flowLayoutPanelDownloads.Controls.Add(this.btnDownloadResume);
            this.flowLayoutPanelDownloads.Controls.Add(this.btnDownloadPause);
            this.flowLayoutPanelDownloads.Controls.Add(this.btnDownloadCancel);
            this.flowLayoutPanelDownloads.Controls.Add(this.btnDownloadClear);
            this.flowLayoutPanelDownloads.Controls.Add(this.btnDownloadPauseAll);
            this.flowLayoutPanelDownloads.Controls.Add(this.btnOpenDirectory);
            this.flowLayoutPanelDownloads.Controls.Add(this.btnDownloadResumeAll);
            this.flowLayoutPanelDownloads.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelDownloads.Name = "flowLayoutPanelDownloads";
            this.flowLayoutPanelDownloads.Size = new System.Drawing.Size(425, 34);
            this.flowLayoutPanelDownloads.TabIndex = 5;
            // 
            // btnDownloadResume
            // 
            this.btnDownloadResume.Image = global::NPS.Properties.Resources.menu_resume;
            this.btnDownloadResume.Location = new System.Drawing.Point(3, 3);
            this.btnDownloadResume.Name = "btnDownloadResume";
            this.btnDownloadResume.Size = new System.Drawing.Size(39, 28);
            this.btnDownloadResume.TabIndex = 0;
            this.btnDownloadResume.UseVisualStyleBackColor = true;
            this.btnDownloadResume.Click += new System.EventHandler(this.resumeToolStripMenuItem_Click);
            // 
            // btnDownloadPause
            // 
            this.btnDownloadPause.Image = global::NPS.Properties.Resources.menu_pause;
            this.btnDownloadPause.Location = new System.Drawing.Point(48, 3);
            this.btnDownloadPause.Name = "btnDownloadPause";
            this.btnDownloadPause.Size = new System.Drawing.Size(39, 28);
            this.btnDownloadPause.TabIndex = 1;
            this.btnDownloadPause.UseVisualStyleBackColor = true;
            this.btnDownloadPause.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // btnDownloadCancel
            // 
            this.btnDownloadCancel.Image = global::NPS.Properties.Resources.menu_cancel;
            this.btnDownloadCancel.Location = new System.Drawing.Point(93, 3);
            this.btnDownloadCancel.Name = "btnDownloadCancel";
            this.btnDownloadCancel.Size = new System.Drawing.Size(39, 28);
            this.btnDownloadCancel.TabIndex = 2;
            this.btnDownloadCancel.UseVisualStyleBackColor = true;
            this.btnDownloadCancel.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
            // 
            // btnDownloadClear
            // 
            this.btnDownloadClear.Image = global::NPS.Properties.Resources.menu_clear;
            this.btnDownloadClear.Location = new System.Drawing.Point(138, 3);
            this.btnDownloadClear.Name = "btnDownloadClear";
            this.btnDownloadClear.Size = new System.Drawing.Size(39, 28);
            this.btnDownloadClear.TabIndex = 3;
            this.btnDownloadClear.UseVisualStyleBackColor = true;
            this.btnDownloadClear.Click += new System.EventHandler(this.clearCompletedToolStripMenuItem_Click);
            // 
            // btnDownloadPauseAll
            // 
            this.btnDownloadPauseAll.Image = global::NPS.Properties.Resources.menu_pause;
            this.btnDownloadPauseAll.Location = new System.Drawing.Point(183, 3);
            this.btnDownloadPauseAll.Name = "btnDownloadPauseAll";
            this.btnDownloadPauseAll.Size = new System.Drawing.Size(93, 28);
            this.btnDownloadPauseAll.TabIndex = 0;
            this.btnDownloadPauseAll.Text = "Pause All";
            this.btnDownloadPauseAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownloadPauseAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDownloadPauseAll.UseVisualStyleBackColor = true;
            this.btnDownloadPauseAll.Click += new System.EventHandler(this.PauseAllBtnClick);
            // 
            // btnOpenDirectory
            // 
            this.btnOpenDirectory.Image = global::NPS.Properties.Resources.opened_folder;
            this.btnOpenDirectory.Location = new System.Drawing.Point(282, 3);
            this.btnOpenDirectory.Name = "btnOpenDirectory";
            this.btnOpenDirectory.Size = new System.Drawing.Size(39, 28);
            this.btnOpenDirectory.TabIndex = 4;
            this.btnOpenDirectory.UseVisualStyleBackColor = true;
            this.btnOpenDirectory.Click += new System.EventHandler(this.BtnOpenDirectoryClick);
            // 
            // btnDownloadResumeAll
            // 
            this.btnDownloadResumeAll.Image = global::NPS.Properties.Resources.menu_resume;
            this.btnDownloadResumeAll.Location = new System.Drawing.Point(327, 3);
            this.btnDownloadResumeAll.Name = "btnDownloadResumeAll";
            this.btnDownloadResumeAll.Size = new System.Drawing.Size(93, 28);
            this.btnDownloadResumeAll.TabIndex = 1;
            this.btnDownloadResumeAll.Text = "Resume All";
            this.btnDownloadResumeAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownloadResumeAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDownloadResumeAll.UseVisualStyleBackColor = true;
            this.btnDownloadResumeAll.Click += new System.EventHandler(this.ResumeAllBtnClick);
            // 
            // lstDownloadStatus
            // 
            this.lstDownloadStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDownloadStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDownloadTitle,
            this.colDownloadSpeed,
            this.colDownloadStatus,
            this.colDownloadProgress});
            this.lstDownloadStatus.FullRowSelect = true;
            this.lstDownloadStatus.HideSelection = false;
            this.lstDownloadStatus.Location = new System.Drawing.Point(0, 37);
            this.lstDownloadStatus.Name = "lstDownloadStatus";
            this.lstDownloadStatus.Size = new System.Drawing.Size(1073, 174);
            this.lstDownloadStatus.TabIndex = 14;
            this.lstDownloadStatus.UseCompatibleStateImageBehavior = false;
            this.lstDownloadStatus.View = System.Windows.Forms.View.Details;
            this.lstDownloadStatus.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstDownloadStatus_KeyDown);
            // 
            // colDownloadTitle
            // 
            this.colDownloadTitle.Text = "Title";
            this.colDownloadTitle.Width = 457;
            // 
            // colDownloadSpeed
            // 
            this.colDownloadSpeed.Text = "Speed";
            this.colDownloadSpeed.Width = 112;
            // 
            // colDownloadStatus
            // 
            this.colDownloadStatus.Text = "Status";
            this.colDownloadStatus.Width = 100;
            // 
            // colDownloadProgress
            // 
            this.colDownloadProgress.Text = "Progress";
            this.colDownloadProgress.Width = 366;
            // 
            // BrowserForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1085, 723);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(statusStrip);
            this.Controls.Add(this.mnuMain);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "BrowserForm";
            this.Text = "NPS Browser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrowserFormClosing);
            this.Load += new System.EventHandler(this.BrowserFormLoad);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.lstTitlesMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptbCover)).EndInit();
            this.lstDownloadStatusMenuStrip.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.splMain.Panel1.ResumeLayout(false);
            this.splMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).EndInit();
            this.splMain.ResumeLayout(false);
            this.splList.Panel1.ResumeLayout(false);
            this.splList.Panel2.ResumeLayout(false);
            this.splList.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splList)).EndInit();
            this.splList.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanelDownloads.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        #region Windows Form Designer generated code


        #endregion

        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ListView lstTitles;
        private System.Windows.Forms.ColumnHeader colTitleID;
        private System.Windows.Forms.ColumnHeader colRegion;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.RadioButton rbnGames;
        private System.Windows.Forms.RadioButton rbnDLC;
        private System.Windows.Forms.Label LblContentTypeSelect;
        private System.Windows.Forms.PictureBox ptbCover;
        private System.Windows.Forms.Label txtPkgInfo;
        private System.Windows.Forms.ContextMenuStrip lstDownloadStatusMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem retryUnpackToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private ListViewEmbeddedControls.ListViewEx lstDownloadStatus;
        private System.Windows.Forms.ColumnHeader colDownloadTitle;
        private System.Windows.Forms.ColumnHeader colDownloadSpeed;
        private System.Windows.Forms.ColumnHeader colDownloadStatus;
        private System.Windows.Forms.ToolStripMenuItem clearCompletedToolStripMenuItem;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader colDLCs;
        private System.Windows.Forms.ToolStripMenuItem downloadUpdateToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip lstTitlesMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showTitleDlcToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader colLastModified;
        private System.Windows.Forms.ToolStripMenuItem downloadAllDlcsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadAllWithPatchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadAndUnpackToolStripMenuItem;
        private System.Windows.Forms.LinkLabel lnkSearchAboutTitle;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.SplitContainer splMain;
        private System.Windows.Forms.SplitContainer splList;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resumeToolStripMenuItem;
        private System.Windows.Forms.Button btnDownloadResumeAll;
        private System.Windows.Forms.Button btnDownloadPauseAll;
        private System.Windows.Forms.ColumnHeader colDownloadProgress;
        private PresentationControls.CheckBoxComboBox cmbRegion;
        private PresentationControls.CheckBoxComboBox cmbType;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ToolStripMenuItem showDescriptionPanelToolStripMenuItem;
        private System.Windows.Forms.Label lblPs3LicenseType;
		private System.Windows.Forms.RadioButton rbnThemes;
		private System.Windows.Forms.RadioButton rbnUpdates;
		private System.Windows.Forms.RadioButton rbnAvatars;
        private System.Windows.Forms.Button btnDownloadCancel;
        private System.Windows.Forms.Button btnDownloadPause;
        private System.Windows.Forms.Button btnDownloadResume;
        private System.Windows.Forms.Button btnDownloadClear;
        private System.Windows.Forms.Button btnOpenDirectory;
        private System.Windows.Forms.ToolStripMenuItem ts_changeLog;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changelogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForPatchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleDownloadedToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkHideDownloaded;
        private System.Windows.Forms.CheckBox chkUnless;
        private System.Windows.Forms.RadioButton rbnDownloaded;
        private System.Windows.Forms.RadioButton rbnUndownloaded;
        private System.Windows.Forms.RadioButton rbnAll;
        private System.Windows.Forms.ToolStripMenuItem libraryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCompPack;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDownloads;
    }
}

