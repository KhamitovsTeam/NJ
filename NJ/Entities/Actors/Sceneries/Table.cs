using KTEngine;

namespace Chip
{
    public class Table : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/rocket/table"], 16, 18);

        public Table()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 8f, 0, 1, 2, 3);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 10f;
            sprite.Play("idle");

            Depth = 2;
        }
    }
}