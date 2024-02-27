using System;
using System.Windows.Forms;
using System.Net;
using NPS.Data;

namespace NPS
{
    /// <summary>
    /// Options window.
    /// </summary>
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();

            Database.Instance.CacheLoadStarted += DatabaseCacheLoadStarted;
            Database.Instance.CacheLoaded += DatabaseCacheLoaded;
            Database.Instance.CacheSyncStarted += DatabaseCacheSyncStarted;
            Database.Instance.CacheSyncing += DatabaseCacheSyncing;
            Database.Instance.CacheSynced += DatabaseCacheSynced;
        }

        private void DatabaseCacheLoadStarted(Database sender, EventArgs args)
        {
            Invoke(new Action(() =>
            {
                btnSyncNow.Enabled = false;
                lblCacheDate.Text = "Cache loading";
            }));
        }

        private void DatabaseCacheLoaded(Database sender, EventArgs args)
        {
            Invoke(new Action(() =>
            {
                btnSyncNow.Enabled = true;
                lblCacheDate.Text = $"Cache date: {Database.Instance.Cache.UpdateDate}";
            }));
        }

        private void DatabaseCacheSyncStarted(Database sender, EventArgs args)
        {
            Invoke(new Action(() =>
            {
                btnSyncNow.Enabled = false;
                lblCacheDate.Text = "Cache sync started";
            }));
        }

        private void DatabaseCacheSyncing(Database sender, int percentage)
        {
            Invoke(new Action(() =>
            {
                btnSyncNow.Enabled = false;
                lblCacheDate.Text = $"Cache sync started {percentage}%";
            }));
        }

        private void DatabaseCacheSynced(Database sender, EventArgs args)
        {
            Invoke(new Action(() =>
            {
                btnSyncNow.Enabled = true;
                lblCacheDate.Text = $"Cache date: {Database.Instance.Cache.UpdateDate}";
            }));
        }

        private void OptionsFormLoad(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void OptionsFormClosing(object sender, FormClosingEventArgs e)
        {
            Database.Instance.CacheLoadStarted -= DatabaseCacheLoadStarted;
            Database.Instance.CacheLoaded -= DatabaseCacheLoaded;
            Database.Instance.CacheSyncStarted -= DatabaseCacheSyncStarted;
            Database.Instance.CacheSyncing -= DatabaseCacheSyncing;
            Database.Instance.CacheSynced -= DatabaseCacheSynced;

            UpdateSettings(true);
        }

        private void LoadSettings()
        {
            // Settings
            cbDeletePkgAfterUnpack.Checked = Settings.Instance.deleteAfterUnpack;
            nudSimultaneousDownloads.Value = Settings.Instance.simultaneousDl;
            cbDownloadImages.Checked = Settings.Instance.IsAutoDownloadImages;
            cbDownloadPromo.Checked = Settings.Instance.IsAutoDownloadPromo;

            textDownloadPath.Text = Settings.Instance.downloadDir;
            textUnpackerPath.Text = Settings.Instance.unpackerPath;
            textUnpackerParams.Text = Settings.Instance.unpackerParams;
            textPSVFSPParser.Text = Settings.Instance.psvParserPath;
            textPSVFSPParserParams.Text = Settings.Instance.psvParserParams;

            // Game URIs
            tb_psvuri.Text = Settings.Instance.PSVUri;
            tb_psmuri.Text = Settings.Instance.PSMUri;
            tb_psxuri.Text = Settings.Instance.PSXUri;
            tb_pspuri.Text = Settings.Instance.PSPUri;
            tb_ps3uri.Text = Settings.Instance.PS3Uri;
            tb_ps4uri.Text = Settings.Instance.PS4Uri;

            // Avatar URIs
            tb_ps3avataruri.Text = Settings.Instance.PS3AvatarUri;

            // DLC URIs
            tb_psvdlcuri.Text = Settings.Instance.PSVDLCUri;
            tb_pspdlcuri.Text = Settings.Instance.PSPDLCUri;
            tb_ps3dlcuri.Text = Settings.Instance.PS3DLCUri;
            tb_ps4dlcuri.Text = Settings.Instance.PS4DLCUri;

            // Theme URIs
            tb_psvthmuri.Text = Settings.Instance.PSVThemeUri;
            tb_pspthmuri.Text = Settings.Instance.PSPThemeUri;
            tb_ps3thmuri.Text = Settings.Instance.PS3ThemeUri;
            tb_ps4thmuri.Text = Settings.Instance.PS4ThemeUri;

            // Update URIs
            tb_psvupduri.Text = Settings.Instance.PSVUpdateUri;
            tb_ps4upduri.Text = Settings.Instance.PS4UpdateUri;
            hmacTB.Text = Settings.Instance.HMACKey;
            tb_compPack.Text = Settings.Instance.compPackUrl;
            tb_compackPatch.Text = Settings.Instance.compPackPatchUrl;

            cbProxy.Checked = Settings.Instance.proxy != null;
            if (Settings.Instance.proxy != null)
            {
                tbProxyAddress.Text = Settings.Instance.proxy.Address.Host;
                tbProxyPort.Text = Settings.Instance.proxy.Address.Port.ToString();
            }

            btnSyncNow.Enabled = true;
            if (Database.Instance.Cache == null)
            {
                lblCacheDate.Text = $"No cache, press \"Sync now\"";
            }
            else
            {
                lblCacheDate.Text =  $"Cache date: {Database.Instance.Cache.UpdateDate}";
            }
        }

        private void ButtonDownloadPathClick(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textDownloadPath.Text = fbd.SelectedPath;
                    Settings.Instance.downloadDir = textDownloadPath.Text;
                }
            }
        }

        private void ButtonUnpackerPathClick(object sender, EventArgs e)
        {
            using (var fbd = new OpenFileDialog())
            {
                if (Type.GetType("Mono.Runtime") != null)
                {
                    fbd.Filter = "|*";
                }
                else
                {
                    fbd.Filter = "|*.exe";
                }

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    textUnpackerPath.Text = fbd.FileName;
                    Settings.Instance.unpackerPath = textUnpackerPath.Text;
                }
            }
        }

        private void BtnPSVParserPathClick(object sender, EventArgs e)
        {
            using (var fbd = new OpenFileDialog())
            {
                if (Type.GetType("Mono.Runtime") != null)
                {
                    fbd.Filter = "|*";
                }
                else
                {
                    fbd.Filter = "|*.exe";
                }

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    textPSVFSPParser.Text = fbd.FileName;
                    Settings.Instance.unpackerPath = textUnpackerPath.Text;
                }
            }
        }

        private bool needResync = false;

        private void UpdateSettings(bool withStoring)
        {
            // Resync after closing settings window when PSV sources changed
            needResync = needResync
                         || (Settings.Instance.PSVUri != null && Settings.Instance.PSVUri.CompareTo(tb_psvuri.Text) == 0)
                         || (Settings.Instance.PSMUri != null && Settings.Instance.PSMUri.CompareTo(tb_psmuri.Text) == 0)
                         || (Settings.Instance.PSXUri != null && Settings.Instance.PSXUri.CompareTo(tb_psxuri.Text) == 0)
                         || (Settings.Instance.PSPUri != null && Settings.Instance.PSPUri.CompareTo(tb_pspuri.Text) == 0)
                         || (Settings.Instance.PS3Uri != null && Settings.Instance.PS3Uri.CompareTo(tb_ps3uri.Text) == 0)
                         || (Settings.Instance.PS4Uri != null && Settings.Instance.PS4Uri.CompareTo(tb_ps4uri.Text) == 0)
                         || (Settings.Instance.PSVThemeUri != null && Settings.Instance.PSVThemeUri.CompareTo(tb_psvthmuri.Text) == 0)
                         || (Settings.Instance.PSVDLCUri != null && Settings.Instance.PSVDLCUri.CompareTo(tb_psvdlcuri.Text) == 0)
                         || (Settings.Instance.PSPDLCUri != null && Settings.Instance.PSPDLCUri.CompareTo(tb_pspdlcuri.Text) == 0)
                         || (Settings.Instance.PS3DLCUri != null && Settings.Instance.PS3DLCUri.CompareTo(tb_ps3dlcuri.Text) == 0)
                         || (Settings.Instance.PS4DLCUri !=  null && Settings.Instance.PS4DLCUri.CompareTo(tb_ps4dlcuri.Text) == 0);

            // Settings
            Settings.Instance.downloadDir = textDownloadPath.Text;
            Settings.Instance.unpackerPath = textUnpackerPath.Text;
            Settings.Instance.unpackerParams = textUnpackerParams.Text;
            Settings.Instance.deleteAfterUnpack = cbDeletePkgAfterUnpack.Checked;
            Settings.Instance.simultaneousDl = (int)nudSimultaneousDownloads.Value;
            Settings.Instance.psvParserPath = textPSVFSPParser.Text;
            Settings.Instance.psvParserParams = textPSVFSPParserParams.Text;
            Settings.Instance.HMACKey = hmacTB.Text;

            // Game URIs
            Settings.Instance.PSVUri = tb_psvuri.Text;
            Settings.Instance.PSMUri = tb_psmuri.Text;
            Settings.Instance.PSXUri = tb_psxuri.Text;
            Settings.Instance.PSPUri = tb_pspuri.Text;
            Settings.Instance.PS3Uri = tb_ps3uri.Text;
            Settings.Instance.PS4Uri = tb_ps4uri.Text;

            // Avatar URIs
            Settings.Instance.PS3AvatarUri = tb_ps3avataruri.Text;

            // DLC URIs
            Settings.Instance.PSVDLCUri = tb_psvdlcuri.Text;
            Settings.Instance.PSPDLCUri = tb_pspdlcuri.Text;
            Settings.Instance.PS3DLCUri = tb_ps3dlcuri.Text;
            Settings.Instance.PS4DLCUri = tb_ps4dlcuri.Text;

            // Theme URIs
            Settings.Instance.PSVThemeUri = tb_psvthmuri.Text;
            Settings.Instance.PSPThemeUri = tb_pspthmuri.Text;
            Settings.Instance.PS3ThemeUri = tb_ps3thmuri.Text;
            Settings.Instance.PS4ThemeUri = tb_ps4thmuri.Text;

            // Update URIs
            Settings.Instance.PSVUpdateUri = tb_psvupduri.Text;
            Settings.Instance.PS4UpdateUri = tb_ps4upduri.Text;

            // CompPack
            if (Settings.Instance.compPackUrl != tb_compPack.Text
                || Settings.Instance.compPackPatchUrl != tb_compackPatch.Text)
            {
                CompPackForm.compPackChanged = true;
            }

            Settings.Instance.compPackUrl = tb_compPack.Text;
            Settings.Instance.compPackPatchUrl = tb_compackPatch.Text;

            // Proxy
            if (cbProxy.Checked)
            {
                Settings.Instance.proxy = new WebProxy(tbProxyAddress.Text, int.Parse(tbProxyPort.Text))
                {
                    Credentials = CredentialCache.DefaultCredentials
                };
            }
            else
            {
                Settings.Instance.proxy = null;
            }

            if (withStoring)
            {
                Settings.Instance.Save();
                if (needResync)
                {
                    Database.Instance.Sync();
                }
            }
        }

        private void btn_psvuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_psvuri);
        }

        private void btn_psmuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_psmuri);
        }

        private void btn_psxuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_psxuri);
        }

        private void btn_ps3uri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_ps3uri);
        }

        private void btn_ps4uri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_ps4uri);
        }

        private void btn_ps3avataruri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_ps3avataruri);
        }

        private void btn_pspuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_pspuri);
        }

        private void btn_psvdlcuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_psvdlcuri);
        }

        private void btn_ps3dlcuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_ps3dlcuri);
        }

        private void btn_pspdlcuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_pspdlcuri);
        }

        private void btn_ps4dlcuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_ps4dlcuri);
        }

        private void btn_psvthmuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_psvthmuri);
        }

        private void btn_ps3thmuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_ps3thmuri);
        }

        private void btn_ps4thmuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_ps4thmuri);
        }

        private void btn_pspthmuri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_pspthmuri);
        }

        private void btn_psvupduri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_psvupduri);
        }

        private void btn_ps4upduri_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_ps4upduri);
        }

        private void ShowOpenFileWindow(TextBox tb)
        {
            using (var fbd = new OpenFileDialog())
            {
                fbd.Filter = "|*.tsv";

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    tb.Text = fbd.FileName;
                }
            }
        }

        private void CbDeletePackageAfterUnpackCheckedChanged(object sender, EventArgs e)
        {
            Settings.Instance.deleteAfterUnpack = cbDeletePkgAfterUnpack.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Settings.Instance.simultaneousDl = (int)nudSimultaneousDownloads.Value;
        }

        private void LinkLabelPkgParamsHelpClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(@"Here you can give parameters to pass to your PKG decompressing tool. Available variables are: 
- {zRifKey}
- {pkgFile}
- {pathOut}
- {gameTitle}
- {region}
- {titleID}
- {contentID}
- {fwversion}");
        }

        private void LinkLabelPSVFSPParserHelpClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(@"Here you can give parameters to pass to your PSV parser tool. Available variables are: 
- {zRifKey}
- {pkgFile}
- {pathIn}
- {pathOut}
- {gameTitle}
- {region}
- {titleID}
- {contentID}
- {fwversion}
- {henkaku}");
        }

        private void BtnSyncNowClick(object sender, EventArgs e)
        {
            Database.Instance.Sync();
        }

        private void TbProxyPortKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void CbProxyCheckedChanged(object sender, EventArgs e)
        {
            bool isProxyEnabled = (sender as CheckBox).Checked;
            tbProxyAddress.Enabled = isProxyEnabled;
            tbProxyPort.Enabled = isProxyEnabled;
        }

        private void CbDownloadImagesCheckedChanged(object sender, EventArgs e)
        {
            Settings.Instance.IsAutoDownloadImages = cbDownloadImages.Checked;
        }

        private void CbDownloadPromoCheckedChanged(object sender, EventArgs e)
        {
            Settings.Instance.IsAutoDownloadPromo = cbDownloadPromo.Checked;
        }

    } // OptionsForm
} // namespace
