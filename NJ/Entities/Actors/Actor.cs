using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    /// <summary>
    /// Базовый класс для всех объектов, которые находятся на уровне.
    /// </summary>
    public class Actor : Entity
    {
        public Vector2 Speed;
        public Vector2 Push;
        public Vector2 LastPosition;
        public int[] Solids = new int[1] { (int)Tags.Solid };
        public Level Level;
        public Room Room;
        public Collider MoveCollider;
        public bool FallThrough;
        public bool NeedToRemove;

        private Vector2 remainder;

        public Actor(int x = 0, int y = 0)
            : base(x, y)
        {
        }

        public virtual void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            ID = xml.AttrInt("id");
            Scene = level;
            Level = level;
            Room = room;
            Tag = "room" + Room.ID;
            X = (float)(offset.X + (double)xml.AttrInt("x") + 8.0);
            Y = (float)(offset.Y + (double)xml.AttrInt("y") + 8.0);
        }

        public virtual void CreateFromSpawn(Vector2 spawn, Room room, Level level)
        {
            Scene = level;
            Level = level;
            Room = room;
            Tag = "room" + Room.ID;
            X = spawn.X + 2f;
            Y = spawn.Y + 8f;
        }

        public override void Update()
        {
            base.Update();
            if (NeedToRemove)
            {
                RemoveSelf();
                return;
            }
            LastPosition.X = X;
            LastPosition.Y = Y;
            Slowdown(ref Push, 400f, 400f);
            Move(ref Push);
        }

        public void Move()
        {
            Move(ref Speed);
        }

        public void MoveBy(float x, float y)
        {
            Vector2 velocity = new Vector2(x, y);
            Move(ref velocity);
        }

        public void Move(ref Vector2 velocity)
        {
            Vector2 vector2 = velocity * Engine.DeltaTime + remainder;
            remainder.X = vector2.X % 1f;
            remainder.Y = vector2.Y % 1f;
            vector2.X = (int)vector2.X;
            vector2.Y = (int)vector2.Y;
            if (MoveCollider == null || Solids.Length == 0)
            {
                X += vector2.X;
                Y += vector2.Y;
            }
            else
            {
                while (Math.Abs(vector2.X) > 0.0)
                {
                    int x = Math.Sign(vector2.X);
                    Collider collision = MoveCollider.Collide(x, 0, (int)Tags.Solid);
                    if (collision == null || collision.Entity is Jumpthrough)
                    {
                        X += x;
                        vector2.X -= x;
                    }
                    else
                    {
                        HitSolid(Hit.Horizontal, ref velocity, collision);
                        break;
                    }
                }
                while (Math.Abs(vector2.Y) > 0.0)
                {
                    int y = Math.Sign(vector2.Y);
                    Collider collision = MoveCollider.Collide(0, y, (int)Tags.Solid);
                    if (collision == null || collision.Entity is Jumpthrough && (FallThrough || y < 0 || Y + (double)MoveCollider.Y + MoveCollider.Height > collision.Entity.Y))
                    {
                        Y += y;
                        vector2.Y -= y;
                    }
                    else
                    {
                        HitSolid(Hit.Vertical, ref velocity, collision);
                        break;
                    }
                }
            }
        }

        public void Slowdown(float frictionX, float frictionY)
        {
            Slowdown(ref Speed, frictionX, frictionY);
        }

        public void Slowdown(ref Vector2 velocity, float frictionX, float frictionY)
        {
            var signX = Math.Sign(velocity.X);
            velocity.X -= signX * frictionX * Engine.DeltaTime;
            if (Math.Sign(velocity.X) != signX)
                velocity.X = 0.0f;
            var signY = Math.Sign(velocity.Y);
            velocity.Y -= signY * frictionY * Engine.DeltaTime;
            if (Math.Sign(velocity.Y) == signY)
                return;
            velocity.Y = 0.0f;
        }

        public void Fall(float gravity)
        {
            Fall(ref Speed, gravity);
        }

        public void Fall(ref Vector2 velocity, float gravity)
        {
            velocity.Y += gravity * Engine.DeltaTime;
        }

        public void Clamp(float mx, float my)
        {
            Clamp(ref Speed, mx, my);
        }

        public void Clamp(ref Vector2 velocity, float mx, float my)
        {
            velocity.X = Math.Max(-mx, Math.Min(mx, velocity.X));
            velocity.Y = Math.Max(-my, Math.Min(my, velocity.Y));
            /*if (mx >= 0.0 && Math.Abs(velocity.X) > (double) mx)
                velocity.X = Math.Sign(velocity.X) * mx;
            if (my < 0.0 || Math.Abs(velocity.Y) <= (double) my)
                return;
            velocity.Y = Math.Sign(velocity.Y) * my;*/
        }

        public int ForceDirection(float fromx)
        {
            int x = Math.Sign(X - fromx);
            if (x == 0)
                x = Utils.Choose(-1, 1);
            if (MoveCollider.Check(x, 0, (int)Tags.Solid))
                x = -x;
            return x;
        }

        public virtual void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Horizontal)
                velocity.X = 0.0f;
            else
                velocity.Y = 0.0f;
        }

        public enum Hit
        {
            Horizontal,
            Vertical
        }
    }
}