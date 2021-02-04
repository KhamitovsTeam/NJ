using KTEngine;
using System.Xml;
#if __ANDROID__
using System.IO;
#endif

namespace Chip
{
    /// <summary>
    /// Атласы со спрайтами
    /// </summary>
    public class GFX
    {
        public static Atlas Game;
        public static Atlas Gui;
        public static Atlas Misc;

        public static XmlElement Sprites;

        public static void LoadGame()
        {
            Game = Atlas.FromAtlas("Graphics/Atlases/Gameplay", Atlas.AtlasDataFormat.TexturePackerSparrow);
        }

        public static void LoadGui()
        {
            Gui = Atlas.FromAtlas("Graphics/Atlases/Gui", Atlas.AtlasDataFormat.TexturePackerSparrow);
        }

        public static void LoadMisc()
        {
            Misc = Atlas.FromAtlas("Graphics/Atlases/Misc", Atlas.AtlasDataFormat.TexturePackerSparrow);
        }

        public static void LoadSprites()
        {
#if __ANDROID__
            string xmlPath = Path.Combine(Engine.ContentDirectory, "Graphics/Sprites.xml");
            Sprites = XML.Load(xmlPath)["Sprites"];
#else
            Sprites = XML.Load("Graphics/Sprites.xml")["Sprites"];
#endif
        }
    }
}