using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Sky : Actor
    {
        private int _type = 0;  //type sprite from XML
        private float orignX = 2f;
        private float orignY = 8f;
        private Animation _sprite;

        public Sky()
            : base(0, 0)
        {


        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");
            X = offset.X;
            Y = offset.Y + 2;

            _sprite = new Animation(GFX.Game["sceneries/sky" + _type], 4, 16);
            Add(_sprite);
            _sprite.Add("idle", 2f, 0, 1);
            _sprite.Origin.X = 2f;
            _sprite.Origin.Y = 4f;
            _sprite.Play("idle");

            MoveCollider = Add(new Hitbox(-2, -8, 4, 16));
            MoveCollider.Tag((int)Tags.Sceneries);
        }
    }
}
