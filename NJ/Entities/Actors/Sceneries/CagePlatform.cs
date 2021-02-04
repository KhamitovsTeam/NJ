using KTEngine;

namespace Chip
{
    public class CagePlatform : Actor
    {
        private Animation sprite = new Animation(GFX.Game["objects/cage_platform"], 16, 16);

        public CagePlatform()
            : base(0, 0)
        {
            Depth = -1;

            Add(sprite);
            sprite.Add("idle", 8f, 0);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            //MoveCollider = Add(new Circle(8));
            MoveCollider.Tag((int)Tags.Solid);
        }
    }
}
