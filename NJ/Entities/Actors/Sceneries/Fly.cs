using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class Fly : Actor
    {
        private readonly Animation _sprite = new Animation(GFX.Game["sceneries/fly"], 8, 8);
        private readonly StateMachine _state = new StateMachine();

        private int _direction = Utils.Choose(-1, 1);

        private readonly int _speed = Utils.Range(32, 40);
        private int _dropping = 1;
        private readonly float _yAmplitude = 16f;
        private float _yAxis;
        private bool _turning;
        private float _turnTimer;

        public Fly()
        {
            Add(_sprite);
            _sprite.Add("fly", 8f, 0, 1);
            _sprite.Origin.X = 4f;
            _sprite.Origin.Y = 4f;
            _sprite.Play("fly");
            _sprite.CenterOrigin();
            Add(_state);
            _state.Add("normal", NormalBegin, NormalUpdate, NormalEnd);
            _state.Set("normal");
            MoveCollider = Add(new Hitbox(-6, -6, 12, 12));
            MoveCollider.Tag((int)Tags.Sceneries);
        }

        public override void Begin()
        {
            _yAxis = Y;
        }

        private void NormalBegin()
        {
            _sprite.Play("fly");
        }

        private void NormalUpdate()
        {
            Speed.X = _speed * _direction;
            Speed.Y += _dropping * 256 * Engine.DeltaTime;
            Clamp(32f, 32f);
            Move();
            _sprite.Scale.X = -_direction;
            if (_dropping < 0 && Y < _yAxis - (double)_yAmplitude ||
                _dropping > 0 && Y > _yAxis + (double)_yAmplitude)
                _dropping = -_dropping;
            if (_turning)
            {
                _turnTimer -= Engine.DeltaTime;
                if (_turnTimer <= 0.0)
                    _direction = -_direction;
                if (!MoveCollider.Check(_direction, 0, Solids))
                    _turning = false;
            }

            if ((X >= (double)(Room.SceneX + 16) || _direction >= 0) &&
                (X <= (double)(Room.SceneX + Room.SceneWidth - 16) || _direction <= 0))
                return;
            _direction = -_direction;
        }

        private void NormalEnd()
        {
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Horizontal)
            {
                if (!_turning)
                {
                    _turning = true;
                    _turnTimer = 0.75f;
                }
            }
            else if (Speed.Y < 0.0)
            {
                _dropping = 1;
                _yAxis = Y + _yAmplitude;
            }
            else
            {
                _dropping = -1;
                _yAxis = Y - _yAmplitude;
            }

            base.HitSolid(axis, ref velocity, collision);
        }
    }
}