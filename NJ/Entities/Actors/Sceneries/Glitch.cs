using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Glitch : Actor
    {
        private int _type = 0;
        private Animation _sprite;

        public Glitch()
            : base(0, 0)
        {
            Depth = -6;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/glitch_" + _type], 16, 16);

            Add(_sprite);

            _sprite.Add("idle", 6f, 0, 1, 2);
            _sprite.Origin.X = 12f;
            _sprite.Origin.Y = 5f;
            _sprite.Play("idle");
        }
    }
}