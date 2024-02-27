using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

namespace NPS
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
        public string TitleId, Region, TitleName, zRif, pkg;
        public DateTime lastModifyDate = DateTime.MinValue;

        public bool ItsCompPack = false;
        public string ParentGameTitle = string.Empty;
        public string ContentId = null;
        public Platform Platform = Platform.UNKNOWN;
        public ContentType ContentType = ContentType.UNKNOWN;

        public string offset = string.Empty;
        public List<Item> DlcItm = new List<Item>();

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
                else if (string.IsNullOrEmpty(ContentId))
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

        public string PkgsDir
        {
            get { return Path.Combine(Settings.Instance.GetDownloadsDirPackages(Platform), ContentType.ToString().ToLowerInvariant()); }
        }

        public string ImagesDir
        {
            get { return Path.Combine(Settings.Instance.GetDownloadsDirImages(Platform), TitleId ?? "UNKNOWN"); }
        }

        public string PromosDir
        {
            get { return Path.Combine(Settings.Instance.GetDownloadsDirPromos(Platform), TitleId ?? "UNKNOWN"); }
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
    }
}
