using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class Copter : Enemy
    {
        private readonly Animation _sprite;
        private int _direction = 1;
        private StateMachine _state;
        private float _timer;
        private float _walkspeed;

        public Copter()
        {
            Points = 1;

            _sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));

            // collider
            MoveCollider = Add(new Hitbox(-7, -5, 14, 10));
            MoveCollider.Tag((int)Tags.Enemy);

            // states
            _state = Add(new StateMachine());
            _state.Add("fly", BeginFly, UpdateFly);
            _state.Add("turn", BeginTurn, UpdateTurn);
            _state.Add("dead");

            _state.Set("fly");
            DeadTimeoutRate = 6f;
            _walkspeed = Utils.Range(24, 48);
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
            _sprite.Scale.X = _direction;
            _sprite.Play("idle");
        }

        private void UpdateFly()
        {
            Speed.X = _direction * _walkspeed;
            Move();
            if (MoveCollider.Check(_direction * 2, 0, (int)Tags.Solid))
            {
                _state.Set("turn");
                return;
            }
            if ((X > (double)(Room.SceneX + 16) || _direction >= 0) &&
                (X < (double)(Room.SceneX + Room.SceneWidth - 16) || _direction <= 0))
                return;
            _state.Set("turn");
        }

        private void BeginTurn()
        {
            _timer = 1f;
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
            Push.X = ForceDirection(@from.X) * 120;
            Speed.X = 0.0f;
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Vertical && _state.State == "fall")
                _state.Set("fly");
            base.HitSolid(axis, ref velocity, collision);
        }
    }
}