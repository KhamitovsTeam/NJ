using KTEngine;
using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public class FakeWall : Actor, IShootable
    {
        private bool shake = true;
        private Graphic graphic;
        private Hitbox hitbox;
        private float shakeTimer;
        private bool opened;

        public FakeWall()
        {
            graphic = Add(new Graphic(GFX.Game["objects/fakewall"]));
            graphic.CenterOrigin();
            hitbox = Add(new Hitbox(-8, -8, 16, 16));
            hitbox.Tag((int)Tags.FakeWall);
            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Solid);
            Depth = -10;
        }

        /*public FakeWall(int x, int y, Texture texture, int tx, int ty, float rotation = 0.0f)
            : this()
        {
            X = x + 8;
            Y = y + 8;
            graphic = Add(new Graphic(texture, new Rectangle(tx * 16, ty * 16, 16, 16)));
            graphic.CenterOrigin();
            graphic.Rotation = rotation;
            hitbox = Add(new Hitbox(-8, -8, 16, 16));
            hitbox.Tag((int) Tags.FakeWall);
            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int) Tags.Solid);
            Depth = -10;
        }*/

        public override void Update()
        {
            base.Update();
            if (!opened)
            {
                shakeTimer -= Engine.DeltaTime;
                if (shakeTimer <= 0.0)
                {
                    shake = !shake;
                    shakeTimer = !shake
                        ? 1f + Utils.Random()
                        : 0.1f + Utils.Random() * 0.1f;
                }
                if (shake && Math.Abs(X - Player.Instance.X) < 16f && Math.Abs(Y - Player.Instance.Y) < 16f)
                {
                    graphic.X = Utils.RangeF(-1f, 1f);
                    graphic.Y = Utils.RangeF(-1f, 1f);
                }
                else
                {
                    graphic.X = graphic.Y = 0.0f;
                }
            }
            else
            {
                if (graphic.Alpha > 0.5)
                    graphic.Alpha -= Engine.DeltaTime;
                else
                    graphic.Alpha = 0.5f;
                if (graphic.Scale.X > 1.0)
                    graphic.Scale.X -= Engine.DeltaTime * 4f;
                else
                    graphic.Scale.X = 1f;
                graphic.Scale.Y = graphic.Scale.X;
            }
        }

        public void Hurt(int damage = 0, Entity from = null)
        {
            if (Player.Instance.CanBreakeFakeWalls)
            {
                opened = true;
                MoveCollider.Collidable = false;
                hitbox.Collidable = false;
                graphic.Scale = Vector2.One * 2f;
                Smoke.Burst(X + 8f, Y + 8f, 8f, 0.0f, Calc.TAU, 4);
            }
        }
    }
}