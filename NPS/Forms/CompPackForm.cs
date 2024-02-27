using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace NPS
{
    public partial class CompPackForm : Form
    {
        public static bool compPackChanged = false;

        private readonly Item _item;
        private readonly Action<Item[]> _finalresult;

        private static List<CompPackItem> compPackList = null;

        public CompPackForm(Item item, Action<Item[]> result)
        {
            InitializeComponent();
            _item = item;
            _finalresult = result;
        }

        private void CompPack_Load(object sender, EventArgs e)
        {
            try
            {
                if (compPackList == null || compPackChanged)
                {
                    compPackChanged = false;
                    compPackList = LoadCompPacks(Settings.Instance.compPackUrl);
                    //Settings.Instance.compPackPatchUrl = "";
                    if (!string.IsNullOrEmpty(Settings.Instance.compPackPatchUrl))
                    {
                        compPackList.AddRange(LoadCompPacks(Settings.Instance.compPackPatchUrl));
                    }
                }

                List<CompPackItem> result = new List<CompPackItem>();
                foreach (var cp in compPackList)
                {
                    if (cp.titleId.Equals(_item.TitleId))
                    {
                        result.Add(cp);
                        comboBox1.Items.Add(cp);
                    }
                }

                if (result.Count == 0)
                {
                    MessageBox.Show("No comp pack found");
                    Close();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
                Close();
            }
        }

        private List<CompPackItem> LoadCompPacks(string url)
        {
            List<CompPackItem> list = new List<CompPackItem>();
            WebClient wc = new WebClient
            {
                Proxy = Settings.Instance.proxy,
                Encoding = Encoding.UTF8
            };
            string content = wc.DownloadString(new Uri(url));
            wc.Dispose();
            content = Encoding.UTF8.GetString(Encoding.Default.GetBytes(content));

            string[] lines = content.Split(new string[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);
            foreach (string s in lines)
            {
                if (!string.IsNullOrEmpty(s))
                    list.Add(new CompPackItem(s));
            }

            return list;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;

            List<Item> res = new List<Item>();

            CompPackItem cpi = comboBox1.SelectedItem as CompPackItem;
            if (!cpi.ver.Equals("01.00"))
            {
                var cpiBase = comboBox1.Items[0] as CompPackItem;
                if (cpiBase.ver.Equals("01.00"))
                {
                    res.Add(cpiBase.ToItem());
                }
            }
            res.Add(cpi.ToItem());

            _finalresult.Invoke(res.ToArray());
            Close();
            //DownloadWorker dw = new DownloadWorker(itm, mainForm);
            //dw.Start();
        }
    }

    public class CompPackItem
    {
        public string titleId;
        public string ver;
        public string title;
        public string url;

        public CompPackItem(string unparsedRow)
        {
            var t = unparsedRow.Split('=');
            url = t[0];
            title = t[1];
            t = t[0].Split('/');
            titleId = t[t.Length - 2];
            ver = t[t.Length - 1].Split('-')[2].Replace("_", ".");/*.Replace(".ppk", "")*/;

        }

        public override string ToString()
        {
            return $"ver: {ver} {title}";
        }

        public Item ToItem()
        {
            Item i = new Item
            {
                ItsCompPack = true,
                TitleId = titleId,
                TitleName = title + " CompPack_" + ver
            };

            var urlArr = Settings.Instance.compPackUrl.Split('/');

            StringBuilder urlWithCompPack = new StringBuilder();
            foreach (var urlPart in urlArr)
            {
                urlWithCompPack.Append(urlPart).Append('/');
            }
            urlWithCompPack.Append(url);
            //string url = Settings.Instance.compPackUrl.Replace("entries.txt", url);
            i.pkg = urlWithCompPack.ToString();

            return i;
        }
    }
}
