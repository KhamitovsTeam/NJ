using KTEngine;
using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public class Panel : Actor
    {
        private Animation _sprite = new Animation(GFX.Game["sceneries/rocket/panel"], 32, 24);
        private Vector2 _hintPosition;
        private bool _hintVisible;
        private float _timer;
        private bool _isOpen;

        public Panel()
            : base(0, 0)
        {
            Depth = 2;

            Add(_sprite);
            _sprite.Add("idle", 6f, 0, 1, 2, 3);
            _sprite.CenterOrigin();
            _sprite.Play("idle");

            MoveCollider = Add(new Hitbox(-16, -12, 32, 24));
            MoveCollider.Tag((int)Tags.RocketPanel);
        }

        public override void Update()
        {

            base.Update();
            if (!_isOpen)
            {
                _hintVisible = (Player.Instance.Position - Position).Length() < 11f;
            }
            else
            {
                _sprite.Play("open");
                _hintVisible = false;
            }
            if (Input.Pressed("action") && _hintVisible)
            {
                Player.Instance.IgnoreAction = true;
            }

            if (!_hintVisible) return;
            _timer += Engine.DeltaTime * 8f;
            _hintPosition = new Vector2(Position.X, Position.Y + (float)Math.Sin(Math.Cos(_timer)) - 22f);
        }

        public override void Render()
        {
            base.Render();
            if (_hintVisible)
                ButtonUI.Render(_hintPosition, "", Constants.Light, "action", 1f);
        }
    }
}