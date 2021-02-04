using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections;

namespace Chip
{
    public class Title : Scene
    {
        private readonly Graphic _logo;
        private readonly Animation _ship;
        private readonly Animation _shipFire;
        private MenuStates _menuState;

        private readonly Text _gamename = new Text(Fonts.MainFont, Texts.MainText["gamename"], Vector2.One, Constants.LightGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text _menu = new Text(Fonts.MainFont, "menu", Vector2.One, Constants.LightGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text version = new Text(Fonts.MainFont, "version", Vector2.One, Constants.LightGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Bottom);
        private readonly Text copyright = new Text(Fonts.MainFont, Texts.MainText["copyright"], Vector2.One, Constants.LightGreen, Text.HorizontalAlign.Right, Text.VerticalAlign.Bottom);

        private readonly Particles _starsSystem;
        private Vector2 _center;

        private float _timer;
        private float _timer_logo;

        private Coroutine _coroutine;

        public Title()
        {
            _logo = new Graphic(GFX.Misc["logo"]);
            _ship = new Animation(GFX.Misc["ship"], 32, 16);
            _ship.Add("idle", 8f, true, 0, 1, 2, 3, 3);
            _ship.X = Engine.Instance.Screen.Width / 2f + _ship.Width / 2f;
            _ship.Y = Engine.Instance.Screen.Height / 2f - _ship.Height / 2f;
            _ship.Play("idle");

            _shipFire = new Animation(GFX.Misc["ship_fire"], 16, 16);
            _shipFire.Add("fire", 20f, true, 0, 1, 2, 3, 4);
            _shipFire.X = Engine.Instance.Screen.Width / 2f - _ship.Width / 2f + 3f;
            _shipFire.Y = Engine.Instance.Screen.Height / 2f - _shipFire.Height / 2f + 1f;
            _shipFire.Play("fire");

            _ship.Scale.X *= -1;
            _shipFire.Scale.X *= -1;

            _menu.Scale = Vector2.One;

            _logo.X = (Engine.Instance.Screen.Width / 2f - _logo.Width / 2f);
            _logo.Y = 4f;

            _gamename.X = Engine.Instance.Screen.Width / 2f - _gamename.Width / 2f;
            _gamename.Y = Engine.Instance.Screen.Height / 3f;

            _starsSystem = new Particles
            {
                Preset = ParticlePresets.Stars
            };

            _center = new Vector2(Engine.Instance.Screen.Width, Engine.Instance.Screen.Height / 2f);

            var entity = new Entity();
            entity.Position = _center;
            Add(entity);
        }

        public override void Begin()
        {
            Engine.Instance.CurrentCamera.Position = Vector2.Zero;
            version.DrawText = "v." + Engine.Instance.Version;
            Music.Play("main_theme");

            _menuState = MenuStates.Title;
        }

        public override void Update()
        {
            _coroutine?.Update();

            _starsSystem.Update();
            _center.Y = Utils.Random() * Engine.Instance.Screen.Height;
            _starsSystem.Burst(_center, 1);

            _timer += Engine.DeltaTime * 8f;
            _timer_logo += Engine.DeltaTime * 3f;
            _logo.Y = (float)Math.Sin(Math.Cos(_timer_logo)) + 5f;
            _ship.Y = (float)Math.Sin(Math.Cos(_timer)) + Engine.Instance.Screen.Height / 2f - _ship.Height / 2f;
            _shipFire.Y = (float)Math.Sin(Math.Cos(_timer)) + Engine.Instance.Screen.Height / 2f - _shipFire.Height / 2f + 1f;
            _ship.Update();
            _shipFire.Update();
            switch (_menuState)
            {
                case MenuStates.Title:
                    if (Input.Pressed("confirm"))
                    {
                        SFX.Play("menu_click");
#if DEBUG && !CONSOLE && !__MOBILE__
                        SaveData data = UserIO.Load<SaveData>(SaveData.GetFilename()) ?? new SaveData();
                        SaveData.Start(data);
#else
                        SaveData.Start(new SaveData());
#endif
                        Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
                        //_coroutine = new Coroutine(ShowMenu(), true);
                    }
                    break;
                case MenuStates.Menu:
                    if (Input.Pressed("cancel"))
                    {
                        _coroutine = new Coroutine(HideMenu(), true);
                    }


                    break;
                case MenuStates.StartGame:
                    if (Input.Pressed("confirm"))
                    {
                        SFX.Play("menu_click");
                        //Engine.Scene = new Loader(new Session());
                    }
                    break;
            }
        }

        public override void OverlayRender()
        {
            Draw.BeginOverlay();
            Draw.Rect(0, 0, Engine.Instance.Screen.Width, Engine.Instance.Screen.Height, Constants.NormalGreen);

            if (_menuState == MenuStates.Title)
            {
                _starsSystem.Render();

                //_chip.Render();
                _logo.Render();
                _gamename.Render();

                _shipFire.Render();
                _ship.Render();

                version.RenderAt(4f, Engine.Instance.Screen.Height - 4f);
                copyright.RenderAt(Engine.Instance.Screen.Width - 4f, Engine.Instance.Screen.Height - 4f);

                ButtonUI.Render(new Vector2(Engine.Instance.Screen.Width / 2f, Engine.Instance.Screen.Height - Engine.Instance.Screen.Height / 4f), Texts.MainText["menu_start"], Constants.Background, "confirm", 1f);
                //ButtonUI.Render(new Vector2(Engine.Instance.Screen.Width / 2f - 48f, Engine.Instance.Screen.Height / 2f + 48), Texts.MainText["menu_jump"], Constants.DarkGreen, "jump", 1f);
                //ButtonUI.Render(new Vector2(Engine.Instance.Screen.Width / 2f + 30f, Engine.Instance.Screen.Height / 2f + 48), Texts.MainText["menu_shoot"], Constants.DarkGreen, "shoot", 1f);
            }
            else if (_menuState == MenuStates.Menu)
            {
                _starsSystem.Render();

                _gamename.Render();

                _shipFire.Render();
                _ship.Render();

                version.RenderAt(4f, Engine.Instance.Screen.Height - 4f);
                copyright.RenderAt(Engine.Instance.Screen.Width - 4f, Engine.Instance.Screen.Height - 4f);


                //ButtonUI.Render(new Vector2(_confirmButtonPadding, Engine.Instance.Screen.Height - _padding*1.25f), Texts.MainText["ok"], Constants.DarkGreen, "confirm", 1f);
                //ButtonUI.Render(new Vector2(_cancelButtonPadding, Engine.Instance.Screen.Height - _padding*1.25f), Texts.MainText["cancel"], Constants.DarkGreen, "cancel", 1f);

                //ButtonUI.Render(new Vector2(Engine.Instance.Screen.Width / 2f, Engine.Instance.Screen.Height / 2f + 22f), Texts.MainText["menu_start"], Constants.DarkGreen, "confirm", 1f);
                //ButtonUI.Render(new Vector2(Engine.Instance.Screen.Width / 2f - 48f, Engine.Instance.Screen.Height / 2f + 48), Texts.MainText["menu_jump"], Constants.DarkGreen, "jump", 1f);
                //ButtonUI.Render(new Vector2(Engine.Instance.Screen.Width / 2f + 30f, Engine.Instance.Screen.Height / 2f + 48), Texts.MainText["menu_shoot"], Constants.DarkGreen, "shoot", 1f);
            }
            else if (_menuState == MenuStates.StartGame)
            {

            }

            Draw.End();
        }

        /*private IEnumerator ShowMenu()
        {
            var ease = Ease.Linear;

            Vector2 targetTitle = new Vector2(_gamename.X, _gamename.Position.Y - 10);
            Vector2 fromTitle = _gamename.Position;
            float p = 0.0f;
            while (p < 1.0)
            {
                _ship.Alpha = 1 - p;
                _shipFire.Alpha = 1 - p;

                _menuAlpha = p;
                
                _gamename.Position = fromTitle + (targetTitle - fromTitle) * ease(p);
                p += Engine.DeltaTime * 5f;
                yield return 0f;
            }
            
            _ship.Alpha = 0;
            _shipFire.Alpha = 0;
            
            _menuAlpha = 1;
            
            _menuState = MenuStates.Menu;
        }*/

        private IEnumerator HideMenu()
        {
            var ease = Ease.Linear;

            Vector2 targetTitle = new Vector2(_gamename.X, _gamename.Position.Y + 10);
            Vector2 fromTitle = _gamename.Position;
            float p = 0.0f;
            while (p < 1.0)
            {
                _ship.Alpha = p;
                _shipFire.Alpha = p;

                //_menuAlpha = 1 - p;

                _gamename.Position = fromTitle + (targetTitle - fromTitle) * ease(p);
                p += Engine.DeltaTime * 5f;
                yield return 0f;
            }

            _ship.Alpha = 1;
            _shipFire.Alpha = 1;

            //_menuAlpha = 0;

            _menuState = MenuStates.Title;
        }

        internal enum MenuStates
        {
            Title,
            Menu,
            StartGame
        }
    }
}