using KTEngine;

namespace Chip
{
    public class OfficeTumba : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/office_tumba"], 21, 21);

        public OfficeTumba()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0);
            sprite.Origin.X = 11f;
            sprite.Origin.Y = -3f;
            sprite.Play("idle");

            Depth = 2;
        }
    }
}