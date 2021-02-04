using System.Collections.Generic;
using System.Xml.Serialization;

namespace Chip
{
    public class LevelData
    {
        [XmlAttribute]
        public string Id;

        [XmlArray("Boxes"), XmlArrayItem(typeof(EntityID), ElementName = "EntityID")]
        public List<EntityID> Boxes;

        [XmlArray("Kittens"), XmlArrayItem(typeof(EntityID), ElementName = "EntityID")]
        public List<EntityID> Kittens;

        [XmlArray("Cages"), XmlArrayItem(typeof(EntityID), ElementName = "EntityID")]
        public List<EntityID> Cages;

        [XmlArray("Coins"), XmlArrayItem(typeof(EntityID), ElementName = "EntityID")]
        public List<EntityID> Coins;

        [XmlArray("Rocks"), XmlArrayItem(typeof(EntityID), ElementName = "EntityID")]
        public List<EntityID> Rocks;

        [XmlArray("Stars"), XmlArrayItem(typeof(EntityID), ElementName = "EntityID")]
        public List<EntityID> Stars;

        [XmlArray("Elevators"), XmlArrayItem(typeof(EntityID), ElementName = "EntityID")]
        public List<EntityID> Elevators;

        [XmlArray("Computers"), XmlArrayItem(typeof(EntityID), ElementName = "EntityID")]
        public List<EntityID> Computers;

        [XmlArray("Enemies"), XmlArrayItem(typeof(EntityID), ElementName = "EntityID")]
        public List<EntityID> Enemies;

        [XmlArray("Bosses"), XmlArrayItem(typeof(EntityID), ElementName = "EntityID")]
        public List<EntityID> Bosses;

        public LevelData()
        {
            Boxes = new List<EntityID>();
            Kittens = new List<EntityID>();
            Cages = new List<EntityID>();
            Coins = new List<EntityID>();
            Rocks = new List<EntityID>();
            Stars = new List<EntityID>();
            Elevators = new List<EntityID>();
            Computers = new List<EntityID>();
            Enemies = new List<EntityID>();
            Bosses = new List<EntityID>();
        }

        public LevelData(string id)
            : this()
        {
            Id = id;
        }
    }
}