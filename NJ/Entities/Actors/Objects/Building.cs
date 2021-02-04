using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Building : Actor
    {
        private int _type = 0;

        public Building()
            : base(0, 0)
        {
            Depth = 1;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            Tilemap tiles = Add(new Tilemap(GFX.Game["objects/building_tiles"], 16, 16, 1, 8));
            tiles.X = -8;
            tiles.Y = -8;
            if (_type == 0)
                tiles.AddRange(0, 0, new int[3] { 0, 1, 2 }, 0);
            else if (_type == 1)
                tiles.AddRange(0, 0, new int[3] { 3, 4, 5 }, 0);
            else if (_type == 2)
                tiles.AddRange(0, 0, new int[3] { 6, 7, 8 }, 0);
            else if (_type == 3)
                tiles.AddRange(0, 0, new int[3] { 9, 10, 11 }, 0);
            else if (_type == 4)
                tiles.AddRange(0, 0, new int[3] { 12, 13, 14 }, 0);
            else if (_type == 5)
                tiles.AddRange(0, 0, new int[3] { 15, 16, 17 }, 0);
            else if (_type == 6)
                tiles.AddRange(0, 0, new int[3] { 18, 19, 20 }, 0);
            else if (_type == 7)
                tiles.AddRange(0, 0, new int[3] { 21, 22, 23 }, 0);
        }
    }
}
