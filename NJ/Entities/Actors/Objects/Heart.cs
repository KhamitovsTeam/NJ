using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Heart : Actor
    {
        private Animation sprite = new Animation(GFX.Game["objects/heart"], 16, 16);

        public Heart()
            : base(0, 0)
        {
            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Item);
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            /*
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Coins != null)
            {
                foreach (var coin in levelData.Coins)
                {
                    if (ID == coin.ID) return;
                }
            }*/

            Add(sprite);
            sprite.Add("idle", 8f, 0, 1, 2, 3, 4, 5);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            Depth = -2;
        }

        public override void Update()
        {
            if (MoveCollider != null && MoveCollider.Check((int)Tags.Player))
            {
                SFX.Play("coin");
                Scene.Remove(this);
                Player.Instance.PlayerData.Lives = 9;

                /*// Добавляем в список монет
                var levelData = Level.Session.GetLevelData(Level.ID);
                if (levelData == null)
                {
                    levelData = new LevelData(Level.ID);
                    Level.Session.LevelsData.Add(levelData);
                }
                levelData.Coins.Add(new EntityID(Level.ID, ID));*/
            }
            base.Update();
        }
    }
}
