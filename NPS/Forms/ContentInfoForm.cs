using System;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

using NPS.Data;

namespace NPS
{
    public partial class ContentInfoForm : Form
    {
        private string contentId;
        private string region;
        private readonly ListView _listView;
        private bool isLoading = false;
        private string currentContentId = string.Empty;

        public ContentInfoForm([NotNull] ListView listView)
        {
            InitializeComponent();
            _listView = listView;
        }

        private void ContentInfoFormLoad(object sender, EventArgs e)
        {
            // Do nothing
        }

        private void ShowDescription(string contentId, string region)
        {
            pb_status.Visible = true;
            pb_status.Image = new Bitmap(Properties.Resources.menu_reload);

            switch (region)
            {
                case "EU": region = "GB/en"; break;
                case "US": region = "CA/en"; break;
                case "JP": region = "JP/ja"; break;
                case "ASIA": region = "JP/ja"; break;
            }

            Task.Run(() =>
            {
                try
                {
                    pictureBox1.Image = null;
                    pictureBox2.Image = null;
                    pictureBox3.Image = null;
                    Invoke(new Action(() =>
                    {
                        label1.Text = "";
                        richTextBox1.Text = "";

                        if (contentId == null || contentId.ToLower().Equals("missing"))
                        {
                            isLoading = false;
                            pb_status.Image = new Bitmap(Properties.Resources.menu_cancel);
                            return;
                        }
                    }));

                    WebClient wc = new WebClient
                    {
                        Proxy = Settings.Instance.proxy,
                        Encoding = Encoding.UTF8
                    };
                    string content = wc.DownloadString(new Uri("https://store.playstation.com/chihiro-api/viewfinder/" + region + "/19/" + contentId));
                    wc.Dispose();
                    //content = Encoding.UTF8.GetString(Encoding.Default.GetBytes(content));

                    // TODO: content description
                    var contentJson = SimpleJson.SimpleJson.DeserializeObject<PSNJson>(content);
                    pictureBox1.ImageLocation = contentJson.Images[0].Url.ToString();
                    pictureBox2.ImageLocation = contentJson.Images[1].Url.ToString();
                    pictureBox3.ImageLocation = contentJson.Images[2].Url.ToString();
                    Invoke(new Action(() =>
                    {
                        pb_status.Visible = false;
                        richTextBox1.Text = contentJson.LongDesc;
                        label1.Text = contentJson.TitleName + " (rating: " + contentJson.StarRating.Score + "/5.00)";
                    }));
                    isLoading = false;
                }
                catch (Exception err)
                {
                    isLoading = false;
                    Invoke(new Action(() =>
                    {
                        pb_status.Visible = true;
                        pb_status.Image = new Bitmap(Properties.Resources.menu_cancel);
                    }));
                }
            });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isLoading) return;

            if (_listView.SelectedItems.Count == 0)
            {
                return;
            }
            var itm = _listView.SelectedItems[0].Tag as Item;
            if (itm.ContentId == currentContentId) return;

            isLoading = true;
            currentContentId = itm.ContentId;
            ShowDescription(itm.ContentId, itm.Region);
        }

        private void pictureClicked(object sender, EventArgs e)
        {
            var pictureBox = sender as PictureBox;
            if (pictureBox.Tag == null)
            {
                pictureBox.Tag = pictureBox.Location;
                pictureBox.Location = new Point(0, 0);
                pictureBox.Size = Size;

                foreach (Control c in Controls)
                {
                    if (pictureBox != c) c.Visible = false;
                }
            }
            else
            {
                pictureBox.Location = (pictureBox.Tag as Point?).Value;
                pictureBox.Tag = null;
                pictureBox.Size = new Size(280, 129);

                foreach (Control c in Controls)
                {
                    if (pictureBox != c && c != pb_status) c.Visible = true;
                }
            }
        }

    }
}
