using KTEngine;
using Microsoft.Xna.Framework;

namespace NJ
{
    public class Player : Actor
    {
        public const float Acceleration = 3000f;
        public const float Maxspeed = 320f;
        
        public Animation Sprite;
        public Hitbox Collider;
        public Movement Movement;
        
        public Player()
        {
            var graphic = Add(new GraphicRect(8, 16, Color.Aqua));
            graphic.CenterOrigin();
            
            Movement = Add(new Movement(Collider, new int[1]));
        }

        public override void Update()
        {
            base.Update();
        }
    }
}