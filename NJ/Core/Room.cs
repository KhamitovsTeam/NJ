using System.Xml;
using KTEngine;
using Microsoft.Xna.Framework;

namespace NJ
{
    public class Room
    {
        public string Name;
        public XmlElement Xml;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public Grid Grid;
        public Vector2 Direction;

        public int Columns => Width / 4;
        public int Rows => Height / 4;

        private Level level;
        
        public Room(Level level, string name, XmlElement xml, int width, int height)
        {
            this.level = level;
            Name = name;
            Xml = xml;
            Width = width;
            Height = height;
        }

        public void SetPosition(Room lastRoom)
        {
            if (lastRoom.Direction.X > 0.0)
            {
                X = lastRoom.X + lastRoom.Width;
                Y = lastRoom.Y;
            }
            else if (lastRoom.Direction.X < 0.0)
            {
                X = lastRoom.X - Width;
                Y = lastRoom.Y;
            }
            else if (lastRoom.Direction.Y > 0.0)
            {
                X = lastRoom.X;
                Y = lastRoom.Y + lastRoom.Height;
            }
            else if (lastRoom.Direction.Y < 0.0)
            {
                X = lastRoom.X;
                Y = lastRoom.Y - Height;
            }
            Vector2 zero = Vector2.Zero;
            Vector2 vector2 = Vector2.Zero;
            foreach (XmlElement xml in Xml["Actors"])
            {
                if (xml.LocalName == "Lock" && (xml.AttrInt("x") <= 0 && lastRoom.Direction.X > 0.0 || xml.AttrInt("x") + xml.AttrInt("width") >= Width && lastRoom.Direction.X < 0.0 || (xml.AttrInt("y") <= 0 && lastRoom.Direction.Y > 0.0 || xml.AttrInt("y") + xml.AttrInt("height") >= Height && lastRoom.Direction.Y < 0.0)))
                {
                    vector2 = new Vector2(xml.AttrInt("x"), xml.AttrInt("y"));
                    break;
                }
            }
            vector2.X += X;
            vector2.Y += Y;
            /*if ((double) lastRoom.Direction.X != 0.0)
                zero.Y = lastRoom.ExitLock.Y - vector2.Y;
            else
                zero.X = lastRoom.ExitLock.X - vector2.X;*/
            X += (int) zero.X;
            Y += (int) zero.Y;
        }

        public void OnRoomEnd()
        {
            level.AddNextRoom(this);
        }
    }
}