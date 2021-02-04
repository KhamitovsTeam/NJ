using KTEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Chip
{
    public class BossRocket : Enemy //WORLD 1: ROOF Walkers
    {
        #region Constants

        private const string STATE_IDLE = "idle";
        private const string STATE_UP = "up";
        private const string STATE_DOWN = "down";
        private const string STATE_OPENDOOR = "opendoor";
        private const string STATE_CLOSEDOOR = "closedoor";
        private const string STATE_OPENROOF = "openroof";
        private const string STATE_CLOSEROOF = "closeroof";
        private const string STATE_COMPRESS = "compress";
        private const string STATE_BOSS = "boss";

        #endregion

        #region Vars

        private readonly StateMachine _stateMachine;
        // private Collider _rocketCollider;
        private Collider _bossCollider;

        private int _compressTick = 0;
        private float _compressTimer;
        private float _downTimer;

        #endregion

        private const int Width = 37;
        private const int Height = 73;
        private const int wlkX = 24; //Walker position
        private const int wlkY = 64;
        private const int WLK_COUNT = 5; // constant for PART of walker count
        private const int TOTAL_COUNT = 15; // constant for total walker count

        private float timer = 0;
        private int walker_cnt = 0; //walker count right now (befor the door close)
        private int total_cnt = 0; // total walker count
        private bool is_boss = false;

        private readonly Animation _sprite;
        //    private int _direction = 1;

        public BossRocket()
        {
            Points = 1;
            Depth = 3;
            AvatarPath = "boss_rocket_avatar";

            // boss sprite
            _sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));
            _sprite.Play("idle");

            // rocket collider
            MoveCollider = Add(new Hitbox(0, 0, Width, Height));
            MoveCollider.Tag((int)Tags.Water);

            // boss collider
            _bossCollider = Add(new Hitbox(0, 0, 16, 16));
            _bossCollider.Tag((int)Tags.Enemy);

            // Player states
            _stateMachine = Add(new StateMachine());
            _stateMachine.Add(STATE_IDLE);
            _stateMachine.Add(STATE_UP);
            _stateMachine.Add(STATE_DOWN, DownBegin, DownUpdate);
            _stateMachine.Add(STATE_OPENDOOR, OpenDoorBegin, OpenDoorUpdate);
            _stateMachine.Add(STATE_CLOSEDOOR);
            _stateMachine.Add(STATE_OPENROOF, OpenRoofBegin, OpenRoofUpdate);
            _stateMachine.Add(STATE_CLOSEROOF);
            _stateMachine.Add(STATE_COMPRESS, CompressBegin, CompressUpdate);
            _stateMachine.Add(STATE_BOSS);

            _stateMachine.Set(STATE_IDLE);
        }

        private void CompressBegin()
        {
            _compressTick = 0;
            _compressTimer = 1f;
            _sprite.Play("compress");
        }

        private void CompressUpdate()
        {
            _compressTimer -= Engine.DeltaTime * 1.8f;
            if (_compressTimer > 0.0)
                return;
            _compressTick += 1;
            if (_compressTick >= 3)
            {
                _stateMachine.Set(STATE_OPENDOOR);
            }
            else
            {
                _sprite.Play("compress");
            }
        }

        private void DownBegin()
        {
            _downTimer = 1f;
            _sprite.Play("down");
        }

        private void DownUpdate()
        {
            _downTimer -= Engine.DeltaTime * 1.8f;
            if (_downTimer > 0.0)
                return;
            _stateMachine.Set(STATE_COMPRESS);
        }

        private void OpenDoorBegin()
        {

        }

        private void OpenDoorUpdate()
        {
            if (timer <= 0)
            {
                if (walker_cnt >= WLK_COUNT)
                {
                    if (total_cnt >= TOTAL_COUNT)
                        _stateMachine.Set(STATE_OPENROOF);
                    else
                        _stateMachine.Set(STATE_DOWN);
                    walker_cnt = 0;
                    return;
                }
                if (total_cnt <= 10)
                    Walker1.Born(Room, new Vector2(Position.X + wlkX, Position.Y + wlkY), 1);
                else
                    Walker2.Born(Room, new Vector2(Position.X + wlkX, Position.Y + wlkY), 1);
                walker_cnt++;
                total_cnt++;
                timer = 0.75f;
            }
        }

        private void OpenRoofBegin()
        {
            _sprite.Play("openroof");
        }

        private void OpenRoofUpdate()
        {
            if (!is_boss)
            {
                Boss2.Born(Level, Room, new Vector2(Position.X + 18, Position.Y + 24), 1);
                is_boss = true;
            }
        }

        public override void Begin()
        {
            base.Begin();
        }

        public override void End()
        {
            base.End();
            Engine.Scene = new GameOver();
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
                _stateMachine.Set(STATE_DOWN);
                Music.Play("boss", true);
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
    }
}