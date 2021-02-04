using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Waterfall : Actor
    {
        private int _height;
        private int _depth;
        private int _type;


        public Waterfall()
            : base(0, 0)
        {
           // Depth = -1;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _height = xml.AttrInt("height");
            _depth = xml.AttrInt("depth");
            _type = xml.AttrInt("type");
            if (_type == 0)
            {
                MoveCollider = Add(new Hitbox(0, -8, 1, _height));
                MoveCollider.Tag((int)Tags.HiddenPlace);
            }

            Depth = _depth;

            var count = _height / 32;
            for (var i = 0; i < count; i++)
            {
                var sprite = new Animation(GFX.Game["sceneries/waterfall_" + _type], 16, 32);
                Add(sprite);
                sprite.Add("idle", 8f, 0, 1);
                sprite.Origin.X = 8;
                sprite.Origin.Y = -i * 32 + 8;
                sprite.Play("idle");
            }

            if (_height % 32 == 0) return;
            var halfSprite = new Animation(GFX.Game["sceneries/waterfall_" + _type], 16, 16);
            Add(halfSprite);
            halfSprite.Add("idle", 8f, 0, 1);
            halfSprite.Origin.X = 8;
            halfSprite.Origin.Y = -count * 32 + 8;
            halfSprite.Play("idle");
        }
    }
}
