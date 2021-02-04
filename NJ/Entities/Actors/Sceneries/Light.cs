using KTEngine;

namespace Chip
{
    public class Light : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/light"], 19, 19);

        public Light()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0, 1);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            Depth = 4;
        }
    }
}