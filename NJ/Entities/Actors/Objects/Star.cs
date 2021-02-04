using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Star : Actor
    {
        private Animation sprite = new Animation(GFX.Game["objects/star"], 16, 16);

        public Star()
            : base(0, 0)
        {

        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            Add(sprite);
            sprite.Add("shine", 8f, 0, 1, 2, 3);
            sprite.Add("empty", 8f, 4);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("shine");

            MoveCollider = Add(new Hitbox(-6, -8, 12, 16));
            MoveCollider.Tag((int)Tags.Item);

            // Если звезда была сохранёна, то делаем её пунктирной 
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Stars == null) return;
            foreach (var star in levelData.Stars)
            {
                if (ID != star.ID) continue;
                sprite.Play("empty");
                MoveCollider = null;
                break;
            }
        }

        public override void Update()
        {
            if (MoveCollider != null && MoveCollider.Check((int)Tags.Player))
            {
                // Добавляем в список звёзд
                var levelData = Level.Session.GetLevelData(Level.ID);
                if (levelData == null)
                {
                    levelData = new LevelData(Level.ID);
                    Level.Session.LevelsData.Add(levelData);
                }
                levelData.Stars.Add(new EntityID(Level.ID, ID));
                Scene.Remove(this);
                Player.Instance.PlayerData.Stars += 1;
                Player.Instance.PlayerData.Coins += 10;
                Player.Instance.StarHintVisible();
                SFX.Play("star");
            }
            base.Update();
        }
    }
}
