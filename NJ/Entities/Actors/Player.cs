using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Chip
{
    /// <summary>
    /// Главный класс игрока
    /// </summary>
    public class Player : Actor, IExplodable
    {
        #region Constants

        public const string StateNormal = "normal";
        public const string StateAir = "air";
        public const string StateWall = "wall";
        public const string StateDead = "dead";
        public const string StateHurt = "hurt";
        public const string StateCutScene = "cutscene";
        public const string StateEndGame = "end_game";
        public const string StateLightShow = "light_show";
        public const string StateGotItem = "got_item";
        public const string StateSlow = "slow_walking";

        private const int AccelerationNormal = 900;
        private const int AccelerationAir = 600;
        private const int FrictionNormal = 900;
        private const int FrictionAir = 800;
        private const int Jump = -60;
        private const int Gravity = 500;
        private const int GravityWall = 300;
        private const int UpwardCornerCorrection = 4;
        private const float JumpDuration = 0.2f;
        private const float WallDuration = 0.08f;
        private const float ShootTimeout = 0.2f;
        private const float LightTime = 0.3f;

        private readonly Vector2 maxSpeedNormal = new Vector2(25f, 0.0f);
        private readonly Vector2 maxSpeedSlow = new Vector2(15f, 0.0f);
        private readonly Vector2 maxSpeedAir = new Vector2(30f, 200f);
        private readonly Vector2 maxSpeedWall = new Vector2(30f, 40f);

        #endregion

        #region Public Vars

        public static Player Instance => instance ?? (instance = new Player());

        public void ClearPlayer()
        {
            instance = null;
        }

        public readonly Animation Sprite;
        public readonly StateMachine StateMachine;
        public Weapon CurrentWeapon;
        public PlayerData PlayerData;
        public int HighestLives = 9;
        public int MaxLives = 12;
        public float Invincible;
        public float Freeze;
        public bool Hidden;
        public bool Dead;
        public bool Ragdoll;
        public bool Weaponless;
        public bool SlowMoving;
        public bool CanBreakeFakeWalls;

        #endregion

        #region Private Vars

        private static Player instance;

        private readonly LightLine lightLine;
        private Vector2 pointing = new Vector2(1f, 0.0f);
        private Vector2 maxspeed;
        private Enemy hurtEnemy;

        private float lightLineTimer;
        private float lightLineSpeed;
        private float lightLineAccleration;
        private float jumpTimer;
        private float lungeTimer;
        private float wallTimer;
        private float glassWalkTimer;
        private float shootTimer;
        private float hurtTimer;
        private float fallThroughDuration;
        private float fade;
        private int direction;
        private int wallDirection;
        private bool jumpKey;
        private bool jumpPressed;
        private bool solidLeft;
        private bool solidRight;
        private bool solidDown;
        private bool doubleJumped;
        private bool pressedShoot;

        #endregion

        private bool InControl
        {
            get
            {
                switch (StateMachine.State)
                {
                    default:
                        return true;

                    case StateDead:
                    case StateLightShow:
                    case StateCutScene:
                    case StateEndGame:
                        return false;
                }
            }
        }

        private bool Moving => direction != 0;

        public bool IgnoreAction;

        public Player()
        {
            PlayerData = new PlayerData();

            // Player sprites
            Sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name, AnimationGotItemHide));

            // Player collider
            MoveCollider = Add(new Hitbox(-2, -4, 4, 8));
            MoveCollider.Tag((int)Tags.Player);

            // Player states
            StateMachine = new StateMachine();
            StateMachine.Add(StateNormal, NormalBegin, NormalUpdate, NormalEnd);
            StateMachine.Add(StateAir, AirBegin, AirUpdate, AirEnd);
            StateMachine.Add(StateWall, WallBegin, WallUpdate, WallEnd);
            StateMachine.Add(StateLightShow, LightShowBegin, LightShowUpdate);
            StateMachine.Add(StateDead, DeadUpdate);
            StateMachine.Add(StateEndGame, EndGameBegin, EndGameUpdate);
            StateMachine.Add(StateHurt, HurtBegin, HurtUpdate, HurtEnd);
            StateMachine.Add(StateCutScene);
            StateMachine.Add(StateGotItem, AnimationGotItem);
            StateMachine.Add(StateSlow, SlowBegin, SlowUpdate, SlowEnd);
            StateMachine.Set(StateNormal);
            Add(StateMachine);

            // Player's weapon
            CurrentWeapon = Add(new Pistol());

            lightLine = Add(new LightLine());
            lightLine.Width = 0f;
            lightLine.Height = 0f;

            Depth = 0;
        }

        public override void Begin()
        {
            Tag = "";
            base.Begin();

            if (Level.Session.PlayerData == null)
            {
                Level.Session.PlayerData = PlayerData;
            }
            else
            {
                PlayerData = Level.Session.PlayerData;
            }

            Weaponless = false;
            SlowMoving = false;
        }

        public override void Update()
        {
            pointing.X = PlayerData.Facing;
            pointing.Y = 0.0f;

#if DEBUG && !CONSOLE && !__MOBILE__
            if (Input.Pressed("debug"))
            {
                Entity elevator = null;
                foreach (var entity in Scene.GetEntities().Where(entity => entity.GetType() == typeof(ElevatorRope)))
                {
                    elevator = entity;
                }
                ((ElevatorRope)elevator)?.TurnOn();
                if (elevator != null) Position = new Vector2(elevator.X + 16, elevator.Y + 16);
            }
#endif
            if (!Input.Down("left"))
            {
                if (!Input.Down("right"))
                    direction = 0;
            }

            if (!Input.Pressed("left"))
            {
                if (Input.Released("right"))
                {
                    if (!Input.Down("left"))
                        goto right;
                }
                else
                    goto right;
            }
            if (InControl)
            {
                direction = -1;
                pointing.X = -1f;
            }
            else
            {
                direction = 0;
                pointing.X = 0;
            }
        right:
            if (!Input.Pressed("right"))
            {
                if (Input.Released("left"))
                {
                    if (!Input.Down("right"))
                        goto left;
                }
                else
                    goto left;
            }
            if (InControl)
            {
                direction = 1;
                pointing.X = 1f;
            }
            else
            {
                direction = 0;
                pointing.X = 0;
            }
        left:
            if (direction != 0)
                PlayerData.Facing = direction;
            jumpKey = Input.Down("jump") && InControl;
            jumpPressed = Input.Pressed("jump") && InControl;
            if (Freeze > 0.0 || Ragdoll)
            {
                Freeze -= Engine.DeltaTime;
            }
            else
            {
                if (fallThroughDuration > 0.0)
                {
                    fallThroughDuration -= Engine.DeltaTime;
                    if (fallThroughDuration <= 0.0)
                        FallThrough = false;
                }
                solidLeft = MoveCollider.Check(-1, 0, Solids) && !(MoveCollider.Collide(-1, 0, Solids).Entity is Jumpthrough);
                solidRight = MoveCollider.Check(1, 0, Solids) && !(MoveCollider.Collide(1, 0, Solids).Entity is Jumpthrough);
                //solidUp = MoveCollider.Check(0, -1, Solids) && !(MoveCollider.Collide(0, -1, Solids).Entity is Jumpthrough);
                solidDown = MoveCollider.Check(0, 1, Solids);
                if (solidDown)
                {
                    Collider collider = MoveCollider.Collide(0, 1, Solids);
                    if (collider.Entity is Jumpthrough &&
                        (FallThrough || Speed.Y < 0.0 ||
                         Y + (double)MoveCollider.Y + MoveCollider.Height > collider.Entity.Y))
                    {
                        solidDown = false;
                    }

                }
                base.Update();
                shootTimer -= Engine.DeltaTime;
                if ((Input.Pressed("shoot") && InControl || pressedShoot) && StateMachine.State != StateLightShow && StateMachine.State != StateDead && StateMachine.State != StateHurt && StateMachine.State != StateSlow && !Weaponless)
                {
                    if (IgnoreAction)
                    {
                        IgnoreAction = false;
                        return;
                    }
                    pressedShoot = true;
                    if (shootTimer <= 0.0)
                    {
                        pressedShoot = false;
                        shootTimer = ShootTimeout;
                        if (CurrentWeapon != null)
                        {
                            CurrentWeapon?.Shoot(pointing);
                            if (StateMachine.State == StateAir && pointing.X != 0.0f)
                                Push.X = (float)(-(double)pointing.X * 60.0);
                        }
                    }
                }
                if (StateMachine.State != StateHurt && StateMachine.State != StateDead && StateMachine.State != StateEndGame && StateMachine.State != StateLightShow)
                {
                    if (StateMachine.State != StateWall)
                        Sprite.Scale.X = PlayerData.Facing;
                    if (StateMachine.State == StateAir || StateMachine.State == StateWall && Speed.Y < 0.0)
                    {
                        if (Weaponless || SlowMoving)
                        {
                            Sprite.Play(Speed.Y < 0.0 ? "jump_without_gun" : "fall_without_gun");
                        }
                        else
                        {
                            Sprite.Play(Speed.Y < 0.0 ? "jump" : "fall");
                        }
                    }
                    else if (StateMachine.State == StateWall)
                        if (Weaponless || SlowMoving)
                        {
                            Sprite.Play("wall_without_gun");
                        }
                        else
                        {
                            Sprite.Play("wall");
                        }
                    else if (direction != 0)
                    {
                        if (Weaponless)
                        {
                            Sprite.Play("run_without_gun");
                        }
                        else if (SlowMoving)
                        {
                            Sprite.Play("slow_walk");
                            StateMachine.Set(StateSlow);
                        }
                        else
                        {
                            Sprite.Play("run");
                        }
                    }
                    else
                    {
                        if (Weaponless)
                        {
                            Sprite.Play("stand_without_gun");
                        }
                        else if (SlowMoving)
                        {
                            Sprite.Play("heavy_breath");
                            StateMachine.Set(StateSlow);
                        }
                        else
                        {
                            Sprite.Play("stand");
                        }
                    }
                }
                if (Dead) return;
                Clamp(maxspeed.X, maxspeed.Y);
                Move();
                Invincible -= Engine.DeltaTime;
                Collider hiddenCollider = MoveCollider.Collide((int)Tags.HiddenPlace);
                if (hiddenCollider != null)
                {
                    Hidden = true;
                }
                else
                {
                    Hidden = false;
                }
                Collider enemyCollider = MoveCollider.Collide(new[] { (int)Tags.Enemy, (int)Tags.Water });
                if (enemyCollider != null)
                {
                    Actor entity = enemyCollider.Entity as Actor;
                    if (entity is Laser laser)
                    {
                        // TODO: 
                        hurtEnemy = laser;
                        switch (laser.Rotation)
                        {
                            case 0:
                                if (Speed.Y < 0)
                                {
                                    Speed.Y = 0;
                                    Push.Y = 220f;
                                }
                                else
                                {
                                    Speed.Y = 0;
                                    Push.Y = -220f;
                                }
                                StateMachine.Set(StateHurt);
                                break;
                            case 90:
                                if (Speed.X > 0)
                                {
                                    Speed.X = 0;
                                    Push.X = -220f;
                                }
                                else
                                {
                                    Speed.X = 0;
                                    Push.X = 220f;
                                }
                                StateMachine.Set(StateHurt);
                                break;
                            case 180:
                                if (Speed.Y < 0)
                                {
                                    Speed.Y = 0;
                                    Push.Y = 220f;
                                }
                                else
                                {
                                    Speed.Y = 0;
                                    Push.Y = -220f;
                                }
                                StateMachine.Set(StateHurt);
                                break;
                            default:
                                if (Speed.X > 0)
                                {
                                    Speed.X = 0;
                                    Push.X = -220f;
                                }
                                else
                                {
                                    Speed.X = 0;
                                    Push.X = 220f;
                                }
                                StateMachine.Set(StateHurt);
                                break;
                        }

                    }
                    else
                    {
                        Hurt(entity);
                    }
                }
            }

            /*if (Position.Y + Sprite.Height / 2f >= Level.Height)
            {
                PlayerData.Lives = 0;
                StateMachine.Set(StateDead);
            }*/

            //CameraPointing.X += (float) (((double) (this._direction * 32) - (double) this.CameraPointing.X) / 40.0);
            //CameraPointing.Y += (float) (((double) this.pointing.Y * 48.0 - (double) this.CameraPointing.Y) / 20.0);
        }

        private void NormalBegin()
        {
            maxspeed = maxSpeedNormal;
            doubleJumped = false;
        }

        private void NormalUpdate()
        {
            if (direction < 0 && !solidLeft || direction > 0 && !solidRight)
                Speed.X += AccelerationNormal * direction * Engine.DeltaTime;
            if (!Moving)
                Slowdown(FrictionNormal, 0.0f);
            if (jumpPressed && InControl)
            {
                if (Input.Down("lunge") && direction != 0)
                {
                    jumpTimer = JumpDuration / 5f;
                    lungeTimer = 0.1f;
                }
                else
                    jumpTimer = JumpDuration;
                SFX.Play("jump");
                StateMachine.Set(StateAir);
                SaveData.Instance.TotalJumps++;
            }
            if (!solidDown)
            {
                StateMachine.Set(StateAir);
            }
            else
                glassWalkTimer = 1f;
        }

        private void NormalEnd()
        {
        }

        private void AirBegin()
        {
            maxspeed = maxSpeedAir;
            if ((StateMachine.LastState == StateNormal || StateMachine.LastState == StateWall) && jumpTimer > 0.0)
            {
                Smoke.Burst(X, Y + 8f, 2f, Calc.PI / 2, 0.3926991f, 2);
            }
            if (StateMachine.LastState != StateNormal || jumpTimer <= 0.0)
                return;
            if (!Input.Down("lunge"))
                return;
            Speed.X = maxSpeedAir.X * PlayerData.Facing;
            Push.X = maxSpeedAir.X * PlayerData.Facing;
        }

        private void AirUpdate()
        {
            Speed.X += AccelerationAir * direction * Engine.DeltaTime;
            if (!Moving)
                Slowdown(FrictionAir, 0.0f);
            jumpTimer -= Engine.DeltaTime;
            if (jumpKey && jumpTimer > 0.0)
            {
                Speed.Y = Jump;
            }
            else
            {
                jumpTimer = 0.0f;
                lungeTimer -= Engine.DeltaTime;
                if (Math.Abs(Speed.Y) < 24.0 && jumpKey ||
                    lungeTimer > 0.0 && Speed.Y < -64.0)
                    Fall(450f);
                else
                    Fall(Gravity);
                /*if (MoveCollider.Check(direction, 0, Solids) && !(MoveCollider.Collide(direction, 0, (int)Tags.Solid).Entity is Jumpthrough)
                        && !(MoveCollider.Collide(direction, 0, (int)Tags.Solid).Entity is LevelWall) && PlayerData.Armwear.Contains(Powerups.Claws))
                    StateMachine.Set(StateWall);*/
            }
            if (Speed.Y < 0.0 || !solidDown)
                return;
            StateMachine.Set(StateNormal);
        }

        private void AirEnd()
        {
            if (StateMachine.NextState != StateNormal || !solidDown)
                return;
            Smoke.Burst(X, Y + 8f, 4f, 0.0f, 0.2617994f, 2);
            Smoke.Burst(X, Y + 8f, 4f, Calc.PI, 0.2617994f, 2);
            SFX.Play("down");
            Input.Rumble(Input.RumbleStrength.Jump, Input.RumbleLength.Short);
        }

        private void WallBegin()
        {
            wallDirection = !solidLeft ? 1 : -1;
            wallTimer = WallDuration;
            Speed.X = 0.0f;
        }

        private void WallUpdate()
        {
            if (Speed.Y < 0.0)
            {
                Fall(Gravity);
                maxspeed = maxSpeedAir;
            }
            else
            {
                Fall(GravityWall);
                maxspeed = maxSpeedWall;
            }
            if (direction != wallDirection)
                wallTimer -= Engine.DeltaTime;
            else
                wallTimer = WallDuration;
            if (pointing.X != 0.0f && Speed.Y > 0.0)
                pointing.X = -wallDirection;
            if (jumpPressed)
            {
                SFX.Play("jump");
                jumpTimer = JumpDuration;
                Speed.X = wallDirection < 0 ? maxSpeedAir.X : -maxSpeedAir.X;
                StateMachine.Set(StateAir);
            }
            if (solidDown)
                StateMachine.Set(StateNormal);
            if (wallTimer > 0.0 && (wallDirection != -1 || solidLeft) &&
                (wallDirection != 1 || solidRight))
                return;
            StateMachine.Set(StateAir);
        }

        private void WallEnd()
        {
        }

        private void LightShowBegin()
        {
            Dead = true;
            SFX.Play("player_death");
            MoveCollider.Collidable = false;
            lightLineTimer = LightTime;
            lightLineSpeed = 10f / lightLineTimer;
            lightLine.Width = 0f;
            lightLine.Height = Math.Min(Position.Y, Constants.GameHeight * 1.75f);
        }

        private void LightShowUpdate()
        {
            lightLineTimer -= Engine.DeltaTime;
            lightLineAccleration = lightLineSpeed * Engine.DeltaTime;
            lightLine.Width += lightLineAccleration;
            if (lightLineTimer > 0f)
                return;
            StateMachine.Set(StateDead);
        }

        private void DeadUpdate()
        {
            fade += Engine.RawDeltaTime * 0.25f;
            //Sprite.Alpha = 1f - _fade;
            if (fade < 1.0)
                return;
            Engine.Scene = new GameOver();
        }

        private void EndGameBegin()
        {
            Dead = true;
        }

        private void EndGameUpdate()
        {
            fade += Engine.RawDeltaTime * 0.25f;
            Sprite.Alpha = 1f - fade;
            if (fade < 1.0)
                return;
            Engine.Scene = new End();
        }

        private void HurtBegin()
        {
            hurtTimer = 0.25f;
            Speed.X = Speed.Y = 0.0f;
            maxspeed = maxSpeedAir;
            if (hurtEnemy is Laser)
            {
                SFX.Play("laser");    
            }
            else
            {
                SFX.Play("player_damage");
            }
        }

        private void HurtUpdate()
        {
            Fall(Gravity);
            hurtTimer -= Engine.DeltaTime;
            if (hurtTimer > 0.0)
                return;
            jumpTimer = 0.0f;
            StateMachine.Set(StateAir);
        }

        private void HurtEnd()
        {
            if (PlayerData.Lives > 0)
                return;
            StateMachine.Set(StateLightShow);
        }

        private void SlowBegin()
        {
            maxspeed = maxSpeedSlow;
        }

        private void SlowUpdate()
        {
            if (direction < 0 && !solidLeft || direction > 0 && !solidRight)
                Speed.X += AccelerationNormal * direction * Engine.DeltaTime;
            if (!Moving)
                Slowdown(FrictionNormal, 0.0f);
            /*if (jumpPressed && InControl)
            {
                if (Input.Down("lunge") && direction != 0)
                {
                    jumpTimer = JumpDuration / 5f;
                    lungeTimer = 0.1f;
                }
                else
                    jumpTimer = JumpDuration;
                SFX.Play("jump");
                StateMachine.Set(StateAir);
                SaveData.Instance.TotalJumps++;
            }
            if (!solidDown)
            {
                if (PlayerData.Footwear.Contains(Powerups.LevitationBoots) && Speed.Y >= 0.0 && pointing.Y <= 0.0)
                {
                    glassWalkTimer -= Engine.DeltaTime * 1.8f;
                    if (glassWalkTimer > 0.0)
                        return;
                    StateMachine.Set(StateAir);
                }
                else
                    StateMachine.Set(StateAir);
            }
            else*/
            //    glassWalkTimer = 1f;
        }
            
        private void SlowEnd()
        {
            //StateMachine.Set(StateNormal);
        }

        private void WeaponlessBegin()
        {
            
        }

        private void WeaponlessUpdate()
        {

        }

        private void WeaponlessEnd()
        {

        }

        public void Explode(Explosion explosion = null)
        {
            if (Invincible > 0.0)
                return;

            float explosionX = explosion?.X ?? 0f;
            float explosionY = explosion?.Y ?? 0f;

            Push = new Vector2(Math.Sign(Position.X - explosionX) * 200,
                Math.Sign(Position.Y - explosionY) * 200);
            TakeDamage(2);
            StateMachine.Set(StateHurt);
        }

        public void Hurt(Actor enemy)
        {
            if (Hidden) return;
            hurtEnemy = enemy as Enemy;
            int num = 20;
            if (Invincible <= 0.0)
            {
                Push.X = ForceDirection(enemy.X) * num;
                Push.Y = -10f;
                StateMachine.Set(StateHurt);
            }
            if (Invincible > 0.0)
                return;
            (enemy as Enemy)?.Knockback(this);
            TakeDamage(1);
        }

        public void TakeDamage(int damage)
        {
            PlayerData.Lives -= damage;
            Engine.Instance.CurrentCamera.Shake(4f, 0.5f);
            Input.Rumble(Input.RumbleStrength.Strong, Input.RumbleLength.Short);
            Invincible = 2f;//PlayerData.Body.Contains(Powerups.EmergencyShield) ? 4f : 2f;
        }

        public void Heal()
        {
            PlayerData.Lives = PlayerData.Lives >= HighestLives ? ++HighestLives : HighestLives;
            if (PlayerData.Lives <= MaxLives)
                return;
            PlayerData.Lives = HighestLives = MaxLives;
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (velocity == Push)
                return;
            if (axis == Hit.Vertical && Speed.Y < 0.0)
            {
                // Корректировка углов при прыжке
                {
                    if (Speed.X <= 0)
                    {
                        for (int i = 1; i <= UpwardCornerCorrection; i++)
                        {
                            if (!MoveCollider.Check(-i, -1, Solids))
                            {
                                Position += new Vector2(-i, -1);
                                return;
                            }
                        }
                    }

                    if (Speed.X >= 0)
                    {
                        for (int i = 1; i <= UpwardCornerCorrection; i++)
                        {
                            if (!MoveCollider.Check(i, -1, Solids))
                            {
                                Position += new Vector2(i, -1);
                                return;
                            }
                        }
                    }
                }
                
                jumpTimer = 0.0f;
            }

            base.HitSolid(axis, ref velocity, collision);
        }

        public override void Render()
        {
            var was = Sprite.ScenePosition;
            //Sprite.ScenePosition = Sprite.ScenePosition.Floor();
            if (Invincible > 0.0)
            {
                Sprite.UpdateBounds();
                Sprite.Alpha = 2f - Invincible;
                base.Render();
            }
            else
                base.Render();
            if (StateMachine.State == StateDead || StateMachine.State == StateLightShow)
                Sprite.Render();

            Sprite.ScenePosition = was;
        }

        public void AnimationGotItem()
        {
            //StateMachine.Set(STATE_GOT_ITEM);
            Sprite.Play("got_item", false);

        }

        public void AnimationGotItemHide(string anim)
        {
            if (anim == "got_item")
            {
                //  StateMachine.Set(STATE_NORMAL);
                Sprite.Play("stand", true);
            }
        }
    }
}