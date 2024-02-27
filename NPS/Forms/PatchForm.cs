using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace NPS
{
    public partial class PatchForm : Form
    {
        private readonly Item _title;
        private readonly Action<Item> _downloadedHandler;
        private Item newItem = null;

        public PatchForm(Item title, Action<Item> downloadedHandler)
        {
            InitializeComponent();
            _title = title;
            _downloadedHandler = downloadedHandler;
        }

        public void AskForUpdate()
        {
            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

            try
            {
                string updateUrl = GetUpdateLink(_title.TitleId);

                WebClient wc = new WebClient
                {
                    Encoding = Encoding.UTF8
                };
                string content = wc.DownloadString(updateUrl);

                string ver = string.Empty;
                string pkgUrl = string.Empty;
                string contentId = string.Empty;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(content);
                var packages = doc.DocumentElement.SelectNodes("/titlepatch/tag/package");

                var lastPackage = packages[packages.Count - 1];
                ver = lastPackage.Attributes["version"].Value;
                string sysVer = lastPackage.Attributes["psp2_system_ver"].Value;

                var changeinfo = lastPackage.SelectSingleNode("changeinfo");
                string changeInfoUrl = changeinfo.Attributes["url"].Value;

                var hybrid_package = lastPackage.SelectSingleNode("hybrid_package");

                if (hybrid_package != null)
                {
                    lastPackage = hybrid_package;
                }

                pkgUrl = lastPackage.Attributes["url"].Value;
                string size = lastPackage.Attributes["size"].Value;
                contentId = lastPackage.Attributes["content_id"].Value;

                string contentChangeset = wc.DownloadString(changeInfoUrl);

                doc.LoadXml(contentChangeset);
                var changesList = doc.DocumentElement.SelectNodes("/changeinfo/changes");

                string changesString = string.Empty;
                foreach (XmlNode itm in changesList)
                {
                    changesString += itm.Attributes["app_ver"].Value + "</br>";
                    changesString += itm.InnerText + "</br>";
                }

                Show();
                sysVer = long.Parse(sysVer).ToString("X").Substring(0, 3).Insert(1, ".");
                BtnDownload.Text = $"Download patch: {ver} (FW: {sysVer})";

                //byte[] bytes = Encoding.Default.GetBytes(changesString);
                //changesString = Encoding.UTF8.GetString(bytes);
                webBrowser1.DocumentText = changesString;

                newItem = new Item
                {
                    ContentId = $"{contentId}_patch_{ver}",
                    pkg = pkgUrl,
                    TitleId = _title.TitleId,
                    Region = _title.Region,
                    TitleName = $"{_title.TitleName} Patch {ver}",
                    ContentType = ContentType.UPDATE
                };
            }
            catch (WebException error)
            {
                var response = error.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                {
                    MessageBox.Show("No patches for title");
                }
                else
                {
                    MessageBox.Show("Unknown error");
                }
                Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unknown error");
                Console.WriteLine($"Error: {e.Message}");
                Close();
            }
        }

        public async Task<Item> DownloadUpdateNoAsk()
        {
            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

            try
            {
                var updateUrl = GetUpdateLink(_title.TitleId);

                var wc = new WebClient
                {
                    Encoding = Encoding.UTF8
                };
                var content = await wc.DownloadStringTaskAsync(updateUrl);

                if (string.IsNullOrEmpty(content))
                {
                    Console.WriteLine($"No patch found for {_title.TitleId}");
                    return null;
                }

                var ver = string.Empty;
                var pkgUrl = string.Empty;
                var contentId = string.Empty;

                var doc = new XmlDocument();
                doc.LoadXml(content);
                var packages = doc.DocumentElement.SelectNodes("/titlepatch/tag/package");

                var lastPackage = packages[packages.Count - 1];
                ver = lastPackage.Attributes["version"].Value;
                var sysVer = lastPackage.Attributes["psp2_system_ver"].Value;

                var changeinfo = lastPackage.SelectSingleNode("changeinfo");
                var changeInfoUrl = changeinfo.Attributes["url"].Value;

                var hybrid_package = lastPackage.SelectSingleNode("hybrid_package");

                if (hybrid_package != null)
                {
                    lastPackage = hybrid_package;
                }

                pkgUrl = lastPackage.Attributes["url"].Value;
                var size = lastPackage.Attributes["size"].Value;
                contentId = lastPackage.Attributes["content_id"].Value;

                newItem = new Item
                {
                    ContentId = $"{contentId}_patch_{ver}",
                    pkg = pkgUrl,
                    TitleId = _title.TitleId,
                    Region = _title.Region,
                    TitleName = $"{_title.TitleName} Patch {ver}",
                    ContentType = ContentType.UPDATE
                };

                _downloadedHandler.Invoke(newItem);
                return newItem;

            }
            catch (WebException error)
            {
                // MessageBox.Show("Unknown error");
                if (error.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Do nothing
                }
                //else
                //{
                //  MessageBox.Show("Unknown error");
                //}
                Console.WriteLine($"Error: {error.Message}");

                Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unknown error");
                Console.WriteLine($"Error: {e.Message}");
                Close();
            }

            return null;
        }

        private static string GetUpdateLink(string title)
        {
            string url = "https://gs-sec.ww.np.dl.playstation.net/pl/np/{0}/{1}/{0}-ver.xml";
            string key = "0x" + Settings.Instance.HMACKey;

            var binary = new List<byte>();
            for (int i = 2; i < key.Length; i += 2)
            {
                string s = new string(new[] { key[i], key[i + 1] });
                binary.Add(byte.Parse(s, NumberStyles.HexNumber));
            }

            var hmac = new HMACSHA256(binary.ToArray());
            var byte_hash = hmac.ComputeHash(Encoding.ASCII.GetBytes($"np_{title}"));

            StringBuilder hash = new StringBuilder();
            foreach (var k in byte_hash)
            {
                hash.Append(k.ToString("X2").ToLower());
            }

            return string.Format(url, title, hash.ToString());
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnDownloadClick(object sender, EventArgs e)
        {
            if (newItem == null)
            {
                MessageBox.Show("Unable to download. Some error occured");
            }
            else
            {
                _downloadedHandler.Invoke(newItem);
            }
            Close();
        }

    } // PatchForm
} // namespace
