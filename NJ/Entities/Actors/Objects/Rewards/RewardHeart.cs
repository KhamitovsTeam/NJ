using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    class RewardHeart: Actor
    {
        private Animation sprite = new Animation(GFX.Game["objects/heart"], 16, 16);

        public RewardHeart()
            : base(0, 0)
        {
            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Item);

            Add(sprite);
            sprite.Add("idle", 8f, 0, 1, 2, 3, 4, 5);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            Depth = -2;
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

           
        }
        public void Define(Vector2 position)
        {
            Position = position;
            MoveCollider.Reset(-8, -8, 16, 16);
            sprite.Visible = true;
        }

        public override void Update()
        {
            if (MoveCollider != null && MoveCollider.Check((int)Tags.Player))
            {
                SFX.Play("coin");
                Scene.Remove(this);
                Player.Instance.PlayerData.Lives = 9;

                // TODO сохранить статус собранного сердца
            }
            base.Update();
        }

        public static void Born(Room room, Vector2 position, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                RewardHeart entity = new RewardHeart();
                entity.Room = room;
                entity.Define(new Vector2(position.X, position.Y - 2));
                Engine.Scene.Add(entity, "default");
            }
        }
    }
}
