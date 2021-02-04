using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Chip
{
    public class ClassicObject
    {
        public Emulator E;

        public int type;
        public bool collideable = true;
        public bool solids = true;
        public Animation spr;
        public bool flipX;
        public bool flipY;
        public float x;
        public float y;
        public Rectangle hitbox = new Rectangle(0, 0, 8, 8);
        public Vector2 spd = new Vector2(0.0f, 0.0f);
        public Vector2 rem = new Vector2(0.0f, 0.0f);

        public virtual void init(Emulator e)
        {
            E = e;
        }

        public virtual void update()
        {
            if (spr == null)
                return;
            spr.Update();
        }

        public virtual void draw()
        {
            if (spr == null)
                return;
            Classic.E.spr(spr, x, y, flipX, flipY);
        }

        public bool is_solid(int ox, int oy)
        {
            return false;
        }

        public T collide<T>(int ox, int oy) where T : ClassicObject
        {
            Type type = typeof(T);
            foreach (ClassicObject classicObject in Classic.Objects.ToList())
            {
                if (classicObject != null && classicObject.GetType() == type &&
                    (classicObject != this && classicObject.collideable) &&
                    (classicObject.x + (double)classicObject.hitbox.X +
                     classicObject.hitbox.Width >
                     x + (double)hitbox.X + ox &&
                     classicObject.y + (double)classicObject.hitbox.Y +
                     classicObject.hitbox.Height >
                     y + (double)hitbox.Y + oy &&
                     (classicObject.x + (double)classicObject.hitbox.X < x +
                      (double)hitbox.X + hitbox.Width + ox &&
                      classicObject.y + (double)classicObject.hitbox.Y < y +
                      (double)hitbox.Y + hitbox.Height + oy)))
                    return classicObject as T;
            }

            return default(T);
        }

        public bool check<T>(int ox, int oy) where T : ClassicObject
        {
            return collide<T>(ox, oy) != null;
        }

        public void move(float ox, float oy)
        {
            rem.X += ox;
            int x = Classic.E.flr(rem.X + 0.5f);
            rem.X -= x;
            move_x(x, 0);

            rem.Y += oy;
            int y = Classic.E.flr(rem.Y + 0.5f);
            rem.Y -= y;
            move_y(y);
        }

        public void move_x(int amount, int start)
        {
            if (solids)
            {
                int ox = Classic.E.sign(amount);
                for (int index = start; (double)index <= (double)Classic.E.abs(amount); ++index)
                {
                    if (!is_solid(ox, 0))
                    {
                        x = x + ox;
                    }
                    else
                    {
                        spd.X = 0.0f;
                        rem.X = 0.0f;
                        break;
                    }
                }
            }
            else
                x = x + amount;
        }

        public void move_y(int amount)
        {
            if (solids)
            {
                int oy = Classic.E.sign(amount);
                for (int index = 0; (double)index <= (double)Classic.E.abs(amount); ++index)
                {
                    if (!is_solid(0, oy))
                    {
                        y = y + oy;
                    }
                    else
                    {
                        spd.Y = 0.0f;
                        rem.Y = 0.0f;
                        break;
                    }
                }
            }
            else
                y = y + amount;
        }
    }
}