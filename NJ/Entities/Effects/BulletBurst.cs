using KTEngine;

namespace Chip
{
    public class BulletBurst : Entity
    {
        private Animation sprite;

        public BulletBurst()
            : base(0, 0)
        {
            sprite = Add(new Animation(GFX.Game["effects/bulletburst"], 2, 6, Clear));
            sprite.CenterOrigin();
            sprite.Add("play", 15f, false, 0, 1, 2);
        }

        public void Define(float x, float y)
        {
            X = x;
            Y = y;
            sprite.Play("play", true);
            Visible = true;
        }

        public void Clear(string anim)
        {
            Visible = false;
            Scene.Remove(this);
            Cache.Store(this);
        }

        public static void Burst(float x, float y)
        {
            BulletBurst entity = Cache.Create<BulletBurst>();
            entity.Define(x, y);
            Engine.Scene.Add(entity, "default");
        }
    }
}