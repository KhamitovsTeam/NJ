using KTEngine;
using System;

namespace Chip
{
    public class OfficeTable : Actor
    {
        private float amp;
        private Animation sprite = new Animation(GFX.Game["sceneries/office_table"], 42, 44);

        private string idle_anim = "work";

        public OfficeTable()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 4f, 0, 1, 2, 3);
            sprite.Add("work", 4f, 6, 7, 8); //second idle animation
            sprite.Add("off", 0f, 9); //not using
            sprite.Add("turn_on", 0f, 10, 11); //not using
            sprite.Add("noise", 8f, 4, 5);
            sprite.Origin.X = 16f;
            sprite.Origin.Y = 4f;

            
            switch (Rand.Instance.Next(0, 3))
            {
                case 0:
                    idle_anim = "idle";
                    break;
                case 1:
                    idle_anim = "work";
                    break;
                default:
                    idle_anim = "off";
                    break;
            }
            
            sprite.Play(idle_anim);

            MoveCollider = Add(new Hitbox(-16, -4, 42, 44));

            Depth = 2;
        }

        public override void Update()
        {
            if (Math.Abs(amp) < 5.0)
            {
                Collider collider = MoveCollider.Collide(new int[1] { (int)Tags.PlayerBullet });
                if (collider != null && Math.Abs(((Actor)collider.Entity).Speed.X) > 16.0)
                    sprite.Play("noise");
                else
                    sprite.Play(idle_anim);
            }
            int num = Math.Sign(amp);
            amp -= num * Engine.DeltaTime;
            if (Math.Sign(amp) != num)
                amp = 0.0f;
            base.Update();
        }
    }
}