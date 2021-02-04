using KTEngine;

namespace Chip
{
    public class OfficeCooler : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/office_cooler"], 16, 37);

        public OfficeCooler()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0, 0, 0, 0, 0, 0, 1, 2, 3);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 13f;
            sprite.Play("idle");

            Depth = 2;
        }
    }
}