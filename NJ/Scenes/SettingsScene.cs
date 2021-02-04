using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class SettingsScene : Scene
    {
        private readonly float _maxItemWidth;
        private readonly Text _text = new Text(Fonts.MainFont, "option", Vector2.One, Constants.Background, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text _coinstext = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text _kittenstext = new Text(Fonts.MainFont, "option", Vector2.One, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);

        private int _coins;
        private int _kittens;
        private int _index;
        private float _closeButtonPadding;
        private int _firstIndex;
        private int _currentLangPosition;
        private int _currentVolume;
        //private int _currentWindow;

        // Поддерживаемые языки
        private string[] _supportedLanguages;
        // Элементы меню
        private string[] _items;

        public SettingsScene()
        {
            _supportedLanguages = new[] { "en", "ru" };

            var currentLang = Settings.Instance.Language;
            for (var i = 0; i < _supportedLanguages.Length; i++)
            {
                if (_supportedLanguages[i].Equals(currentLang))
                {
                    _currentLangPosition = i;
                }
            }

            _currentVolume = Settings.Instance.MusicVolume;

            UpdateItems();

            if (Player.Instance != null)
            {
                _coins = Player.Instance.PlayerData.Coins;
                _kittens = Player.Instance.PlayerData.Kittens;
            }
            _firstIndex = 0;
            _index = _firstIndex;

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

            foreach (var item in _items)
            {
                var size = Fonts.MeasureString("< " + item + " >").X;
                if (_maxItemWidth < size)
                    _maxItemWidth = size;
            }
        }

        public override void Update()
        {
            _firstIndex = 0;
            if (Input.Pressed("up"))
            {
                SFX.Play("menu_choose");
                _index -= 2;
            }
            if (Input.Pressed("down"))
            {
                SFX.Play("menu_choose");
                _index += 2;
            }
            if (_index >= _items.Length)
                _index = 0;
            if (_index < _firstIndex)
                _index = _items.Length - 2;

            if (Input.Pressed("left"))
            {
                SFX.Play("menu_click");
                switch (_index)
                {
                    case 0:
                        ChangeLanguageLeft();
                        break;
                    case 2:
                        ChangeVolumeLeft();
                        break;
                    case 4:
                        ChangeWindowLeft();
                        break;
                    case 6:
                        ChangeFullscreen();
                        break;
                }
            }

            if (Input.Pressed("right"))
            {
                SFX.Play("menu_click");
                switch (_index)
                {
                    case 0:
                        ChangeLanguageRight();
                        break;
                    case 2:
                        ChangeVolumeRight();
                        break;
                    case 4:
                        ChangeWindowRight();
                        break;
                    case 6:
                        ChangeFullscreen();
                        break;
                }
            }

            if (Input.Pressed("cancel"))
            {
                Engine.Scene = new MainMenu();
            }
            base.Update();
        }

        public override void OverlayRender()
        {
            Draw.BeginOverlay();
            for (var i = 0; i < _items.Length; ++i)
            {
                if (i == _index)
                {
                    // Текущий заголовок
                    _text.DrawText = ">> " + _items[i];
                    _text.Color = Constants.Background;
                }
                else if (i == _index + 1)
                {
                    // Текущий элемент для изменения
                    _text.DrawText = " <" + _items[i] + ">";
                    _text.Color = Constants.NormalGreen;
                }
                else
                {
                    // Остальные элементы
                    _text.DrawText = "  " + _items[i];
                    _text.Color = i % 2 == 0 ? Constants.Background : Constants.NormalGreen;
                }

                _text.Position.X = Engine.Instance.CurrentCamera.Render.X +
                                   (Engine.Instance.Screen.Width - _maxItemWidth) / 2;
                _text.Position.Y = Engine.Instance.CurrentCamera.Render.Y +
                                   (Engine.Instance.Screen.Height - _items.Length * 14f) / 2 + i * 14f;
                _text.Render();
            }

            ButtonUI.Render(new Vector2(_closeButtonPadding, Engine.Instance.Screen.Height - 13f), Texts.MainText["close"], Constants.Background, "cancel", 1f);

            if (_kittens == 0)
                _kittenstext.DrawText = "???";
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
                _coinstext.DrawText = "???";
            else
            {
                _coinstext.DrawText = _coins.ToString();
                while (_coinstext.DrawText.Length < 3)
                    _coinstext.DrawText = "0" + _coinstext.DrawText;
            }
            _coinstext.Position.X = Engine.Instance.CurrentCamera.Render.X + Engine.Instance.Screen.Width - 48;
            _coinstext.Position.Y = Engine.Instance.CurrentCamera.Render.Y + 50;
            _coinstext.Render();

            Draw.End();
        }

        public override void Render()
        {
            Draw.Begin();
            Draw.Rect(0, 0, Engine.Instance.Screen.Width, Engine.Instance.Screen.Height, Constants.DarkGreen);
            Draw.End();
            base.Render();
        }

        private void UpdateItems()
        {
            _items = new[]
            {
                Texts.MainText["settings_language"],
                Texts.MainText["lang_name"],
                Texts.MainText["settings_volume"],
                Settings.Instance.MusicVolume.ToString(),
                Texts.MainText["settings_display"],
                Texts.MainText["settings_window"] + Settings.Instance.WindowScale,
                Texts.MainText["settings_fullscreen"],
                Settings.Instance.Fullscreen ? Texts.MainText["on"] : Texts.MainText["off"]
            };

            UpdateCloseButton();
        }

        private void UpdateCloseButton()
        {
            _closeButtonPadding = Engine.Instance.Screen.Width - ButtonUI.Width(Texts.MainText["close"], "cancel") / 2f - 8f;
        }

        private void ChangeVolumeLeft()
        {
            _currentVolume--;
            if (_currentVolume <= 0)
            {
                _currentVolume = 0;
            }
            Settings.Instance.MusicVolume = _currentVolume;
            Settings.Instance.SFXVolume = _currentVolume;
            Music.Volume = _currentVolume / 10f;
            Sounds.Volume = _currentVolume / 10f;
            UserIO.SaveHandler(false, true, UpdateItems, false);
        }

        private void ChangeVolumeRight()
        {
            _currentVolume++;
            if (_currentVolume >= 10)
            {
                _currentVolume = 10;
            }
            Settings.Instance.MusicVolume = _currentVolume;
            Settings.Instance.SFXVolume = _currentVolume;
            Music.Volume = _currentVolume / 10f;
            Sounds.Volume = _currentVolume / 10f;
            UserIO.SaveHandler(false, true, UpdateItems, false);
        }

        private void ChangeWindowLeft()
        {

        }

        private void ChangeWindowRight()
        {

        }

        private void ChangeLanguageLeft()
        {
            _currentLangPosition--;
            if (_currentLangPosition <= 0)
            {
                _currentLangPosition = 0;
            }
            Settings.Instance.Language = _supportedLanguages[_currentLangPosition];
            UserIO.SaveHandler(false, true, () =>
            {
                Localization.SetLang(Settings.Instance.Language);
                Texts.Load(Localization.Lang);
                UpdateItems();
            }, false);
        }

        private void ChangeLanguageRight()
        {
            _currentLangPosition++;
            if (_currentLangPosition >= _supportedLanguages.Length - 1)
            {
                _currentLangPosition = _supportedLanguages.Length - 1;
            }
            Settings.Instance.Language = _supportedLanguages[_currentLangPosition];
            UserIO.SaveHandler(false, true, () =>
            {
                Localization.SetLang(Settings.Instance.Language);
                Texts.Load(Localization.Lang);
                UpdateItems();
            }, false);
        }

        private void ChangeFullscreen()
        {
            Settings.Instance.Fullscreen = !Settings.Instance.Fullscreen;
            UpdateItems();
            Engine.Instance.Screen.Fullscreen = Settings.Instance.Fullscreen;
            Engine.Graphics.ApplyChanges();
            UserIO.SaveHandler(false, true, null, false);
        }
    }
}