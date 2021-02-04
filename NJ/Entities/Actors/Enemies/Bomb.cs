using KTEngine;
using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public class Bomb : Actor, IExplodable
    {
        private readonly Graphic sprite;
        private float timer;
        //private float startXSpeed;

        public Bomb()
            : base(0, 0)
        {
            // sprite
            sprite = Add(XML.LoadSprite<Graphic>(GFX.Game, GFX.Sprites, GetType().Name));

            MoveCollider = Add(new Hitbox(-4, -4, 8, 8));
            MoveCollider.Tag((int)Tags.Enemy);
        }

        private void Define(Vector2 position, int direction, Vector2? speed = null)
        {
            Position = position;
            var diff = Level.Player.Position - Position;
            Push = new Vector2(0f, -200f);
            Speed = 100f * Vector2.One;//speed ?? new Vector2(Utils.RangeF(50f, 150f) * direction, Utils.RangeF(-240f, -240f));
            //startXSpeed = Speed.X;
            sprite.Visible = true;
            timer = 0.1f;
        }

        public override void Update()
        {
            base.Update();
            if (MoveCollider.Check(new[] { (int) Tags.Player, (int) Tags.PlayerBullet }))
            {
                Smoke.Burst(Position.X, Position.Y, 8f, 0.0f, Calc.TAU, 4);
                Scene.Remove(this);
                return;
            }
            timer -= Engine.DeltaTime;
            //sprite.Rotation = (float)(Math.Sign(Speed.X + Push.X) * ((Speed.X + (double)Push.X) / startXSpeed) * Calc.TAU * Engine.DeltaTime * 2.0);
            if (timer > 0f)
                return;
            Vector2 diff = Level.Player.Position - Position;
            diff.Normalize();
            Speed = diff * 20f;
            Move();
        }

        public void Explode(Explosion from = null)
        {
            throw new NotImplementedException();
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Horizontal)
            {
                Push.X = ForceDirection(velocity.X) * 120;
            }
            else
            {
                Push.Y = ForceDirection(velocity.Y) * 120;
            }
            base.HitSolid(axis, ref velocity, collision);
        }

        public static void Burst(Vector2 position, int direction, int count, Level level = null)
        {
            for (int index = 0; index < count; ++index)
            {
                Bomb entity = Cache.Create<Bomb>();
                entity.Level = level;
                entity.Define(position, direction, new Vector2?());
                Engine.Scene.Add(entity, "default");
            }
        }
    }
}