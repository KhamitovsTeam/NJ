using KTEngine;

namespace Chip
{
    public class Waterfallbottom : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/waterfallbottom"], 16, 16);

        public Waterfallbottom()
            : base(0, 0)
        {
            Depth = -2;

            Add(sprite);
            sprite.Add("idle", 8f, 0, 1, 2, 3, 4);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Sceneries);
        }
    }
}
