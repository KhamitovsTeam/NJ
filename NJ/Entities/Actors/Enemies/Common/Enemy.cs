using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public abstract class Enemy : Actor, IShootable, IExplodable
    {
        public enum Rewards
        {
            None,
            Coins10,
            Kitten,
            Heart
        }
        public int Health = 1;
        public int Points = 1;
        public bool Explodable = true;
        public int Dust = 4;
        public bool Stompable = true;
        public string AvatarPath = "default_avatar";
        public bool Invincible;
        public bool Dead;
        public Rewards reward = Rewards.None;

        internal float HurtTimeout = 1f;
        internal float HurtTimeoutRate = 4f;
        internal float DeadTimeoutRate = 4f;
        internal float HurtTimer;
        internal float DeadTimer;
        internal bool DeadTimeout;

        public Particles Particles;

        public Enemy()
            : base(0, 0)
        {
            Depth = -10;
            Particles = Add(new Particles());
            Particles.Preset = ParticlePresets.Piece;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            X = offset.X;
            Y = offset.Y + 2;

            if (xml.HasAttr("reward"))
            {
                reward = (Rewards)xml.AttrInt("reward");
            }
        }

        public override void Begin()
        {
            Solids = new int[1] { (int)Tags.Solid };
            base.Begin();
        }

        public override void Update()
        {
            base.Update();
            if (Level != null)
            {
                var room = Level.GetRoomFromPoint(Position);
                if (Room.ID != room.ID)
                {
                    Room = room;
                    Tag = "room" + Room.ID;
                }
            }

            if (DeadTimeout)
            {
                DyingUpdate(DeadTimer);
                DeadTimer -= Engine.DeltaTime * DeadTimeoutRate;
                if (DeadTimer <= 0.0)
                    DyingEnd();
            }
            else if (HurtTimer > 0.0)
            {
                HurtTimer -= Engine.DeltaTime * HurtTimeoutRate;
                HurtUpdate(HurtTimer / HurtTimeout);
                if (HurtTimer <= 0.0)
                    HurtEnd();
            }
            if (Room == null || Dead)
                return;
            ClampHorizontallyToRoom();
        }

        public void ClampHorizontallyToRoom()
        {
            if (X < (double)Room.SceneX)
                X = Room.SceneX;
            if (X <= (double)(Room.SceneX + Room.SceneWidth))
                return;
            X = Room.SceneX + Room.SceneWidth;
        }

        public void Revive(int health)
        {
            Health = health;
            Dead = false;
            HurtTimer = 0.0f;
            DeadTimer = 0.0f;
            DeadTimeout = false;
        }

        public void Kill()
        {
            if (MoveCollider != null)
                MoveCollider.Collidable = false;
            Health = 0;
            DeadTimeout = true;
            DeadTimer = 1f;
            DyingBegin();
        }

        public virtual void Knockback(Entity from)
        {
        }

        public virtual void Explode(Explosion explosion = null)
        {
            Kill();
        }

        public virtual void Hurt(int damage = 0, Entity from = null)
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
                HurtBegin(from);
        }

        public void DieMutateGrahpic(Graphic graphic, float timer)
        {
            //graphic.Scale.X = Math.Sign(graphic.Scale.X) * (float) (1.0 + (1.0 - timer) * 0.5);
            //graphic.Scale.Y = Math.Abs(graphic.Scale.X);
            //graphic.Color = Color.Lerp(Color.White, Color.PaleVioletRed, 1f - timer);
        }

        public void HurtMutateGraphic(Graphic graphic, float timer)
        {
            //graphic.Scale.X = Math.Sign(graphic.Scale.X) * (float) (1.0 + timer * 0.5);
            //graphic.Scale.Y = Math.Abs(graphic.Scale.X);
            //graphic.Color = Color.Lerp(Color.White, Color.PaleVioletRed, 1f - timer);
        }

        public virtual void HurtBegin(Entity from)
        {
        }

        public virtual void HurtUpdate(float timer)
        {
        }

        public virtual void HurtEnd()
        {
        }

        public virtual void DyingBegin()
        {

        }

        public virtual void DyingUpdate(float timer)
        {
        }

        public virtual void DyingEnd()
        {
            if (Dead)
                return;
            Dead = true;
            Engine.Instance.CurrentCamera.Shake(2f, 0.3f);
            Input.Rumble(Input.RumbleStrength.Strong, Input.RumbleLength.Medium);
            Scene.Remove(this);
            Smoke.Burst(X, Y, 8f, 0.0f, Calc.TAU, Dust);
            ExplodePieces.Burst(Position, 30);
            SFX.Play("enemy_death");
            CreateReward();
        }

        private void CreateReward()
        {
            if (reward == Rewards.Coins10)
            {
                RewardCoin.Born(Room, Position, 10);
            }
            else if (reward == Rewards.Heart)
            {
                RewardHeart.Born(Room, Position, 1);
            }
            else if (reward == Rewards.Kitten)
            {
                RewardKitten.Born(Room, Position);
            }
        }
    }
}