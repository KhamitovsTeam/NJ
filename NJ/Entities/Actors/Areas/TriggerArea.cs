using System;
using System.Collections.Generic;
using System.Xml;
using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class TriggerArea : Actor
    {
        // TriggerArea props
        private int width;
        private int height;
        private string scriptName;
        private bool enter;
        private bool exit;
        
        private Script script;

        private bool isInside;

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            width = xml.AttrInt("width");
            height = xml.AttrInt("height");
            
            scriptName = xml.Attr("script");
            enter = xml.AttrBool("enter", false);
            exit = xml.AttrBool("exit", false);

            MoveCollider = Add(new Hitbox(-8, -8, width, height));
            MoveCollider.Tag((int) Tags.Trigger);
            
            // Init script
            Type type = Type.GetType("Chip." + scriptName);
            if (!(type != null))
                return;
            script = (Script) Activator.CreateInstance(type);
        }

        public override void Update()
        {
            base.Update();
            if (MoveCollider != null && MoveCollider.Check((int) Tags.Player))
            {
                if (isInside) return;
                isInside = true;
                if (!enter) return;
                script?.OnTriggerEnter();
                Console.WriteLine("Is enter");
            }
            else
            {
                if (!isInside) return;
                isInside = false;
                if (!exit) return;
                script?.OnTriggerExit();
                Console.WriteLine("Is exit");
            }
        }
    }
}