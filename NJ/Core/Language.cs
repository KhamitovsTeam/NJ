using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Chip
{
    public class Language
    {
        public int Order = 100;
        public string SplitRegex = "(\\s|\\{|\\})";
        public string CommaCharacters = ",";
        public string PeriodCharacters = ".!?";
        public Dictionary<string, string> Dialog = new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, string> Cleaned = new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
        public string Id;
        public string Label;
        public KTexture Icon;
        public string FontFace;
        public float FontFaceSize;
        public bool Initialized;
        public int Lines;
        public int Words;

        public PixelFont Font
        {
            get
            {
                return Fonts.MainFont;
            }
        }

        public Vector2 FontSize
        {
            get
            {
                return this.Font.Sizes[0].Measure("H");
            }
        }

        public string this[string name]
        {
            get
            {
                return this.Dialog[name];
            }
        }

        public void Dispose()
        {
            if (this.Icon.Texture == null || this.Icon.Texture.IsDisposed)
                return;
            this.Icon.Texture.Dispose();
        }
    }
}