using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    public class Grass : Actor
    {
        private Animation _sprite;
        private int _type = 0;
        private float amp;
        //    private int fade;

        public Grass()
            : base(0, 0)
        {
            Depth = -1;

            MoveCollider = Add(new Hitbox(-8, 3, 16, 5));
            MoveCollider.Tag((int)Tags.Sceneries);
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/mushroom" + _type], 16, 5);
            Add(_sprite);
            _sprite.Add("idle", 0f, 0);
            _sprite.Add("move", 4f, 0, 1, 0);
            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = -3f;
            _sprite.Play("idle");
        }

        public override void Update()
        {
            if (Math.Abs(amp) < 5.0)
            {
                Collider collider = MoveCollider.Collide(new int[2] { (int)Tags.Player, (int)Tags.Enemy });
                if (collider != null && Math.Abs(((Actor)collider.Entity).Speed.X) > 16.0)
                    _sprite.Play("move");
                else
                    _sprite.Play("idle");
            }
            int num = Math.Sign(amp);
            amp -= num * Engine.DeltaTime;
            if (Math.Sign(amp) != num)
                amp = 0.0f;
            base.Update();
        }
    }
}
