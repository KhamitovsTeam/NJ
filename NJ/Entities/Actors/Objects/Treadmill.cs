using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    public class Treadmill : Actor
    {
        private float _strength = 60f;
        private int _width;
        private int _direction = 1;

        public Treadmill()
            : base(0, 0)
        {
            Depth = -10;
            MoveCollider = Add(new Hitbox(16, 16));
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _width = xml.AttrInt("width");
            _direction = xml.AttrInt("direction");

            MoveCollider = Add(new Hitbox(-8, -8, _width, 16));
            MoveCollider.Tag((int)Tags.Solid);
            MoveCollider.Reset(_width, 16);

            int offsetX = 0;
            while (offsetX < _width)
            {
                Animation component = new Animation(GFX.Game["objects/treadmill"], 16, 16);
                int[] frames;
                if (offsetX == 0)
                    frames = new[] { 0, 1, 2, 3 };
                else if (offsetX == _width - 16.0)
                    frames = new[] { 8, 9, 10, 11 };
                else
                    frames = new[] { 4, 5, 6, 7 };
                component.Add("running", 1f, frames);
                component.Play("running");
                component.X = offsetX + 8f;
                component.Y = 8f;
                component.CenterOrigin();
                component.Scale.Y = _direction;
                Add(component);
                offsetX += 16;
            }

            X -= 8f;
            Y -= 8f;
        }

        public override void Update()
        {
            foreach (Animation animation in GetAll<Animation>())
            {
                animation.Scale.Y = _direction;
                Animation.Anim anim = animation.Animations["running"];
                anim.Delay = _strength / 5.0f;
                animation.Animations["running"] = anim;
            }

            foreach (Collider collider in MoveCollider.CollideAll(0, -1, new int[2] { (int)Tags.Player, (int)Tags.Enemy }))
            {
                var entity = collider.Entity as Actor;
                if (entity == null) continue;
                if (Math.Abs(entity.Push.X) < (double)_strength)
                    entity.Push.X = _direction * _strength;
            }
            base.Update();
        }
    }
}