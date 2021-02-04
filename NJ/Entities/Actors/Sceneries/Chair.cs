using KTEngine;

namespace Chip
{
    public class Chair : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/rocket/chair"], 8, 12);

        public Chair()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0);
            sprite.Origin.X = 4f;
            sprite.Origin.Y = 4f;
            sprite.Play("idle");

            Depth = 1;
        }
    }
}