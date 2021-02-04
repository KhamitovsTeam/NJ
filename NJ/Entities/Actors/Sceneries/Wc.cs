using KTEngine;

namespace Chip
{
    public class Wc : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/rocket/wc"], 32, 16);

        public Wc()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0, 1, 2, 3, 2, 1, 1, 0, 1, 2, 3, 1);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            Depth = 1;
        }
    }
}