using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    public class Tv : Actor
    {
        private int _type = 0;  //type sprite from XML
        private int orignX = 8;
        private int orignY = 8;
        private int posX = -12;
        private int posY = 5;
        private int width = 24;
        private int height = 19;
        private float amp;
        private Animation _sprite;

        public Tv()
            : base(0, 0)
        {
           

            Depth = 2;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            switch (_type)
            {
                case 0:
                    orignX = 9;
                    orignY = -5;
                    posX = -9;
                    posY = 5;
                    width = 24;
                    height = 19;
                    break;
                case 1:
                    orignX = 9;
                    orignY = -1;
                    posX = -9;
                    posY = 1;
                    width = 28;
                    height = 23;
                    break;
                case 2:
                    orignX = 9;
                    orignY = -2;
                    posX = -9;
                    posY = 2;
                    width = 34;
                    height = 23;
                    break;
                case 3:
                    orignX = 9;
                    orignY = 0;
                    posX = -9;
                    posY = 0;
                    width = 32;
                    height = 25;
                    break;
                default:
                    orignX = 9;
                    orignY = -5;
                    posX = -9;
                    posY = 5;
                    width = 24;
                    height = 19;
                    break;
            }

            _sprite = new Animation(GFX.Game["objects/tv" + _type], width, height);
            Add(_sprite);
            _sprite.Add("idle", 6f, 0, 1, 2, 3, 4);
            _sprite.Add("turn_on", 6f, 5, 6, 7, 8, 9);
            _sprite.Add("shoot", 6f, 10, 11);
            _sprite.Origin.X = orignX;
            _sprite.Origin.Y = orignY;
            _sprite.Play("idle");

            MoveCollider = Add(new Hitbox(posX, posY, width, height));
            MoveCollider.Tag((int)Tags.Sceneries);
        }

        public override void Update()
        {
            if (Math.Abs(amp) < 5.0)
            {
                Collider collider = MoveCollider.Collide(new int[1] { (int)Tags.PlayerBullet });
                if (collider != null && Math.Abs(((Actor)collider.Entity).Speed.X) > 16.0)
                    _sprite.Play("shoot");
                else
                    _sprite.Play("idle");
            }
            int num = Math.Sign(amp);
            amp -= num * Engine.DeltaTime;
            if (Math.Sign(amp) != num)
                amp = 0.0f;
            base.Update();
        }
    }
}