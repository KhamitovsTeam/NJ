using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class Explosion : Entity
    {
        private readonly Animation sprite;
        private readonly Hitbox collider;
        private float delay;
        private bool started;
        private bool damaged;

        public Explosion()
            : base(0, 0)
        {
            sprite = Add(new Animation(GFX.Game["effects/explosion"], 16, 16, Done));
            sprite.Add("explode", 20f, false, 0, 1, 2, 3);
            sprite.CenterOrigin();
            collider = Add(new Hitbox(-6, -6, 12, 12));

            Depth = -10;
        }

        public void Define(Vector2 position, float delay)
        {
            Position = position;
            this.delay = delay;
            Visible = false;
            started = false;
            damaged = false;
            sprite.Rotation = Utils.Random() * Calc.TAU;
        }

        public override void Update()
        {
            if (!started)
            {
                delay -= Engine.DeltaTime;
                if (delay <= 0.0)
                {
                    sprite.Play("explode", true);
                    started = true;
                    Visible = true;
                }
            }
            else if (sprite.Frame > 0 && !damaged)
            {
                damaged = true;
                Hitbox hitbox = collider;
                int[] intArray = {
                    (int) Tags.Solid,
                    (int) Tags.Enemy,
                    (int) Tags.Player,
                    (int) Tags.Bomb
                };
                foreach (Collider collider in hitbox.CollideAll(intArray))
                {
                    (collider.Entity as IExplodable)?.Explode(this);
                }
            }
            base.Update();
        }

        public void Done(string animation)
        {
            if (animation != "explode" || !started)
                return;
            Cache.Store(this);
            Scene.Remove(this);
        }

        public static void Burst(Vector2 position, float delay)
        {
            Explosion entity = Cache.Create<Explosion>();
            entity.Define(position, delay);
            Engine.Scene.Add(entity, "default");
        }
    }
}