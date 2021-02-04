using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class KittenShelf : Actor
    {
        private int _type;
        private Animation _sprite;

        public KittenShelf()
            : base(0, 0)
        {
            Depth = -2;

        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            if (_type == 0)
            {
                _sprite = new Animation(GFX.Game["sceneries/kitten_shelf" + _type], 16, 16);
                _sprite.Origin.X = 8f;
                _sprite.Origin.Y = 8f;
                //MoveCollider = Add(new Hitbox(-8, -8, 16, 6));
                //MoveCollider.Tag((int)Tags.Solid);
            }
            else if (_type == 1)
            {
                _sprite = new Animation(GFX.Game["sceneries/kitten_shelf" + _type], 16, 32);
                _sprite.Origin.X = 8f;
                _sprite.Origin.Y = 24f;
                //MoveCollider = Add(new Hitbox(-8, -24, 16, 6));
                //MoveCollider.Tag((int)Tags.Solid);
            }
            else
            {
                _sprite = new Animation(GFX.Game["sceneries/kitten_shelf" + _type], 24, 28);
                _sprite.Origin.X = 12f;
                _sprite.Origin.Y = 20f;
                //MoveCollider = Add(new Hitbox(-12, -6, 16, 6));
                //MoveCollider.Tag((int)Tags.Solid);
                //MoveCollider = Add(new Hitbox(-4, -17, 16, 6));
                //MoveCollider.Tag((int)Tags.Solid);
            }
            Add(_sprite);
            _sprite.Add("idle", 1f, 0);
            _sprite.Play("idle");

        }

    }
}