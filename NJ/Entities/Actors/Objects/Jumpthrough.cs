using KTEngine;

namespace Chip
{
    public class Jumpthrough : Actor
    {
        public Jumpthrough()
          : base(0, 0)
        {
            MoveCollider = Add(new Hitbox(-8, 0, 16, 4));
        }

        public override void Begin()
        {
            Y -= 8f;
            Collider collider = MoveCollider.Collide((int)Tags.Solid);
            if (collider == null || !(collider.Entity is Wall))
                return;
            Scene.Remove(this);
        }

        public override void Update()
        {
            MoveCollider.Tag((int)Tags.Solid);
            base.Update();
        }
    }
}
