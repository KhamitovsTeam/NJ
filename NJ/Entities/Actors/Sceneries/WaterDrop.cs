using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class WaterDrop : Actor
    {
        private int _size;

        public WaterDrop()
            : base(0, 0)
        {
            Depth = 2;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _size = xml.AttrInt("size");

            var halfSprite = new Animation(GFX.Game["sceneries/water_drop"], 16, 16);
            Add(halfSprite);
            halfSprite.Add("idle", 20f, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 11, 11, 11, 11, 11, 11, 11);
            halfSprite.Origin.X = 8;
            halfSprite.Origin.Y = 10;
            halfSprite.Play("idle");

            var dawnSprite = new Animation(GFX.Game["sceneries/water_drop_down"], 16, 16);
            Add(dawnSprite);
            dawnSprite.Add("idle", 20f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 0, 0, 0, 0);
            dawnSprite.Origin.X = 8;
            dawnSprite.Origin.Y = -_size * 16 + 23;
            dawnSprite.Play("idle");
        }
    }
}
