using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using KTEngine;
using Microsoft.Xna.Framework;

namespace NJ
{
    public class Level : Scene
    {
        public static Level Instance;
        
        public string ID;
        public int Width;
        public int Height;
        public int RoomID;
        public int RoomWidth;
        public int RoomHeight;
        public Grid Grid;
        public Grid GridBack;

        public Room CurrentRoom;
        public Room NextRoom;
        public Room PrevRoom;
        public int RoomIndex;
        public bool LoadNextRoom;

        private Player player;

        public Level()
        {
            Instance = this;
        }

        public override void Begin()
        {
            RoomID = 1;
            player = new Player();
            CurrentRoom = LoadRoom(null, RoomID.ToString(), true);
            Camera.Position = new Vector2(CurrentRoom.X, CurrentRoom.Y);
            //Camera.Position = Vector2.Zero;
        }
        
        public void AddNextRoom(Room lastRoom)
        {
            LoadNextRoom = true;
        }

        public void ClearPrevRoom()
        {
            foreach (Entity entity in GetEntitiesByTag(PrevRoom.Name))
                Remove(entity);
        }

        private Room LoadRoom(Room lastRoom, string roomFile, bool spawnPlayer = false)
        {
            XmlElement xmlLevel = XML.Load("Levels/" + roomFile + ".oel")["level"];
            int width = xmlLevel.AttrInt("width");
            int height = xmlLevel.AttrInt("height");
            
            Room room = new Room(this, "room" + roomFile, xmlLevel, width, height);
            
            if (lastRoom != null)
                room.SetPosition(lastRoom);
            
            room.Grid = new Grid(4, 4, room.Columns, room.Rows);
            
            XmlElement sceneries = room.Xml["Sceneries"];
            if (sceneries != null)
            {
                foreach (XmlElement childNode in sceneries)
                    LoadActor(childNode, room);
            }
            
            XmlElement walls = room.Xml["Walls"];
            if (walls != null)
            {
                foreach (XmlElement xmlWall in walls)
                    room.Grid[xmlWall.AttrInt("x"), xmlWall.AttrInt("y"), xmlWall.AttrInt("w"), xmlWall.AttrInt("h")] =
                        true;
            }

            XmlElement actors = room.Xml["Actors"];
            if (actors != null)
            {
                foreach (XmlElement xmlActor in actors)
                {
                    if (ActorExists(xmlActor))
                        LoadActor(xmlActor, room);
                }
            }
            
            XmlElement areas = room.Xml["Areas"];
            if (areas != null)
            {
                foreach (XmlElement childNode in areas)
                {
                    if (spawnPlayer)
                    {
                        if (childNode.Name.Equals("SpawnArea"))
                        {
                            player.X = childNode.AttrInt("x") + 2;
                            player.Y = childNode.AttrInt("y") + 4;
                            Add(player);
                        }
                    }
                    
                    if (!childNode.Name.Equals("SpawnArea"))
                        LoadActor(childNode, room);
                }
            }

            Add(new Wall(room.X, room.Y, room.Grid, this)).Tag = room.Name;
            UpdateLists();
            if (lastRoom != null)
            {
                foreach (var item in GetEntitiesByTag(room.Name))
                    item.Active = false;
            }
            return room;
        }
        
        public Entity LoadActor(XmlElement actor, Room room, string toCall = "CreateFromXml")
        {
            Type type = Type.GetType("NJ." + actor.LocalName);
            if (type != null)
            {
                Entity entity = Activator.CreateInstance(type) as Entity;
                if (entity == null)
                    throw new Exception("Unable to find entity " + type);
                Add(entity);
                entity.Tag = room.Name;
                Dictionary<string, XmlAttribute> dictionary = new Dictionary<string, XmlAttribute>();
                for (int index = 0; index < actor.Attributes.Count; ++index)
                    dictionary.Add(actor.Attributes[index].Name, actor.Attributes[index]);
                MethodInfo method = type.GetMethod(toCall);
                if (method != null)
                    method.Invoke(entity, new object[2]
                    {
                        room,
                        dictionary
                    });
                return entity;
            }
            Console.WriteLine("[bunker] Unindentified entity type {0}", actor.LocalName);
            return null;
        }

        public bool ActorExists(XmlElement actor)
        {
            return Type.GetType("NJ." + actor.LocalName) != null;
        }
        
        public override void Update()
        {
            if (LoadNextRoom)
            {
                NextRoom = LoadRoom(CurrentRoom, (RoomID++).ToString());
                LoadNextRoom = false;
            }
            if (NextRoom != null)
            {
                bool flag = true;
                if (!player.InNextRoom)
                    flag = false;
                if (flag)
                {
                    PrevRoom = CurrentRoom;
                    CurrentRoom = NextRoom;
                    NextRoom = null;
                    foreach (Base @base in GetEntitiesByTag(CurrentRoom.Name))
                        @base.Active = true;
                    //Camera.UnClamp();
                    //Camera.ClearTargets();
                }
            }
            base.Update();
        }
        
        public bool InRoom(Room room, Entity entity, int padding)
        {
            if (entity.X >= (double) (room.X - padding) && entity.Y >= (double) (room.Y - padding) && entity.X < (double) (room.X + room.Width + padding))
                return entity.Y < (double) (room.Y + room.Height + padding);
            return false;
        }
        
        public override void Render()
        {
            base.Render();
            Draw.Begin();
            Draw.Rect(0, 0, Width, Height, Color.Azure);
            Draw.End();
            //Renderer.Begin();
            //Renderer.Rect(0.0f, 0.0f, (float) Ark.Width, (float) Ark.Height, Color.Black, this.fade);
            //Renderer.End();
        }

    }
}