using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Tube : Actor
    {
        private Graphic _sprite;
        private int _type = 0;
        private int _rotate = 0;

        public Tube()
            : base(0, 0)
        {
            Depth = 2;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");
            _rotate = xml.AttrInt("rotate");

            _sprite = new Graphic(GFX.Game["sceneries/tube" + _type]);
            Add(_sprite);
            //_sprite.Rotation = Calc.PI / 2f;
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

            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;

        }
    }
}
