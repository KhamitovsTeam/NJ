using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Grave : Actor
    {
        private Animation _sprite;
        private int _type = 0;
        private float amp;
        private int fade;

        public Grave()
            : base(0, 0)
        {
            Depth = 2;

            MoveCollider = Add(new Hitbox(-2, -4, 4, 8));
            //MoveCollider.Tag((int)Tags.HiddenPlace);
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            X = offset.X;
            Y = offset.Y + 2;

            _sprite = new Animation(GFX.Game["sceneries/grave" + _type], 4, 8);
            Add(_sprite);
            _sprite.Add("idle", 0f, 0);
            _sprite.Origin.X = 2f;
            _sprite.Origin.Y = 4f;
            _sprite.Play("idle");
        }
    }
}
