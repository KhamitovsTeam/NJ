using KTEngine;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;

namespace Chip
{
    public static class Fonts
    {
        public static PixelFont MainFont;

        //public static readonly Dictionary<string, PixelFont> Faces = new Dictionary<string, PixelFont>();

        public static void Load()
        {
            Unload();
            string path = Path.Combine(Engine.ContentDirectory, "Fonts/NokiaFont.fnt");
            XmlElement data = XML.Load(path)["font"];
            if (data != null)
            {
                string index = data["info"].Attr("face");
                MainFont = new PixelFont(index);
            }
            MainFont.AddFontSize(path);

            /*Unload();
            foreach (var file in Directory.GetFiles(Path.Combine(Engine.ContentDirectory, "Fonts"), "*.fnt", SearchOption.AllDirectories))
            {
                XmlElement data = XML.Load(file)["font"];
                string index = data["info"].Attr("face");
                if (!Faces.ContainsKey(index))
                    Faces.Add(index, new PixelFont(index));
                Faces[index].AddFontSize(file, data, GFX.Gui, false);
            }*/
        }

        public static Vector2 MeasureString(string text)
        {
            return Draw.DefaultFont.Sizes[0].Measure(text);
        }

        private static void Unload()
        {
            /*foreach (var pixelFont in Faces.Values)
                pixelFont.Dispose();
            Faces.Clear();*/
            MainFont?.Dispose();
        }
    }
}