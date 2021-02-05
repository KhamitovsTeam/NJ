using KTEngine;
using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public class SaveLoadIcon : Entity
    {
        private static SaveLoadIcon _instance;

        private readonly Animation _cat;
        private readonly Text _savingText;

        public static void Show()
        {
            if (_instance != null)
                Engine.Scene.Remove(_instance);
            Engine.Scene.Add((Entity)(_instance = new SaveLoadIcon()));
        }

        public static void Hide()
        {
            _instance?.RemoveSelf();
            _instance?.Remove(_instance._savingText);
            _instance?.Remove(_instance._cat);
            _instance = null;
        }

        private SaveLoadIcon()
        {
            _savingText = Add(new Text(Fonts.MainFont, Texts.MainText["saver_saving"], Vector2.Zero, Constants.DarkGreen, Text.HorizontalAlign.Right, Text.VerticalAlign.Bottom));

            _cat = Add(new Animation(GFX.Gui["loader_cat"], 8, 8));
            _cat.Add("walk", 4f, true, 0, 1, 2, 3);
            _cat.Play("walk");

            Depth = -10000;
        }

        public override void Render()
        {
            if (_instance == null) return;
            _savingText.X = (float)Math.Floor(Engine.Instance.CurrentCamera.X + Engine.Instance.Screen.Width - _cat.Width * 1.5f);
            _savingText.Y = (float)Math.Floor(Engine.Instance.CurrentCamera.Y + Engine.Instance.Screen.Height - _savingText.Height);
            Draw.Rect(_savingText.X - _savingText.Width - 1f, _savingText.Y - _savingText.Height - 1f, _savingText.Width + 1f, _savingText.Height + 1f, Constants.LightGreen);
            _cat.X = (float)Math.Floor(Engine.Instance.CurrentCamera.X + Engine.Instance.CurrentCamera.Width - 20f);
            _cat.Y = _savingText.Y - _cat.Height;
            //_cat.Y = (float) Math.Floor(Engine.Instance.CurrentCamera.Y + Engine.Instance.CurrentCamera.Height - 20f);
            _savingText.Render();
            _cat.Render();
            base.Render();
        }
    }
}