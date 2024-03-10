using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleJson;
using System.Globalization;

using JetBrains.Annotations;

using NPS.Data;
using NPS.Helpers;

namespace NPS
{
    /// <summary>
    /// NPS content browser form.
    /// </summary>
    public partial class BrowserForm : Form
    {
        public const string version = "0.96";

        private List<Item> currentDatabase;

        private int currentOrderColumn = 0;
        private bool currentOrderInverted = false;

        private readonly List<DownloadWorker> downloads = new List<DownloadWorker>();
        private Release[] releases = null;

        // Form
        public BrowserForm()
        {
            InitializeComponent();
            Text = $"{Text} {version}";
            Icon = Properties.Resources._8_512;
            new Settings();

            // jon: not hooked up to my repo yet, and this repo is unmaintained.
            //NewVersionCheck();
        }


        private void DatabaseCacheLoadStarted(Database sender, EventArgs args)
        {
            Invoke(new Action(() =>
            {
                toolStripStatusLabel.Text = "DB loading";
                toolStripProgressBar.Visible = false;
            }));
        }

        private void DatabaseCacheLoaded(Database sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                toolStripStatusLabel.Text = "Ready";
                toolStripProgressBar.Visible = false;
                UpdateUiAfterDatabaseSync();
            }));
        }

        private void DatabaseCacheSyncStarted(Database sender, EventArgs args)
        {
            Invoke(new Action(() =>
            {
                toolStripStatusLabel.Text = "SB synchronization 0%";
                toolStripProgressBar.Value = 0;
                toolStripProgressBar.Visible = true;
            }));
        }

        private void DatabaseCacheSyncing(Database sender, int percentage)
        {
            Invoke(new Action(() =>
            {
                toolStripStatusLabel.Text = $"DB synchronization {percentage}%";
                toolStripProgressBar.Value = percentage;
                toolStripProgressBar.Visible = true;
            }));
        }

        private void DatabaseCacheSynced(Database sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                toolStripStatusLabel.Text = "Ready";
                toolStripProgressBar.Visible = false;
                UpdateUiAfterDatabaseSync();
            }));
        }

        private void DatabaseCacheSaved(Database sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                toolStripStatusLabel.Text = "DB saved";
                toolStripProgressBar.Visible = false;
            }));
        }


        // Form
        private void BrowserFormLoad(object sender, EventArgs e)
        {
            // Subscribe
            Database.Instance.CacheLoadStarted += DatabaseCacheLoadStarted;
            Database.Instance.CacheLoaded += DatabaseCacheLoaded;
            Database.Instance.CacheSaved += DatabaseCacheSaved;
            Database.Instance.CacheSyncStarted += DatabaseCacheSyncStarted;
            Database.Instance.CacheSyncing += DatabaseCacheSyncing;
            Database.Instance.CacheSynced += DatabaseCacheSynced;

            Database.Instance.Load();

            BeginInvoke(new Action(() =>
            {
                // Show notification about missing settings
                if (string.IsNullOrEmpty(Settings.Instance.PSVUri) && string.IsNullOrEmpty(Settings.Instance.PSVDLCUri))
                {
                    MessageBox.Show("Application did not provide any links to external files or decrypt mechanism.\r\nYou need to specify tsv (tab splitted text) file with your personal links to pkg files on your own.\r\n\r\nFormat: TitleId Region Name Pkg Key", "Disclaimer!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    OptionsForm o = new OptionsForm();
                    o.ShowDialog();
                }
            }));

            // Restore downloads
            foreach (var hi in Settings.Instance.history.currentlyDownloading)
            {
                DownloadWorker dw = hi;
                dw.Recreate(this);
                lstDownloadStatus.Items.Add(dw.lvi);
                lstDownloadStatus.AddEmbeddedControl(dw.progress, 3, lstDownloadStatus.Items.Count - 1);
                downloads.Add(dw);
            }

            ServicePointManager.DefaultConnectionLimit = 10;
        }

        private void BrowserFormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Instance.history.currentlyDownloading.Clear();

            foreach (var lstItm in lstDownloadStatus.Items)
            {
                DownloadWorker dw = (lstItm as ListViewItem).Tag as DownloadWorker;

                Settings.Instance.history.currentlyDownloading.Add(dw);
            }

            Settings.Instance.selectedRegions.Clear();
            foreach (var a in cmbRegion.CheckBoxItems)
            {
                if (a.Checked)
                {
                    Settings.Instance.selectedRegions.Add(a.Text);
                }
            }

            Settings.Instance.selectedTypes.Clear();
            foreach (var a in cmbType.CheckBoxItems)
            {
                if (a.Checked)
                {
                    Settings.Instance.selectedTypes.Add(a.Text);
                }
            }

            // Save files
            Settings.Instance.Save();
            Database.Instance.Save();

            // Unsubscribe
            Database.Instance.CacheLoaded -= DatabaseCacheLoaded;
            Database.Instance.CacheSaved -= DatabaseCacheSaved;
            Database.Instance.CacheSyncStarted -= DatabaseCacheSyncStarted;
            Database.Instance.CacheSyncing -= DatabaseCacheSyncing;
            Database.Instance.CacheSynced -= DatabaseCacheSynced;
        }

        private void LoadAllDatabases(object sender, EventArgs e)
        {
            //
        }

        public void Sync(object sender, EventArgs e)
        {
            Database.Instance.Sync();
        }

        private void UpdateUiAfterDatabaseSync()
        {
            rbnGames.Enabled = Database.Instance.IsAppsLoaded;
            rbnDLC.Enabled = Database.Instance.IsDLCsLoaded;
            rbnThemes.Enabled = Database.Instance.IsThemesLoaded;
            rbnAvatars.Enabled = Database.Instance.IsAvatarsLoaded;
            rbnUpdates.Enabled = Database.Instance.IsUpdatesLoaded;

            rbnGames.Checked = true;

            currentDatabase = Database.Instance.Apps;

            // Content types
            cmbType.Items.Clear();
            foreach (string s in Database.Instance.Types)
            {
                cmbType.Items.Add(s);
            }

            // Regions
            cmbRegion.Items.Clear();
            foreach (string s in Database.Instance.Regions)
            {
                cmbRegion.Items.Add(s);
            }

            int countSelected = Settings.Instance.selectedRegions.Count;
            foreach (var a in cmbRegion.CheckBoxItems)
            {
                if (countSelected > 0)
                {
                    if (Settings.Instance.selectedRegions.Contains(a.Text))
                    {
                        a.Checked = true;
                    }
                }
                else
                {
                    a.Checked = true;
                }
            }

            countSelected = Settings.Instance.selectedTypes.Count;

            foreach (var a in cmbType.CheckBoxItems)
            {
                if (countSelected > 0)
                {
                    if (Settings.Instance.selectedTypes.Contains(a.Text))
                    {
                        a.Checked = true;
                    }
                }
                else
                {
                    a.Checked = true;
                }
            }

            cmbRegion.CheckBoxCheckedChanged += txtSearch_TextChanged;
            cmbType.CheckBoxCheckedChanged += txtSearch_TextChanged;

            UpdateSearch();
        }

        private void SetCheckboxState(List<Item> list, int id)
        {
            if (list.Count == 0)
            {
                cmbType.CheckBoxItems[id].Enabled = false;
                cmbType.CheckBoxItems[id].Checked = false;
            }
            else
            {
                cmbType.CheckBoxItems[id].Enabled = true;
                cmbType.CheckBoxItems[id].Checked = true;
            }
        }

        private void CmbRegion_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            UpdateSearch();
        }

        private void NewVersionCheck()
        {
            if (version.Contains("beta")) return;

            Task.Run(() =>
            {
                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (WebClient wc = new WebClient
                    {
                        Proxy = Settings.Instance.proxy,
                        Encoding = Encoding.UTF8,
                        Credentials = CredentialCache.DefaultCredentials
                    })
                    {
                        wc.Headers.Add("user-agent", "MyPersonalApp :)");
                        string content = wc.DownloadString("https://nopaystation.com/vita/npsReleases/version.json");

                        //dynamic test = JsonConvert.DeserializeObject<dynamic>(content);
                        releases = SimpleJson.SimpleJson.DeserializeObject<Release[]>(content);
                    }


                    string newVer = releases[0].version;
                    if (version != newVer)
                    {
                        Invoke(new Action(() =>
                        {
                            downloadUpdateToolStripMenuItem.Visible = true;
                            Text += $"         (!! new version {newVer} available !!)";

                        }));
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            });
        }

        private void RefreshList(List<Item> items)
        {
            List<ListViewItem> list = new List<ListViewItem>();

            foreach (var item in items)
            {
                var a = new ListViewItem(item.TitleId);
                if (Settings.Instance.history.completedDownloading.Contains(item))
                {
                    int newdlc = 0;
                    foreach (var i in item.DlcItm)
                    {
                        if (!Settings.Instance.history.completedDownloading.Contains(i))
                        {
                            ++newdlc;
                        }
                    }

                    if (newdlc > 0)
                    {
                        a.BackColor = ColorTranslator.FromHtml("#E700E7");
                    }
                    else
                    {
                        a.BackColor = ColorTranslator.FromHtml("#B7FF7C");
                    }
                }

                a.SubItems.Add(item.Region);
                a.SubItems.Add(item.TitleName);
                a.SubItems.Add(item.Platform.ToString());
                if (item.DLCs > 0)
                {
                    a.SubItems.Add(item.DLCs.ToString("D", CultureInfo.CurrentCulture));
                }
                else
                {
                    a.SubItems.Add(string.Empty);
                }
                if (item.lastModifyDate != DateTime.MinValue)
                {
                    a.SubItems.Add(item.lastModifyDate.ToString());
                }
                else
                {
                    a.SubItems.Add(string.Empty);
                }
                a.Tag = item;
                list.Add(a);
            }

            lstTitles.BeginUpdate();
            if (rbnDLC.Checked)
            {
                lstTitles.Columns[4].Width = 0;
            }
            else
            {
                lstTitles.Columns[4].Width = 60;
            }
            lstTitles.Items.Clear();
            lstTitles.Items.AddRange(list.ToArray());

            lstTitles.ListViewItemSorter = new ListViewItemComparer(2, false);
            lstTitles.Sort();

            lstTitles.EndUpdate();

            string type = string.Empty;
            if (rbnGames.Checked) { type = "Games"; }
            else if (rbnAvatars.Checked) { type = "Avatars"; }
            else if (rbnDLC.Checked) { type = "DLCs"; }
            else if (rbnThemes.Checked) { type = "Themes"; }
            else if (rbnUpdates.Checked) { type = "Updates"; }
            //else if (rbnPSM.Checked) { type = "PSM Games"; }
            //else if (rbnPSX.Checked) {type = "PSX Games"; }

            lblCount.Text = $"{list.Count}/{currentDatabase.Count} {type}";
        }


        // Menu
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm o = new OptionsForm();
            o.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void downloadUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = releases?[0]?.url;
            if (!string.IsNullOrEmpty(url))
                Process.Start(url);
        }

        private bool IsTitleMatchesSearchEntries(
            [NotNull] Item title,
            [NotNull] IReadOnlyCollection<List<string>> searchEntries)
        {
            if (searchEntries == null || searchEntries.Count == 0)
            {
                return false;
            }

            var titleName = title.TitleName?.ToLowerInvariant();
            var titleId = title.TitleId?.ToLowerInvariant();

            int searchEntryMatches = 0;
            foreach (List<string> searchEntry in searchEntries)
            {
                bool isNotMatch = false;

                foreach (string searchTerm in searchEntry)
                {
                    if (searchTerm.Length == 0)
                    {
                        continue;
                    }
                    if (searchTerm.StartsWith("-"))
                    {
                        // Negative term
                        string negativeTerm = searchTerm.Substring(1);
                        if ((titleName != null && titleName.Contains(negativeTerm))
                            || (titleId != null && titleId.Contains(negativeTerm)))
                        {
                            // Exclude title from result
                            isNotMatch = true;
                            break;
                        }
                    }
                    else if (
                        (titleName == null || (titleName != null && !titleName.Contains(searchTerm)))
                        &&
                        (titleId == null || (titleId != null && !titleId.Contains(searchTerm)))
                        )
                    {
                        isNotMatch = true;
                    }

                }
                if (!isNotMatch)
                {
                    ++searchEntryMatches;
                }
            }
            return searchEntryMatches > 0;
        }


        /// <summary>
        /// Parse search query and return list of separated entries.
        /// </summary>
        /// <param name="searchQuery"> Search query. </param>
        /// <returns> Search entries. </returns>
        [NotNull]
        private List<List<string>> ParseSearchQuery([CanBeNull] string searchQuery)
        {
            List<List<string>> searchEntries = new List<List<string>>();

            if (string.IsNullOrEmpty(searchQuery))
            {
                return searchEntries;
            }

            string[] rawEntries = searchQuery.Split("||".ToCharArray());
            foreach (string entry in rawEntries)
            {
                if (string.IsNullOrWhiteSpace(entry))
                {
                    continue;
                }

                List<string> searchTerms = new List<string>();
                string[] rawTerms = entry?.Split(' ');
                foreach (string term in rawTerms)
                {
                    if (string.IsNullOrWhiteSpace(term))
                    {
                        continue;
                    }
                    searchTerms.Add(term);
                }
                searchEntries.Add(searchTerms);
            }
            return searchEntries;
        }

        public void UpdateSearch()
        {
            if (currentDatabase == null)
            {
                return;
            }

            List<List<string>> searchEntries = ParseSearchQuery(txtSearch.Text.ToLowerInvariant());

            List<Item> itms = new List<Item>();

            foreach (var item in currentDatabase)
            {
                bool dirty = searchEntries.Count != 0 && !IsTitleMatchesSearchEntries(item, searchEntries);

                if (!dirty)
                {
                    if (rbnDLC.Checked)
                    {
                        if (rbnUndownloaded.Checked && Settings.Instance.history.completedDownloading.Contains(item))
                        {
                            dirty = true;
                        }
                        if (rbnDownloaded.Checked && (!Settings.Instance.history.completedDownloading.Contains(item)))
                        {
                            dirty = true;
                        }
                    }
                    else
                    {
                        if (rbnDownloaded.Checked && (!Settings.Instance.history.completedDownloading.Contains(item)))
                        {
                            dirty = true;
                        }
                        else if (Settings.Instance.history.completedDownloading.Contains(item))
                        {
                            if (rbnUndownloaded.Checked && (chkUnless.Checked == false))
                            {
                                dirty = true;
                            }
                            else if (rbnUndownloaded.Checked && chkUnless.Checked)
                            {
                                int newDLC = 0;

                                foreach (var item2 in item.DlcItm)
                                {
                                    if (!Settings.Instance.history.completedDownloading.Contains(item2))
                                    {
                                        ++newDLC;
                                    }
                                }
                                if (newDLC == 0)
                                {
                                    dirty = true;
                                }
                            }
                        }
                    }
                }

                // TODO: optimize platform selection
                if (!dirty
                     && ContainsCmbBox(cmbRegion, item.Region)
                     && ContainsCmbBox(cmbType, item.Platform.ToString()))
                /*(cmbRegion.Text == "ALL" || item.Region.Contains(cmbRegion.Text)))*/
                {
                    itms.Add(item);
                }
            }

            RefreshList(itms);
        }


        // Search
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateSearch();
        }

        private bool ContainsCmbBox([NotNull] PresentationControls.CheckBoxComboBox chkbcmb, [NotNull] string text)
        {
            if (chkbcmb == null || string.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            foreach (var item in chkbcmb.CheckBoxItems)
            {
                if (item.Checked && text.Contains(item.Text))
                {
                    return true;
                }
            }
            return false;
        }

        // Browse
        private void rbnGames_CheckedChanged(object sender, EventArgs e)
        {
            downloadAllToolStripMenuItem.Enabled = rbnGames.Checked;

            if (rbnGames.Checked)
            {
                currentDatabase = Database.Instance.Apps;
                UpdateSearch();
            }
        }

        private void rbnAvatars_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnAvatars.Checked)
            {
                currentDatabase = Database.Instance.Avatars;
                UpdateSearch();
            }
        }

        private void rbnDLC_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnDLC.Checked)
            {
                currentDatabase = Database.Instance.DLCs;
                UpdateSearch();
            }
        }

        private void rbnThemes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnThemes.Checked)
            {
                currentDatabase = Database.Instance.Themes;
                UpdateSearch();
            }
        }

        private void rbnUpdates_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnUpdates.Checked)
            {
                currentDatabase = Database.Instance.Updates;
                UpdateSearch();
            }
        }

        //private void rbnPSM_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rbnPSM.Checked)
        //    {
        //        currentDatabase = psmDbs;
        //        UpdateSearch();
        //    }
        //}

        //private void rbnPSX_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rbnPSX.Checked)
        //    {
        //        currentDatabase = psxDbs;
        //        UpdateSearch();
        //    }
        //}

        // Download
        private void BtnDownloadClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Instance.downloadDir) || string.IsNullOrEmpty(Settings.Instance.unpackerPath))
            {
                MessageBox.Show("You don't have a proper configuration.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OptionsForm o = new OptionsForm();
                o.ShowDialog();
                return;
            }


            if (lstTitles.SelectedItems.Count == 0) return;
            List<Item> toDownload = new List<Item>();

            foreach (ListViewItem itm in lstTitles.SelectedItems)
            {
                var a = itm.Tag as Item;

                if (!a.pkg.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    toDownload.Add(a);
                    continue;
                }

                WebClient p4client = new WebClient
                {
                    Credentials = CredentialCache.DefaultCredentials
                };
                p4client.Headers.Add("user-agent", "MyPersonalApp :)");
                string json = p4client.DownloadString(a.pkg);

                JsonObject fields = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(json);
                JsonArray pieces = fields["pieces"] as JsonArray;
                foreach (JsonObject piece in pieces.Cast<JsonObject>())
                {
                    Item inneritm = new Item()
                    {
                        TitleId = a.TitleId,
                        Region = a.Region,
                        TitleName = $"{a.TitleName} (Offset {piece["fileOffset"]})",
                        offset = piece["fileOffset"].ToString(),
                        pkg = piece["url"].ToString(),
                        zRif = a.zRif,
                        ContentId = a.ContentId,
                        lastModifyDate = a.lastModifyDate,

                        Platform = a.Platform,
                        ContentType = a.ContentType,

                        DlcItm = a.DlcItm,
                        ParentGameTitle = a.ParentGameTitle,
                    };

                    toDownload.Add(inneritm);
                }
            }

            foreach (var a in toDownload)
            {
                bool contains = false;
                foreach (var d in downloads)
                {
                    if (d.currentDownload == a)
                    {
                        // Already downloading
                        contains = true;
                        break;
                    }
                }

                if (contains)
                {
                    // Skip queued
                    continue;
                }

                if (a.ContentType == ContentType.DLC)
                {
                    var result = Database.Instance.Apps.FirstOrDefault(i => i.TitleId.StartsWith(a.TitleId.Substring(0, 9)))?.TitleName;
                    a.ParentGameTitle = result ?? string.Empty;
                }

                DownloadWorker dw = new DownloadWorker(a, this);
                lstDownloadStatus.Items.Add(dw.lvi);
                lstDownloadStatus.AddEmbeddedControl(dw.progress, 3, lstDownloadStatus.Items.Count - 1);
                downloads.Add(dw);
            }
        }

        private void lnkOpenRenaScene_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //var u = new Uri("https://www.youtube.com/results?search_query=dead or alive");
            Process.Start(lnkSearchAboutTitle.Tag.ToString());
        }

        // lstTitles
        private void lstTitles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTitles.SelectedItems.Count == 0)
            {
                return;
            }
            var itm = lstTitles.SelectedItems[0].Tag as Item;
            if (itm.Platform == Platform.PS3 || itm.Platform == Platform.PS4)
            {
                if (string.IsNullOrEmpty(itm.zRif))
                {
                    lblPs3LicenseType.Visible = true;
                    lblPs3LicenseType.BackColor = Color.LawnGreen;
                    lblPs3LicenseType.Text = "RAP NOT REQUIRED,\nuse ReActPSN/PSNPatch";
                }
                else if (itm.zRif.Contains("UNLOCK") || itm.zRif.Contains("DLC"))
                {
                    lblPs3LicenseType.Visible = true;
                    lblPs3LicenseType.BackColor = Color.YellowGreen;
                    lblPs3LicenseType.Text = "UNLOCK BY DLC";
                }
                else
                {
                    lblPs3LicenseType.Visible = false;
                    lblPs3LicenseType.Text = string.Empty;
                }
            }
            else
            {
                lblPs3LicenseType.Visible = false;
                lblPs3LicenseType.Text = string.Empty;
            }
        }

        private void lstTitles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (currentOrderColumn == e.Column)
            {
                currentOrderInverted = !currentOrderInverted;
            }
            else
            {
                currentOrderColumn = e.Column; currentOrderInverted = false;
            }

            lstTitles.ListViewItemSorter = new ListViewItemComparer(currentOrderColumn, currentOrderInverted);
            // Call the sort method to manually sort.
            lstTitles.Sort();
        }

        private void lstTitles_KeyDown(object sender, KeyEventArgs e)
        {
            // Select all
            if (e.KeyCode == Keys.A && e.Control)
            {
                //listView1.MultiSelect = true;
                foreach (ListViewItem item in lstTitles.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void downloadAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnDownloadClick(null, null);
            downloadAllDlcsToolStripMenuItem_Click(null, null);
        }

        private async void downloadAllWithPatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnDownloadClick(null, null);
            downloadAllDlcsToolStripMenuItem_Click(null, null);
            await checkForPatchesAndDownloadAsync();
        }

        // lstTitles Menu Strip
        private void showTitleDlcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstTitles.SelectedItems.Count == 0) return;

            Item t = lstTitles.SelectedItems[0].Tag as Item;
            if (t.DLCs > 0)
            {
                rbnDLC.Checked = true;
                txtSearch.Text = t.TitleId;
                rbnAll.Checked = true;
            }
        }

        private void downloadAllDlcsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itm in lstTitles.SelectedItems)
            {
                var parrent = itm.Tag as Item;

                foreach (var a in parrent.DlcItm)
                {
                    a.ParentGameTitle = parrent.TitleName;
                    bool contains = false;
                    foreach (var d in downloads)
                    {
                        if (d.currentDownload == a)
                        {
                            contains = true;
                            // Already downloading
                            break;
                        }
                    }

                    if (!contains)
                    {
                        DownloadWorker dw = new DownloadWorker(a, this);
                        lstDownloadStatus.Items.Add(dw.lvi);
                        lstDownloadStatus.AddEmbeddedControl(dw.progress, 3, lstDownloadStatus.Items.Count - 1);
                        downloads.Add(dw);
                    }
                }
            }
        }

        // lstDownloadStatus
        private void lstDownloadStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                //listView1.MultiSelect = true;
                foreach (ListViewItem item in lstDownloadStatus.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstDownloadStatus.SelectedItems.Count == 0) return;
            foreach (ListViewItem a in lstDownloadStatus.SelectedItems)
            {
                DownloadWorker itm = a.Tag as DownloadWorker;
                itm?.Pause();
            }
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstDownloadStatus.SelectedItems.Count == 0) return;
            foreach (ListViewItem a in lstDownloadStatus.SelectedItems)
            {
                DownloadWorker itm = a.Tag as DownloadWorker;
                itm?.Resume();
            }
        }

        // lstDownloadStatus Menu Strip
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstDownloadStatus.SelectedItems.Count == 0) return;
            foreach (ListViewItem a in lstDownloadStatus.SelectedItems)
            {
                DownloadWorker itm = a.Tag as DownloadWorker;
                itm?.Cancel();
                //itm?.DeletePkg();
            }
        }

        private void retryUnpackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstDownloadStatus.SelectedItems.Count == 0) return;
            foreach (ListViewItem a in lstDownloadStatus.SelectedItems)
            {
                DownloadWorker itm = a.Tag as DownloadWorker;
                itm?.Unpack();
            }
        }

        private void clearCompletedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DownloadWorker> toDel = new List<DownloadWorker>();
            List<ListViewItem> toDelLVI = new List<ListViewItem>();

            foreach (var i in downloads)
            {
                if (i.Status == WorkerStatus.Canceled
                    || i.Status == WorkerStatus.Completed)
                {
                    toDel.Add(i);
                }
            }

            foreach (ListViewItem i in lstDownloadStatus.Items)
            {
                if (toDel.Contains(i.Tag as DownloadWorker))
                {
                    toDelLVI.Add(i);
                }
            }

            foreach (var i in toDel)
            {
                downloads.Remove(i);
            }
            toDel.Clear();

            foreach (var i in toDelLVI)
            {
                lstDownloadStatus.Items.Remove(i);
            }
            toDelLVI.Clear();
        }

        // Timers
        private void timer1_Tick(object sender, EventArgs e)
        {
            int workingThreads = 0;
            int workingCompPack = 0;

            foreach (var dw in downloads)
            {
                if (dw.Status == WorkerStatus.Running)
                {
                    ++workingThreads;
                    if (dw.currentDownload.ItsCompPack)
                    {
                        ++workingCompPack;
                    }
                }

            }

            if (workingThreads < Settings.Instance.simultaneousDl)
            {
                foreach (var dw in downloads)
                {
                    if (dw.Status == WorkerStatus.Queued)
                    {
                        if (dw.currentDownload.ItsCompPack && workingCompPack > 0)
                            break;
                        else
                        {
                            dw.Start();
                            break;
                        }
                    }
                }
            }
        }

        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private Item previousSelectedItem = null;

        private async void timer2_Tick(object sender, EventArgs e)
        {
            // Update view

            if (lstTitles.SelectedItems.Count == 0) return;
            Item itm = lstTitles.SelectedItems[0].Tag as Item;

            if (itm == previousSelectedItem)
            {
                return;
            }

            previousSelectedItem = itm;

            // Reset info controls to empty/loading state
            ptbCover.Image = ptbCover.InitialImage;
            txtPkgInfo.Text = string.Empty;
            lnkSearchAboutTitle.Text = string.Empty;

            tokenSource?.Cancel();
            tokenSource = new CancellationTokenSource();

            await Task.Run(() =>
            {
                try
                {
                    StoreInfo storeInfo = StoreInfo.GetStoreInfo(itm, ImagesDownloadedHandler);

                    var path = Path.Combine(itm.ImagesDir, "1.png");
                    System.Drawing.Image image = null;

                    // We have some info about title
                    string infoText = storeInfo.ToString();
                    // Hope we load image
                    if (File.Exists(path))
                    {
                        image = System.Drawing.Image.FromFile(path);
                    }
                    Invoke(new Action(() =>
                    {
                        if (image != null)
                        {
                            ptbCover.Image = image;
                        }
                        txtPkgInfo.Text = infoText;
                        //lnkSearchAboutTitle.Tag = r.Url;
                        lnkSearchAboutTitle.Tag = $"https://www.google.com/search?safe=off&source=lnms&tbm=isch&sa=X&biw=785&bih=698&q={itm.TitleName}%20{itm.Platform}";
                        lnkSearchAboutTitle.Visible = true;
                    }));
                }
                catch (OperationCanceledException)
                {
                    // do nothing
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }, tokenSource.Token);
        }

        private void ImagesDownloadedHandler(FileDownloader sender, bool isOk, string uri, string localPath)
        {
            Invoke(new Action(() =>
            {
                if (lstTitles.SelectedItems.Count == 0) return;
                Item itm = lstTitles.SelectedItems[0].Tag as Item;

                var path = Path.Combine(itm.ImagesDir, "1.png");
                if (!path.Equals(localPath))
                {
                    // Changed selection before images downloaded
                    return;
                }
                System.Drawing.Image image = null;
                if (isOk)
                {
                    image = System.Drawing.Image.FromFile(path);
                }
                else
                {
                    // TODO: placeholder
                }
                if (image != null)
                {
                    ptbCover.Image = image;
                }
            }));
        }

        private void PauseAllBtnClick(object sender, EventArgs e)
        {
            foreach (ListViewItem itm in lstDownloadStatus.Items)
            {
                (itm.Tag as DownloadWorker).Pause();
            }
        }

        private void ResumeAllBtnClick(object sender, EventArgs e)
        {
            foreach (ListViewItem itm in lstDownloadStatus.Items)
            {
                (itm.Tag as DownloadWorker).Resume();
            }
        }

        private void lstTitles_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var a = sender as ListView;
                if (a.SelectedItems.Count > 0)
                {
                    var itm = a.SelectedItems[0].Tag as Item;
                    if (itm.DLCs == 0)
                    {
                        showTitleDlcToolStripMenuItem.Enabled = false;
                        downloadAllDlcsToolStripMenuItem.Enabled = false;
                    }
                    else
                    {
                        showTitleDlcToolStripMenuItem.Enabled = true;
                        downloadAllDlcsToolStripMenuItem.Enabled = true;
                    }
                }
            }
        }

        private void ShowDescriptionPanel(object sender, EventArgs e)
        {
            ContentInfoForm d = new ContentInfoForm(lstTitles);
            d.Show();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnOpenDirectoryClick(object sender, EventArgs e)
        {
            if (lstDownloadStatus.SelectedItems.Count == 0) return;
            var worker = lstDownloadStatus.SelectedItems[0];
            DownloadWorker itm = worker.Tag as DownloadWorker;

            if (File.Exists(itm.Pkg))
            {
                Process.Start("explorer.exe", $"/select, {itm.Pkg}");
            }
        }

        private void ts_changeLog_Click(object sender, EventArgs e)
        {
            if (releases == null)
            {
                return;
            }
            foreach (var r in releases)
            {
                if (r.version != version)
                {
                    continue;
                }
                ShowChanglelog(r);
                break;
            }
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (releases == null)
            {
                return;
            }
            Release r = releases[0];
            ShowChanglelog(r);
        }

        private void ShowChanglelog([NotNull] Release r)
        {
            var txt = new StringBuilder();
            foreach (var s in r.changelog)
            {
                txt.AppendLine(s);
            }

            MessageBox.Show(txt.ToString(), $"Changelog {r.version}");
        }

        private void checkForPatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Instance.HMACKey))
            {
                MessageBox.Show("No hmackey");
                return;
            }

            if (lstTitles.SelectedItems.Count == 0) return;

            foreach (var entry in lstTitles.SelectedItems)
            {
                PatchForm gp = new PatchForm(lstTitles.SelectedItems[0].Tag as Item, (item) =>
                {
                    DownloadWorker dw = new DownloadWorker(item, this);
                    lstDownloadStatus.Items.Add(dw.lvi);
                    lstDownloadStatus.AddEmbeddedControl(dw.progress, 3, lstDownloadStatus.Items.Count - 1);
                    downloads.Add(dw);
                });

                gp.AskForUpdate();
            }
        }

        private async Task checkForPatchesAndDownloadAsync()
        {
            if (string.IsNullOrEmpty(Settings.Instance.HMACKey))
            {
                MessageBox.Show("No hmackey");
                return;
            }

            if (lstTitles.SelectedItems.Count == 0) return;

            foreach (var entry in lstTitles.SelectedItems)
            {
                PatchForm gp = new PatchForm(lstTitles.SelectedItems[0].Tag as Item, (item) =>
                {
                    DownloadWorker dw = new DownloadWorker(item, this);
                    lstDownloadStatus.Items.Add(dw.lvi);
                    lstDownloadStatus.AddEmbeddedControl(dw.progress, 3, lstDownloadStatus.Items.Count - 1);
                    downloads.Add(dw);
                });
                await gp.DownloadUpdateNoAsk();
            }
        }

        private void toggleDownloadedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstTitles.SelectedItems.Count == 0) { return; }

            for (int i = 0; i < lstTitles.SelectedItems.Count; ++i)
            {
                if (Settings.Instance.history.completedDownloading.Contains(lstTitles.SelectedItems[i].Tag as Item))
                {
                    //lstTitles.SelectedItems[i].BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    Settings.Instance.history.completedDownloading.Remove(lstTitles.SelectedItems[i].Tag as Item);
                }
                else
                {
                    //lstTitles.SelectedItems[i].BackColor = ColorTranslator.FromHtml("#B7FF7C");
                    Settings.Instance.history.completedDownloading.Add(lstTitles.SelectedItems[i].Tag as Item);
                }
            }

            UpdateSearch();
        }

        private void chkHideDownloaded_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkUnless_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSearch();
        }

        private void rbnDownloaded_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSearch();
        }

        private void rbnUndownloaded_CheckedChanged(object sender, EventArgs e)
        {
            chkUnless.Enabled = rbnUndownloaded.Checked;
            UpdateSearch();
        }

        private void rbnAll_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSearch();
        }

        private void libraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LibraryForm l = new LibraryForm();
            l.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Instance.compPackUrl))
            {
                MessageBox.Show("No CompPack url");
                return;
            }

            if (lstTitles.SelectedItems.Count == 0) return;

            CompPackForm cp = new CompPackForm(lstTitles.SelectedItems[0].Tag as Item, (item) =>
            {
                foreach (var itm in item)
                {
                    DownloadWorker dw = new DownloadWorker(itm, this);
                    lstDownloadStatus.Items.Add(dw.lvi);
                    lstDownloadStatus.AddEmbeddedControl(dw.progress, 3, lstDownloadStatus.Items.Count - 1);
                    downloads.Add(dw);
                }
            });
            cp.ShowDialog();
        }
    } // class BrowserForm
} // namespace
