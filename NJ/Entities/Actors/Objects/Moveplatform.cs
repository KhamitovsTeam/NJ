using KTEngine;

namespace Chip
{
    public class Movedoor : Actor
    {
        public Animation Sprite => sprite;
        private Animation sprite = new Animation(GFX.Game["objects/movedoor"], 32, 16);

        public Movedoor()
        {
            Add(sprite);
            sprite.CenterOrigin();
            sprite.Add("idle", 0.0f, 0);
            sprite.Add("work", 10f, 0, 1, 2, 3, 4, 4, 4, 4, 4, 4);
            sprite.Origin.Y = 11f;
            sprite.Origin.X = 8f;

            // collider
            MoveCollider = Add(new Hitbox(-8, -8, 32, 14));
            MoveCollider.Tag((int)Tags.Solid);

            sprite.Play("work");
        }

    }
}