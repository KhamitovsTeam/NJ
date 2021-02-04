using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class OverlayPause : OverlayComponent
    {
        public static OverlayPause Instance;

        private readonly string[] _items = {
            Texts.MainText["pause_continue"],
            Texts.MainText["pause_restart"],
            Texts.MainText["pause_main_menu"],
            Texts.MainText["pause_quit"]
        };

        private int _coins;
        private int _kittens;
        private readonly Text _pauseTitle = new Text(Fonts.MainFont, Texts.MainText["pause"], Vector2.One, Constants.DarkGreen);
        private readonly Text _text = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text _coinstext = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text _kittenstext = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);

        private int _index;
        private readonly float _maxItemWidth;
        private const int Padding = 32;

        private readonly float _confirmButtonPadding;
        private readonly float _cancelButtonPadding;

        public OverlayPause(Level level)
            : base(level)
        {
            Instance = this;
            foreach (var item in _items)
            {
                var size = Fonts.MeasureString(item).X;
                if (_maxItemWidth < size)
                    _maxItemWidth = size;
            }

            _cancelButtonPadding = Engine.Instance.Screen.Width - ButtonUI.Width(Texts.MainText["cancel"], "cancel") / 2f - 10f;
            _confirmButtonPadding = Engine.Instance.Screen.Width - ButtonUI.Width(Texts.MainText["cancel"], "cancel") - ButtonUI.Width(Texts.MainText["ok"], "confirm") - 10f;
        }

        public override void Show()
        {
            base.Show();
            if (Player.Instance != null)
            {
                _coins = Player.Instance.PlayerData.Coins;
                _kittens = Player.Instance.PlayerData.Kittens;
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
                --_index;
                SFX.Play("menu_choose");
            }
            if (Input.Pressed("down"))
            {
                ++_index;
                SFX.Play("menu_choose");
            }
            if (_index >= _items.Length)
                _index = 0;
            if (_index < 0)
                _index = _items.Length - 1;
            if (Alpha > 0.0)
            {
                if (!Input.Pressed("confirm"))
                {
                    if (!Input.Pressed("cancel"))
                        goto update;
                }
                Hide();
                if (Input.Pressed("confirm") && _index != 0)
                {
                    SFX.Play("menu_click");
                    switch (_index)
                    {
                        case 1:
                            Engine.Scene = new Loader(Level.Session);
                            break;
                        case 2:
                            Engine.Scene = new MainMenu();
                            break;
                        case 3:
                            Engine.Instance.Exit();
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

            _pauseTitle.DrawText = Calc.WrapText(Fonts.MainFont, Texts.MainText["pause_pause"], Engine.Instance.Screen.Width - Padding);
            _pauseTitle.Color = Constants.Background;
            _pauseTitle.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width / 2f;
            _pauseTitle.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 30 + _pauseTitle.Height;
            _pauseTitle.Alpha = Alpha;
            _pauseTitle.Render();

            for (var i = 0; i < _items.Length; ++i)
            {
                _text.DrawText = (_index == i ? ">> " : "  ") + _items[i];
                _text.Color = Constants.Background;
                _text.Position.X = Engine.Instance.CurrentCamera.Render.X + (Engine.Instance.Screen.Width - _maxItemWidth) / 2;
                _text.Position.Y = Engine.Instance.CurrentCamera.Render.Y + (Engine.Instance.Screen.Height - _items.Length * 14f) / 2 + i * 14f;
                _text.Alpha = Alpha;
                _text.Render();
            }

            ButtonUI.Render(new Vector2(_confirmButtonPadding, Engine.Instance.Screen.Height - Padding * 0.4f), Texts.MainText["ok"], Constants.Background, "confirm", 1f);
            ButtonUI.Render(new Vector2(_cancelButtonPadding, Engine.Instance.Screen.Height - Padding * 0.4f), Texts.MainText["cancel"], Constants.Background, "cancel", 1f);

            if (_kittens == 0)
                _kittenstext.DrawText = "000";
            else
            {
                _kittenstext.DrawText = _kittens.ToString();
                while (_kittenstext.DrawText.Length < 3)
                    _kittenstext.DrawText = "0" + _kittenstext.DrawText;
            }
            _kittenstext.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            _kittenstext.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 35;
            _kittenstext.Render();

            if (_coins == 0)
                _coinstext.DrawText = "000";
            else
            {
                _coinstext.DrawText = _coins.ToString();
                while (_coinstext.DrawText.Length < 3)
                    _coinstext.DrawText = "0" + _coinstext.DrawText;
            }
            _coinstext.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            _coinstext.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 50;
            _coinstext.Render();

            Draw.ResetOffset();

            //Renderer.Rect(_pauseTitle.Position.X, _pauseTitle.Position.Y + _pauseTitle.Height, _pauseTitle.Width - 9, 1f, Constants.Background, 1f);
        }
    }
}
