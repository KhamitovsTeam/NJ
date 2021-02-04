using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class BugBoss : Enemy
    {
        private readonly Animation _sprite;
        private readonly StateMachine _state;
        private readonly float _walkSpeed;
        private readonly float _roundSpeed;
        private int _direction = 1;
        private float _timer;
        private Animation _spriteArms;

        public BugBoss()
        {
            Health = 24;
            Points = 1;
            AvatarPath = "bug_boss_avatar";

            _sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));
            _sprite.OnFinish += animation =>
            {
                // Если закончилась анимация пробуждения, то начинаем ходить
                if (animation.Equals("awake"))
                {
                    _state.Set("walk");
                }
            };

            // collider
            MoveCollider = Add(new Hitbox(-15, -15, 30, 30));
            MoveCollider.Tag((int)Tags.Enemy);

            // states
            _state = Add(new StateMachine());
            _state.Add("sleep", SleepBegin);
            _state.Add("activate", ActivateBegin, ActivateUpdate);
            _state.Add("walk", WalkBegin, WalkUpdate);
            //_state.Add("shoot", ShootBegin, ShootUpdate, ShootEnd);
            _state.Add("turn", TurnBegin, TurnUpdate);
            _state.Add("fall", FallBegin, FallUpdate, FallEnd);
            _state.Add("dead");
            _state.Set("sleep");

            DeadTimeoutRate = 6f;

            _walkSpeed = Utils.Range(64, 64);
            _roundSpeed = Utils.Range(15, 15);

            CreateArms(0, "enemies/bug_boss_arms", 41, 37, 20, 16);
            //Depth = 2;
        }

        public override void Update()
        {
            if ((Player.Instance.Position - Position).Length() < 64f && _state.State == "sleep")
            {
                _state.Set("activate");
                Music.Play("boss", true);
            }
            if (MoveCollider.Check(0, 1, (int)Tags.Acid))
                Hurt(Health);
            base.Update();
        }

        private void SleepBegin()
        {
            _sprite.Play("sleep");
        }

        private void ActivateBegin()
        {
            Level.HUD.SetEnemy(this);
            _sprite.Play("awake");
        }

        private void ActivateUpdate()
        {

        }

        private void WalkBegin()
        {
            _timer = 2f;
            _direction = -_direction;
            _sprite.Scale.X = -_direction;
            _sprite.Play("walk");
        }

        private void WalkUpdate()
        {
            Speed.X = _direction * _walkSpeed;
            Move();
            _timer -= Engine.DeltaTime;
            _sprite.Rotation = -_direction * _timer * _roundSpeed;
            if (_timer <= 0.0)
            {
                //_state.Set("shoot");
                return;
            }
            if (!MoveCollider.Check(_direction * 16, 1, (int)Tags.Solid))
            {
                _state.Set("turn");
            }
            if (!MoveCollider.Check(0, 1, (int)Tags.Solid))
                _state.Set("fall");
            if ((X > (double)(Room.SceneX + 16) || _direction >= 0) &&
                (X < (double)(Room.SceneX + Room.SceneWidth - 16) || _direction <= 0))
                return;
            _state.Set("turn");
        }

        private void TurnBegin()
        {
            _timer = 1f;
            _sprite.Play("stand");
            _sprite.Rotation = 0;
        }

        private void TurnUpdate()
        {
            _timer -= Engine.DeltaTime;
            if (_timer <= 0.0)
                _state.Set("walk");
            if (MoveCollider.Check(0, 1, (int)Tags.Solid))
                return;
            _state.Set("fall");
        }

        private void FallBegin()
        {
            Speed.X = 0.0f;
            Speed.Y = 0.0f;
        }

        private void FallUpdate()
        {
            Fall(600f);
            Move();
        }

        private void FallEnd()
        {

        }

        private void ShootBegin()
        {
            _sprite.Play("shoot");
            _timer = 0.25f;
        }

        private void ShootUpdate()
        {
            _timer -= Engine.DeltaTime;
            if (_timer > 0.0)
                return;
            var num = Utils.Choose(-1, 1) * Utils.RangeF(60f, 120f);
            Bomb.Burst(Position, _direction, 1);
            _direction *= -1;
            _state.Set("walk");
        }

        private void ShootEnd()
        {

        }

        public override void DyingBegin()
        {
            MoveCollider.Collidable = false;
            Level.HUD.ClearEnemy();
            _state.Set("dead");
        }

        public override void DyingUpdate(float timer)
        {
            DieMutateGrahpic(_sprite, timer);
        }

        public override void DyingEnd()
        {
            base.DyingEnd();
            Player.Instance.Speed.X = 0;
            var cut = new CS341_BugBossKilled
            {
                Level = Engine.Scene as Level
            };
            cut.Start();
            Engine.Scene.Add(cut);
        }

        public override void Knockback(Entity from)
        {
            Push.X = ForceDirection(from.X) * 120;
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

        public override void HurtUpdate(float timer)
        {
            if (_state.State == "sleep") return;
            base.HurtUpdate(timer);
            _sprite.Alpha = 1f - timer;
        }

        private void CreateArms(int index, string animName, int _wight, int _height, int _originX, int _originY)
        {
            //Points = 1;

            // sprite
            _spriteArms = new Animation(GFX.Game[animName], _wight, _height);
            _spriteArms.Add("idle", 8f, 0, 1, 2);
            _spriteArms.Add("shoot", 8f, 3, 4, 5);
            _spriteArms.Origin.X = _originX;
            _spriteArms.Origin.Y = _originY;
            _spriteArms.Play("idle");
            Add(_spriteArms);
            //Depth = -1;
        }

    }
}