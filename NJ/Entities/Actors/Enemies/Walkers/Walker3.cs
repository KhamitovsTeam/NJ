using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class Walker3 : Enemy
    {
        private readonly Animation _sprite;
        private int _direction = 1;
        private StateMachine _state;
        private float _timer;
        private float _walkspeed;

        public Walker3()
        {
            Health = 2;
            Points = 1;

            // sprite
            _sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));

            // collider
            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Enemy);

            // states
            _state = Add(new StateMachine());
            _state.Add("walk", BeginWalk, UpdateWalk);
            _state.Add("turn", BeginTurn, UpdateTurn);
            _state.Add("fall", BeginFall, UpdateFall, EndFall);
            _state.Add("dead");

            _state.Set("walk");
            DeadTimeoutRate = 6f;
            _walkspeed = Utils.Range(50, 80);
        }

        public void Define(Vector2 position)
        {
            Position = position;
            MoveCollider.Reset(-8, -8, 16, 16);
            _sprite.Visible = true;
        }

        public override void Update()
        {
            if (MoveCollider.Check(0, 1, (int)Tags.Acid))
                Hurt(Health);
            base.Update();
        }

        private void BeginWalk()
        {
            _direction = -_direction;
            _sprite.Scale.X = _direction;
            _sprite.Play("walk");
        }

        private void UpdateWalk()
        {
            Speed.X = _direction * _walkspeed;
            Move();
            if (!MoveCollider.Check(_direction * 16, 1, (int)Tags.Solid))
                _state.Set("turn");
            if (!MoveCollider.Check(0, 1, (int)Tags.Solid))
                _state.Set("fall");
            if ((X > (double)(Room.SceneX + 16) || _direction >= 0) &&
                (X < (double)(Room.SceneX + Room.SceneWidth - 16) || _direction <= 0))
                return;
            _state.Set("turn");
        }

        private void BeginTurn()
        {
            _timer = 1f;
            _sprite.Play("stand");
        }

        private void UpdateTurn()
        {
            _timer -= Engine.DeltaTime;
            if (_timer <= 0.0)
                _state.Set("walk");
            if (MoveCollider.Check(0, 1, (int)Tags.Solid))
                return;
            _state.Set("fall");
        }

        private void BeginFall()
        {
            Speed.X = 0.0f;
            Speed.Y = 0.0f;
        }

        private void UpdateFall()
        {
            Fall(600f);
            Move();
        }

        private void EndFall()
        {
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
            _state.Set("turn");
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Horizontal)
                _state.Set("turn");
            else if (axis == Hit.Vertical && _state.State == "fall")
                _state.Set("walk");
            base.HitSolid(axis, ref velocity, collision);
        }

        public static void Born(Room room, Vector2 position, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                Walker1 entity = new Walker1();
                entity.Room = room;
                entity.Define(position);
                Engine.Scene.Add(entity, "default");
            }
        }
    }
}