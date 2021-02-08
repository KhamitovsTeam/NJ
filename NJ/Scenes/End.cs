using KTEngine;
using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public class End : Scene
    {
        private readonly Text gamename = new Text(Fonts.MainFont, Texts.MainText["gamename"], Vector2.One, Constants.Dark, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        private readonly Text gameover = new Text(Fonts.MainFont, Texts.MainText["end_title"], Vector2.One, Constants.Dark, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        
        public End()
        {
            gamename.X = Engine.Instance.Screen.Width / 2f - gamename.Width / 2f;
            gamename.Y = 2f;
            
            gameover.X = Engine.Instance.Screen.Width / 2f - gameover.Width / 2f;
            gameover.Y = gamename.Y + gamename.Height + 2f;
        }

        public override void Begin()
        {
            Engine.Instance.CurrentCamera.Position = Vector2.Zero;
            Music.Play("main_theme");
        }

        public override void Update()
        {
            if (Input.Pressed("confirm"))
            {
                SFX.Play("menu_click");
                Player.Instance.ClearPlayer();
                SaveData.Start(new SaveData());
                Engine.Scene = new Loader(null);
            }
        }

        public override void Render()
        {
            Draw.Begin();
            Draw.Rect(-5, -5, Engine.Instance.Screen.Width + 10, Engine.Instance.Screen.Height + 10, Constants.Light);
            Draw.End();
            base.Render();
        }

        public override void OverlayRender()
        {
            Draw.BeginOverlay();
            gamename.Render();
            gameover.Render();
            ButtonUI.Render(new Vector2(Engine.Instance.Screen.Width / 2f, Engine.Instance.Screen.Height - Engine.Instance.Screen.Height / 4f), Texts.MainText["pause_restart"], Constants.Dark, "confirm", 1f);
            Draw.End();
        }
    }
}