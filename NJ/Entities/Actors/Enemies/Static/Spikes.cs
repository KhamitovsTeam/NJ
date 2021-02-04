using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Spikes : Actor
    {
        private Animation _sprite;
        private int _type;
        private int _rotation;

        public Spikes()
            : base(0, 0)
        {


        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");
            _sprite = _sprite = new Animation(GFX.Game["enemies/spikes" + _type], 16, 16);
            Add(_sprite);
            switch (_type)
            {
                case 1:
                    _sprite.Add("idle", 8f, 0, 1, 2, 3, 4, 5, 6, 7, 6, 7, 6, 7, 6);
                    break;
                default:
                    _sprite.Add("idle", 2f, 0, 1, 1, 1, 2, 3);
                    break;
            }

            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;
            _sprite.Play("idle");

            _rotation = xml.AttrInt("rotation");
            switch (_rotation)
            {
                case 90:
                    _sprite.Rotation = Calc.PI / 2f;
                    MoveCollider = Add(new Hitbox(-8, -8, 8, 16));
                    break;
                case 180:
                    _sprite.Rotation = Calc.PI;
                    MoveCollider = Add(new Hitbox(-8, -8, 16, 8));
                    break;
                case 270:
                    _sprite.Rotation = 3f * Calc.PI / 2f;
                    MoveCollider = Add(new Hitbox(0, -8, 8, 16));
                    break;
                default:
                    MoveCollider = Add(new Hitbox(-8, 0, 16, 8));
                    break;
            }
            MoveCollider.Tag((int)Tags.Enemy);
        }
    }
}
