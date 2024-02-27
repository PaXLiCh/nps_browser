using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

using NPS;
using NPS.Data;

[Serializable]
public class Settings
{
    #region Constants

    public const string DIR_PACKAGES = "_pkgs";
    public const string DIR_IMAGES = "_images";
    public const string DIR_PROMOS = "_promos";

    public const string DIR_APP = "app";
    public const string DIR_ADDCONT = "addcont";

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

    public string GetDownloadsDirApp(Platform platform)
    {
        return Path.Combine(downloadDir, platform.ToString().ToUpperInvariant(), DIR_APP);
    }

    public string GetDownloadsDirAddons(Platform platform)
    {
        return Path.Combine(downloadDir, platform.ToString().ToUpperInvariant(), DIR_ADDCONT);
    }

    public string GetDownloadsDirPackages(Platform platform)
    {
        return Path.Combine(downloadDir, platform.ToString().ToUpperInvariant(), DIR_PACKAGES);
    }

    public string GetDownloadsDirImages(Platform platform)
    {
        return Path.Combine(downloadDir, platform.ToString().ToUpperInvariant(), DIR_IMAGES);
    }

    public string GetDownloadsDirPromos(Platform platform)
    {
        return Path.Combine(downloadDir, platform.ToString().ToUpperInvariant(), DIR_PROMOS);
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
    public string PSVUri, PSMUri, PSXUri, PSPUri, PS3Uri, PS4Uri;

    // Avatar URIs
    public string PS3AvatarUri;

    // DLC URIs
    public string PSVDLCUri, PSPDLCUri, PS3DLCUri, PS4DLCUri;

    // Theme URIs
    public string PSVThemeUri, PSPThemeUri, PS3ThemeUri, PS4ThemeUri;

    public string HMACKey = "";

    // Update URIs
    public string PSVUpdateUri, PS4UpdateUri;

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
                var formatter = new BinaryFormatter();
                _i = (Settings)formatter.Deserialize(stream);
            }
        }
        else
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
}
