using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

using NPS.Data;

namespace NPS
{
    [Serializable]
    public class Settings
    {
        #region Constants

        public const string DIR_PACKAGES = "_pkgs";
        public const string DIR_IMAGES = "_images";
        public const string DIR_PROMOS = "_promos";

        public const string DIR_APP = "app";
        public const string DIR_ADDCONT = "addcont";
        public const string DIR_PATCH = "patch";
        public const string DIR_DATA = "data";
        public const string DIR_REPATCH = "rePatch";
        public const string DIR_READDCONT = "reAddcont";
        public const string DIR_REDATA = "reData";

        private const string path = "npsSettings.dat";

        #endregion

        private static Settings _i;

        /// <summary>
        /// Current loaded settings.
        /// </summary>
        public static Settings Instance
        {
            get
            {
                if (_i == null)
                {
                    Load();
                }
                return _i;
            }
        }

        public string GetDownloadsDir(Platform platform)
        {
            return Path.Combine(downloadDir, platform.ToString().ToUpperInvariant());
        }

        public string GetDownloadsDirUnpacked(Platform platform, ContentType contentType)
        {
            return Path.Combine(GetDownloadsDir(platform), contentType.ToString().ToLowerInvariant());
        }

        public string GetDownloadsDirApp(Platform platform)
        {
            return Path.Combine(GetDownloadsDir(platform), DIR_APP);
        }

        public string GetDownloadsDirAddons(Platform platform)
        {
            return Path.Combine(GetDownloadsDir(platform), DIR_ADDCONT);
        }

        public string GetDownloadsDirPatch(Platform platform)
        {
            return Path.Combine(GetDownloadsDir(platform), DIR_PATCH);
        }

        public string GetDownloadsDirRePatch(Platform platform)
        {
            return Path.Combine(GetDownloadsDir(platform), DIR_REPATCH);
        }

        public string GetDownloadsDirReAddons(Platform platform)
        {
            return Path.Combine(GetDownloadsDir(platform), DIR_READDCONT);
        }

        public string GetDownloadsDirPackages(Platform platform)
        {
            return Path.Combine(GetDownloadsDir(platform), DIR_PACKAGES);
        }

        public string GetDownloadsDirPackages(Platform platform, ContentType contentType)
        {
            return Path.Combine(GetDownloadsDir(platform), DIR_PACKAGES, contentType.ToString().ToLowerInvariant());
        }

        public string GetDownloadsDirImages(Platform platform)
        {
            return Path.Combine(GetDownloadsDir(platform), DIR_IMAGES);
        }

        public string GetDownloadsDirPromos(Platform platform)
        {
            return Path.Combine(GetDownloadsDir(platform), DIR_PROMOS);
        }

        // Settings
        public string downloadDir;
        [DefaultValue("pkg2zip.exe")]
        public string unpackerPath = "pkg2zip.exe";
        [DefaultValue("-x {pkgFile} \"{zRifKey}\"")]
        public string unpackerParams = "-x {pkgFile} \"{zRifKey}\"";
        [DefaultValue("psvpfsparser.exe")]
        public string psvParserPath = "psvpfsparser.exe";
        [DefaultValue("-i \"{pathIn}\" -o \"{pathOut}\" -z \"{zRifKey}\" -f http://cma.henkaku.xyz/")]
        public string psvParserParams = "-i \"{pathIn}\" -o \"{pathOut}\" -z \"{zRifKey}\" -f http://cma.henkaku.xyz/";

        public bool deleteAfterUnpack = false;
        public int simultaneousDl = 1;

        [DefaultValue(true)]
        public bool IsAutoDownloadImages = true;

        [DefaultValue(false)]
        public bool IsAutoDownloadPromo;

        // Game URIs
        public string PSPUri, PSVUri, PSMUri, PSXUri, PS3Uri, PS4Uri;

        // Demo URIs
        public string PSVDemoUri, PS3DemoUri;

        // Avatar URIs
        public string PS3AvatarUri;

        // DLC URIs
        public string PSPDLCUri, PSVDLCUri, PS3DLCUri, PS4DLCUri;

        // Theme URIs
        public string PSPThemeUri, PSVThemeUri, PS3ThemeUri, PS4ThemeUri;

        // Update URIs
        public string PSPUpdateUri, PSVUpdateUri, PS4UpdateUri;

        public string HMACKey = "";

        public List<string> selectedRegions = new List<string>();

        public List<string> selectedTypes = new List<string>();

        public WebProxy proxy;

        public History history = new History();

        public string compPackUrl = null;
        public string compPackPatchUrl = null;


        /// <summary>
        /// Read settings from local file.
        /// </summary>
        public static void Load()
        {
            if (File.Exists(path))
            {
                using (var stream = File.OpenRead(path))
                {
                    try
                    {
                        var formatter = new BinaryFormatter();
                        _i = (Settings)formatter.Deserialize(stream);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unable to read settings.");
                        Console.WriteLine($"Error: {e.Message}");
                    }
                    finally
                    {
                        // Do nothing
                    }
                }
            }

            // Create new settings if file does not exist or malformed
            if (_i == null)
            {
                _i = new Settings();
            }
        }

        /// <summary>
        /// Save current settings to local file.
        /// </summary>
        public void Save()
        {
            using (FileStream stream = File.Create(path))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
            }
        }

    } // class Settings
} // namespace
