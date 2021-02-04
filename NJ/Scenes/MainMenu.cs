using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class MainMenu : Scene
    {
        private SaveData savedData;
        private int coins;
        private int kittens;
        private int index;
        private readonly float confirmButtonPadding;
        private readonly float maxItemWidth;
        private readonly Text text = new Text(Fonts.MainFont, "option", Vector2.One, Constants.LightGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text coinstext = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text kittenstext = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly bool canContinue;
        private int firstIndex;

        private readonly string[] items = {
            Texts.MainText["menu_continue"],
            Texts.MainText["menu_new_game"],
            Texts.MainText["menu_settings"],
            Texts.MainText["menu_credits"],
            Texts.MainText["menu_quit"]
        };

        public MainMenu()
        {
            if (Player.Instance != null)
            {
                coins = Player.Instance.PlayerData.Coins;
                kittens = Player.Instance.PlayerData.Kittens;
            }

            savedData = UserIO.Load<SaveData>(SaveData.GetFilename());
            canContinue = savedData != null;

            firstIndex = canContinue ? 0 : 1;
            index = firstIndex;

            var navigator = Add(new Entity());
            navigator.Add(new Graphic(GFX.Gui["navigator/panel"]));

            var smallDisplay = new Graphic(GFX.Gui["navigator/small_display_coins_kittens"]);
            smallDisplay.X = Engine.Instance.Screen.Width - 69;
            smallDisplay.Y = 25;
            navigator.Add(smallDisplay);

            var radar = new Animation(GFX.Gui["navigator/radar"], 59, 56);
            radar.Add("idle", 12f, true, 0, 1, 2, 3, 4, 5);
            radar.Play("idle");
            radar.X = Engine.Instance.Screen.Width - 71;
            radar.Y = 71;
            navigator.Add(radar);

            var disco = new Animation(GFX.Gui["navigator/disco"], 59, 28);
            disco.Add("idle", 12f, true, 0, 1, 2, 3, 4, 5);
            disco.Play("idle");
            disco.X = 13;
            disco.Y = 70;
            navigator.Add(disco);

            foreach (var item in items)
            {
                var size = Fonts.MeasureString(">> " + item).X;
                if (maxItemWidth < size)
                    maxItemWidth = size;
            }

            confirmButtonPadding = Engine.Instance.Screen.Width - ButtonUI.Width(Texts.MainText["confirm"], "confirm") / 2f - 8f;
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
                        SettingsScreen();
                        break;
                    case 3:
                        CreditsScreen();
                        break;
                    case 4:
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
                if (i == 0)
                {
                    text.Color = canContinue ? Constants.Background : Constants.NormalGreen;
                }
                else
                {
                    text.Color = Constants.Background;
                }
                text.Position.X = Engine.Instance.CurrentCamera.Render.X + (Engine.Instance.Screen.Width - maxItemWidth) / 2;
                text.Position.Y = Engine.Instance.CurrentCamera.Render.Y + (Engine.Instance.Screen.Height - items.Length * 14f) / 2 + i * 14f;
                text.Render();
            }

            ButtonUI.Render(new Vector2(confirmButtonPadding, Engine.Instance.Screen.Height - 13f), Texts.MainText["confirm"], Constants.Background, "confirm", 1f);

            if (kittens == 0)
                kittenstext.DrawText = "???";
            else
            {
                kittenstext.DrawText = kittens.ToString();
                while (kittenstext.DrawText.Length < 3)
                    kittenstext.DrawText = "0" + kittenstext.DrawText;
            }
            kittenstext.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            kittenstext.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 35;
            kittenstext.Render();

            if (coins == 0)
                coinstext.DrawText = "???";
            else
            {
                coinstext.DrawText = coins.ToString();
                while (coinstext.DrawText.Length < 3)
                    coinstext.DrawText = "0" + coinstext.DrawText;
            }
            coinstext.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            coinstext.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 50;
            coinstext.Render();

            Draw.End();
        }

        public override void Render()
        {
            Draw.Begin();
            Draw.Rect(0, 0, Engine.Instance.Screen.Width, Engine.Instance.Screen.Height, Constants.DarkGreen);
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

        private void SettingsScreen()
        {
            Engine.Scene = new SettingsScene();
        }

        private void CreditsScreen()
        {
            Engine.Scene = new Credits();
        }

        private void QuitGame()
        {
#if !__IOS__ && !__TVOS__
            Engine.Instance.Exit();
#endif
        }
    }
}