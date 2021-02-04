using KTEngine;

namespace Chip
{
    public class Bed : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/rocket/bed"], 23, 18);

        public Bed()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0);
            sprite.Origin.X = 7f;
            sprite.Origin.Y = 10f;
            sprite.Play("idle");

            Depth = 1;
        }
    }
}