using KTEngine;

namespace Chip
{
    public class OverlayComponent
    {
        public bool Open;
        public float Alpha;
        public Level Level;

        public OverlayComponent(Level level)
        {
            Level = level;
        }

        public virtual void Show()
        {
            Open = true;
            if (Level != null)
                Level.OverlayComponent = this;
        }

        public virtual void Hide()
        {
            if (!Open)
                return;
            Open = false;
        }

        public virtual void Closed()
        {
            if (Level != null)
                Level.OverlayComponent = null;
        }

        public virtual void Update()
        {
            if (Open)
            {
                if (Alpha >= 1.0)
                    return;
                Alpha = 1;
            }
            else
            {
                if (Alpha <= 0.0)
                    return;
                Alpha = 0;
                if (Alpha > 0.0)
                    return;
                Closed();
            }
        }

        public virtual void Render()
        {
            Draw.Rect(0, 0, Engine.Instance.Screen.Width, Engine.Instance.Screen.Height, Constants.Dark);
        }
    }
}
