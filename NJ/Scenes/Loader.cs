using KTEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace Chip
{
    public class Loader : Scene
    {
        private readonly Text loadingText = new Text(Fonts.MainFont, Texts.MainText["loader_loading"], Vector2.Zero, Constants.Dark, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Animation cat;

        private readonly Session session;
        private Coroutine loadingCoroutine;

        private Level Level { get; set; }

        public Loader(Session session)
        {
            this.session = session ?? new Session();

            cat = new Animation(GFX.Gui["loader_cat"], 8, 8);
            cat.Add("walk", 4f, true, 0, 1, 2, 3);
            cat.Play("walk");
        }

        public override void Begin()
        {
            Level = new Level(session/*, player*/);
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
            cat.Update();
            if (!loadingCoroutine.Finished)
            {
                loadingCoroutine.MaxSteps();
            }
        }

        public override void OverlayRender()
        {
            Draw.BeginOverlay(BlendState.AlphaBlend, SamplerState.PointClamp);
            Draw.Rect(0, 0, Engine.Instance.Screen.Width, Engine.Instance.Screen.Height, Constants.Dark);
            loadingText.X = Engine.Instance.Screen.Width - loadingText.Width - cat.Width * 1.5f;
            loadingText.Y = Engine.Instance.Screen.Height - loadingText.Height - 8f;
            cat.X = loadingText.X + loadingText.Width;
            cat.Y = loadingText.Y - cat.Height / 2f;
            loadingText.Render();
            cat.Render();
            Draw.End();
        }
    }
}