using System;
using System.IO;
using System.Net;

using JetBrains.Annotations;

using NPS.Helpers;

namespace NPS.Data
{
    /// <summary>
    /// Additional info about title from the PS store.
    /// </summary>
    [Serializable]
    public class StoreInfo
    {
        public string Genre, Language, Publisher, Developer, Url, Description, Rating;

        public string TitleId;
        public string ContentId;
        public ulong Size;

        /// <summary>
        /// Instantiate new store info about specified title.
        /// </summary>
        /// <param name="title"> Title. </param>
        public StoreInfo([NotNull] Item title)
        {
            TitleId = title.TitleId;
            ContentId = title.ContentId;
        }

        /// <summary>
        /// Get PS store info about specified content.
        /// </summary>
        /// <param name="contentId"> ID of content. </param>
        /// <param name="region"> Region of content. </param>
        /// <returns> Parsed info from PS store about specified content. </returns>
        [CanBeNull]
        public static PSNJson GetContentInfo([NotNull] Item item)
        {
            var region = string.Empty;
            switch (item.Region)
            {
                case "EU": region = "GB/en"; break;
                case "US": region = "US/en"; break;
                case "JP": region = "JP/ja"; break;
                case "ASIA": region = "HK/en"; break;
            }
            try
            {
                WebClient wc = new WebClient
                {
                    Encoding = System.Text.Encoding.UTF8,
                    Proxy = Settings.Instance.proxy
                };

                var url = $"https://store.playstation.com/store/api/chihiro/00_09_000/container/{region}/999/{item.ContentId}";
                var contentInfoJson = wc.DownloadString(new Uri(url));
                var contentInfo = PSNJson.FromJson(contentInfoJson);
                return contentInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            return null;
        }

        public void UpdateInfo([NotNull] PSNJson contentInfo)
        {
            if (contentInfo == null)
            {
                return;
            }

            Genre = contentInfo.Metadata?.Genre?.Values != null
                && contentInfo.Metadata.Genre.Values.Length > 0
                    ? string.Join(", ", contentInfo.Metadata?.Genre.Values)
                    : string.Empty;
            Publisher = contentInfo?.ProviderName;
            Description = contentInfo?.LongDesc;
            Rating = contentInfo?.StarRating?.Score;
            // TODO: Language, Developer
        }

        /// <summary>
        /// Gets PS store info about specified title from local DB or fetch it from PS store server.
        /// </summary>
        /// <param name="title"> Title to get info. </param>
        /// <param name="contentDownloadedHandler"> When content file downloaded from PS store server. </param>
        /// <returns> PS store info about specified title. </returns>
        public static StoreInfo GetStoreInfo([NotNull] Item title, [CanBeNull] FileDownloader.DownloadHandler contentDownloadedHandler)
        {
            StoreInfo storeInfo = null;

            foreach (var savedInfo in Database.Instance.renasceneCache)
            {
                if (title.ContentId.Equals(savedInfo.ContentId))
                {
                    storeInfo = savedInfo;
                    break;
                }
            }

            if (storeInfo != null)
            {
                return storeInfo;
            }

            // First time item clicked
            storeInfo = new StoreInfo(title);
            Database.Instance.renasceneCache.Add(storeInfo);

            PSNJson contentInfo = GetContentInfo(title);

            storeInfo.UpdateInfo(contentInfo);

            if (title.FileSize == 0UL)
            {
                storeInfo.Size = Utils.GetSize(title.pkg);
            }
            else
            {
                storeInfo.Size = title.FileSize;
            }

            // Download images
            if (Settings.Instance.IsAutoDownloadImages)
            {
                DownloadImages(title, contentInfo, contentDownloadedHandler);
            }

            // Download promo materials
            if (Settings.Instance.IsAutoDownloadPromo)
            {
                DownloadPromo(title, contentInfo, contentDownloadedHandler);
            }

            return storeInfo;
        }

        /// <summary>
        /// Download images for specified title.
        /// </summary>
        /// <param name="item"> Title info to download images. </param>
        /// <param name="contentInfo"> PS store info for title with links to images.
        /// <param name="fileDownloadedHandler"> Files downloaded handler. </param>
        /// Use <see cref="GetContentInfo"/> to get info about title from store. </param>
        public static void DownloadImages([NotNull] Item item, [NotNull] PSNJson contentInfo, [CanBeNull] FileDownloader.DownloadHandler fileDownloadedHandler)
        {
            if ((contentInfo?.Images) == null || string.IsNullOrEmpty(item.TitleId))
            {
                return;
            }

            string imagesPath = item.ImagesDir;

            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            foreach (var image in contentInfo.Images)
            {
                if (image == null || image.Url == null)
                {
                    continue;
                }

                var imageFileName = $"{image.Type}.png";

                FileDownloader.Instance.DownloadFile(image.Url, imagesPath, imageFileName, fileDownloadedHandler);
            }
        }

        /// <summary>
        /// Download promotional materials for specified title.
        /// </summary>
        /// <param name="item"> Title info to download materials. </param>
        /// <param name="contentInfo"> PS store info for title with described promo materials.
        /// <param name="fileDownloadedHandler"> Files downloaded handler. </param>
        /// Use <see cref="GetContentInfo"/> to get info about title from store. </param>
        public static void DownloadPromo([NotNull] Item item, [NotNull] PSNJson contentInfo, [CanBeNull] FileDownloader.DownloadHandler fileDownloadedHandler)
        {
            if ((contentInfo?.Promomedia) == null || string.IsNullOrEmpty(item.TitleId))
            {
                return;
            }

            string promosPath = item.PromosDir;

            if (!Directory.Exists(promosPath))
            {
                Directory.CreateDirectory(promosPath);
            }

            foreach (var promomedia in contentInfo.Promomedia)
            {
                if (promomedia.Materials == null)
                {
                    continue;
                }

                foreach (var promomaterial in promomedia.Materials)
                {
                    if (promomaterial == null || promomaterial.Urls == null)
                    {
                        continue;
                    }

                    foreach (var image in promomaterial.Urls)
                    {
                        if (image == null || image.Url == null)
                        {
                            continue;
                        }

                        FileDownloader.Instance.DownloadFile(image.Url, promosPath, fileDownloadedHandler);
                    }
                }
            }
        }

        public override string ToString()
        {
            var (sizeShort, unit) = Utils.GetSizeShort(Size);
            string s = $"{sizeShort} {unit}";

            return string.Format(
@"Size: {4}
Genre: {0}
Rating: {3}/5
Published: {2}",
Genre, Language, Publisher, Rating, s);
        }

        public string SafeTitle(string title)
        {
            return title.Replace("(DLC)", string.Empty).Replace(" ", string.Empty);
        }

        public string ExtractString(string s, string start, string end)
        {
            int startIndex = s.IndexOf(start) + start.Length;
            int endIndex = s.IndexOf(end, startIndex);
            return s.Substring(startIndex, endIndex - startIndex);
        }
    }
}
