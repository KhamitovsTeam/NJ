using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Arrow : Actor
    {
        private Animation _sprite;
        private int _type = 0;
        private int _rotate = 0;

        public Arrow()
            : base(0, 0)
        {
            Depth = 1;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");
            _rotate = xml.AttrInt("rotate");

            _sprite = new Animation(GFX.Game["sceneries/arrow_down" + _type], 9, 9);

            Add(_sprite);
            switch (_rotate)
            {
                case 0:
                    break;
                case 90:
                    _sprite.Rotation = Calc.PI / 2f;
                    break;
                case 180:
                    _sprite.Rotation = Calc.PI;
                    break;
                case 270:
                    _sprite.Rotation = Calc.PI * 3f / 2f;
                    break;
            }

            _sprite.Add("idle", 8f, 2, 1, 0, 0, 0, 0);
            _sprite.Origin.X = 5f;
            _sprite.Origin.Y = 5f;
            _sprite.Play("idle");
        }
    }
}
