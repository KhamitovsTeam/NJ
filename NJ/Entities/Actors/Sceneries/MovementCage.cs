using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class MovementCage : Actor
    {
        private Animation _sprite;
        private int _type = 0;
        private StateMachine _state;
        private float _timer;
        private float _movespeed;
        private int _direction = 1;

        public MovementCage()
            : base(0, 0)
        {
            Depth = -8;

            // states
            _state = Add(new StateMachine());
            _state.Add("move", BeginMove, UpdateMove);
            _state.Add("turn", BeginTurn, UpdateTurn);

            _state.Set("move");
            _movespeed = 32f;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/movement_cage" + _type], 32, 32);
            Add(_sprite);

            _sprite.Add("idle", 8f, 0);
            _sprite.Origin.X = 16f;
            _sprite.Origin.Y = 0f;
            _sprite.Play("idle");

            MoveCollider = Add(new Hitbox(-12, 0, 24, 32));
        }

        private void BeginMove()
        {
            _direction = -_direction;
            _sprite.Scale.X = -_direction;
        }

        private void UpdateMove()
        {
            Speed.X = _direction * _movespeed;
            Move();
            // if (!MoveCollider.Check(_direction * 6, 1, (int)Tags.Solid))
            //     _state.Set("turn");
            if ((X > (double)(Room.SceneX + 6) || _direction >= 0) &&
                (X < (double)(Room.SceneX + Room.SceneWidth - 6) || _direction <= 0))
                return;
            _state.Set("turn");
        }

        private void UpdateTurn()
        {
            _timer -= Engine.DeltaTime;
            if (_timer <= 0.0)
                _state.Set("move");
        }

        private void BeginTurn()
        {
            _timer = 0.5f;
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Horizontal)
                _state.Set("turn");
            base.HitSolid(axis, ref velocity, collision);
        }

    }
}
