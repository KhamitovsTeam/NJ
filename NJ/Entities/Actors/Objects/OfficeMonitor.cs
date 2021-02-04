using KTEngine;

namespace Chip
{
    public class OfficeMonitor : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/office_monitor"], 136, 96);

        public OfficeMonitor()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 6f, 0, 1, 2, 3, 4, 5);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");


            Depth = 2;
        }
    }
}