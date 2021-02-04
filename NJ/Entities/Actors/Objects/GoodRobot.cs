using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class GoodRobot : Actor
    {
        private Animation sprite = new Animation(GFX.Game["objects/good_robot"], 16, 16);
        private Animation sprite_cloud = new Animation(GFX.Game["sceneries/message_cloud"], 16, 16);
        private bool _nearPlayer;

        public GoodRobot()
            : base(0, 0)
        {
            Depth = 0;

            Add(sprite);
            sprite.Add("idle", 8f, 0, 1, 2, 3, 4, 5);
            sprite.Add("speak", 8f, 6, 7, 8, 9);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            MoveCollider = Add(new Hitbox(-5, -4, 10, 12));
            MoveCollider.Tag((int)Tags.Sceneries);

            Add(sprite_cloud);
            sprite_cloud.Add("idle", 4f, 0, 1);
            sprite_cloud.Origin.X = 5f;
            sprite_cloud.Origin.Y = 24f;
            sprite_cloud.Play("idle");
            sprite_cloud.Visible = false;
           // sprite_cloud.Stop();
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            // Если котик был сохранён, то не показываем 
            /*  var levelData = Level.Session.GetLevelData(Level.ID);
              if (levelData?.Kittens == null) return;
              foreach (var kitten in levelData.Kittens)
              {
                  if (ID != kitten.ID) continue;
                  sprite.Play("empty");
                  MoveCollider.Collidable = false;
              }*/
        }

        public override void Update()
        {
            base.Update();

            _nearPlayer = (Player.Instance.Position - Position).Length() < 32f;

            if (_nearPlayer)
            {
                sprite.Play("speak");
                sprite_cloud.Visible = true;
            }
            else
            {
                sprite.Play("idle");
                sprite_cloud.Visible = false;
            }
        }

        public override void Render()
        {

            base.Render();
            sprite.Render();
        }

    }
}
