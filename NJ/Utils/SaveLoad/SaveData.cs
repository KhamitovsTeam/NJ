using KTEngine;
using System;
using System.Xml.Serialization;

namespace Chip
{
    [Serializable]
    public class SaveData
    {
        public static SaveData Instance;
        public string Version;
        public DateTime LastSave;

        public long TotalTime;

        public int TotalDeaths;

        public int TotalJumps;
        public int TotalWallJumps;

        public int TotalShots;

        public int TotalStars;
        public int TotalCoins;
        public int TotalCats;

        public Session CurrentSession;

        [XmlIgnore]
        [NonSerialized]
        public bool DoNotSave;
        [XmlIgnore]
        [NonSerialized]
        public bool DebugMode;

        public static void Start(SaveData data)
        {
            Instance = data;
            Instance.AfterInitialize();
        }

        public static string GetFilename(int slot = 0)
        {
            return slot.ToString();
        }

        public static bool TryDelete(int slot)
        {
            return UserIO.Delete(GetFilename(slot));
        }

        public void AfterInitialize()
        {
            if (DebugMode)
            {
                CurrentSession = null;
            }
        }

        public void BeforeSave()
        {
            Instance.Version = Engine.Instance.Version.ToString();
            Instance.LastSave = DateTime.Now;
        }

        public void StartSession(Session session)
        {
            CurrentSession = session;
            if (!DebugMode)
                return;
        }
    }
}