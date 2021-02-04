using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    public class Waterfall_old : Actor
    {
        private Color color = Constants.LightGreen;
        private Color center = Constants.NormalGreen;
        private float _timer;
        private int _width;
        private int _height;

        public Waterfall_old()
            : base(0, 0)
        {
            Depth = -5;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _width = xml.AttrInt("width");
            _height = xml.AttrInt("height");
            MoveCollider = Add(new Hitbox(-8, 0, _width, _height));
            MoveCollider.Tag((int)Tags.Solid);
            //X -= 16f;
            Y -= 0f;
        }

        public override void Begin()
        {
            MoveCollider.Y = 4f;
            MoveCollider.Height = 8;
            while (true)
            {
                if (!MoveCollider.Check(0, 1, (int)Tags.Solid) && MoveCollider.ScenePosition.Y + (double)MoveCollider.Height < Level.Height)
                    ++MoveCollider.Height;
                else
                    break;
            }
        }

        public override void Render()
        {
            base.Render();
            _timer += Engine.DeltaTime * Calc.TAU;
            int width = 0;
            while (width < _width)
            {
                float amp = (float)Math.Sin(_timer + width / 16.0 * Calc.TAU);
                Draw.Rect((float)(X + (double)width - 8.0), Y + amp, 2f, MoveCollider.Height + 4 - amp, color, 1f);
                Draw.Rect((float)(X + (double)width - 8.0), Y + amp, 2f, 2f, center, 1f);
                width += 2;
            }
        }
    }
}