using KTEngine;

namespace Chip
{
    public class Cond : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/cond"], 128, 64);

        public Cond()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0, 1);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            Depth = 2;
        }
    }
}