using KTEngine;

namespace Chip
{
    public class OfficeChair : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/office_chair"], 18, 26);

        public OfficeChair()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0);
            sprite.Origin.X = 9f;
            sprite.Origin.Y = 2f;
            sprite.Play("idle");

            Depth = 1;
        }
    }
}