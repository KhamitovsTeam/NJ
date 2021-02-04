using KTEngine;

namespace Chip
{
    public class Elevator : Actor
    {
        public Animation Sprite => sprite;
        private Animation sprite = new Animation(GFX.Game["objects/moveplatform"], 16, 16);

        public Elevator()
        {
            Add(sprite);
            sprite.CenterOrigin();
            sprite.Add("idle", 0.0f, 0);
            sprite.Add("work", 10f, 0, 1, 2, 3);
            sprite.Origin.Y = 17f;

            // collider
            MoveCollider = Add(new Hitbox(-8, -8, 16, 8));
            MoveCollider.Tag((int)Tags.Solid);

            sprite.Play("work");
        }
    }
}