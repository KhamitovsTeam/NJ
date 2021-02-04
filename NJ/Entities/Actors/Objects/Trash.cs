using KTEngine;

namespace Chip
{
    public class Trash : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/trash"], 32, 32);

        public Trash()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0, 1, 2);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            MoveCollider = Add(new Hitbox(-8, 4, 32, 18));
            MoveCollider.Tag((int)Tags.Solid);

            Depth = -2;
        }
    }
}