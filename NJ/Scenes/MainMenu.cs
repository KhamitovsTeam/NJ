using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class MainMenu : Scene
    {
        private SaveData savedData;
        private int index;
        private readonly float maxItemWidth;
        private readonly Text text = new Text(Fonts.MainFont, "option", Vector2.One, Constants.Dark, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly bool canContinue;
        private int firstIndex;

        private readonly string[] items = {
            Texts.MainText["menu_continue"],
            Texts.MainText["menu_new_game"],
            Texts.MainText["menu_quit"]
        };

        public MainMenu()
        {
            savedData = UserIO.Load<SaveData>(SaveData.GetFilename());
            canContinue = savedData != null;

            firstIndex = canContinue ? 0 : 1;
            index = firstIndex;

            foreach (var item in items)
            {
                var size = Fonts.MeasureString(">> " + item).X;
                if (maxItemWidth < size)
                    maxItemWidth = size;
            }
        }

        public override void Update()
        {
            firstIndex = canContinue ? 0 : 1;
            if (Input.Pressed("up"))
            {
                SFX.Play("menu_choose");
                --index;
            }
            if (Input.Pressed("down"))
            {
                SFX.Play("menu_choose");
                ++index;
            }
            if (index >= items.Length)
                index = canContinue ? 0 : 1;
            if (index < firstIndex)
                index = items.Length - 1;
            if (Input.Pressed("confirm"))
            {
                SFX.Play("menu_click");
                switch (index)
                {
                    case 0:
                        if (canContinue)
                            ContinueGame();
                        break;
                    case 1:
                        NewGame();
                        break;
                    case 2:
                        QuitGame();
                        break;
                    default:
                        Log.Error("WTF?");
                        break;
                }
            }
            base.Update();
        }

        public override void OverlayRender()
        {
            Draw.BeginOverlay();
            for (var i = 0; i < items.Length; ++i)
            {
                text.DrawText = (index == i ? ">> " : "  ") + items[i];
                text.Color = Constants.Light;
                text.Position.X = Engine.Instance.CurrentCamera.Render.X + 2;
                text.Position.Y = Engine.Instance.CurrentCamera.Render.Y + (Engine.Instance.Screen.Height - items.Length * 14f) / 2 + i * 14f;
                text.Render();
            }
            Draw.End();
        }

        public override void Render()
        {
            Draw.Begin();
            Draw.Rect(0, 0, Engine.Instance.Screen.Width, Engine.Instance.Screen.Height, Constants.Dark);
            Draw.End();
            base.Render();
        }

        private void NewGame()
        {
            SaveData.Start(new SaveData());
            Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
        }

        private void ContinueGame()
        {
            if (savedData == null)
                savedData = new SaveData();
            SaveData.Start(savedData);
            Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
        }

        private void QuitGame()
        {
#if !__IOS__ && !__TVOS__
            Engine.Instance.Exit();
#endif
        }
    }
}