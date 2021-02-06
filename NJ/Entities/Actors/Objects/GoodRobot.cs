using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class GoodRobot : Actor
    {
        private Animation sprite = new Animation(GFX.Game["objects/good_robot"], 6, 8);
        private Animation sprite_cloud = new Animation(GFX.Game["sceneries/message_cloud"], 4, 4);
        private bool _nearPlayer;

        public GoodRobot()
            : base(0, 0)
        {
            Depth = 0;

            Add(sprite);
            sprite.Add("idle", 6f, 0, 1, 2);
            sprite.Add("speak", 6f, 0, 1);
            sprite.Origin.X = 3f;
            sprite.Origin.Y = 4f;
            sprite.Play("idle");

            MoveCollider = Add(new Hitbox(-3, -4, 6, 8));
            MoveCollider.Tag((int)Tags.Sceneries);

            Add(sprite_cloud);
            sprite_cloud.Add("idle", 8f, true, 0, 1, 2);
            sprite_cloud.Origin.X = 0f;
            sprite_cloud.Origin.Y = 9f;
            sprite_cloud.Play("idle");
            sprite_cloud.Visible = false;
            sprite_cloud.Stop();
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            X = offset.X;
            Y = offset.Y + 2;

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

            _nearPlayer = (Player.Instance.Position - Position).Length() < 8f;

            if (_nearPlayer)
            {
                sprite.Play("speak");
                sprite_cloud.Visible = true;
                sprite_cloud.Play("idle");
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
