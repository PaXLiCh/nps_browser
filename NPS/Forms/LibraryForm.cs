using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

using NPS.Data;
using NPS.Helpers;

namespace NPS
{
    /// <summary>
    /// Form of library for downloaded titles.
    /// </summary>
    public partial class LibraryForm : Form
    {
        private class LibraryItem
        {
            public Item itm;
            public bool isPkg = false;
            public string path;
        }

        private readonly List<Item> db;

        private readonly List<string> file_list = new List<string>();

        private const string PKG_EXTENSION = "*.pkg";

        private Task _imagesDownloadTask;
        private readonly List<Item> imagesToLoad = new List<Item>();

        public LibraryForm()
        {
            InitializeComponent();
            db = Database.Instance.Apps;
        }

        private void LibraryFormLoad(object sender, EventArgs e)
        {
            lvDownloaded.Items.Clear();
            lvCopy.Items.Clear();

            LblDownloadsRootPath.Text = Settings.Instance.downloadDir;

            FindTitlesPerPlatform(Platform.PSP);
            FindTitlesPerPlatform(Platform.PSV);
            FindTitlesPerPlatform(Platform.PS3);
            FindTitlesPerPlatform(Platform.PS4);
            FindTitlesPerPlatform(Platform.PSM);
            FindTitlesPerPlatform(Platform.PSX);

            DownloadImages(imagesToLoad);
        }

        private void FindTitlesPerPlatform(Platform platform)
        {
            string[] apps = new string[0];
            string[] dlcs = new string[0];

            // Directory not specified
            if (string.IsNullOrWhiteSpace(Settings.Instance.downloadDir))
            {
                return;
            }

            // Directory for downloads not exists, create one
            if (!Directory.Exists(Settings.Instance.downloadDir))
            {
                Directory.CreateDirectory(Settings.Instance.downloadDir);
            }

            // List downloaded content
            List<string> files = Directory.GetFiles(Settings.Instance.downloadDir, PKG_EXTENSION).ToList();

            var pathPackagesDir = Settings.Instance.GetDownloadsDirPackages(platform);
            var pathAppDir = Settings.Instance.GetDownloadsDirApp(platform);
            var pathAddcontDir = Settings.Instance.GetDownloadsDirAddons(platform);

            if (Directory.Exists(pathPackagesDir))
            {
                files.AddRange(Directory.GetFiles(pathPackagesDir, PKG_EXTENSION));
            }

            if (Directory.Exists(pathAppDir))
            {
                apps = Directory.GetDirectories(pathAppDir);
            }

            if (Directory.Exists(pathAddcontDir))
            {
                dlcs = Directory.GetDirectories(pathAddcontDir);
            }

            // List of packed PKGs
            foreach (string filePath in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);

                bool found = false;
                foreach (var itm in db)
                {
                    if (fileName.Equals(itm.PackageFileName))
                    {
                        ListViewItem lvi = new ListViewItem(itm.TitleName + " (PKG)");

                        lvDownloaded.Items.Add(lvi);

                        imagesToLoad.Add(itm);
                        lvi.ImageKey = itm.ContentId;

                        LibraryItem item = new LibraryItem
                        {
                            itm = itm,
                            path = filePath,
                            isPkg = true
                        };
                        lvi.Tag = item;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    ListViewItem lvi = new ListViewItem(fileName + " (UNKNOWN PKG)");

                    lvDownloaded.Items.Add(lvi);

                    LibraryItem library = new LibraryItem
                    {
                        path = filePath,
                        isPkg = true
                    };
                    lvi.Tag = library;
                }
            }

            // List of unpacked APPs
            foreach (string appPath in apps)
            {
                string titleId = Path.GetFullPath(appPath).TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).Last();

                bool found = false;
                foreach (var itm in db)
                {
                    if (itm.ContentType != ContentType.APP)
                    {
                        continue;
                    }
                    if (itm.TitleId.Equals(titleId))
                    {
                        ListViewItem lvi = new ListViewItem(itm.TitleName);

                        lvDownloaded.Items.Add(lvi);

                        imagesToLoad.Add(itm);
                        lvi.ImageKey = itm.ContentId;

                        LibraryItem library = new LibraryItem
                        {
                            itm = itm,
                            path = appPath,
                            isPkg = false
                        };
                        lvi.Tag = library;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    ListViewItem lvi = new ListViewItem(titleId + " UNKNOWN");

                    lvDownloaded.Items.Add(lvi);

                    LibraryItem library = new LibraryItem
                    {
                        path = appPath,
                        isPkg = false
                    };
                    lvi.Tag = library;
                }
            }

            // List of unpacked DLCs
            foreach (string dlcPath in dlcs)
            {
                string dlcId = Path.GetFullPath(dlcPath).TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).Last();
                foreach (var itm in Database.Instance.DLCs)
                {
                    if (itm.ContentType != ContentType.DLC)
                    {
                        continue;
                    }
                    if (itm.TitleId.Equals(dlcId))
                    {
                        ListViewItem lvi = new ListViewItem(itm.TitleName);

                        lvDownloaded.Items.Add(lvi);

                        imagesToLoad.Add(itm);
                        lvi.ImageKey = itm.ContentId;

                        LibraryItem library = new LibraryItem
                        {
                            itm = itm,
                            path = dlcPath,
                            isPkg = false
                        };
                        lvi.Tag = library;
                        break;
                    }
                }
            }
        }

