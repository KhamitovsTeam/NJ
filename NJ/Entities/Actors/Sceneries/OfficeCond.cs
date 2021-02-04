using KTEngine;

namespace Chip
{
    public class OfficeCond : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/office_cond"], 32, 16);

        public OfficeCond()
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