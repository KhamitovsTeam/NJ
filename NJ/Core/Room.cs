using KTEngine;
using System.Xml;

namespace Chip
{
    public class Room
    {
        public XmlElement Xml;
        public int ID;
        public string Index = "";
        public string Type = "";
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public int Depth;
        public bool Active;
        public bool Visited;

        private Scene level;

        public int SceneX => X * ((Level) level).RoomWidth;
        public int SceneY => Y * ((Level) level).RoomHeight;
        public int SceneWidth => Width * ((Level) level).RoomWidth;
        public int SceneHeight => Height * ((Level) level).RoomHeight;

        public Room(Scene scene)
        {
            this.level = scene;
        }

        public bool IsType(string type)
        {
            return Type == type;
        }

        public void Reset()
        {
            Log.Message("Room: " + ID + " was resetted");
        }
    }
}