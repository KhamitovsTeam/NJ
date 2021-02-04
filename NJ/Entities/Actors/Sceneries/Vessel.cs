using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Vessel : Actor
    {
        private Animation _sprite;
        /*private int _frWight = 15;
        private int _frHeight = 35;
        private int _frOrgnX = 7;
        private int _frOrgnY = 11;*/

        public Vessel()
            : base(0, 0)
        {
            Depth = -1;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            CreateSprite("sceneries/vessel1", 15, 35, 1, 11);
            CreateSprite("sceneries/vessels", 22, 20, -14, -4);
            CreateSprite("sceneries/vessel2", 22, 23, 23, -1);
            CreateSprite("sceneries/vessel1", 15, 35, -36, 11);
            CreateSprite("sceneries/vessel1", 15, 35, -49, 11);
        }

        private void CreateSprite(string animName, int _wight, int _height, int _originX, int _originY)
        {
            _sprite = new Animation(GFX.Game[animName], _wight, _height);
            Add(_sprite);
            _sprite.Add("idle", 8f, 0, 1, 2, 3, 4, 5, 6);
            _sprite.Origin.X = _originX;
            _sprite.Origin.Y = _originY;
            _sprite.Play("idle");

            Depth = 2;

        }
    }
}
