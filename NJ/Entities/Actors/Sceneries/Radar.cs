using KTEngine;

namespace Chip
{
    public class Radar : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/rocket/radar"], 16, 16);

        public Radar()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0, 1, 2, 3);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            Depth = 1;
        }
    }
}