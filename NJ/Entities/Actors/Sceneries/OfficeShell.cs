using KTEngine;

namespace Chip
{
    public class OfficeShell : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/office_shell"], 19, 40);

        public OfficeShell()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0);
            sprite.Origin.X = 10f;
            sprite.Origin.Y = 0f;
            sprite.Play("idle");

            Depth = 2;
        }
    }
}