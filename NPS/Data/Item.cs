using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

namespace NPS.Data
{
    public enum Platform
    {
        UNKNOWN,
        PSP,
        PSV,
        PSM,
        PSX,
        PS3,
        PS4
    }

    public enum ContentType
    {
        UNKNOWN,
        APP,
        DLC,
        UPDATE,
        THEME,
        AVATAR,
    }

    [Serializable]
    public class Item : IEquatable<Item>
    {
        public string TitleId, Region, TitleName, zRif;

        public string TitleNameOriginal;

        /// <summary>
        /// URL to download PKG file.
        /// </summary>
        public string pkg;

        public DateTime lastModifyDate = DateTime.MinValue;

        public bool ItsCompPack = false;
        public string ParentGameTitle = string.Empty;
        public string ContentId = null;
        public Platform Platform = Platform.UNKNOWN;
        public ContentType ContentType = ContentType.UNKNOWN;

        public string offset = string.Empty;
        public List<Item> DlcItm = new List<Item>();

        public ulong FileSize = 0;

        public string Sha256Sum;

        /// <summary>
        /// Required FW version.
        /// </summary>
        public string FwVersion;

        /// <summary>
        /// Content version.
        /// </summary>
        public string Version;

        /// <summary>
        /// Gets number of addons for this title. <seealso cref="DlcItm"/>.
        /// </summary>
        public int DLCs { get { return DlcItm.Count; } }

        /// <summary>
        /// Gets package file extension.
        /// </summary>
        public string PackageFileExtension
        {
            get
            {
                if (ItsCompPack)
                {
                    return ".ppk";
                }
                return ".pkg";
            }
        }

        /// <summary>
        /// Gets package file name without extension.
        /// </summary>
        public string PackageFileName
        {
            get
            {
                string res;
                if (Platform == Platform.PS3 || ItsCompPack)
                {
                    res = TitleName;
                }
                else if (!IsContentIdSpecified)
                {
                    res = TitleId;
                }
                else
                {
                    res = ContentId;
                }

                if (!string.IsNullOrEmpty(offset))
                {
                    res += "_" + offset;
                }

                string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                Regex r = new Regex($"[{Regex.Escape(regexSearch)}]");
                return r.Replace(res, string.Empty);
            }
        }

        /// <summary>
        /// Gets package file name with extension.
        /// <seealso cref="PackageFileName"/>, <seealso cref="PackageFileExtension"/>
        /// </summary>
        public string PackageFileNameWithExtension
        {
            get
            {
                return string.Concat(PackageFileName, PackageFileExtension);
            }
        }

        /// <summary>
        /// Gets region code for this package.
        /// </summary>
        public string RegionCode
        {
            get
            {
                if (Region == "ASIA")
                {
                    return "AS";
                }
                else return Region;
            }
        }

        /// <summary>
        /// Gets directory in <see cref="Settings.downloadDir"/>> for this package to store unpacked rePatch content.
        /// </summary>
        public string RePatchDir
        {
            get
            {
                return Path.Combine(Settings.Instance.GetDownloadsDirRePatch(Platform), TitleId ?? "UNKNOWN");
            }
        }

        /// <summary>
        /// Gets directory in <see cref="Settings.downloadDir"/>> for this package to store unpacked reAddcont content.
        /// </summary>
        public string ReAddcontDir
        {
            get
            {
                return Path.Combine(Settings.Instance.GetDownloadsDirReAddons(Platform), TitleId ?? "UNKNOWN");
            }
        }

        /// <summary>
        /// Gets directory in <see cref="Settings.downloadDir"/> for this package where stored main unpacked content.
        /// Downloads\Platform\ContentType\TitleID.
        /// </summary>
        public string UnpackedContentDir
        {
            get { return Path.Combine(Settings.Instance.GetDownloadsDirUnpacked(Platform, ContentType), TitleId ?? "UNKNOWN"); }
        }

        /// <summary>
        /// Gets directory in <see cref="Settings.downloadDir"/>> where package file stored.
        /// </summary>
        public string PkgsDir
        {
            get { return Settings.Instance.GetDownloadsDirPackages(Platform, ContentType); }
        }

        /// <summary>
        /// Gets directory in <see cref="Settings.downloadDir"/>> where images for this package stored.
        /// </summary>
        public string ImagesDir
        {
            get { return Path.Combine(Settings.Instance.GetDownloadsDirImages(Platform), TitleId ?? "UNKNOWN"); }
        }

        /// <summary>
        /// Gets directory in <see cref="Settings.downloadDir"/>> where promotional materials for this package stored.
        /// </summary>
        public string PromosDir
        {
            get { return Path.Combine(Settings.Instance.GetDownloadsDirPromos(Platform), TitleId ?? "UNKNOWN"); }
        }

        /// <summary>
        /// Returns <see langword="true"/> if a valid content identifier <see cref="ContentId"/> is specified.
        /// </summary>
        public bool IsContentIdSpecified
        {
            get
            {
                bool isBad;
                if (string.IsNullOrWhiteSpace(ContentId))
                {
                    isBad = true;
                }
                else
                {
                    isBad = ContentId.ToLowerInvariant().Equals("missing");
                }
                return !isBad;
            }
        }

        /// <summary>
        /// Returns <see langword="true"/> if a valid zRif identifier <see cref="zRif"/> is specified.
        /// </summary>
        public bool IsZRifSpecified
        {
            get
            {
                bool isBad;
                if (string.IsNullOrWhiteSpace(zRif))
                {
                    isBad = true;
                }
                else if (zRif.ToLowerInvariant().Contains("not required"))
                {
                    isBad = true;
                }
                else
                {
                    isBad = zRif.Length % 2 > 0;
                }
                return !isBad;
            }
        }

        public Item()
        {
            // Do nothing.
        }

        public void CalculateDlCs([NotNull] IEnumerable<Item> dlcDbs)
        {
            DlcItm = new List<Item>();
            foreach (Item i in dlcDbs)
            {
                if (i.Region == Region && i.TitleId.Contains(TitleId))
                {
                    DlcItm.Add(i);
                }
            }
        }

        public bool CompareName(string name)
        {
            name = name.ToLowerInvariant();

            if (TitleId.ToLowerInvariant().Contains(name))
            {
                return true;
            }
            if (TitleName.ToLowerInvariant().Contains(name))
            {
                return true;
            }
            return false;
        }

        public bool Equals(Item other)
        {
            if (other == null)
            {
                return false;
            }

            return Platform == other.Platform
                   && ContentType == other.ContentType
                   && TitleId == other.TitleId
                   && Region == other.Region
                   //&& pkg == other.pkg
                   //&& zRif == other.zRif
                   //&& TitleName == other.TitleName
                   && PackageFileName == other.PackageFileName;
        }

    } // class
} // namespace
