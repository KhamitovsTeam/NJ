using KTEngine;

namespace Chip
{
    public class Doorjamb : Actor
    {
        private Graphic _sprite;
        //    private Hitbox hitbox;

        public Doorjamb()
        {
            _sprite = Add(new Graphic(GFX.Game["objects/doorjamb"]));
            _sprite.CenterOrigin();
            MoveCollider = Add(new Hitbox(-8, -8, 6, 16));
            Depth = -10;
        }
    }
}