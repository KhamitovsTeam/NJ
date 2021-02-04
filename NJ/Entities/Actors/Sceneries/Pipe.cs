using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Pipe : Actor
    {
        private int _type;
        private Animation _sprite;

        public Pipe()
            : base(0, 0)
        {
            Depth = -1;            
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/pipe_" + _type], 16, 16);
            Add(_sprite);
            _sprite.Add("idle", 8f, 0, 1);

            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;
            _sprite.Play("idle");
            MoveCollider = Add(new Hitbox(-7, -7, 14, 14));
            MoveCollider.Tag((int)Tags.Solid);
        }
    }
}
