using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class ProgressBar : Component
    {
        public float Timer;
        public float Duration;
        public float Width;
        public float Height;
        public Callback OnComplete;

        public bool Running
        {
            get { return Active; }
        }

        public bool Finished
        {
            get { return Timer >= (double)Duration; }
        }

        public ProgressBar(float x, float y, float width, float height, float duration, Callback complete)
        {
            Position = new Vector2(x, y);
            Width = width;
            Height = height;
            Timer = 0.0f;
            Duration = duration;
            Active = false;
            Visible = true;
            OnComplete = complete;
        }

        public void Start()
        {
            Active = true;
            Timer = 0.0f;
        }

        public void Resume()
        {
            Active = true;
        }

        public void Pause()
        {
            Active = false;
        }

        public void Stop()
        {
            Active = false;
            Timer = 0.0f;
        }

        public override void Update()
        {
            if (Timer >= (double)Duration)
                return;
            Timer += Engine.DeltaTime;
            if (Timer < (double)Duration)
                return;
            Timer = Duration;
            if (OnComplete == null)
                return;
            OnComplete();
        }

        public override void Render()
        {
            if (Timer == 0.0)
                return;
            Draw.Rect(ScenePosition.X - Width / 2f, ScenePosition.Y - Height / 2f, Width,
                Height, Color.White, 1f);
            Draw.Rect((float)(ScenePosition.X - Width / 2.0 + 1.0),
                (float)(ScenePosition.Y - Height / 2.0 + 1.0), Width - 2f,
                Height - 2f, Color.Black, 1f);
            Draw.Rect((float)(ScenePosition.X - Width / 2.0 + 1.0),
                (float)(ScenePosition.Y - Height / 2.0 + 1.0),
                (float)((Width - 2.0) * (Timer / (double)Duration)),
                Height - 2f, Color.White, 1f);
        }

        public delegate void Callback();
    }
}