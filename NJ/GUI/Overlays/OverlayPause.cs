using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class OverlayPause : OverlayComponent
    {
        public static OverlayPause Instance;

        private readonly string[] items = {
            Texts.MainText["pause_continue"],
            Texts.MainText["pause_restart"],
            Texts.MainText["pause_quit"]
        };

        private readonly Text text = new Text(Fonts.MainFont, "option", Vector2.One, Constants.Dark, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);

        private int index;
        private readonly float maxItemWidth;

        public OverlayPause(Level level)
            : base(level)
        {
            Instance = this;
            foreach (var item in items)
            {
                var size = Fonts.MeasureString(item).X;
                if (maxItemWidth < size)
                    maxItemWidth = size;
            }
        }

        public override void Hide()
        {
            base.Hide();
            Level.IsGamePaused = false;
        }

        public override void Update()
        {
            if (Input.Pressed("up"))
            {
                --index;
                SFX.Play("menu_choose");
            }
            if (Input.Pressed("down"))
            {
                ++index;
                SFX.Play("menu_choose");
            }
            if (index >= items.Length)
                index = 0;
            if (index < 0)
                index = items.Length - 1;
            if (Alpha > 0.0)
            {
                if (!Input.Pressed("confirm"))
                {
                    if (!Input.Pressed("cancel"))
                        goto update;
                }
                Hide();
                if (Input.Pressed("confirm") && index != 0)
                {
                    SFX.Play("menu_click");
                    switch (index)
                    {
                        case 1:
                            Engine.Scene = new Loader(Level.Session);
                            break;
                        case 2:
#if !__IOS__ && !__TVOS__
                            Engine.Instance.Exit();
#endif
                            break;
                    }
                }
            }
            update:
            base.Update();
        }

        public override void Render()
        {
            Draw.SetOffset(Engine.Instance.CurrentCamera.Render);

            base.Render();

            for (var i = 0; i < items.Length; ++i)
            {
                text.DrawText = (index == i ? ">> " : "  ") + items[i];
                text.Color = Constants.Light;
                text.Position.X = Engine.Instance.CurrentCamera.Render.X + 2;
                text.Position.Y = Engine.Instance.CurrentCamera.Render.Y + (Engine.Instance.Screen.Height - items.Length * 14f) / 2 + i * 14f;
                text.Alpha = Alpha;
                text.Render();
            }

            Draw.ResetOffset();
        }
    }
}
