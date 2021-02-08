using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Torch : Actor
    {
        private Animation sprite = new Animation(GFX.Game["objects/torch"], 8, 16);

        public Torch()
            : base(0, 0)
        {

        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            X = offset.X;
            Y = offset.Y + 6;

            Add(sprite);
            sprite.Add("shine", 20f, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
            sprite.Add("empty", 8f, 17);
            sprite.Origin.X = 4f;
            sprite.Origin.Y = 8f;
            sprite.Play("empty");

            MoveCollider = Add(new Hitbox(0, 0, 12, 7));
            MoveCollider.Tag((int)Tags.Item);

            // Если звезда была сохранёна, то делаем её пунктирной 
           /* var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Stars == null) return;
            foreach (var star in levelData.Stars)
            {
                if (ID != star.ID) continue;
                sprite.Play("empty");
                MoveCollider = null;
                break;
            }*/
        }

        public override void Update()
        {
            if (MoveCollider != null && MoveCollider.Check((int)Tags.Player))
            {
                if (Player.Instance.PlayerData.HasFire)
                {
                    sprite.Play("shine");
                }
                SFX.Play("star");
            }
            base.Update();
        }
    }
}
