using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Back : Actor
    {
        private Animation _sprite;
        private int _type = 0;

        public Back()
            : base(0, 0)
        {
            Depth = 6;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/back" + _type], 16, 16);
            Add(_sprite);

            _sprite.Add("idle", 8f, 0);
            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;
            _sprite.Play("idle");
        }
    }
}
