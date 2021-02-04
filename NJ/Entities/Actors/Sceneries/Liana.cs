using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    public class Liana : Actor
    {

        private Animation _sprite;
        private float value = Utils.Random();
        private Hitbox hitbox;
        private int _type;
        private float amp;
        private int fade;

        public Liana()
            : base(0, 0)
        {
            Depth = -1;

            hitbox = Add(new Hitbox(-8, -8, 16, 16));
            fade = 1;
        }

        public override void Update()
        {
            if (Math.Abs(amp) < 5.0)
            {
                Collider collider = hitbox.Collide(new int[2] { (int)Tags.Player, (int)Tags.PlayerBullet });
                if (collider != null && Math.Abs(((Actor)collider.Entity).Speed.X) > 16.0)
                    amp += Math.Sign(((Actor)collider.Entity).Speed.X) * 8f * Engine.DeltaTime;
            }
            int num = Math.Sign(amp);
            amp -= num * Engine.DeltaTime;
            if (Math.Sign(amp) != num)
                amp = 0.0f;
            value += Engine.DeltaTime * 8f;
            base.Update();
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/liana" + _type], 16, 16);
            Add(_sprite);
            if (_type == 2) //if spider
            {
                _sprite.Add("idle", 3f, 0, 1, 2, 3, 4, 5);
            }
            else if (_type == 7)
            {
                if (Utils.Range(0, 5) % 2 == 0)
                    _sprite.Add("idle", 1f, 0, 1, 0, 0, 1, 1);
                else
                    _sprite.Add("idle", 1f, 1, 0);
            }
            else
            {
                _sprite.Add("idle", 1f, 0);
            }
            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;
            _sprite.Play("idle");
        }

        public override void Render()
        {
            if (amp != 0.0 && _type != 2)
                Draw.TextureSineH(_sprite.Texture, value, amp, fade, _sprite.Bounds, _sprite.ScenePosition, _sprite.Origin, _sprite.Scale, _sprite.Rotation, _sprite.Color);
            else
                base.Render();
        }
    }
}
