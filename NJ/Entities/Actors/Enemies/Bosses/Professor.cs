using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Professor : Enemy
    {
        private readonly Animation sprite;
        private readonly StateMachine state;

        public Professor()
        {
            Health = 3;
            Points = 1;
            AvatarPath = "professor_avatar";

            sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));

            // collider
            MoveCollider = Add(new Hitbox(-13, -12, 26, 52));
            MoveCollider.Tag((int)Tags.Enemy);

            // states
            state = Add(new StateMachine());
            state.Add("idle", SleepBegin);
            state.Add("evil", ActivateBegin, ActivateUpdate);
            state.Add("dead");
            state.Set("idle");

            DeadTimeoutRate = 6f;
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
            if ((Player.Instance.Position - Position).Length() < 74f && state.State == "idle")
            {
                state.Set("evil");
            }
            if (MoveCollider.Check(0, 1, (int)Tags.Acid))
                Hurt(Health);
            base.Update();
        }

        private void SleepBegin()
        {
            sprite.Play("idle");
        }

        private void ActivateBegin()
        {
            Level.HUD.SetEnemy(this);
            sprite.Play("evil");
        }

        private void ActivateUpdate()
        {

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
            var cut = new CS122_BossKilled
            {
                Level = Engine.Scene as Level
            };
            cut.Start();
            Engine.Scene.Add(cut);
        }

        public override void HurtUpdate(float timer)
        {
            if (state.State == "idle") return;
            base.HurtUpdate(timer);
            sprite.Alpha = 1f - timer;
        }
    }
}