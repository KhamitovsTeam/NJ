using KTEngine;

namespace Chip
{
    public class Shest : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/rocket/shest"], 16, 16);

        public Shest()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            Depth = 1;
        }
    }
}