using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Chip
{
    [Serializable]
    public class Session
    {
        public string ToLevel;
        public string FromLevel;
        public Vector2 LastCheckpoint;
        public List<LevelData> LevelsData;
        public PlayerData PlayerData;

        public Session()
        {
            ToLevel = "01_00";
            FromLevel = "wellboy";
            LevelsData = new List<LevelData>();
        }

        public LevelData GetLevelData(string level)
        {
            if (LevelsData == null) return null;
            foreach (var levelData in LevelsData)
            {
                if (levelData.Id == level)
                    return levelData;
            }
            return null;
        }
    }
}