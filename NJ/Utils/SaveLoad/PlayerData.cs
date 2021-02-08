using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Chip
{
    /// <summary>
    /// Данные игрока для сохранения
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public int Coins = 0;
        public int Stars = 0;
        public int Score = 0;
        public int Kittens = 0;
        public int Lives = 9;
        public int HeartContainers = 3;
        public int Facing = 1;
        public bool HasFire = false;

        /// <summary>
        /// Скилы для тела
        /// </summary>
        [XmlArray("Body"), XmlArrayItem(typeof(Powerup), ElementName = "Powerup")]
        public List<Powerup> Body;

        /// <summary>
        /// Скилы для ног
        /// </summary>
        [XmlArray("Footwear"), XmlArrayItem(typeof(Powerup), ElementName = "Powerup")]
        public List<Powerup> Footwear;

        /// <summary>
        /// Скилы для рук
        /// </summary>
        [XmlArray("Armwear"), XmlArrayItem(typeof(Powerup), ElementName = "Powerup")]
        public List<Powerup> Armwear;

        public PlayerData()
        {
            Body = new List<Powerup>();
            Footwear = new List<Powerup>();
            Armwear = new List<Powerup>();
        }
    }
}