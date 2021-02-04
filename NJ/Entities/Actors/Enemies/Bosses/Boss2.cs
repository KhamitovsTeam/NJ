using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Boss2 : Enemy //WORLD 1: ROOF BOSS
    {
        private readonly Animation sprite;
        private readonly StateMachine state;
        private readonly float walkSpeed;
        private int direction = 1;
        private float timer;

        public Boss2()
        {
            Health = 24;
            Points = 1;
            Depth = 4;
            AvatarPath = "boss_rocket_avatar";


            sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));
            // collider
            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Enemy);

            // states
            state = Add(new StateMachine());
            state.Add("sleep", SleepBegin);
            state.Add("activate", ActivateBegin, ActivateUpdate);
            state.Add("idle", IdleBegin, IdleUpdate);
            state.Add("shoot", ShootBegin, ShootUpdate, ShootEnd);
            state.Add("fall", FallBegin, FallUpdate, FallEnd);
            state.Add("dead");
            state.Set("sleep");

            DeadTimeoutRate = 6f;

            walkSpeed = Utils.Range(64, 64);
        }

        public void Define(Vector2 position)
        {
            Position = position;
            MoveCollider.Reset(-8, -8, 16, 16);
            sprite.Visible = true;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            // Если клетка была сохранена, то не показываем 
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Bosses == null) return;
            foreach (var boss in levelData.Bosses)
            {
                if (ID != boss.ID) continue;
                NeedToRemove = true;
            }
        }

        public override void Update()
        {
            if ((Player.Instance.Position - Position).Length() < 64f && state.State == "sleep")
            {
                state.Set("activate");
            }
            if (MoveCollider.Check(0, 1, (int)Tags.Acid))
                Hurt(Health);
            base.Update();
        }

        private void SleepBegin()
        {
            sprite.Play("sleep");
        }

        private void ActivateBegin()
        {
            Level.HUD.SetEnemy(this);
            sprite.Play("awake");
        }

        private void ActivateUpdate()
        {
            sprite.Play("idle");
        }

        private void IdleBegin()
        {
            timer = 2f;
            direction = -direction;
            sprite.Scale.X = -direction;
            sprite.Play("idle");
        }

        private void IdleUpdate()
        {
            Speed.X = direction * walkSpeed;
            Move();
            timer -= Engine.DeltaTime;
            if (timer <= 0.0)
            {
                state.Set("shoot");
                return;
            }
            if (!MoveCollider.Check(direction * 16, 1, (int)Tags.Solid))
                state.Set("turn");
            if (!MoveCollider.Check(0, 1, (int)Tags.Solid))
                state.Set("fall");
            if ((X > (double)(Room.SceneX + 16) || direction >= 0) &&
                (X < (double)(Room.SceneX + Room.SceneWidth - 16) || direction <= 0))
                return;
            state.Set("turn");
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
            sprite.Play("shoot");
            timer = 0.25f;
        }

        private void ShootUpdate()
        {
            timer -= Engine.DeltaTime;
            if (timer > 0.0)
                return;
            var num = Utils.Choose(-1, 1) * Utils.RangeF(60f, 120f);
            Bomb.Burst(Position, direction, 1);
            direction *= -1;
            state.Set("walk");
        }

        private void ShootEnd()
        {

        }

        public override void DyingBegin()
        {
            MoveCollider.Collidable = false;
            Level.HUD.ClearEnemy();
            state.Set("dead");
        }

        public override void DyingUpdate(float timer)
        {
            DieMutateGrahpic(sprite, timer);
        }

        public override void DyingEnd()
        {
            base.DyingEnd();

            // Добавляем в список поверженных боссов
            if (Level != null)
            {
                var levelData = Level.Session.GetLevelData(Level.ID);
                if (levelData == null)
                {
                    levelData = new LevelData(Level.ID);
                    Level.Session.LevelsData.Add(levelData);
                }
                levelData.Bosses.Add(new EntityID(Level.ID, ID));
            }

            Player.Instance.Speed.X = 0;
            var cut = new CS151_RoofBossKilled
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
            state.Set("turn");
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Horizontal)
                state.Set("turn");
            else if (axis == Hit.Vertical && state.State == "fall")
                state.Set("walk");
            base.HitSolid(axis, ref velocity, collision);
        }

        public override void HurtUpdate(float timer)
        {
            if (state.State == "sleep") return;
            base.HurtUpdate(timer);
            sprite.Alpha = 1f - timer;
        }

        public static void Born(Level level, Room room, Vector2 position, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                Boss2 entity = new Boss2();
                entity.Room = room;
                entity.Define(position);
                entity.Level = level;
                Engine.Scene.Add(entity, "default");
            }
        }
    }
}