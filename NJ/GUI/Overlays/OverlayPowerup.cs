
using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class OverlayPowerup : OverlayComponent
    {
        public static OverlayPowerup Instance;
        public Powerup Powerup;
        public string Title;

        private int coins;  // how many coins was selected
        private int kittens;  //  how many kittens was selected
        private readonly Text title = new Text(Fonts.MainFont, "title", Vector2.One, Constants.Dark);
        private readonly Text _text = new Text(Fonts.MainFont, "powerup", Vector2.One, Constants.Dark, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text _coinstext = new Text(Fonts.MainFont, "option", Vector2.One, Constants.Dark, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text _kittenstext = new Text(Fonts.MainFont, "option", Vector2.One, Constants.Dark, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        public Graphic icon;

        private int _padding = 170;
        private float _confirmButtonPadding;

        public OverlayPowerup(Level level)
            : base(level)
        {
            Instance = this;
            _confirmButtonPadding = Engine.Instance.Screen.Width - ButtonUI.Width(Texts.MainText["close"], "cancel") / 2f - 4f;
        }

        public override void Show()
        {
            base.Show();
            Level.IsGamePaused = true;
            if (Player.Instance != null)
            {
                coins = Player.Instance.PlayerData.Coins;
                kittens = Player.Instance.PlayerData.Kittens;
            }
        }

        public override void Hide()
        {
            base.Hide();
            Level.IsGamePaused = false;
        }

        public override void Closed()
        {
            base.Closed();
            SFX.Play("menu_click");
        }

        public override void Update()
        {
            if (!Input.Pressed("cancel"))
                goto update;
            Hide();
        update:
            base.Update();
        }

        public override void Render()
        {
            Draw.SetOffset(Engine.Instance.CurrentCamera.Render);

            base.Render();

            title.DrawText = Calc.WrapText(Fonts.MainFont, Title, Engine.Instance.Screen.Width - _padding);
            title.Color = Constants.Dark;
            title.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width / 2f;
            title.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 30 + title.Height;
            title.Alpha = Alpha;
            title.Render();

            icon = Powerup.Icon;

            _text.DrawText = Calc.WrapText(Fonts.MainFont, Powerup.Description, Engine.Instance.Screen.Width - _padding);
            _text.Color = Constants.Dark;
            _text.Position.X = Engine.Instance.CurrentCamera.Render.X + (Engine.Instance.Screen.Width - _text.Width) / 2f;// + icon.Width / 2f;
            _text.Position.Y = Engine.Instance.CurrentCamera.Render.Y + Engine.Instance.Screen.Height / 2f - _text.Height / 2f;
            _text.Alpha = Alpha;
            _text.Render();

            icon.Position.X = icon.Width / 2f;
            icon.Position.Y = Engine.Instance.Screen.Height / 2f - icon.Height / 2f;
            //icon.Render();

            ButtonUI.Render(new Vector2(_confirmButtonPadding, Engine.Instance.Screen.Height - 32 * 0.4f), Texts.MainText["close"], Constants.Dark, "cancel", 1f);

            if (kittens == 0)
                _kittenstext.DrawText = "000";
            else
            {
                _kittenstext.DrawText = kittens.ToString();
                while (_kittenstext.DrawText.Length < 3)
                    _kittenstext.DrawText = "0" + _kittenstext.DrawText;
            }
            _kittenstext.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            _kittenstext.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 35;
            _kittenstext.Render();

            if (coins == 0)
                _coinstext.DrawText = "000";
            else
            {
                _coinstext.DrawText = coins.ToString();
                while (_coinstext.DrawText.Length < 3)
                    _coinstext.DrawText = "0" + _coinstext.DrawText;
            }
            _coinstext.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            _coinstext.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 50;
            _coinstext.Render();

            Draw.ResetOffset();

            //Renderer.Rect(title.X, title.Y + title.Height, title.Width - 9, 1f, Constants.DarkGreen, 1f);
        }
    }
}