        private void DownloadImages([NotNull] List<Item> images)
        {
            Task downloader = new Task(() =>
            {
                foreach (var itm in images)
                {
                    System.Drawing.Image image = null;
                    var path = Path.Combine(itm.ImagesDir, "2.png");
                    if (File.Exists(path))
                    {
                        image = System.Drawing.Image.FromFile(path);
                    }
                    else
                    {
                        path = Path.Combine(itm.ImagesDir, "1.png");
                        if (File.Exists(path))
                        {
                            image = System.Drawing.Image.FromFile(path);
                            image = Utils.GetThumb(image);
                        }
                    }

                    // TODO: download images
                    /*if (image == null)
                    {
                        WebClient wc = new WebClient
                        {
                            Proxy = Settings.Instance.proxy,
                            Encoding = Encoding.UTF8
                        };
                        var img = wc.DownloadData(titleId);
                        using (var ms = new MemoryStream(img))
                        {
                            image = Image.FromStream(ms);
                            //image = Utils.GetThumb(image);
                        }
                    }*/
                    if (image == null)
                    {
                        continue;
                    }
                    Invoke(new Action(() =>
                    {
                        imageList1.Images.Add(itm.ContentId, image);
                    }));
                }
            });
            if (_imagesDownloadTask != null && ((int)_imagesDownloadTask.Status < 4))
            {
                _imagesDownloadTask.ContinueWith((antTask) =>
                {
                    _imagesDownloadTask = downloader;
                    downloader.Start();
                });
            }
            else
            {
                _imagesDownloadTask = downloader;
                downloader.Start();
            }
        }

        private void BtnOpenDirectoryClick(object sender, EventArgs e)
        {
            if (lvDownloaded.SelectedItems.Count == 0) return;
            string path = (lvDownloaded.SelectedItems[0].Tag as LibraryItem).path;
            Process.Start("explorer.exe", "/select, " + path);
        }

        private void BtnDeleteFromListClick(object sender, EventArgs e)
        {
            if (lvDownloaded.SelectedItems.Count == 0) return;
            var itm = lvDownloaded.SelectedItems[0].Tag as LibraryItem;

            try
            {
                if (itm.isPkg)
                {
                    File.Delete(itm.path);
                }
                else
                {
                    Directory.Delete(itm.path, true);
                }

                LibraryFormLoad(null, null);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void BtnUnpackPackageClick(object sender, EventArgs e)
        {
            if (lvDownloaded.SelectedItems.Count == 0) return;
            var itm = lvDownloaded.SelectedItems[0].Tag as LibraryItem;
            if (itm.isPkg == false) return;
            if (itm.itm == null)
            {
                MessageBox.Show("Can't unpack unknown pkg");
                return;
            }

            var pathToUnpack = Path.Combine(Settings.Instance.downloadDir, Path.GetFileName(itm.path));

            if ((itm.itm.Platform == Platform.PS3) && itm.path.ToLower().Contains(Settings.DIR_PACKAGES))
            {
                File.Move(itm.path, pathToUnpack);
            }

            DownloadWorker dw = new DownloadWorker(itm.itm, this);
            dw.Start();
        }

        private void ListViewDownloadedSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDownloaded.SelectedItems.Count == 0) return;
            var itm = lvDownloaded.SelectedItems[0].Tag as LibraryItem;
            btnUnpackPackage.Enabled = itm.isPkg;
        }

        private void BtnListRefreshClick(object sender, EventArgs e)
        {
            LibraryFormLoad(null, null);
        }

        private void BtnAddCopyToListClick(object sender, EventArgs e)
        {
            if (lvDownloaded.SelectedItems.Count == 0) return;
            string path = (lvDownloaded.SelectedItems[0].Tag as LibraryItem).path;
            //System.Diagnostics.Process.Start("explorer.exe", "/select, " + path);
            //List<string> file_list = new List<string>();
            
            /*
            foreach (string file_name in Directory.GetFiles(path))
            {
                file_list.Add(file_name);
            } 
            foreach (string file_name in Directory.GetDirectories(path))
            {
                file_list.Add(file_name);
            }*/
            file_list.Add(path);
            lvCopy.Items.Add(lvDownloaded.SelectedItems[0].Text);
            Clipboard.SetData(DataFormats.FileDrop, file_list.ToArray());
        }

        private void BtnClearCopyListClick(object sender, EventArgs e)
        {
            lvCopy.Items.Clear();
            Clipboard.Clear();
        }

    } // LibraryForm
} // namespace
