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

        public int SceneX
        {
            get
            {
                if (level.GetType() == typeof(Test))
                    return X * ((Test) level).RoomWidth;
                return X * ((Level) level).RoomWidth;
            }
        }

        public int SceneY
        {
            get
            {
                if (level.GetType() == typeof(Test))
                    return Y * ((Test) level).RoomHeight;
                return Y * ((Level) level).RoomHeight;
            }
        }

        public int SceneWidth
        {
            get
            {
                if (level.GetType() == typeof(Test))
                    return Width * ((Test) level).RoomWidth;
                return Width * ((Level) level).RoomWidth;
            }
        }

        public int SceneHeight
        {
            get
            {
                if (level.GetType() == typeof(Test))
                    return Height * ((Test) level).RoomHeight;
                return Height * ((Level) level).RoomHeight;
            }
        }

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