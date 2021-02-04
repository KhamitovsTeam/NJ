using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Secretwall : Actor
    {
        private Animation _sprite;
        private int _type = 0;
        //    private float amp;
        //    private int fade;

        public Secretwall()
            : base(0, 0)
        {
            Depth = -2;

            MoveCollider = Add(new Hitbox(0, 6, 1, 1));
            MoveCollider.Tag((int)Tags.HiddenPlace);
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/secretwall" + _type], 16, 16);
            Add(_sprite);
            _sprite.Add("idle", 0f, 0);
            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;
            _sprite.Play("idle");
        }
    }
}
