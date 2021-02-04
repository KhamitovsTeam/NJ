using KTEngine;

namespace Chip
{
    public class OfficeServers : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/office_servers"], 32, 32);

        public OfficeServers()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 6f, 0, 1, 2, 3);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            MoveCollider = Add(new Hitbox(-8, -8, 32, 32));
            MoveCollider.Tag((int)Tags.Solid);

            Depth = 2;
        }
    }
}