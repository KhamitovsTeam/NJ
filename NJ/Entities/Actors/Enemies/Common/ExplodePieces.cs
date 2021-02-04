using KTEngine;
using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public class ExplodePieces : Actor
    {
        private Graphic _sprite;
        private float timeout = 1f;
        private bool stuck;
        private Vector2 stuckDirection;
        private float timer;
        private Particles Particles;

        public ExplodePieces()
            : base(0, 0)
        {
            _sprite = new Graphic(GFX.Game["effects/pieces" + Utils.Choose(0, 3)]);
            Add(_sprite);
            _sprite.CenterOrigin();
            MoveCollider = Add(new Hitbox(-1, -1, 2, 2));
            MoveCollider.Tag((int)Tags.Piece);
            Particles = Add(new Particles());
            Particles.Preset = ParticlePresets.Piece;

            Depth = -10;
        }

        public void Define(Vector2 position, Vector2? speed = null)
        {
            Position = position;
            Speed = speed ?? new Vector2(Utils.RangeF(-60f, 60f), Utils.RangeF(-60f, -140f));
            stuck = false;
            MoveCollider.Reset(0, 0, 1, 1);
            _sprite.Visible = true;
        }

        public override void Update()
        {
            base.Update();
            Particles.Burst(Position, 1);
            if (!stuck)
            {
                Slowdown(60, 0.0f);
                Fall(200f);
                Move();
            }
            else
            {
                timer -= Engine.DeltaTime;
                if (timer >= 0.0)
                    return;
                Scene.Remove(this);
                Cache.Store(this);//.Recycle(this);
            }
        }

        public void Stick(int x, int y)
        {
            if (x > 0)
                MoveCollider.Reset(-2, -2, 2, 4);
            else if (x < 0)
                MoveCollider.Reset(0, -2, 2, 4);
            else if (y > 0)
                MoveCollider.Reset(-2, -2, 4, 2);
            else
                MoveCollider.Reset(-2, 0, 4, 2);
            X += x * 4;
            Y += y * 4;
            X = (float)(Math.Floor(X / 8.0) * 8.0 + 4.0) - x * 4;
            Y = (float)(Math.Floor(Y / 8.0) * 8.0 + 4.0) - y * 4;
            stuck = true;
            stuckDirection = new Vector2(x, y);
            timer = timeout;
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (!stuck)
            {
                if (axis == Hit.Horizontal)
                    Stick(Math.Sign(velocity.X), 0);
                else
                    Stick(0, Math.Sign(velocity.Y));
            }
            base.HitSolid(axis, ref velocity, collision);
        }

        public void Hurt(int damage = 0, Entity from = null)
        {
        }

        public static void Burst(Vector2 position, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                ExplodePieces entity = Cache.Create<ExplodePieces>();
                entity.Define(position, new Vector2?());
                Engine.Scene.Add(entity, "default");
            }
        }

        public static void Burst(Vector2 position, Vector2 speed, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                ExplodePieces entity = Cache.Create<ExplodePieces>();
                entity.Define(position, speed);
                Engine.Scene.Add(entity, "default");
            }
        }
    }
}