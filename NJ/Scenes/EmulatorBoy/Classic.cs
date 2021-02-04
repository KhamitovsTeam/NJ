using KTEngine;
using System.Collections.Generic;

namespace Chip
{
    public abstract class Classic
    {
        public static Emulator E;
        public static List<ClassicObject> Objects;

        private static bool isRun = false;

        public virtual void Init(Emulator emulator)
        {
            E = emulator;
            isRun = true;
            Objects = new List<ClassicObject>();
        }

        public virtual void Destroy()
        {
            isRun = false;

            if (Objects != null)
            {
                var list = new List<ClassicObject>(Objects);
                foreach (var classicObject in list)
                {
                    destroy_object(classicObject);
                }
                Objects.Clear();
                Objects = null;
            }

            E = null;
        }

        public virtual void Update()
        {
            if (!isRun) return;

            var list = new List<ClassicObject>(Objects);
            foreach (var classicObject in list)
            {
                update_object(classicObject);
            }
        }

        public virtual void Draw()
        {
            if (!isRun) return;
            var list = new List<ClassicObject>(Objects);
            foreach (var classicObject in list)
            {
                draw_object(classicObject);
            }
        }

        protected static T init_object<T>(T obj, float x, float y, Animation tile = null) where T : ClassicObject
        {
            Objects.Add(obj);
            if (tile != null)
                obj.spr = tile;
            obj.x = (int)x;
            obj.y = (int)y;
            obj.init(E);
            return obj;
        }

        protected static void destroy_object(ClassicObject obj)
        {
            if (Objects == null) return;
            var index = Objects.IndexOf(obj);
            if (index < 0)
                return;
            Objects.RemoveAt(index);
        }

        protected static void update_object(ClassicObject obj)
        {
            obj.update();
        }

        protected static void draw_object(ClassicObject obj)
        {
            obj.draw();
        }
    }
}