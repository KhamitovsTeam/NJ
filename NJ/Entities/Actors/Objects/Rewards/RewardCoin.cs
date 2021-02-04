using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class RewardCoin : Actor
    {
        private Animation sprite = new Animation(GFX.Game["objects/coins"], 16, 16);

        public RewardCoin()
            : base(0, 0)
        {
            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Reward);

            Add(sprite);
            sprite.Add("shine", 8f, 0, 1, 2, 3);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("shine");

            Depth = -2;

            Fall(3000f); // падение монет
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            // Если монетка была сохранёна, то не показываем 
            /*var levelData = Level.Session.GetLevelData(Level.ID);
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
                Player.Instance.PlayerData.Coins += 1;
                Player.Instance.CoinHintVisible();

                // TODO сохранить статус собранной монеты

            }
            Move();
            base.Update();
        }

        public static void Born(Room room, Vector2 position, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                RewardCoin entity = new RewardCoin();
                entity.Room = room;
                entity.Define(new Vector2(position.X, position.Y - 1));
                Engine.Scene.Add(entity, "default");
            }
        }
    }
}
