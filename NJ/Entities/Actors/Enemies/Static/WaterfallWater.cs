using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class WaterfallWater : Actor
    {
        private Animation _sprite;
        private int _type;

        public WaterfallWater()
            : base(0, 0)
        {

        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");
            _sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));
            _sprite.Play("idle");
            //_sprite = _sprite = new Animation(GFX.Game["enemies/waterfall_water"], 16, 16);

            // collider
            MoveCollider = Add(new Hitbox(-8, -8, 32, 16));
            MoveCollider.Tag((int)Tags.Enemy);
            Depth = -1;
        }
    }
}
