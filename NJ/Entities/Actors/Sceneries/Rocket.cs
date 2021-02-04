using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Rocket : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/rocket"], 16, 32);

        //    private bool _animationPlayed = true;

        public Rocket()
            : base(0, 0)
        {
            Depth = 3;

            MoveCollider = Add(new Hitbox(-8, -16, 16, 32));
            MoveCollider.Tag((int)Tags.Rocket);
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            Add(sprite);
            sprite.Add("open", 16f, false, 0, 1, 2, 3);
            sprite.Add("close", 16f, false, 3, 2, 1, 0);

            sprite.Add("idleOpen", 1f, false, 3);
            sprite.Add("idleClose", 1f, false, 0);

            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idleOpen");

            sprite.OnFinish += animation =>
            {
                switch (animation)
                {
                    case "close":
                        sprite.Play("idleClose");
                        break;
                    case "open":
                        sprite.Play("idleOpen");
                        break;
                }
            };
        }

        public override void Update()
        {
            base.Update();
            if ((Player.Instance.Position - Position).Length() < 64.0)
            {
                if (sprite.CurrentAnimationID.Equals("idleClose"))
                    sprite.Play("open");
            }
            else
            {
                if (sprite.CurrentAnimationID.Equals("idleOpen"))
                    sprite.Play("close");
            }
        }
    }
}
