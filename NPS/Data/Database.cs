using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using NPS.Helpers;

namespace NPS.Data
{
    /// <summary>
    /// Local database.
    /// </summary>
    public class Database
    {
        private const string path = "nps.cache";

        /// <summary>
        /// Instance for DB.
        /// </summary>
        public static Database Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Database();
                }
                return _instance;
            }
        }

        private static Database _instance;

        public List<StoreInfo> renasceneCache
        {
            get
            {
                return _cache.renasceneCache;
            }
        }

        public NPCache Cache
        {
            get
            {
                return _cache;
            }
        }


        private NPCache _cache;

        public event DatabaseEventHandler CacheLoadStarted;

        public event DatabaseEventHandler CacheLoaded;

        public event DatabaseEventHandler CacheSyncStarted;

        public event DatabaseUpdateEventHandler CacheSyncing;

        public event DatabaseEventHandler CacheSynced;

        public event DatabaseEventHandler CacheSaved;

        /// <summary>
        /// Handler for database events.
        /// </summary>
        /// <param name="sender"> Current database. </param>
        /// <param name="args"> Event values. </param>
        public delegate void DatabaseEventHandler(Database sender, EventArgs args);

        public delegate void DatabaseUpdateEventHandler(Database sender, int percentage);

        private List<Item> databaseAll;
        private List<Item> appsDbs;
        private List<Item> dlcsDbs;
        private List<Item> avatarsDbs;
        private List<Item> themesDbs;
        private List<Item> updatesDbs;

        private HashSet<string> types;
        private HashSet<string> regions;

        private static SemaphoreSlim _semaphore;
        private readonly List<Task<List<Item>>> tasks = new List<Task<List<Item>>>();
        private CancellationTokenSource cancellationTokenSource;

        private int _jobsAdded = 0;//17
        private int _jobsFinished = 0;

        [CanBeNull]
        public List<Item> Apps => appsDbs;

        [CanBeNull]
        public List<Item> DLCs => dlcsDbs;

        [CanBeNull]
        public List<Item> Avatars => avatarsDbs;

        [CanBeNull]
        public List<Item> Themes => themesDbs;

        [CanBeNull]
        public List<Item> Updates => updatesDbs;

        [CanBeNull]
        public HashSet<string> Types => types;

        [CanBeNull]
        public HashSet<string> Regions => regions;

        public bool IsAppsLoaded
        {
            get { return appsDbs?.Count > 0; }
        }

        public bool IsThemesLoaded
        {
            get { return themesDbs?.Count > 0; }
        }

        public bool IsDLCsLoaded
        {
            get { return dlcsDbs?.Count > 0; }
        }

        public bool IsUpdatesLoaded
        {
            get { return updatesDbs?.Count > 0; }
        }

        public bool IsAvatarsLoaded
        {
            get { return avatarsDbs?.Count > 0; }
        }

        public Database()
        {
            // Create the semaphore.
            _semaphore = new SemaphoreSlim(0, Settings.Instance.simultaneousDl);
        }

        /// <summary>
        /// Initialize DB with loading data.
        /// </summary>
        public void Load()
        {
            Load(path);
        }

        /// <summary>
        /// Initialize DB with loading data.
        /// </summary>
        /// <param name="path"> Database location. </param>
        public void Load([NotNull] string path)
        {
            Task.Run(() =>
            {
                CacheLoadStarted?.Invoke(this, new EventArgs());

                if (File.Exists(path))
                {
                    using (var stream = File.OpenRead(path))
                    {
                        try
                        {
                            var formatter = new BinaryFormatter();
                            _cache = (NPCache)formatter.Deserialize(stream);
                            if (_cache.IsCacheIsInvalid)
                            {
                                _cache = null;
                            }
                        }
                        catch
                        {
                            //
                        }
                        finally
                        {
                            //
                        }
                    }
                }
                if (_cache == null)
                {
                    Sync();
                }
                else
                {
                    appsDbs = _cache.gamesDatabase;
                    dlcsDbs = _cache.dlcsDatabase;
                    updatesDbs = _cache.updatesDatabase;
                    themesDbs = _cache.themesDatabase;
                    avatarsDbs = _cache.avatarsDatabase;

                    //databaseAll = _cache.gamesDatabase;
                    types = new HashSet<string>(_cache.types);
                    regions = new HashSet<string>(_cache.regions);
                    CacheLoaded?.Invoke(this, new EventArgs());
                }
            });
        }

        public void Save()
        {
            Save(path);

            CacheSaved?.Invoke(this, new EventArgs());
        }

        public void Save([NotNull] string path)
        {
            using (FileStream stream = File.Create(path))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, _cache);
            }
        }

        public async void Sync()
        {
            _jobsAdded = 0;
            _jobsFinished = 0;

            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            tasks.Clear();

            if (_semaphore.CurrentCount == 0)
            {
                _semaphore.Release(Settings.Instance.simultaneousDl);
            }

            CacheSyncStarted?.Invoke(this, new EventArgs());

            // Update DBs
            var taskUpdatePSV = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PSVUpdateUri, Platform.PSV, ContentType.UPDATE, ParseLinePSV);
            var taskUpdatePS4 = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PS4UpdateUri, Platform.PS4, ContentType.UPDATE, ParseLinePS4);

            // Theme DBs
            var taskThemePSV = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PSVThemeUri, Platform.PSV, ContentType.THEME, ParseLinePSV);
            var taskThemePSP = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PSPThemeUri, Platform.PSP, ContentType.THEME, ParseLinePSP);
            var taskThemePS3 = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PS3ThemeUri, Platform.PS3, ContentType.THEME, ParseLinePS3);
            var taskThemePS4 = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PS4ThemeUri, Platform.PS4, ContentType.THEME, ParseLinePS4);

            // DLC DBs
            var taskDLCPSV = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PSVDLCUri, Platform.PSV, ContentType.DLC, ParseLinePSV);
            var taskDLCPSP = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PSPDLCUri, Platform.PSP, ContentType.DLC, ParseLinePSP);
            var taskDLCPS3 = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PS3DLCUri, Platform.PS3, ContentType.DLC, ParseLinePS3);
            var taskDLCPS4 = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PS4DLCUri, Platform.PS4, ContentType.DLC, ParseLinePS4);

            // Avatar DBs
            var taskAvatarPS3 = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PS3AvatarUri, Platform.PS3, ContentType.AVATAR, ParseLinePS3);

            // Game DBs
            var taskGamePSV = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PSVUri, Platform.PSV, ContentType.APP, ParseLinePSV);
            var taskGamePSM = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PSMUri, Platform.PSM, ContentType.APP, ParseLinePSM);
            var taskGamePSX = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PSXUri, Platform.PSX, ContentType.APP, ParseLinePSX);
            var taskGamePSP = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PSPUri, Platform.PSP, ContentType.APP, ParseLinePSP);
            var taskGamePS3 = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PS3Uri, Platform.PS3, ContentType.APP, ParseLinePS3);
            var taskGamePS4 = LoadDatabase(cancellationTokenSource.Token, Settings.Instance.PS4Uri, Platform.PS4, ContentType.APP, ParseLinePS4);

            await Task.WhenAll(tasks);

            var databaseAll = new List<Item>();
            appsDbs = new List<Item>();
            dlcsDbs = new List<Item>();
            avatarsDbs = new List<Item>();
            themesDbs = new List<Item>();
            updatesDbs = new List<Item>();

            appsDbs.AddRange(taskGamePSV.Result);
            appsDbs.AddRange(taskGamePSM.Result);
            appsDbs.AddRange(taskGamePSX.Result);
            appsDbs.AddRange(taskGamePSP.Result);
            appsDbs.AddRange(taskGamePS3.Result);
            appsDbs.AddRange(taskGamePS4.Result);

            databaseAll.AddRange(appsDbs);

            dlcsDbs.AddRange(taskDLCPSV.Result);
            dlcsDbs.AddRange(taskDLCPSP.Result);
            dlcsDbs.AddRange(taskDLCPS3.Result);
            dlcsDbs.AddRange(taskDLCPS4.Result);

            databaseAll.AddRange(dlcsDbs);

            themesDbs.AddRange(taskThemePSV.Result);
            themesDbs.AddRange(taskThemePSP.Result);
            themesDbs.AddRange(taskThemePS3.Result);
            themesDbs.AddRange(taskThemePS4.Result);

            databaseAll.AddRange(themesDbs);

            avatarsDbs.AddRange(taskAvatarPS3.Result);
            databaseAll.AddRange(avatarsDbs);

            updatesDbs.AddRange(taskUpdatePSV.Result);
            updatesDbs.AddRange(taskUpdatePS4.Result);
            databaseAll.AddRange(updatesDbs);

            types = new HashSet<string>();
            regions = new HashSet<string>();

            // TODO: Optimize
            foreach (var itm in databaseAll)
            {
                regions.Add(itm.Region);
                types.Add(itm.Platform.ToString());
            }

            foreach (var itm in appsDbs)
            {
                //if (!itm.IsAvatar && !itm.IsDLC && !itm.IsTheme && !itm.IsUpdate && !itm.ItsPsx)
                //if (dbType == DatabaseType.Vita || dbType == DatabaseType.PSP || dbType == DatabaseType.PS3 || dbType == DatabaseType.PS4)
                itm.CalculateDlCs(dlcsDbs);
            }

            // Populate DLC Parent Titles
            //var gamesDb = Database.Instance.GetDatabase();
            //var dlcDb = Database.Instance.GetDatabase(true);
            //foreach (var item in dlcDb)
            //{
            //    var result = gamesDb.FirstOrDefault(i => i.TitleId.StartsWith(item.TitleId.Substring(0, 9)))?.TitleName;
            //    item.ParentGameTitle = result ?? string.Empty;
            //}

            // New cache, build it now
            _cache = new NPCache(DateTime.Now);

            _cache.gamesDatabase = appsDbs;
            _cache.dlcsDatabase = dlcsDbs;
            _cache.updatesDatabase = updatesDbs;
            _cache.themesDatabase = themesDbs;
            _cache.avatarsDatabase = avatarsDbs;

            _cache.types = types.ToList();
            _cache.regions = regions.ToList();
            Save();

            CacheSynced?.Invoke(this, new EventArgs() { });
            CacheLoaded?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Cancels synchronization.
        /// </summary>
        public void Cancel()
        {
            cancellationTokenSource?.Cancel();
            _cache?.InvalidateCache();
        }

        private Task<List<Item>> LoadDatabase(CancellationToken cancellationToken, string path, Platform platform, ContentType contentType, Action<ContentType, Item, string[]> lineParser)
        {
            ++_jobsAdded;
            var task = Task.Run(() =>
            {
                _semaphore.Wait(cancellationToken);

                List<Item> dbs = new List<Item>();

                try
                {
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        path = new Uri(path).ToString();

                        WebClient wc = new WebClient
                        {
                            Encoding = Encoding.UTF8,
                            Proxy = Settings.Instance.proxy
                        };
                        //wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                        string content = wc.DownloadStringTaskAsync(new Uri(path)).Result;
                        wc.Dispose();
                        //content = Encoding.UTF8.GetString(Encoding.Default.GetBytes(content));

                        string[] lines = content.Split(new string[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

                        foreach (var line in lines)
                        {
                            var a = line.Split('\t');

                            if (a.Length < 5)
                            {
                                continue;
                            }

                            // Parse common data
                            var itm = new Item()
                            {
                                Platform = platform,
                                ContentType = contentType,
                                TitleId = a[0],
                                Region = a[1],
                                TitleName = a[2],
                                pkg = a[3],
                                zRif = a[4],
                                ContentId = a[5],
                            };

                            // Parse platform/content specific data
                            lineParser?.Invoke(contentType, itm, a);

                            // Cleanup data
                            var pkgUrl = itm.pkg.ToLowerInvariant();
                            if ((pkgUrl.Contains("http://") || pkgUrl.Contains("https://")) && !itm.zRif.ToLower().Contains("missing"))
                            {
                                if (itm.zRif.ToLower().Contains("not required"))
                                {
                                    itm.zRif = string.Empty;
                                }
                                itm.Region = itm.Region.Replace(" ", string.Empty);
                                dbs.Add(itm);
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // do nothing
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
                finally
                {
                    _semaphore.Release();
                    ++_jobsFinished;

                    float progressValue = _jobsFinished * 100.0F / _jobsAdded;
                    if (progressValue > 100.0F)
                    {
                        progressValue = 100.0F;
                    }
                    else if (progressValue < 0.0F)
                    {
                        progressValue = 0.0F;
                    }
                    CacheSyncing?.Invoke(this, (int)progressValue);
                }
                return dbs;
            }, cancellationToken);

            tasks.Add(task);

            return task;
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //tasks.Count dbCounterReady / dbCounter
            CacheSyncing?.Invoke(this, e.ProgressPercentage);
        }

        private void ParseLinePSP(ContentType contentType, Item itm, string[] a)
        {
            if (contentType == ContentType.APP)
            {
                // TODO: improve PSP parsing
                //itm.Platform = a[2];
                itm.TitleName = a[3];
                itm.pkg = a[4];
                itm.ContentId = a[5];
                DateTime.TryParse(a[6], out itm.lastModifyDate);
                itm.zRif = a[7];
            }
            else if (contentType == ContentType.DLC)
            {
                itm.ContentId = a[4];
                DateTime.TryParse(a[5], out itm.lastModifyDate);
                itm.zRif = a[6];
            }
            else if (contentType == ContentType.THEME)
            {
                itm.zRif = string.Empty;
                itm.ContentId = a[4];
                DateTime.TryParse(a[5], out itm.lastModifyDate);
            }
        }

        private void ParseLinePSV(ContentType contentType, Item itm, string[] a)
        {
            // PSV
            if (contentType == ContentType.APP)
            {
                DateTime.TryParse(a[6], out itm.lastModifyDate);
            }
            else if (contentType == ContentType.DLC)
            {
                DateTime.TryParse(a[6], out itm.lastModifyDate);
            }
            else if (contentType == ContentType.THEME)
            {
                DateTime.TryParse(a[6], out itm.lastModifyDate);
            }
            else if (contentType == ContentType.UPDATE)
            {
                itm.ContentId = null;
                itm.zRif = string.Empty;
                itm.TitleName = $"{a[2]} ({a[3]})";
                itm.pkg = a[5];
                DateTime.TryParse(a[7], out itm.lastModifyDate);
            }
        }

        private void ParseLinePS3(ContentType contentType, Item itm, string[] a)
        {
            DateTime.TryParse(a[6], out itm.lastModifyDate);
        }

        private void ParseLinePS4(ContentType contentType, Item itm, string[] a)
        {
            DateTime.TryParse(a[6], out itm.lastModifyDate);

            // PS4
            if (contentType == ContentType.UPDATE)
            {
                itm.ContentId = null;
                itm.zRif = string.Empty;
                itm.TitleName = $"{a[2]} ({a[3]})";
                itm.pkg = a[5];
            }
        }

        private void ParseLinePSM(ContentType contentType, Item itm, string[] a)
        {
            itm.ContentId = null;
            DateTime.TryParse(a[6], out itm.lastModifyDate);
        }

        private void ParseLinePSX(ContentType contentType, Item itm, string[] a)
        {
            itm.zRif = string.Empty;
            itm.ContentId = a[4];
            DateTime.TryParse(a[5], out itm.lastModifyDate);
        }

    } // class Database
} // Namespace
