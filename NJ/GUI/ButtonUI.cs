using KTEngine;
using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public static class ButtonUI
    {
        public static string Id(string button)
        {
            var prefix = "keyboard";
            if (Input.HasController)
            {
#if FNA
                prefix = Input.IsPS4Controller ? "dualshock" : "xbox";
#else
                prefix = "xbox";
#endif
            }
            return "controls/" + prefix + "/" + button;
        }

        public static float Width(string label, string button)
        {
            var texture = GFX.Gui[Id(button)];
            return label.Equals("") ? texture.Width : Fonts.MeasureString(label).X + texture.Width;
        }

        public static float Height(string label, string button)
        {
            var texture = GFX.Gui[Id(button)];
            return label.Equals("") ? texture.Height : Math.Max(Fonts.MeasureString(label).Y, texture.Width);
        }

        public static void Render(Vector2 position, string label, Color color, string button, float scale, float justifyX = 0f, float wiggle = 0.0f, float alpha = 1f)
        {
            var texture = GFX.Gui[Id(button)];
            var width = Width(label, button);
            var padding = 0f;
            position.X -= (float)(scale * width * (justifyX - 0.5));
            if (!label.Equals(""))
            {
                DrawText(label, position, color, scale + wiggle, alpha);
                padding = 4f;
            }
            Draw.Texture(texture, position, new Vector2(width + padding, texture.Height / 2f), Vector2.One * (scale + wiggle), 0f, Color.White * alpha);
        }

        private static void DrawText(string label, Vector2 position, Color color, float scale, float alpha)
        {
            var size = Draw.DefaultFont.Sizes[0].Measure(label);
            var origin = new Vector2
            {
                X = size.X,
                Y = size.Y / 2
            };
            var pos = Engine.Instance.CurrentCamera.Render + position;
            Draw.Rect(position.X - origin.X, position.Y - origin.Y, size.X, size.Y, Constants.Light);
            pos = new Vector2(pos.X - origin.X, pos.Y - origin.Y);
            Draw.DefaultFont.Draw(label, pos, color);
        }
    }
}