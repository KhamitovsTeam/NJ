using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Blocker : Actor
    {
        //public Animation Sprite => sprite;
        private int height;
       

        public Blocker()
        {
            
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            height = xml.AttrInt("height");

            int offsetY = 0;
            while (offsetY < height)
            {
                Animation sprite = new Animation(GFX.Game["objects/blocker"], 16, 16);
                
                sprite.CenterOrigin();
                sprite.Add("idle", 0.0f, 0);
                sprite.Origin.Y = - offsetY + 8f;
                sprite.Origin.X = 8f;
                Add(sprite);
                sprite.Play("idle");
                offsetY += 16;
            }
            

            MoveCollider = Add(new Hitbox(-8, -8, 16, height));
            MoveCollider.Tag((int)Tags.Solid);

        }

    }
}