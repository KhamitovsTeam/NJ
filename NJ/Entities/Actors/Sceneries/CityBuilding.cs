using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class CityBuilding : Actor
    {
        private Animation _sprite;
        private int _type = 0;

        public CityBuilding()
            : base(0, 0)
        {
            Depth = 7;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/city_building" + _type], 64, 64);
            Add(_sprite);

            _sprite.Add("idle", 8f, 0);
            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;
            _sprite.Play("idle");
        }
    }
}
