using System;
using System.Collections.Generic;

namespace NPS.Data
{
    /// <summary>
    /// Content local database.
    /// </summary>
    // TODO: Rewrire to SQLite
    [Serializable]
    public class NPCache
    {
        private bool _cacheInvalid = false;
        public bool IsCacheIsInvalid { get { return _cacheInvalid || UpdateDate < DateTime.Now.AddMonths(-1); } }

        public const int ver = 1;
        public DateTime UpdateDate;
        public List<Item> gamesDatabase = new List<Item>();
        public List<Item> updatesDatabase = new List<Item>();
        public List<Item> dlcsDatabase = new List<Item>();
        public List<Item> themesDatabase = new List<Item>();
        public List<Item> avatarsDatabase = new List<Item>();

        public List<string> regions = new List<string>(), types = new List<string>();
        public List<StoreInfo> renasceneCache = new List<StoreInfo>();

        public void InvalidateCache()
        {
            _cacheInvalid = true;
        }
        public NPCache(DateTime creationDate)
        {
            UpdateDate = creationDate;
        }
    }
}
