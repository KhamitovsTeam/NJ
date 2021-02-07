using KTEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace Chip
{
    public class Loader : Scene
    {
        private readonly Animation sprite;
        private readonly Session session;
        private Coroutine loadingCoroutine;

        private Level Level { get; set; }

        public Loader(Session session)
        {
            this.session = session ?? new Session();
            sprite = new Animation(GFX.Gui["loader_cat"], 8, 8);
            sprite.Add("walk", 4f, true, 0, 1, 2, 3);
            sprite.Play("walk");
        }

        public override void Begin()
        {
            Level = new Level(session);
            Engine.Instance.CurrentCamera.Position = Vector2.Zero;
            loadingCoroutine = new Coroutine(IntroRoutine(), true);
        }

        private IEnumerator IntroRoutine()
        {
            Level.Session = session;
            SaveData.Instance.StartSession(Level.Session);
            yield return Level.Create();
            StartLevel();
            yield return 0f;
        }

        private void StartLevel()
        {
            Engine.Instance.IsFixedTimeStep = true;
            if (Engine.Scene != this)
                return;
            Engine.Scene = Level;
        }

        public override void Update()
        {
            base.Update();
            sprite.Update();
            if (!loadingCoroutine.Finished)
            {
                loadingCoroutine.MaxSteps();
            }
        }

        public override void OverlayRender()
        {
            Draw.BeginOverlay(BlendState.AlphaBlend, SamplerState.PointClamp);
            Draw.Rect(0, 0, Engine.Instance.Screen.Width, Engine.Instance.Screen.Height, Constants.Dark);
            sprite.X = Engine.Instance.Screen.Width - sprite.Width - 2f;
            sprite.Y = Engine.Instance.Screen.Height - sprite.Height - 2f;
            sprite.Render();
            Draw.End();
        }
    }
}