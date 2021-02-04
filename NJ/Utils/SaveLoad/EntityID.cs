using System;
using System.Xml.Serialization;

namespace Chip
{
    [Serializable]
    public struct EntityID
    {
        public static readonly EntityID None = new EntityID("null", -1);
        [XmlIgnore]
        public string Level;
        [XmlIgnore]
        public int ID;

        [XmlAttribute]
        public string Key
        {
            get
            {
                return Level + ":" + ID;
            }
            set
            {
                string[] strArray = value.Split(':');
                Level = strArray[0];
                ID = int.Parse(strArray[1]);
            }
        }

        public EntityID(string level, int entityID)
        {
            Level = level;
            ID = entityID;
        }

        public override string ToString()
        {
            return Key;
        }

        public override int GetHashCode()
        {
            return Level.GetHashCode() ^ ID;
        }
    }
}