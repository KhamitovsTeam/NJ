using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using KTEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chip
{
    public class Test : Scene
    {
        public string ID;
        public int Width;
        public int Height;
        public int RoomID;
        public int RoomWidth;
        public int RoomHeight;
        public Grid Grid;
        public Grid GridBack;
        public XmlElement Xml;

        private Coroutine loadingCoroutine;
        
        private readonly List<Room> rooms = new List<Room>();

        public Test()
        {
            ID = "01_01";
            
            string xmlPath = null;
#if __ANDROID__
            xmlPath = Path.Combine(Engine.ContentDirectory, "Levels/Levels.xml");
#else
            xmlPath = "Levels/Levels.xml";
#endif
            XmlElement xmlLevels = XML.Load(xmlPath)["Levels"];
            if (xmlLevels == null)
                throw new Exception("Levels does not exist");
            XmlElement xmlRoom = null;
            XmlNodeList levelNodeList = xmlLevels.GetElementsByTagName("Level");
            foreach (XmlElement level in levelNodeList)
            {
                if (!level.Attr("id").Equals(ID)) continue;
                xmlRoom = level;
                break;
            }
            if (xmlRoom == null)
                throw new Exception("Region does not exist with id: " + ID);
            Name = xmlRoom.Attr("name");
            Xml = xmlRoom;
            RoomWidth = xmlRoom.AttrInt("roomWidth");
            RoomHeight = xmlRoom.AttrInt("roomHeight");   
        }

        public override void Begin()
        {
            loadingCoroutine = new Coroutine(Create(), true);
        }

        public override void Update()
        {
            if (!loadingCoroutine.Finished)
            {
                loadingCoroutine.MaxSteps();
            }
        }

        public override void Render()
        {
            base.Render();

            Draw.Begin(BlendState.AlphaBlend, SamplerState.PointClamp);
            const float padding = 6 * 16;
            if (Camera.X < -(double)padding)
                Draw.Rect(Camera.X, Camera.Y, Math.Abs(Camera.X) - padding, Engine.Instance.Screen.Height, Constants.Background, 1f);
            if (Camera.Y < -(double)padding)
                Draw.Rect(Camera.X, Camera.Y, Engine.Instance.Screen.Width, Math.Abs(Camera.Y) - padding, Constants.Background, 1f);
            if (Camera.X + (double)Engine.Instance.Screen.Width > Width + (double)padding)
                Draw.Rect(Width + padding, Camera.Y, Camera.X + Engine.Instance.Screen.Width - Width - padding, Engine.Instance.Screen.Height, Constants.Background, 1f);
            if (Camera.Y + (double)Engine.Instance.Screen.Height > Height + (double)padding)
                Draw.Rect(Camera.X, Height + padding, Engine.Instance.Screen.Width, Camera.Y + Engine.Instance.Screen.Height - Height - padding, Constants.Background, 1f);

#if DEBUG && !CONSOLE && !__MOBILE__
            if (Engine.Instance.Debug.Enabled)
            {
                foreach (Room room in rooms)
                {
                    Draw.HollowRect(room.SceneX, room.SceneY, room.SceneWidth, room.SceneHeight, 1f, Color.Blue);
                    Draw.Text(new Vector2(room.SceneX + 5, room.SceneY + 5), room.Index, Color.Blue);
                    Draw.Text(new Vector2(room.SceneX + 4, room.SceneY + 4), room.Index, Color.White);
                }
            }
#endif

            Draw.End();
        }
        
        public IEnumerator Create()
        {
            yield return GenerateMap();
            yield return LoadMap();
        }
        
        private IEnumerator GenerateMap()
        {
            var depth = 0;
            var position = Vector2.Zero;
            foreach (XmlElement xml in Xml)
            {
                var count = int.Parse(xml.Attr("count"));
                var room = 0;
                while (count > 0)
                {
                    Room newRoom = GetNewRoom(position, Vector2.UnitX, xml.Attr("rooms").Split(',')[room++], xml.Attr("name"));
                    newRoom.ID = RoomID++;
                    newRoom.Depth = depth;
                    rooms.Add(newRoom);
                    position = new Vector2(position.X + 1, depth);
                    --count;
                }
                ++depth;
                position = new Vector2(0, depth);
            }
            Vector2 min = RegionMin();
            foreach (Room room in rooms)
            {
                room.X -= (int)min.X;
                room.Y -= (int)min.Y;
            }
            Vector2 max = RegionMax();
            Width = (int)(max.X * (double)RoomWidth);
            Height = (int)(max.Y * (double)RoomHeight);
            yield return 0f;
        }
        
        private IEnumerator LoadMap()
        {
            Grid = new Grid(16, 16, Width / 16, Height / 16);
            Grid[0, 0, Grid.Columns, Grid.Rows] = true;
            GridBack = new Grid(16, 16, Width / 16, Height / 16);
            GridBack[0, 0, GridBack.Columns, GridBack.Rows] = true;

            //Wall = Add(new Wall("terrain/" + ID + "/", Grid, this));
            foreach (Room room in rooms)
            {
                LoadRoom(room);
            }

            /*if (Player == null)
                Player = new Player();*/
            UpdateLists();
            //yield return Wall.Generate();
            //BackTiles = Add(new BackTiles("sceneries/building_back", GridBack, this), "parallax");

            // left wall
            Add(new LevelWall(0, 0, 0, 0, 2, Height));
            // right wall
            Add(new LevelWall(Width, 0, 0, 0, 2, Height));
            // bottom wall
            Add(new LevelWall(0, Height, 0, Height, Width, 2));
            // top wall
            Add(new LevelWall(0, 0, 0, 0, Width, 2));

            yield return 0f;//BackTiles.Generate();
        }

        private void LoadRoom(Room room)
        {
            room.Active = false;
            Grid[room.X * RoomWidth / 16, room.Y * RoomHeight / 16,
                room.Width * RoomWidth / 16, room.Height * RoomHeight / 16] = false;
            GridBack[room.X * RoomWidth / 16, room.Y * RoomHeight / 16,
                room.Width * RoomWidth / 16, room.Height * RoomHeight / 16] = false;
            LoadEmbed(room.Xml, room, new Vector2(room.SceneX, room.SceneY), true);
        }
        
        private void LoadEmbed(XmlElement xml, Room room, Vector2 offset, bool top)
        {
            XmlElement sceneries = xml["Sceneries"];
            if (sceneries != null)
            {
                /*foreach (XmlElement childNode in sceneries.ChildNodes)
                    LoadActor(childNode, room, offset);*/
            }
            XmlElement wall = xml["Wall"];
            if (wall != null)
            {
                foreach (XmlElement xmlElement in wall)
                    Grid[(int)((offset.X + (double)xmlElement.AttrInt("x")) / 16.0),
                        (int)((offset.Y + (double)xmlElement.AttrInt("y")) / 16.0), xmlElement.AttrInt("w") / 16,
                        xmlElement.AttrInt("h") / 16] = true;
            }
            XmlElement backTiles = xml["BackTiles"];
            if (backTiles != null)
            {
                foreach (XmlElement xmlElement in backTiles)
                    GridBack[(int)((offset.X + (double)xmlElement.AttrInt("x")) / 16.0),
                        (int)((offset.Y + (double)xmlElement.AttrInt("y")) / 16.0), xmlElement.AttrInt("w") / 16,
                        xmlElement.AttrInt("h") / 16] = true;
            }
            XmlElement actors = xml["Actors"];
            if (actors != null)
            {
                /*foreach (XmlElement childNode in actors.ChildNodes)
                    LoadActor(childNode, room, offset);*/
            }
            XmlElement areas = xml["Areas"];
            if (areas != null)
            {
                foreach (XmlElement childNode in areas.ChildNodes)
                {
                    if (childNode.Name.Equals("SpawnArea"))
                    {
                        Vector2 vector2 = offset + new Vector2(childNode.AttrInt("x"), childNode.AttrInt("y"));
                        SpawnArea spawnArea = new SpawnArea
                        {
                            X = vector2.X,
                            Y = vector2.Y,
                            FromLevel = childNode.Attr("from"),
                            CameraX = childNode.AttrFloat("camX"),
                            CameraY = childNode.AttrFloat("camY")
                        };
                        //spawns.Add(spawnArea);
                    }
                    else
                    {
                        //LoadActor(childNode, room, offset);
                    }
                }
            }
        }
        
        private Room GetNewRoom(Vector2 position, Vector2 direction, string index, string type)
        {
            string xmlPath = null;
#if __ANDROID__
            xmlPath = Path.Combine(Engine.ContentDirectory, "Levels/" + ID + "_" +index + ".oel");
#else
            xmlPath = "Levels/" + ID + "_" + index + ".oel";
#endif
            XmlElement level = XML.Load(xmlPath)["level"];
            Room room = new Room(this);
            room.Xml = level;
            room.Index = index;
            room.X = (int)position.X;
            room.Y = (int)position.Y;
            room.Width = level.AttrInt("width") / RoomWidth;
            room.Height = level.AttrInt("height") / RoomHeight;
            room.Type = type;
            if (direction.X < 0.0)
                room.X -= room.Width - 1;
            if (direction.Y < 0.0)
                room.Y -= room.Height - 1;
            return room;
        }

        private Vector2 RegionMin(bool secrets = true, bool onlyVisited = false)
        {
            Vector2 vector2 = new Vector2(100f, 100f);
            foreach (Room room in rooms)
            {
                if ((room.Visited || !onlyVisited) && (room.Visited || secrets || !room.IsType("secret")))
                {
                    if (room.X < (double)vector2.X)
                        vector2.X = room.X;
                    if (room.Y < (double)vector2.Y)
                        vector2.Y = room.Y;
                }
            }
            return vector2;
        }

        private Vector2 RegionMax(bool secrets = true, bool onlyVisited = false)
        {
            Vector2 vector2 = new Vector2(0.0f, 0.0f);
            foreach (Room room in rooms)
            {
                if ((room.Visited || !onlyVisited) && (room.Visited || secrets || !room.IsType("secret")))
                {
                    if (room.X + room.Width > (double)vector2.X)
                        vector2.X = room.X + room.Width;
                    if (room.Y + room.Height > (double)vector2.Y)
                        vector2.Y = room.Y + room.Height;
                }
            }
            return vector2;
        }
    }
}