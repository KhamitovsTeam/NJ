using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class BackTree : Actor
    {
        private Animation _sprite;
        private int _type = 0;

        public BackTree()
            : base(0, 0)
        {
            Depth = 4;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/back_tree" + _type], 84, 52);
            Add(_sprite);

            _sprite.Add("idle", 8f, 0);
            _sprite.Origin.X = 0f;
            _sprite.Origin.Y = 0f;
            _sprite.Play("idle");
        }
    }
}
