using KTEngine;

namespace Chip
{
    public class Door : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/rocket/door"], 16, 16);

        public Door()
            : base(0, 0)
        {
            Depth = 1;

            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Door);

            Add(sprite);
            sprite.Add("idle", 2f, 0, 1);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");
        }
    }
}