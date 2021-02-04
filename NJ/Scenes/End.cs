using KTEngine;
using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public class End : Scene
    {
        private readonly Graphic _logo;
        private readonly Animation _ship;
        private readonly Animation _shipFire;

        private Text _gamename = new Text(Fonts.MainFont, Texts.MainText["gamename"], Vector2.One, Constants.Background, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private Text _title = new Text(Fonts.MainFont, Texts.MainText["end_title"], Vector2.One, Constants.Background, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text _version = new Text(Fonts.MainFont, "version", Vector2.One, Constants.LightGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Bottom);
        private readonly Text _copyright = new Text(Fonts.MainFont, Texts.MainText["copyright"], Vector2.One, Constants.LightGreen, Text.HorizontalAlign.Right, Text.VerticalAlign.Bottom);

        private readonly Particles _starsSystem;
        private Vector2 _center;

        private float _timer;

        public End()
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

            _logo.X = (Engine.Instance.Screen.Width / 2f - _logo.Width / 2f);
            _logo.Y = 4f;

            _gamename.X = Engine.Instance.Screen.Width / 2f - _gamename.Width / 2f;
            _gamename.Y = Engine.Instance.Screen.Height / 3f;

            _version.Scale = Vector2.One * 0.5f;
            _copyright.Scale = Vector2.One * 0.5f;

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
            _version.DrawText = "v." + Engine.Instance.Version;
            Music.Play("main_theme");
        }

        public override void Update()
        {
            _starsSystem.Update();
            _center.Y = Utils.Random() * Engine.Instance.Screen.Height;
            _starsSystem.Burst(_center, 1);

            _timer += Engine.DeltaTime * 8f;
            _ship.Y = (float)Math.Sin(Math.Cos(_timer)) + Engine.Instance.Screen.Height / 2f - _ship.Height / 2f;
            _shipFire.Y = (float)Math.Sin(Math.Cos(_timer)) + Engine.Instance.Screen.Height / 2f - _shipFire.Height / 2f + 1f;
            _ship.Update();
            _shipFire.Update();

            if (Input.Pressed("confirm"))
            {
                SFX.Play("menu_click");

                var data = UserIO.Load<SaveData>(SaveData.GetFilename()) ?? new SaveData();
                SaveData.Start(data);

                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
        }

        public override void Render()
        {
            Draw.Begin();
            Draw.Rect(-5, -5, Engine.Instance.Screen.Width + 10, Engine.Instance.Screen.Height + 10, Constants.NormalGreen);
            Draw.End();
            base.Render();
        }

        public override void OverlayRender()
        {
            Draw.BeginOverlay();

            _starsSystem.Render();

            _logo.Render();
            _gamename.Render();

            _shipFire.Render();
            _ship.Render();

            _title.RenderAt(Engine.Instance.Screen.Width / 2f - _title.Width / 2f, Engine.Instance.Screen.Height / 2f + _title.Height + 16f);

            _version.RenderAt(4f, Engine.Instance.Screen.Height - 4f);
            _copyright.RenderAt(Engine.Instance.Screen.Width - 4f, Engine.Instance.Screen.Height - 4f);

            ButtonUI.Render(new Vector2(Engine.Instance.Screen.Width / 2f, Engine.Instance.Screen.Height - Engine.Instance.Screen.Height / 4f), Texts.MainText["pause_restart"], Constants.Background, "confirm", 1f);

            Draw.End();
        }
    }
}