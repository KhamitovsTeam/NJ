using KTEngine;

namespace Chip
{
    public class OverlayComponent
    {
        public bool Open;
        public float Alpha;
        public Level Level;

        protected Entity Navigator;

        public OverlayComponent(Level level)
        {
            Level = level;

            Navigator = new Entity();

            Navigator.Add(new Graphic(GFX.Gui["navigator/panel"]));

            var small_display = new Graphic(GFX.Gui["navigator/small_display_coins_kittens"]);
            small_display.X = Engine.Instance.Screen.Width - 69;
            small_display.Y = 25;
            Navigator.Add(small_display);

            var radar = new Animation(GFX.Gui["navigator/radar"], 59, 56);
            radar.Add("idle", 12f, true, 0, 1, 2, 3, 4, 5);
            radar.Play("idle");
            radar.X = Engine.Instance.Screen.Width - 71;
            radar.Y = 71;
            Navigator.Add(radar);

            var disco = new Animation(GFX.Gui["navigator/disco"], 59, 28);
            disco.Add("idle", 12f, true, 0, 1, 2, 3, 4, 5);
            disco.Play("idle");
            disco.X = 13;
            disco.Y = 70;
            Navigator.Add(disco);
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
            Navigator.Update();
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
            Draw.Rect(0, 0, Engine.Instance.Screen.Width, Engine.Instance.Screen.Height, Constants.DarkGreen);
            Navigator.Render();
        }
    }
}
