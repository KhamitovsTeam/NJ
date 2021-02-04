using KTEngine;
using System;
using System.Xml.Serialization;

namespace Chip
{
    /// <summary>
    /// Структура для скилла
    /// </summary>
    [Serializable]
    public struct Powerup
    {
        [XmlAttribute]
        public int Id;
        [XmlIgnore]
        public readonly string Name;
        [XmlIgnore]
        public readonly string Description;
        [XmlAttribute]
        public PowerupType Type;
        [XmlIgnore]
        public readonly Graphic Icon;

        public Powerup(string name, string description, PowerupType type, Graphic icon = null)
        {
            Id = Powerups.Count++;
            Name = name;
            Description = description;
            Type = type;
            Icon = icon;
        }

        public static bool operator ==(Powerup a, Powerup b)
        {
            return a.Id == b.Id;
        }

        public static bool operator !=(Powerup a, Powerup b)
        {
            return a.Id != b.Id;
        }

        public bool Equals(Powerup other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Powerup powerup && Equals(powerup);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)Type;
                hashCode = (hashCode * 397) ^ (Icon != null ? Icon.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}