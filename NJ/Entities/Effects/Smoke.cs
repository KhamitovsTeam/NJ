using KTEngine;
using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public class Smoke : Entity
    {
        private Vector2 speed;
        private const float duration = 0.5f;
        private Graphic graphic;
        private float timer;

        public Smoke()
            : base(0, 0)
        {
            graphic = Add(new Graphic(GFX.Game["effects/smoke"],
                new Rectangle(16 * Utils.Range(0, 4), 0, 16, 16)));
            graphic.CenterOrigin();
        }

        public void Define(float x, float y, float angle)
        {
            X = x;
            Y = y;
            speed.X = (float)Math.Cos(angle) * 40f;
            speed.Y = (float)(-Math.Sin(angle) * 40.0);
            timer = 0.5f;
            graphic.Rotation = Utils.RangeF(0.0f, Calc.TAU);
        }

        public override void Update()
        {
            base.Update();
            X += (float)(speed.X * (double)Engine.DeltaTime);
            Y += (float)(speed.Y * (double)Engine.DeltaTime);
            timer -= Engine.DeltaTime;
            if (timer <= 0.0)
            {
                Scene.Remove(this);
                Cache.Store(this);
            }
            graphic.Scale = Vector2.One * (float)(0.5 + timer / 0.5 * 0.5);
        }

        public static void Burst(float x, float y, float range, float angle, float angleRange, int count = 1)
        {
            for (int index = 0; index < count; ++index)
            {
                Smoke entity = Cache.Create<Smoke>();
                entity.Define(x + Utils.RangeF(-range, range), y + Utils.RangeF(-range, range),
                    angle + Utils.RangeF(-angleRange, angleRange));
                Engine.Scene.Add(entity, "default");
            }
        }
    }
}