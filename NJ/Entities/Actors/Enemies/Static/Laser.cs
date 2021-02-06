using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Laser : Enemy
    {
        private readonly Animation sprite;
        private int width;
        private int rotation;
        private Vector2 pos;
        private readonly Color color = Constants.Dark;

        public int Rotation => rotation;

        public Laser()
        {
            sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));
            sprite.Play("light");
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            width = xml.AttrInt("width");
            rotation = xml.AttrInt("rotation");
            switch (rotation)
            {
                case 0:
                    sprite.Rotation = Calc.PI / 2f;
                    MoveCollider = Add(new Hitbox(-8, -1, width, 2));
                    break;
                case 90:
                    sprite.Rotation = Calc.PI;
                    MoveCollider = Add(new Hitbox(-2, -8, 4, width));
                    break;
                case 180:
                    sprite.Rotation = 3f * Calc.PI / 2f;
                    MoveCollider = Add(new Hitbox(-8, -1, width, 2));
                    break;
                default:
                    MoveCollider = Add(new Hitbox(-4, -8, 4, width));
                    break;
            }
            MoveCollider.Tag((int)Tags.Enemy);
        }

        public void Enable()
        {
            MoveCollider.Collidable = true;
        }

        public void Disable()
        {
            MoveCollider.Collidable = false;
        }

        public override void Hurt(int damage = 0, Entity from = null)
        {
            
        }

        public override void Update()
        {
            base.Update();
            switch (rotation)
            {
                case 0:
                    if (pos.X > width - 7f)
                    {
                        pos.X = 0;
                    }
                    pos.X += 1;
                    break;
                case 90:
                    if (pos.Y > width - 7f)
                    {
                        pos.Y = 0;
                    }
                    pos.Y += 1;
                    break;
                case 180:
                    if (pos.X < -width + 7f)
                    {
                        pos.X = 0;
                    }
                    pos.X -= 1;
                    break;
                default:
                    if (pos.Y < -width + 7f)
                    {
                        pos.Y = 0;
                    }
                    pos.Y -= 1;
                    break;
            }

        }

        public override void Render()
        {
            base.Render();
            if (!MoveCollider.Collidable) return;
            switch (rotation)
            {
                case 0:
                    Draw.Rect(X - 3f, Y - 1f, width - 6f, 2f, color);
                    Draw.Rect(X + width - 9f, Y - 2f, 1f, 4f, color);
                    Draw.Pixel(X - 4f + pos.X, Y - 1f + pos.Y, Constants.Light);
                    Draw.Pixel(X - 4f + pos.X + 1, Y - 1f + pos.Y + 1, Constants.Light);
                    break;
                case 90:
                    Draw.Rect(X - 1f, Y - 3f, 2f, width - 6f, color);
                    Draw.Rect(X - 2f, Y + width - 9f, 4f, 1f, color);
                    Draw.Pixel(X - 1f + pos.X, Y - 3f + pos.Y, Constants.Light);
                    Draw.Pixel(X - 1f + pos.X + 1, Y - 3f + pos.Y + 1, Constants.Light);
                    break;
                case 180:
                    Draw.Rect(X - 6f, Y - 1f, width - 6f, 2f, color);
                    Draw.Rect(X - width + 9f, Y - 2f, 1f, 4f, color);
                    Draw.Pixel(X + 2f + pos.X, Y - 1f + pos.Y, Constants.Light);
                    Draw.Pixel(X + 2f + pos.X + 1, Y - 1f + pos.Y + 1, Constants.Light);
                    break;
                default:
                    Draw.Rect(X - 1f, Y - 7f, 2f, width - 6f, color);
                    Draw.Rect(X - 2f, Y - width + 9f, 4f, 1f, color);
                    Draw.Pixel(X - 1f + pos.X, Y + 2f + pos.Y, Constants.Light);
                    Draw.Pixel(X - 1f + pos.X + 1, Y + 2f + pos.Y + 1, Constants.Light);
                    break;
            }
        }
    }
}