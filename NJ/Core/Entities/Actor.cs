using System.Xml;
using KTEngine;
using Microsoft.Xna.Framework;

namespace NJ
{
    public class Actor : Entity
    {
        public Room Room;
        public Collider MoveCollider;
        
        public Actor(int x = 0, int y = 0)
            : base(x, y)
        {
        }
        
        public virtual void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            ID = xml.AttrInt("id");
            Scene = level;
            Room = room;
            Tag = Room.Name;
            X = offset.X + xml.AttrInt("x") + 8f;
            Y = offset.Y + xml.AttrInt("y") + 8f;
        }
    }
}