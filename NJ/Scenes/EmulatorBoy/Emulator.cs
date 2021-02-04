using KTEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Chip
{
    public class Emulator : Scene
    {
        // gameboy colors
        private Color[] colors = {
            Constants.Background,
            Constants.LightGreen,
            Constants.NormalGreen,
            Constants.DarkGreen
        };

        // font
        private KTexture[] font;
        private string fontMap = "abcdefghijklmnopqrstuvwxyz0123456789~!@#4%^&*()_+-=?:.";

        private readonly Color fillColor;

        private readonly Text insert = new Text(Fonts.MainFont, Texts.MainText["insert"], Vector2.One, Constants.LightGreen, Text.HorizontalAlign.Center, Text.VerticalAlign.Top);
        private readonly Text coin = new Text(Fonts.MainFont, Texts.MainText["coin"], Vector2.One, Constants.LightGreen, Text.HorizontalAlign.Center, Text.VerticalAlign.Top);

        private readonly Vector2 screenCenter;

        private float timer;

        // current game
        private Classic classicGame;

        public Emulator(string game = "wellboy")
        {
            switch (game)
            {
                case "wellboy":
                    classicGame = new WellboyClassic();
                    break;

                default:
                    classicGame = new SampleClassic();
                    break;
            }

            var arcadeLight = new Animation(GFX.Gui["emulator/arcade_light"], 105, 12);
            var arcadePanel = new Animation(GFX.Gui["emulator/arcade_panel"], 140, 180);
            var arcadeScreen = new Animation(GFX.Gui["emulator/screen_effect"], 92, 120);

            fillColor = Constants.NormalGreen;

            screenCenter = new Vector2(Engine.Instance.Screen.Width / 2f, Engine.Instance.Screen.Height - Engine.Instance.Screen.Height / 2f);

            var panel = new Entity();

            panel.Add(arcadePanel);
            arcadePanel.X = Engine.Instance.Screen.Width / 2f - arcadePanel.Width / 2f;

            panel.Add(arcadeScreen);
            arcadeScreen.X = Engine.Instance.Screen.Width / 2f - arcadeScreen.Width / 2f;
            arcadeScreen.Y = Engine.Instance.Screen.Height / 2f - arcadeScreen.Height / 2f - 1;

            panel.Add(arcadeLight);
            arcadeLight.Add("idle", 8f, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1);
            arcadeLight.X = Engine.Instance.Screen.Width / 2f - arcadeLight.Width / 2f;
            arcadeLight.Play("idle");

            Add(panel);

            // font
            var fontAtlas = GFX.Gui["pico8/font"];
            font = new KTexture[(fontAtlas.Width / 4) * (fontAtlas.Height / 6)];
            for (var ty = 0; ty < fontAtlas.Height / 6; ty++)
                for (var tx = 0; tx < fontAtlas.Width / 4; tx++)
                    font[tx + ty * (fontAtlas.Width / 4)] = fontAtlas.GetSubtexture(tx * 4, ty * 6, 4, 6);
        }

        public override void Update()
        {
            base.Update();

            if (Input.Pressed("confirm"))
            {
                if (Classic.E != null) return;
                classicGame.Init(this);
            }
            else if (Input.Pressed("cancel"))
            {
                classicGame.Destroy();
                Engine.Scene = new Loader(SaveData.Instance.CurrentSession);
            }
            else
            {
                if (Classic.E == null) return;
                classicGame.Update();
            }
        }

        public override void Render()
        {
            Draw.Begin();
            Draw.Rect(0, 0, Engine.Instance.CurrentCamera.Width, Engine.Instance.CurrentCamera.Height, Constants.DarkGreen);
            if (Classic.E != null)
            {
                classicGame.Draw();
            }
            // Left side
            Draw.Rect((float)Math.Floor(Engine.Instance.CurrentCamera.Render.X), (float)Math.Floor(Engine.Instance.CurrentCamera.Render.Y), Engine.Instance.Screen.Width / 2f - 70, Engine.Instance.CurrentCamera.Height, fillColor);
            // Right side
            Draw.Rect(Engine.Instance.CurrentCamera.Width / 2f + 70f, (float)Math.Floor(Engine.Instance.CurrentCamera.Render.Y), Engine.Instance.Screen.Width / 2f - 70, Engine.Instance.CurrentCamera.Height, fillColor);

            if (Classic.E == null)
            {
                timer += Engine.DeltaTime * 8f;
                var pos = (float)Math.Sin(Math.Cos(timer)) + 8;

                insert.RenderAt(screenCenter - Vector2.UnitY * 16 + Vector2.UnitY * pos);
                coin.RenderAt(screenCenter + Vector2.UnitY * pos);
                ButtonUI.Render(screenCenter + Vector2.UnitY * 21 + Vector2.UnitY * pos, "", Constants.LightGreen, "confirm", 1f);
            }
            else
            {
                ButtonUI.Render(new Vector2(Engine.Instance.Screen.Width - 45f, Engine.Instance.Screen.Height - 10f), Texts.MainText["close"], Constants.LightGreen, "cancel", 1f);
            }

            Draw.End();

            base.Render();
        }

        #region Emulator Methods

        public void music(int index, int fade, int mask)
        {
        }

        public void sfx(int sfx)
        {
        }

        public int rnd(int max)
        {
            return Rand.Instance.Next(max);
        }

        public int flr(float value)
        {
            return (int)Math.Floor(value);
        }

        public int sign(float value)
        {
            return Math.Sign(value);
        }

        public float abs(float value)
        {
            return Math.Abs(value);
        }

        public float min(float a, float b)
        {
            return Math.Min(a, b);
        }

        public float max(float a, float b)
        {
            return Math.Max(a, b);
        }

        public float sin(float a)
        {
            return (float)Math.Sin((1 - a) * Calc.TAU);
        }

        public float cos(float a)
        {
            return (float)Math.Cos((1 - a) * Calc.TAU);
        }

        public void rectfill(float x, float y, float x2, float y2, float c)
        {
            var left = Math.Min(x, x2);
            var top = Math.Min(y, y2);
            var width = Math.Max(x, x2) - left + 1;
            var height = Math.Max(y, y2) - top + 1;
            Draw.Rect(left, top, width, height, colors[((int)c) % 16]);
        }

        public void circfill(float x, float y, float r, float c)
        {
            var color = colors[((int)c) % 16];
            if (r <= 1)
            {
                Draw.Rect(x - 1, y, 3, 1, color);
                Draw.Rect(x, y - 1, 1, 3, color);
            }
            else if (r <= 2)
            {
                Draw.Rect(x - 2, y - 1, 5, 3, color);
                Draw.Rect(x - 1, y - 2, 3, 5, color);
            }
            else if (r <= 3)
            {
                Draw.Rect(x - 3, y - 1, 7, 3, color);
                Draw.Rect(x - 1, y - 3, 3, 7, color);
                Draw.Rect(x - 2, y - 2, 5, 5, color);
            }
        }

        public void print(string str, float x, float y, float c)
        {
            var left = x;
            var color = colors[((int)c) % 16];
            for (var i = 0; i < str.Length; i++)
            {
                var character = str[i];
                var index = -1;
                for (var j = 0; j < fontMap.Length; j++)
                    if (fontMap[j] == character)
                    {
                        index = j;
                        break;
                    }
                if (index >= 0)
                    font[index].Draw(new Vector2(left, y), Vector2.Zero, color);
                left += 4;
            }
        }

        public void spr(Animation sprite, float x, float y, bool flipX = false, bool flipY = false)
        {
            SpriteEffects flip = SpriteEffects.None;
            if (flipX)
                flip |= SpriteEffects.FlipHorizontally;
            if (flipY)
                flip |= SpriteEffects.FlipVertically;
            sprite.UpdateBounds();
            if (sprite.Visible)
                Draw.Texture(sprite.Texture, sprite.Bounds, new Vector2(x, y), Vector2.Zero, Vector2.One, 0f, Color.White, flip);
        }

        #endregion
    }
}