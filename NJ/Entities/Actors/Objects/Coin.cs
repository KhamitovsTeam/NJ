using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Coin : Actor
    {
        private Animation sprite = new Animation(GFX.Game["objects/coin"], 8, 9);

        public Coin()
            : base(0, 0)
        {
            MoveCollider = Add(new Hitbox(-4, -4, 8, 9));
            MoveCollider.Tag((int)Tags.Item);


            Add(sprite);
            sprite.Add("shine", 8f, 0, 1, 2, 3);
            sprite.Origin.X = 4f;
            sprite.Origin.Y = 4f;
            sprite.Play("shine");

            Depth = -2;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            // Если монетка была сохранёна, то не показываем 
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Coins != null)
            {
                foreach (var coin in levelData.Coins)
                {
                    if (ID == coin.ID) return;
                }
            }

        }

        public override void Update()
        {
            if (MoveCollider != null && MoveCollider.Check((int)Tags.Player))
            {
                SFX.Play("coin");
                Scene.Remove(this);
                Player.Instance.PlayerData.Coins += 1;

                // Добавляем в список монет
                var levelData = Level.Session.GetLevelData(Level.ID);
                if (levelData == null)
                {
                    levelData = new LevelData(Level.ID);
                    Level.Session.LevelsData.Add(levelData);
                }
                levelData.Coins.Add(new EntityID(Level.ID, ID));
            }
            base.Update();
        }
    }
}
