using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class Explosion : Entity
    {
        private readonly Animation _sprite;
        private readonly Hitbox _collider;
        private float _delay;
        private bool _started;
        private bool _damaged;

        public Explosion()
            : base(0, 0)
        {
            _sprite = Add(new Animation(GFX.Game["effects/explosion"], 16, 16, Done));
            _sprite.Add("explode", 20f, false, 0, 1, 2, 3);
            _sprite.CenterOrigin();
            _collider = Add(new Hitbox(-6, -6, 12, 12));

            Depth = -10;
        }

        public void Define(Vector2 position, float delay)
        {
            Position = position;
            _delay = delay;
            Visible = false;
            _started = false;
            _damaged = false;
            _sprite.Rotation = Utils.Random() * Calc.TAU;
        }

        public override void Update()
        {
            if (!_started)
            {
                _delay -= Engine.DeltaTime;
                if (_delay <= 0.0)
                {
                    _sprite.Play("explode", true);
                    _started = true;
                    Visible = true;
                }
            }
            else if (_sprite.Frame > 0 && !_damaged)
            {
                _damaged = true;
                Hitbox hitbox = _collider;
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
            if (animation != "explode" || !_started)
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