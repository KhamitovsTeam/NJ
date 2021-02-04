using KTEngine;

namespace Chip
{
    public class ExitSign : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/exit"], 15, 7);

        public ExitSign()
            : base(0, 0)
        {
            Depth = 1;

            Add(sprite);
            sprite.Add("idle", 8f, 0);
            sprite.Origin.X = 7f;
            sprite.Origin.Y = 4f;
            sprite.Play("idle");
        }
    }
}
