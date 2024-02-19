using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

namespace NPS
{
    /// <summary>
    /// Form of library for downloaded titles.
    /// </summary>
    public partial class Library : Form
    {
        private readonly List<Item> db;

        private readonly List<string> file_list = new List<string>();

        private const string PKG_EXTENSION = "*.pkg";
        private const string DIR_APP = "app";
        private const string DIR_PACKAGES = "packages";
        private const string DIR_ADDCONT = "addcont";

        private Task _imagesDownloadTask;

        public Library(List<Item> db)
        {
            InitializeComponent();
            this.db = db;
        }

        private void Library_Load(object sender, EventArgs e)
        {
            lvDownloaded.Items.Clear();
            lvCopy.Items.Clear();

            label1.Text = Settings.Instance.downloadDir;

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

            var pathPackagesDir = Path.Combine(Settings.Instance.downloadDir, DIR_PACKAGES);
            var pathAppDir = Path.Combine(Settings.Instance.downloadDir, DIR_APP);
            var pathAddcontDir = Path.Combine(Settings.Instance.downloadDir, DIR_ADDCONT);

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

            List<string> imagesToLoad = new List<string>();

            foreach (string filePath in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);

                bool found = false;
                foreach (var itm in db)
                {
                    if (fileName.Equals(itm.DownloadFileName))
                    {
                        ListViewItem lvi = new ListViewItem(itm.TitleName + " (PKG)");

                        lvDownloaded.Items.Add(lvi);

                        foreach (var r in NPCache.I.renasceneCache)
                        {
                            if (itm.Equals(r.itm))
                            {
                                imagesToLoad.Add(r.imgUrl);
                                lvi.ImageKey = r.imgUrl;
                                break;
                            }
                        }
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

            foreach (string appPath in apps)
            {
                string titleId = Path.GetFullPath(appPath).TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).Last();

                bool found = false;
                foreach (var itm in db)
                {
                    if (itm.IsDLC)
                    {
                        continue;
                    }
                    if (itm.TitleId.Equals(titleId))
                    {
                        ListViewItem lvi = new ListViewItem(itm.TitleName);

                        lvDownloaded.Items.Add(lvi);

                        foreach (var r in NPCache.I.renasceneCache)
                        {
                            if (itm.Equals(r.itm))
                            {
                                imagesToLoad.Add(r.imgUrl);
                                lvi.ImageKey = r.imgUrl;
                                break;
                            }
                        }
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

            //foreach (string s in dlcs)
            //{
            //    string d = Path.GetFullPath(s).TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).Last();
            //    foreach (var itm in db)
            //    {
            //        if (itm.IsDLC && itm.TitleId.Equals(d))
            //        {
            //            ListViewItem lvi = new ListViewItem(itm.TitleName);

            //            listView1.Items.Add(lvi);

            //            foreach (var r in NPCache.I.renasceneCache)
            //                if (itm == r.itm)
            //                {
            //                    imagesToLoad.Add(r.imgUrl);
            //                    lvi.ImageKey = r.imgUrl;
            //    break;
            //                }
            //            LibraryItem library = new LibraryItem();
            //            library.itm = itm;
            //            library.patch = s;
            //            library.isPkg = false;
            //            lvi.Tag = library;
            //break;
            //        }
            //    }
            //}

            DownloadImages(imagesToLoad);
        }

        private void DownloadImages([NotNull] List<string> images)
        {
            Task downloader = new Task(() =>
            {
                foreach (string url in images)
                {
                    WebClient wc = new WebClient
                    {
                        Proxy = Settings.Instance.proxy,
                        Encoding = Encoding.UTF8
                    };
                    var img = wc.DownloadData(url);
                    using (var ms = new MemoryStream(img))
                    {
                        Image image = Image.FromStream(ms);
                        image = GetThumb(image);
                        Invoke(new Action(() =>
                        {
                            imageList1.Images.Add(url, image);
                        }));
                    }
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

        public Bitmap GetThumb(Image image)
        {
            int tw, th, tx, ty;
            int w = image.Width;
            int h = image.Height;
            double whRatio = (double)w / h;

            if (image.Width >= image.Height)
            {
                tw = 100;
                th = (int)(tw / whRatio);
            }
            else
            {
                th = 100;
                tw = (int)(th * whRatio);
            }
            tx = (100 - tw) / 2;
            ty = (100 - th) / 2;
            Bitmap thumb = new Bitmap(100, 100, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(thumb);
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(image,
            new Rectangle(tx, ty, tw, th),
            new Rectangle(0, 0, w, h),
            GraphicsUnit.Pixel);
            return thumb;
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
            var itm = (lvDownloaded.SelectedItems[0].Tag as LibraryItem);

            try
            {
                if (itm.isPkg)
                    File.Delete(itm.path);
                else Directory.Delete(itm.path, true);

                Library_Load(null, null);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void BtnUnpackPackageClick(object sender, EventArgs e)
        {
            if (lvDownloaded.SelectedItems.Count == 0) return;
            var itm = (lvDownloaded.SelectedItems[0].Tag as LibraryItem);
            if (itm.isPkg == false) return;
            if (itm.itm == null)
            {
                MessageBox.Show("Can't unpack unknown pkg");
                return;
            }

            var pathToUnpack = Path.Combine(Settings.Instance.downloadDir, Path.GetFileName(itm.path));

            if (itm.itm.ItsPS3 && itm.path.ToLower().Contains(DIR_PACKAGES))
            {
                File.Move(itm.path, pathToUnpack);
            }

            DownloadWorker dw = new DownloadWorker(itm.itm, this);
            dw.Start();
        }

        private void ListViewDownloadedSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDownloaded.SelectedItems.Count == 0) return;
            var itm = (lvDownloaded.SelectedItems[0].Tag as LibraryItem);
            btnUnpackPackage.Enabled = itm.isPkg;
        }

        private void BtnListRefreshClick(object sender, EventArgs e)
        {
            Library_Load(null, null);
        }

        private void BtnAddCopyToListClick(object sender, EventArgs e)
        {
            if (lvDownloaded.SelectedItems.Count == 0) return;
            string path = (lvDownloaded.SelectedItems[0].Tag as LibraryItem).path;
            //System.Diagnostics.Process.Start("explorer.exe", "/select, " + path);
            // List<string>  file_list = new List<string>();
            
            /*
            foreach (string file_name in Directory.GetFiles(path)){ 

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

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void BtnClearCopyListClick(object sender, EventArgs e)
        {
            lvCopy.Items.Clear();
            Clipboard.Clear();
        }
    }

    class LibraryItem
    {
        public Item itm;
        public bool isPkg = false;
        public string path;
    }
}
