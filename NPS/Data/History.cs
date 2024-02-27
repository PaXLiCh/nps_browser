using System;
using System.Collections.Generic;

namespace NPS.Data
{
    [Serializable]
    public class History
    {
        public const int ver = 1;
        public List<DownloadWorker> currentlyDownloading = new List<DownloadWorker>();
        public List<Item> completedDownloading = new List<Item>();
    }
}
