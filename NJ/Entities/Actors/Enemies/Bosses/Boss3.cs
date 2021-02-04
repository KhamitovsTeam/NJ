using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class Boss3 : Enemy //WORLD 2: Right Roots
    {
        private readonly Animation _sprite;
        private readonly StateMachine _state;
        private readonly float _walkSpeed;
        private int _hits = 0;  //hit count
        private int _index = 0;  //walker index on walkers' array
        private int _direction = 1;
        private float _timer;

        private Animation _spriteWalker;  //walker (on boss)
        private Animation[] _walkers;   //walker's array (on boss)

        const int cons_to_attack = 3;  //hits count to attack by walker

        public Boss3()
        {
            Health = 24;
            Points = 1;
            AvatarPath = "boss3_avatar";

            _walkers = new Animation[7];

            CreateWalkerOnBoss(0, "enemies/walker1", 16, 16, 20, 28);
            CreateWalkerOnBoss(1, "enemies/walker1", 16, 16, 7, 28);
            CreateWalkerOnBoss(2, "enemies/walker1", 16, 16, -6, 28);

            CreateWalkerOnBoss(3, "enemies/walker1", 16, 16, 25, 21);
            CreateWalkerOnBoss(4, "enemies/walker1", 16, 16, 13, 21);
            CreateWalkerOnBoss(5, "enemies/walker1", 16, 16, 1, 21);
            CreateWalkerOnBoss(6, "enemies/walker1", 16, 16, -11, 21);

            _sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));

            // collider
            MoveCollider = Add(new Hitbox(-28, -11, 57, 19));
            MoveCollider.Tag((int)Tags.Enemy);

            // states
            _state = Add(new StateMachine());
            _state.Add("sleep", SleepBegin);
            _state.Add("activate", ActivateBegin, ActivateUpdate);
            _state.Add("walk", WalkBegin, WalkUpdate);
            _state.Add("attack", AttackBegin, AttackUpdate, AttackEnd);
            _state.Add("turn", TurnBegin, TurnUpdate);
            _state.Add("fall", FallBegin, FallUpdate, FallEnd);
            _state.Add("dead");
            _state.Set("sleep");

            Depth = 2;

            DeadTimeoutRate = 6f;

            _walkSpeed = Utils.Range(64, 64);
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

        public override void Hurt(int damage = 0, Entity from = null)
        {
            if (Invincible || HurtTimer > 0.0 ||
                !Explodable && from != null && from is Explosion)
                return;
            HurtTimer = HurtTimeout;
            Health -= damage;
            SFX.Play("enemy_damage");
            if (Health <= 0 && !DeadTimeout)
                Kill();
            else
            {
                _hits += 1;
                if (_hits >= cons_to_attack)
                {
                    //remove walker sprite from boss
                    var walker = _walkers[_index];
                    Remove(walker);
                    _walkers[_index] = null;

                    _index += 1;
                    _hits = 0;
                    //add walker to attack
                    Walker1.Born(Room, Position, 1);
                }
                HurtBegin(from);
            }
        }

        private void SleepBegin()
        {
            _sprite.Play("idle");
        }

        private void ActivateBegin()
        {
            Level.HUD.SetEnemy(this);
            _sprite.Play("walk");
            _state.Set("walk");
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
            if (_timer <= 0.0)
            {
                _state.Set("attack");
                return;
            }
            if (!MoveCollider.Check(_direction * 16, 1, (int)Tags.Solid))
                _state.Set("turn");
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
            _sprite.Play("idle");
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

        private void AttackBegin()
        {
            _sprite.Play("attack");
            _timer = 0.25f;
        }

        private void AttackUpdate()
        {
            _timer -= Engine.DeltaTime;
            if (_timer > 0.0)
                return;
            var num = Utils.Choose(-1, 1) * Utils.RangeF(60f, 120f);
            //Walker1.Born(Room, Position, 1);
            _direction *= -1;
            _state.Set("walk");
        }

        private void AttackEnd()
        {

        }

        public override void DyingBegin()
        {
            Level.HUD.ClearEnemy();
            MoveCollider.Collidable = false;
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
            var cut = new CS231_RootBossKilled
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

        private void CreateWalkerOnBoss(int index, string animName, int _wight, int _height, int _originX, int _originY)
        {
            Points = 1;

            // sprite
            _spriteWalker = new Animation(GFX.Game[animName], _wight, _height);
            _spriteWalker.Add("idle", 8f, 0, 1, 2);
            _spriteWalker.Origin.X = _originX;
            _spriteWalker.Origin.Y = _originY;
            _spriteWalker.Play("idle");
            Add(_spriteWalker);

            _walkers[index] = _spriteWalker;
            // collider
            //  MoveCollider = Add(new Hitbox(-5, -5, 10, 10));
            //  MoveCollider.Tag((int)Tags.Enemy);

        }

        /*
        private void CreateEnemy(string animName, int _wight, int _height, int _originX, int _originY)
        {
            _enWalker = new Walker1();
        }
        */
    }
}