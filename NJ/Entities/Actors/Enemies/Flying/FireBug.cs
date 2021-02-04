using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class FireBug : Enemy
    {
        private readonly Animation _sprite;
        private int _direction = 1;
        private StateMachine _state;
        private float _timer;
        private float _walkspeed;

        public FireBug()
        {
            Points = 1;

            _sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));

            // collider
            MoveCollider = Add(new Hitbox(-7, -5, 14, 16));
            MoveCollider.Tag((int)Tags.Enemy);

            // states
            _state = Add(new StateMachine());
            _state.Add("fly", BeginFly, UpdateFly);
            _state.Add("turn", BeginTurn, UpdateTurn);
            _state.Add("dead");

            _state.Set("fly");
            DeadTimeoutRate = 6f;
            _walkspeed = Utils.Range(100, 120);

            FallThrough = true;

            Depth = 3;
        }

        public override void Update()
        {
            if (MoveCollider.Check(0, 1, (int)Tags.Acid))
                Hurt(Health);
            base.Update();
        }

        private void BeginFly()
        {
            _direction = -_direction;
            _sprite.Scale.Y = -_direction;
            _sprite.Play("idle");
        }

        private void UpdateFly()
        {
            Speed.Y = _direction * _walkspeed;
            Move();    
            if ((Y > (double)(Room.SceneY + 32) || _direction <= 0) &&
                (Y < (double)(Room.SceneY + Room.SceneHeight - 16) || _direction >= 0))
                return;
            _state.Set("turn");
        }

        private void BeginTurn()
        {
            _timer = 0.5f;
        }

        private void UpdateTurn()
        {
            _timer -= Engine.DeltaTime;
            if (_timer <= 0.0)
                _state.Set("fly");
        }

        public override void DyingBegin()
        {
            MoveCollider.Collidable = false;
            _state.Set("dead");
        }

        public override void DyingUpdate(float timer)
        {
            DieMutateGrahpic(_sprite, timer);
        }

        public override void Knockback(Entity from)
        {
            Push.Y = ForceDirection(@from.Y) * 120;
            Speed.Y = 0.0f;
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Vertical)
            {
                _state.Set("turn");
            }
            base.HitSolid(axis, ref velocity, collision);
        }
    }
}