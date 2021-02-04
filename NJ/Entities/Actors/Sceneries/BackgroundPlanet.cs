using KTEngine;

namespace Chip
{
    public class BackgroundPlanet : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/planet"], 30, 28);

        public BackgroundPlanet()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("idle", 15f, 0);
            sprite.Origin.X = 15f;
            sprite.Origin.Y = 14f;
            sprite.Play("idle");

            Depth = 3;
        }

        public override void Update()
        {
            base.Update();
            //if (Player.Instance != null)
            //    Position.X = Calc.LerpSnap(Engine.Instance.CurrentCamera.X + Engine.Instance.CurrentCamera.Width / 2f, 0, 0.01f);
            //Position.X = Player.Instance.Position.X;
            //Speed.X = Player.Instance.Speed.X;
            //Position.X += Player.Instance.Speed.X * Engine.DeltaTime * 0.85f;
        }

        public override void Render()
        {
            // Position.X = Player.Instance.Position.X;
            //Position.X = Player.Instance.Position.X + Player.Instance.Speed.X * 0.85f;
            base.Render();
        }
    }
}
