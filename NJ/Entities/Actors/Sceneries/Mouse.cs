using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class Mouse : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/mouse"], 6, 2);
        private int _direction = 1;
        private StateMachine _state;
        private float _timer;
        private float _walkspeed;

        public Mouse()
        {
            Add(sprite);
            sprite.Add("stand", 0.0f, 0);
            sprite.Add("walk", 12f, 0, 1);
            sprite.Origin.X = 3f;
            sprite.Origin.Y = -6f;

            // collider
            MoveCollider = Add(new Hitbox(-3, 6, 6, 2));

            // states
            _state = Add(new StateMachine());
            _state.Add("walk", BeginWalk, UpdateWalk);
            _state.Add("turn", BeginTurn, UpdateTurn);

            _state.Set("walk");
            _walkspeed = Utils.Range(48, 96);
        }

        private void BeginWalk()
        {
            _direction = -_direction;
            sprite.Scale.X = -_direction;
            sprite.Play("walk");
        }

        private void UpdateWalk()
        {
            Speed.X = _direction * _walkspeed;
            Move();
            if (!MoveCollider.Check(_direction * 6, 1, (int)Tags.Solid))
                _state.Set("turn");
            if ((X > (double)(Room.SceneX + 6) || _direction >= 0) &&
                (X < (double)(Room.SceneX + Room.SceneWidth - 6) || _direction <= 0))
                return;
            _state.Set("turn");
        }

        private void BeginTurn()
        {
            _timer = 1f;
            sprite.Play("stand");
        }

        private void UpdateTurn()
        {
            _timer -= Engine.DeltaTime;
            if (_timer <= 0.0)
                _state.Set("walk");
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Horizontal)
                _state.Set("turn");
            base.HitSolid(axis, ref velocity, collision);
        }
    }
}