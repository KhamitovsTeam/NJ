using KTEngine;
using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public class Bullet : Actor
    {
        private bool canHurt = true;
        private const int MAXSPEED = 256;
        private const int MAXDIST = 128;
        public Vector2 Direction;
        private Graphic graphic;
        private Hitbox enemyCollider;
        private Vector2 start;
        private bool dead;
        private Entity ignore;

        public Bullet()
            : base(0, 0)
        {
            graphic = Add(new Graphic(GFX.Game["bullet"]));
            graphic.CenterOrigin();
            enemyCollider = Add(new Hitbox(-4, -4, 8, 8));
            MoveCollider = Add(new Hitbox(-2, -2, 4, 4));
            enemyCollider.Tag((int)Tags.PlayerBullet);

            Depth = 1;
        }

        public void Define(Vector2 position, Vector2 direction, float speed, Vector2 fromSpeed)
        {
            ignore = null;
            Position = position;
            Direction = direction;
            Speed = speed * direction * MAXSPEED + fromSpeed * new Vector2(Math.Abs(direction.Y), 0.0f);
            start = position;
            dead = false;
            canHurt = true;
            Collider collider = enemyCollider.Collide((int)Tags.Computer);
            if (collider != null)
                ignore = collider.Entity;
            graphic.Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public override void Update()
        {
            Move();
            CheckEnemyHit();
            if ((start - Position).Length() <= MAXDIST)
                return;
            Kill();
        }

        public void CheckEnemyHit()
        {
            if (!canHurt)
                return;
            Collider collider = enemyCollider.Collide(new int[3] { (int)Tags.Enemy, (int)Tags.FakeWall, (int)Tags.Computer });
            if (collider == null || collider.Entity == ignore || !(collider.Entity is IShootable))
                return;
            ((IShootable)collider.Entity).Hurt(1, this);
            canHurt = false;
            Kill();
        }

        public void Kill()
        {
            if (dead)
                return;
            dead = true;
            BulletBurst.Burst(X, Y);
            Scene.Remove(this);
            Cache.Store(this);
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            CheckEnemyHit();
            Kill();
            base.HitSolid(axis, ref velocity, collision);
        }

        public static void Burst(Vector2 position, Vector2 direction, int bulletCount, float bulletSpeed, Vector2? fromSpeed = null)
        {
            Engine.Scene.Add(Cache.Create<Bullet>(), "default")
                .Define(position, direction, bulletSpeed, fromSpeed ?? Vector2.Zero);
        }
    }
}