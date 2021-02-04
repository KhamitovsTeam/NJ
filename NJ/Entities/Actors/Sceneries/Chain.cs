using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Chain : Actor
    {
        private Animation _sprite;
        public Chain()
            : base(0, 0)
        {
            Depth = -8;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            for (int i = 0; i < 2; i++)
            {
                _sprite = new Animation(GFX.Game["sceneries/chain"], 8, 3);
                Add(_sprite);
                _sprite.Add("idle", 8f, 0, 1, 2, 3);
                _sprite.Origin.X = 8f - i * 8f;
                _sprite.Origin.Y = 3f;
                _sprite.Play("idle");
            }
        }
    }
}