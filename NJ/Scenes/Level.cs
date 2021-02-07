using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using KTEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#if __ANDROID__
using System.IO;
#endif

namespace Chip
{
    public class Level : Scene
    {
        #region Public Properties

        public string ID;
        public int Width;
        public int Height;
        public int RoomID;
        public int RoomWidth;
        public int RoomHeight;
        public Grid Grid;
        public Grid GridBack;
        public bool IsGamePaused;
        public bool InCutscene;
        public bool LockCamera;

        #endregion

        #region Private Variables

        private Color fillColor;
        private float roomTimer;
        private bool isEditMode = false;
        private readonly List<Room> rooms = new List<Room>();
        private readonly List<SpawnArea> spawns = new List<SpawnArea>();

        private Vector2 prevMousePosition = Vector2.Zero;
        private Vector2 currentMousePosition = Vector2.Zero;

        #endregion

        public Session Session;
        public XmlElement Xml;
        public Wall Wall;
        public Player Player;
        public BackTiles BackTiles;
        public Room CurrentRoom;
        public GameplayRenderer GameplayRenderer;
        public HiresRenderer HiresRenderer;

        public OverlayComponent OverlayComponent = null;

        #region Public Constructor

        public Level(Session session)
            : base("parallax", "parallax2")
        {
            ID = session.ToLevel;
            
            GameplayRenderer = new GameplayRenderer();
            HiresRenderer = new HiresRenderer();

            Player = Player.Instance;

            Layers["parallax"].Parallax = new Vector2(-0.01f, 1f);
            Layers["parallax2"].Parallax = new Vector2(-0.03f, 1f);

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

        #endregion

        #region Public Methods

        public override void Begin()
        {
            GameplayBuffers.Create();

            SetBackgroundColor();

            LoadActor(Player, "intro");

            CurrentRoom = GetPlayerRoom();
            CurrentRoom.Visited = true;

            foreach (var room in rooms)
            {
                if (room != CurrentRoom)
                    DeactivateEntities(GetEntitiesByTag("room" + room.ID));
            }
            OverlayPause overlayPause = new OverlayPause(this);
            OverlayPowerup overlayPowerup = new OverlayPowerup(this);
            OverlayDialog overlayDialog = new OverlayDialog(this);
        }

        public override void End()
        {
            base.End();
            GameplayBuffers.Unload();
        }

        public override void Update()
        {
            if (!IsGamePaused)
            {
                base.Update();
                if (Input.Pressed("pause"))
                {
                    Pause();
                }
            }

            OverlayComponent?.Update();

            var playerRoom = GetPlayerRoom();
            roomTimer -= Engine.DeltaTime;
            if (playerRoom != CurrentRoom && playerRoom != null && roomTimer <= 0.0)
            {
                CurrentRoom = playerRoom;
                CurrentRoom.Visited = true;
                roomTimer = 1f;
            }

            bool isDead = Player?.Dead ?? true;

            if (Input.Pressed(Keys.E))
            {
                isEditMode = !isEditMode;
            }

            if (isEditMode)
            {
                if (Input.IsLeftMouseButtonPressed())
                {
                    currentMousePosition = new Vector2(Input.MouseState.X, Input.MouseState.Y);
                    if (prevMousePosition == Vector2.Zero)
                    {
                        prevMousePosition = currentMousePosition;
                    }
                    
                    if (prevMousePosition != currentMousePosition)
                    {
                        var diff = prevMousePosition - currentMousePosition;
                        Camera.X += diff.X / 5f;
                        Camera.Y += diff.Y / 5f;
                        prevMousePosition = currentMousePosition;
                    }
                }
                else
                {
                    prevMousePosition = Vector2.Zero;
                }

                int scale = Math.Abs(Input.MouseState.Wheel) / 100;
                if (scale != 0)
                {
                    Console.WriteLine(scale);
                    Engine.Instance.Screen.SetScale(scale);
                }
            }
            else
            {
                if (CurrentRoom != null && !isDead)
                {
                    if (LockCamera)
                    {
                        LockCameraInBounds();
                    }
                    else
                    {
                        KeepCameraInBounds();
                    }
                }
            }

            foreach (Room room in rooms)
            {
                if (Camera.X + (double)Engine.Instance.Screen.Width > room.SceneX &&
                    Camera.Y + (double)Engine.Instance.Screen.Height > room.SceneY &&
                    (Camera.X < (double)(room.SceneX + room.SceneWidth) &&
                     Camera.Y < (double)(room.SceneY + room.SceneHeight)))
                    ActivateEntities(GetEntitiesByTag("room" + room.ID));
                else
                    DeactivateEntities(GetEntitiesByTag("room" + room.ID));
            }

            //if (Player.Speed.X != 0)
            {
                var listEntities = GetEntities();
                foreach (var entity in listEntities)
                {
                    if (entity.GetType() == typeof(BackgroundPlanet))
                    {
                        //   entity.X = (float) Math.Floor(Camera.X + Camera.Width - entity.X * Engine.DeltaTime);
                    }
                }
            }
        }

        public override void Render()
        {
            Draw.Begin();
            Draw.Rect(-5, -5, Engine.Instance.Screen.Width + 10, Engine.Instance.Screen.Height + 10, fillColor);
            Draw.End();

            base.Render();

            Draw.Begin(BlendState.AlphaBlend, SamplerState.PointClamp);
            const float padding = 6 * 4;
            if (Camera.X < -(double)padding)
                Draw.Rect(Camera.X, Camera.Y, Math.Abs(Camera.X) - padding, Engine.Instance.Screen.Height, Constants.Dark, 1f);
            if (Camera.Y < -(double)padding)
                Draw.Rect(Camera.X, Camera.Y, Engine.Instance.Screen.Width, Math.Abs(Camera.Y) - padding, Constants.Dark, 1f);
            if (Camera.X + (double)Engine.Instance.Screen.Width > Width + (double)padding)
                Draw.Rect(Width + padding, Camera.Y, Camera.X + Engine.Instance.Screen.Width - Width - padding, Engine.Instance.Screen.Height, Constants.Dark, 1f);
            if (Camera.Y + (double)Engine.Instance.Screen.Height > Height + (double)padding)
                Draw.Rect(Camera.X, Height + padding, Engine.Instance.Screen.Width, Camera.Y + Engine.Instance.Screen.Height - Height - padding, Constants.Dark, 1f);

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

        public override void OverlayRender()
        {
            Draw.BeginOverlay(BlendState.AlphaBlend, SamplerState.PointClamp);
            OverlayComponent?.Render();
            Draw.End();
        }

        public void StartCutscene()
        {
            InCutscene = true;
            Player?.StateMachine.Set(Player.StateCutScene);
        }

        public void EndCutscene()
        {
            InCutscene = false;
            Player?.StateMachine.Set(Player.StateNormal);
        }

        public override void LoseFocus()
        {
            base.LoseFocus();
            Pause();
        }

        public Room GetRoomFromPoint(Vector2 point)
        {
            foreach (var room in rooms)
            {
                if (point.X >= (double)room.SceneX && point.Y >= (double)room.SceneY &&
                    (point.X <= (double)(room.SceneX + room.SceneWidth) &&
                     point.Y <= (double)(room.SceneY + room.SceneHeight)))
                    return room;
            }
            return null;
        }

        public IEnumerator Create()
        {
            yield return GenerateMap();
            yield return LoadMap();
            yield return 0f;
        }

        #endregion

        #region Private Methods

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
            Grid = new Grid(4, 4, Width / 4, Height / 4);
            Grid[0, 0, Grid.Columns, Grid.Rows] = true;
            GridBack = new Grid(4, 4, Width / 4, Height / 4);
            GridBack[0, 0, GridBack.Columns, GridBack.Rows] = true;

            Wall = Add(new Wall("terrain/" + ID + "/", Grid, this));
            foreach (Room room in rooms)
            {
                LoadRoom(room);
            }

            /*if (Player == null)
                Player = new Player();*/
            UpdateLists();
            yield return Wall.Generate();
            BackTiles = Add(new BackTiles("sceneries/building_back", GridBack, this), "parallax");

            // left wall
            Add(new LevelWall(-1, 0, 0, 0, 1, Height));
            // right wall
            Add(new LevelWall(Width, 0, 0, 0, 1, Height));
            // bottom wall
            Add(new LevelWall(0, Height, 0, Height, Width, 2));
            // top wall
            Add(new LevelWall(0, 0, 0, 0, Width, 2));

            yield return BackTiles.Generate();
        }

        private void LoadRoom(Room room)
        {
            room.Active = false;
            Grid[room.X * RoomWidth / 4, room.Y * RoomHeight / 4,
                room.Width * RoomWidth / 4, room.Height * RoomHeight / 4] = false;
            GridBack[room.X * RoomWidth / 4, room.Y * RoomHeight / 4,
                room.Width * RoomWidth / 4, room.Height * RoomHeight / 4] = false;
            LoadEmbed(room.Xml, room, new Vector2(room.SceneX, room.SceneY), true);
        }

        private void LoadEmbed(XmlElement xml, Room room, Vector2 offset, bool top)
        {
            XmlElement sceneries = xml["Sceneries"];
            if (sceneries != null)
            {
                foreach (XmlElement childNode in sceneries.ChildNodes)
                    LoadActor(childNode, room, offset);
            }
            XmlElement wall = xml["Wall"];
            if (wall != null)
            {
                foreach (XmlElement xmlElement in wall)
                    Grid[(int)((offset.X + (double)xmlElement.AttrInt("x")) / 4f),
                        (int)((offset.Y + (double)xmlElement.AttrInt("y")) / 4f), xmlElement.AttrInt("w") / 4,
                        xmlElement.AttrInt("h") / 4] = true;
            }
            XmlElement backTiles = xml["BackTiles"];
            if (backTiles != null)
            {
                foreach (XmlElement xmlElement in backTiles)
                    GridBack[(int)((offset.X + (double)xmlElement.AttrInt("x")) / 4f),
                        (int)((offset.Y + (double)xmlElement.AttrInt("y")) / 4f), xmlElement.AttrInt("w") / 4,
                        xmlElement.AttrInt("h") / 4] = true;
            }
            XmlElement actors = xml["Actors"];
            if (actors != null)
            {
                foreach (XmlElement childNode in actors.ChildNodes)
                    LoadActor(childNode, room, offset);
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
                            X = vector2.X - 2,
                            Y = vector2.Y - 4,
                            FromLevel = childNode.Attr("from"),
                            CameraX = childNode.AttrFloat("camX"),
                            CameraY = childNode.AttrFloat("camY")
                        };
                        spawns.Add(spawnArea);
                    }
                    else
                    {
                        LoadActor(childNode, room, offset);
                    }
                }
            }
        }

        private void LoadActor(XmlElement actor, Room room, Vector2 offset, string layer = "default")
        {
            Type type = Type.GetType("Chip." + actor.LocalName);
            if (!(type != null))
                return;
            Actor instance = (Actor)Activator.CreateInstance(type);
            Add(instance, layer);
            Vector2 vector2 = offset + new Vector2(actor.AttrInt("x") + 2, actor.AttrInt("y") + 2);
            instance.CreateFromXml(actor, room, vector2, this);
        }

        private T LoadActor<T>(T actor, string roomtype = "normal") where T : Actor
        {
            Add(actor);
            Vector2 spawn = GetSpawn(Session);
            actor.CreateFromSpawn(spawn, GetRoomFromPoint(spawn), this);
            return actor;
        }

        private T LoadActor<T>(T actor, List<Vector2> points) where T : Actor
        {
            Add(actor);
            Vector2 vector2 = Utils.Choose(points);
            actor.CreateFromSpawn(vector2, GetRoomFromPoint(vector2), this);
            return actor;
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

        private Vector2 GetSpawn(Session session)
        {
            Vector2 spawnPoint = new Vector2();

            if (session.ToLevel == session.FromLevel)
            {
                spawnPoint.X = session.LastCheckpoint.X;
                spawnPoint.Y = session.LastCheckpoint.Y;

                if (session.LastCheckpoint.X - Engine.Instance.Screen.Width / 2f > Width)
                {
                    Camera.X = Width - Engine.Instance.Screen.Width;
                }
                else
                {
                    Camera.X = session.LastCheckpoint.X - Engine.Instance.Screen.Width / 2f;
                }

                if (session.LastCheckpoint.Y - Engine.Instance.Screen.Height / 2f > Height)
                {
                    Camera.Y = Height - Engine.Instance.Screen.Height;
                }
                else
                {
                    Camera.Y = session.LastCheckpoint.Y - Engine.Instance.Screen.Height / 2f;
                }

                return spawnPoint;
            }

            foreach (var spawn in spawns)
            {
                if (spawn.FromLevel != session.FromLevel) continue;
                spawnPoint.X = spawn.X;
                spawnPoint.Y = spawn.Y;
                Camera.X = spawn.CameraX;
                Camera.Y = spawn.CameraY;
                break;
            }
            return spawnPoint;
        }

        private Room GetPlayerRoom()
        {
            return GetRoomFromPoint(Player?.Position ?? Vector2.Zero);
        }

        private void Pause()
        {
            if (OverlayComponent != null)
            {
                OverlayComponent.Hide();
                return;
            }
            IsGamePaused = true;
            OverlayPause.Instance.Show();
        }

        private void LockCameraInBounds()
        {
            if (InCutscene) return;
            if (Player == null) return;

            if (Player.Position.X < CurrentRoom.SceneX ||
                Player.Position.X > CurrentRoom.SceneWidth)
            {

            }
            else
            {

                Camera.X = Player.Position.X - Engine.Instance.Screen.Width / 2f;
            }

            if (Player.Position.Y <= CurrentRoom.SceneY ||
                Player.Position.Y >= CurrentRoom.SceneHeight / 2f)
            {

            }
            else
            {
                Camera.Y = Player.Position.Y - Engine.Instance.Screen.Height / 2f;
            }
        }

        private void KeepCameraInBounds()
        {
            if (InCutscene) return;
            if (Player == null) return;

            if (Player.Position.X < Engine.Instance.Screen.Width / 2f)
            {
                Camera.X = Calc.LerpSnap(Camera.X, 0, 0.166667f);
            }
            else if (Player.Position.X > Width - Engine.Instance.Screen.Width / 2f)
            {
                Camera.X = Calc.LerpSnap(Camera.X, Width - Camera.Width, 0.166667f);
            }
            else
            {
                Camera.X = Player.Position.X - Engine.Instance.Screen.Width / 2f;
            }

            if (Player.Position.Y < Engine.Instance.Screen.Height / 2f)
            {
                Camera.Y = Calc.LerpSnap(Camera.Y, 0, 0.166667f);
            }
            else if (Player.Position.Y > Height - Engine.Instance.Screen.Height / 2f)
            {
                Camera.Y = Calc.LerpSnap(Camera.Y, Height - Camera.Height, 0.166667f);
            }
            else
            {
                Camera.Y = Calc.LerpSnap(Camera.Y, Player.Position.Y - Camera.Height / 2f, 0.166667f);
            }
        }

        private void SetBackgroundColor()
        {
            Engine.ClearColor = Color.Black;
            switch (ID)
            {
                case "01_00":
                    fillColor = Constants.Light;
                    Engine.ClearColor = Constants.Dark;
                    break;
                case "01_01":
                    fillColor = Constants.Light;
                    break;
                case "01_02":
                    fillColor = Constants.Light;
                    break;
                case "01_03":
                    fillColor = Constants.Light;
                    break;
                case "01_04":
                    fillColor = Constants.Light;
                    break;
                case "01_05":
                    fillColor = Constants.Light;
                    break;
                case "02_00":
                    fillColor = Constants.Light;
                    break;
                case "02_01":
                    fillColor = Constants.Light;
                    break;
                case "02_02":
                    fillColor = Constants.Light;
                    break;
                case "02_03":
                    fillColor = Constants.Light;
                    break;
                case "02_04":
                    fillColor = Constants.Light;
                    break;
               
                case "03_00":
                    fillColor = Constants.Light;
                    break;
                case "03_01":
                    fillColor = Constants.Light;
                    break;
                case "03_02":
                    fillColor = Constants.Light;
                    break;
                case "03_03":
                    fillColor = Constants.Light;
                    break;
                case "03_04":
                    fillColor = Constants.Light;
                    break;
                case "03_05":
                    fillColor = Constants.Light;
                    break;
                case "99_99":
                    fillColor = Constants.Light;
                    break;
                default:
                    fillColor = Constants.Light;
                    break;
            }
        }

        #endregion
    }
}