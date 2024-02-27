namespace NPS
{
    partial class LibraryForm
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Testowy item",
            "test",
            "test2"}, "High-Definition-Ultra-HD-Wallpaper-96262544.jpg");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("test", "(none)");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryForm));
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Testowy item",
            "test",
            "test2"}, "High-Definition-Ultra-HD-Wallpaper-96262544.jpg");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("test", "(none)");
            this.lvDownloaded = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.LblDownloadsRootPath = new System.Windows.Forms.Label();
            this.btnOpenDirectory = new System.Windows.Forms.Button();
            this.btnDeleteFromList = new System.Windows.Forms.Button();
            this.btnUnpackPackage = new System.Windows.Forms.Button();
            this.btnListRefresh = new System.Windows.Forms.Button();
            this.btnAddCopyToList = new System.Windows.Forms.Button();
            this.lvCopy = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClearCopyList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvDownloaded
            // 
            this.lvDownloaded.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDownloaded.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvDownloaded.FullRowSelect = true;
            this.lvDownloaded.HideSelection = false;
            this.lvDownloaded.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lvDownloaded.LargeImageList = this.imageList1;
            this.lvDownloaded.Location = new System.Drawing.Point(5, 30);
            this.lvDownloaded.MultiSelect = false;
            this.lvDownloaded.Name = "lvDownloaded";
            this.lvDownloaded.Size = new System.Drawing.Size(390, 622);
            this.lvDownloaded.SmallImageList = this.imageList1;
            this.lvDownloaded.TabIndex = 0;
            this.lvDownloaded.UseCompatibleStateImageBehavior = false;
            this.lvDownloaded.View = System.Windows.Forms.View.Details;
            this.lvDownloaded.SelectedIndexChanged += new System.EventHandler(this.ListViewDownloadedSelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 600;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "High-Definition-Ultra-HD-Wallpaper-96262544.jpg");
            // 
            // LblDownloadsRootPath
            // 
            this.LblDownloadsRootPath.AutoSize = true;
            this.LblDownloadsRootPath.Location = new System.Drawing.Point(13, 13);
            this.LblDownloadsRootPath.Name = "LblDownloadsRootPath";
            this.LblDownloadsRootPath.Size = new System.Drawing.Size(35, 13);
            this.LblDownloadsRootPath.TabIndex = 1;
            this.LblDownloadsRootPath.Text = ".";
            // 
            // btnOpenDirectory
            // 
            this.btnOpenDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenDirectory.Image = global::NPS.Properties.Resources.opened_folder;
            this.btnOpenDirectory.Location = new System.Drawing.Point(940, 30);
            this.btnOpenDirectory.Name = "btnOpenDirectory";
            this.btnOpenDirectory.Size = new System.Drawing.Size(73, 42);
            this.btnOpenDirectory.TabIndex = 2;
            this.btnOpenDirectory.Text = "Open directory";
            this.btnOpenDirectory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenDirectory.UseVisualStyleBackColor = true;
            this.btnOpenDirectory.Click += new System.EventHandler(this.BtnOpenDirectoryClick);
            // 
            // btnDeleteFromList
            // 
            this.btnDeleteFromList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteFromList.Image = global::NPS.Properties.Resources.menu_cancel;
            this.btnDeleteFromList.Location = new System.Drawing.Point(940, 194);
            this.btnDeleteFromList.Name = "btnDeleteFromList";
            this.btnDeleteFromList.Size = new System.Drawing.Size(75, 39);
            this.btnDeleteFromList.TabIndex = 3;
            this.btnDeleteFromList.Text = "Delete";
            this.btnDeleteFromList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeleteFromList.UseVisualStyleBackColor = true;
            this.btnDeleteFromList.Click += new System.EventHandler(this.BtnDeleteFromListClick);
            // 
            // btnUnpackPackage
            // 
            this.btnUnpackPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnpackPackage.Enabled = false;
            this.btnUnpackPackage.Image = global::NPS.Properties.Resources.menu_unpack;
            this.btnUnpackPackage.Location = new System.Drawing.Point(940, 239);
            this.btnUnpackPackage.Name = "btnUnpackPackage";
            this.btnUnpackPackage.Size = new System.Drawing.Size(75, 43);
            this.btnUnpackPackage.TabIndex = 4;
            this.btnUnpackPackage.Text = "Unpack PKG";
            this.btnUnpackPackage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUnpackPackage.UseVisualStyleBackColor = true;
            this.btnUnpackPackage.Click += new System.EventHandler(this.BtnUnpackPackageClick);
            // 
            // btnListRefresh
            // 
            this.btnListRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnListRefresh.Image = global::NPS.Properties.Resources.menu_reload;
            this.btnListRefresh.Location = new System.Drawing.Point(940, 309);
            this.btnListRefresh.Name = "btnListRefresh";
            this.btnListRefresh.Size = new System.Drawing.Size(75, 43);
            this.btnListRefresh.TabIndex = 5;
            this.btnListRefresh.Text = "Refresh";
            this.btnListRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnListRefresh.UseVisualStyleBackColor = true;
            this.btnListRefresh.Click += new System.EventHandler(this.BtnListRefreshClick);
            // 
            // btnAddCopyToList
            // 
            this.btnAddCopyToList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCopyToList.Image = global::NPS.Properties.Resources.opened_folder;
            this.btnAddCopyToList.Location = new System.Drawing.Point(940, 78);
            this.btnAddCopyToList.Name = "btnAddCopyToList";
            this.btnAddCopyToList.Size = new System.Drawing.Size(75, 55);
            this.btnAddCopyToList.TabIndex = 6;
            this.btnAddCopyToList.Text = "add to copy list";
            this.btnAddCopyToList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddCopyToList.UseVisualStyleBackColor = true;
            this.btnAddCopyToList.Click += new System.EventHandler(this.BtnAddCopyToListClick);
            // 
            // lvCopy
            // 
            this.lvCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCopy.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lvCopy.FullRowSelect = true;
            this.lvCopy.HideSelection = false;
            this.lvCopy.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem3,
            listViewItem4});
            this.lvCopy.LargeImageList = this.imageList1;
            this.lvCopy.Location = new System.Drawing.Point(419, 30);
            this.lvCopy.MultiSelect = false;
            this.lvCopy.Name = "lvCopy";
            this.lvCopy.Size = new System.Drawing.Size(390, 622);
            this.lvCopy.SmallImageList = this.imageList1;
            this.lvCopy.TabIndex = 7;
            this.lvCopy.UseCompatibleStateImageBehavior = false;
            this.lvCopy.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "";
            this.columnHeader2.Width = 600;
            // 
            // btnClearCopyList
            // 
            this.btnClearCopyList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearCopyList.Image = global::NPS.Properties.Resources.opened_folder;
            this.btnClearCopyList.Location = new System.Drawing.Point(940, 133);
            this.btnClearCopyList.Name = "btnClearCopyList";
            this.btnClearCopyList.Size = new System.Drawing.Size(75, 55);
            this.btnClearCopyList.TabIndex = 8;
            this.btnClearCopyList.Text = "clear list";
            this.btnClearCopyList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearCopyList.UseVisualStyleBackColor = true;
            this.btnClearCopyList.Click += new System.EventHandler(this.BtnClearCopyListClick);
            // 
            // Library
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 653);
            this.Controls.Add(this.btnClearCopyList);
            this.Controls.Add(this.lvCopy);
            this.Controls.Add(this.btnAddCopyToList);
            this.Controls.Add(this.btnListRefresh);
            this.Controls.Add(this.btnUnpackPackage);
            this.Controls.Add(this.btnDeleteFromList);
            this.Controls.Add(this.btnOpenDirectory);
            this.Controls.Add(this.LblDownloadsRootPath);
            this.Controls.Add(this.lvDownloaded);
            this.Name = "Library";
            this.Text = "Library";
            this.Load += new System.EventHandler(this.LibraryFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvDownloaded;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label LblDownloadsRootPath;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnOpenDirectory;
        private System.Windows.Forms.Button btnDeleteFromList;
        private System.Windows.Forms.Button btnUnpackPackage;
        private System.Windows.Forms.Button btnListRefresh;
        private System.Windows.Forms.Button btnAddCopyToList;
        private System.Windows.Forms.ListView lvCopy;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnClearCopyList;
    }
}