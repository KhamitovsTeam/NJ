using KTEngine;

namespace Chip
{
    public class OfficePlant : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/office_plant"], 16, 30);

        public OfficePlant()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 5f;
            sprite.Play("idle");

            Depth = 2;
        }
    }
}