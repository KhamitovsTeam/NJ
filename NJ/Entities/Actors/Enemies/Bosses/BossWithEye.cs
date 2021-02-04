using KTEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Chip
{
    public class BossWithEye : Enemy //WORLD 2: in bottom
    {
        #region Constants

        private const string STATE_IDLE = "idle";
        private const string STATE_SLEEP = "sleep";
        private const string STATE_WALK = "walk";
        private const string STATE_FAKEDEAD = "fakedead";
        private const string STATE_FALL = "fall";
        private const string STATE_OPENEYE = "openeye";
        private const string STATE_SHOOT = "shoot";
        private const string STATE_TURN = "turn";

        private const float walkSpeed = 64;

        #endregion

        #region Vars

        private readonly StateMachine _stateMachine;

        private Collider _eyeCollider;
        private int _openeyeTick = 0;
        private bool is_eye = false;

        #endregion

        private const int Width = 22;
        private const int Height = 29;
        private int direction = 1;
        private float timer = 0;

        private readonly Animation _sprite;

        public BossWithEye()
        {
            Depth = 3;
            Health = 9;
            Points = 1;
            AvatarPath = "default_avatar";

            // boss sprite
            _sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));
            _sprite.Play("stand");

            // boss collider
            MoveCollider = Add(new Hitbox(-11, -5, Width, Height));
            MoveCollider.Tag((int)Tags.Enemy);

            // eye collider
            //        _eyeCollider = Add(new Hitbox(0, 0, 8, 20));
            //        _eyeCollider.Tag((int) Tags.Enemy);

            // Player states
            _stateMachine = Add(new StateMachine());
            _stateMachine.Add(STATE_IDLE);
            _stateMachine.Add(STATE_SLEEP, SleepBegin);
            _stateMachine.Add(STATE_WALK, WalkBegin, WalkUpdate);
            _stateMachine.Add(STATE_TURN, TurnBegin, TurnUpdate);
            _stateMachine.Add(STATE_FAKEDEAD, FakeDeadBegin, FakeDeadUpdate);
            _stateMachine.Add(STATE_FALL);
            _stateMachine.Add(STATE_OPENEYE, OpenEyeBegin, OpenEyeUpdate);
            //_stateMachine.Add(STATE_SHOOT);

            _stateMachine.Set(STATE_IDLE);
        }
        public override void Update()
        {
            timer -= Engine.DeltaTime;
            base.Update();

            if (Input.Pressed(Keys.Q))
            {

            }

            if (_stateMachine.State == STATE_IDLE && (Player.Instance.Position - Position).Length() < 128f)
            {
                _stateMachine.Set(STATE_WALK);
                Music.Play("boss", true);
            }
            base.Update();
        }

        private void SleepBegin()
        {
            _sprite.Play("stand");
        }

        private void WalkBegin()
        {
            Level.HUD.SetEnemy(this);
            timer = 2f;
            direction = -direction;
            _sprite.Scale.X = -direction;
            _sprite.Play("walk");
        }

        private void WalkUpdate()
        {
            Speed.X = direction * walkSpeed;
            Move();

            if (!MoveCollider.Check(direction * 16, 1, (int)Tags.Solid))
                _stateMachine.Set(STATE_TURN);
            if ((X > (double)(Room.SceneX + 16) || direction >= 0) &&
                (X < (double)(Room.SceneX + Room.SceneWidth - 16) || direction <= 0))
                return;
            _stateMachine.Set(STATE_TURN);
        }
        private void TurnBegin()
        {
            timer = 1f;
            _sprite.Play("stand");
        }

        private void TurnUpdate()
        {
            timer -= Engine.DeltaTime;
            if (timer <= 0.0)
                _stateMachine.Set(STATE_WALK);
            if (MoveCollider.Check(0, 1, (int)Tags.Solid))
                return;
        }

        private void FakeDeadBegin()
        {

        }

        private void FakeDeadUpdate()
        {

        }

        private void OpenEyeBegin()
        {
            _sprite.Play("openeye");
        }

        private void OpenEyeUpdate()
        {
            if (!is_eye)
            {
                is_eye = true;
            }
        }

        public override void DyingBegin()
        {
            MoveCollider.Collidable = false;
        }

        public override void DyingUpdate(float timer)
        {
            DieMutateGrahpic(_sprite, timer);
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Horizontal)
                _stateMachine.Set(STATE_TURN);
            base.HitSolid(axis, ref velocity, collision);
        }
    }
}