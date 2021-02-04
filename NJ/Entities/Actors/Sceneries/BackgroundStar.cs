using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class BackgroundStar : Actor
    {
        private Animation _sprite;
        private int _type;

        public BackgroundStar()
            : base(0, 0)
        {
            Depth = 3;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/star" + _type], 5, 5);
            Add(_sprite);
            _sprite.Add("idle", Utils.Range(6, 12), 0, 1, 2, 3, 4);

            _sprite.Origin.X = 2f;
            _sprite.Origin.Y = 2f;
            _sprite.Play("idle");
        }
    }
}
