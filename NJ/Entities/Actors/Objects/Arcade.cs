using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    public class Arcade : Actor
    {
        private readonly Animation _sprite = new Animation(GFX.Game["objects/rocket/arcade"], 16, 16);

        private float timer;
        private bool isOpen;
        private bool _hintVisible;
        private Vector2 _hintPosition;

        public Arcade()
            : base(0, 0)
        {
            Depth = 1;

            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Arcade);
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            Add(_sprite);
            _sprite.Add("idle", 9f, 0, 1, 2, 3);
            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;
            _sprite.Play("idle");
        }

        public override void Update()
        {

            base.Update();
            if (!isOpen)
            {
                _hintVisible = (Player.Instance.Position - Position).Length() < 8f;
            }
            else
            {
                _sprite.Play("open");
                _hintVisible = false;
            }
            if (Input.Pressed("action") && _hintVisible)
            {
                Player.Instance.IgnoreAction = true;
                Engine.Scene = new Emulator("wellboy");
                //Player.Instance.IgnoreAction = false;
            }

            if (!_hintVisible) return;
            timer += Engine.DeltaTime * 8f;
            _hintPosition = new Vector2(Position.X, Position.Y + (float)Math.Sin(Math.Cos(timer)) - 16);
        }

        public override void Render()
        {
            base.Render();
            if (_hintVisible)
                ButtonUI.Render(_hintPosition, "", Constants.LightGreen, "action", 1f);
        }
    }
}