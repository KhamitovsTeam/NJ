using KTEngine;

namespace Chip
{
    public class Window : Actor
    {
        private Graphic _sprite;
        //    private Hitbox hitbox;

        public Window()
        {
            _sprite = Add(new Graphic(GFX.Game["objects/window"]));
            _sprite.CenterOrigin();
            MoveCollider = Add(new Hitbox(-8, -8, 6, 16));
            MoveCollider.Tag((int)Tags.Solid);
            Depth = -10;
        }
    }
}