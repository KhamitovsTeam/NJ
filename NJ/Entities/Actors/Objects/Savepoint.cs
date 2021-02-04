using System.Collections;
using System.Xml;
using Microsoft.Xna.Framework;
using KTEngine;

namespace Chip
{
    public class Savepoint : Actor
    {
        private readonly Coroutine savingCoroutine;
        private readonly Animation sprite = new Animation(GFX.Game["objects/savepoint"], 11, 16);        

        public Savepoint()
        {
            Depth = 1;

            MoveCollider = Add(new Hitbox(-6, -8, 11, 16));
            MoveCollider.Tag((int) Tags.SavePoint);
            
            savingCoroutine = new Coroutine(false);
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);    

            Add(sprite);
            sprite.Add("idle", 4f, 0, 1);
            sprite.Add("open", 12f, 2, 3);
            sprite.Add("close", 12f, 3, 2);
            sprite.Add("saving", 9f, 4, 5, 6, 7, 8, 9);

            sprite.Origin.X = 6f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            sprite.OnFinish += animation =>
            {
                switch (animation)
                {
                    case "close":
                        sprite.Play("idle");
                        break;
                    case "open":
                        sprite.Play("saving");
                        break;
                }
            };
        }

        public override void Update()
        {
            base.Update();
            
            if (savingCoroutine != null && !savingCoroutine.Finished && savingCoroutine.Active)
            {
                savingCoroutine.MaxSteps();
            }
            
            if ((Player.Instance.Position - Position).Length() < 24.0)
            {
                if (!sprite.CurrentAnimationID.Equals("idle")) return;
                
                SaveData.Instance.CurrentSession.ToLevel = Level.ID;
                SaveData.Instance.CurrentSession.FromLevel = Level.ID;
                SaveData.Instance.CurrentSession.LastCheckpoint = new Vector2(Position.X - Player.Instance.Sprite.Width / 2f, 
                    Position.Y - Player.Instance.Sprite.Height / 2f);
                
                savingCoroutine?.Add(SavingRoutine());
                sprite.Play("open");
            }
            else
            {
                if (sprite.CurrentAnimationID.Equals("saving"))
                    sprite.Play("close");
            }

        }

        private IEnumerator SavingRoutine()
        {
            UserIO.SaveHandler(true, false);
            yield return 0f;
        }
    }
}
