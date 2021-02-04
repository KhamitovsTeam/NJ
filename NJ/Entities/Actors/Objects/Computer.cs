using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    public class Computer : Actor
    {
        private readonly Animation _sprite = new Animation(GFX.Game["objects/computer"], 16, 16);
        private string _cutscene;

        private bool _isOpen;
        private float _timer;

        private bool _hintVisible;
        private Vector2 _hintPosition;

        public Computer()
            : base(0, 0)
        {
            Depth = 1;

            Add(_sprite);
            _sprite.Add("idle", 2f, 0, 1, 2, 3, 4);
            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;
            _sprite.Play("idle");

            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Computer);
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _cutscene = xml.Attr("cutscene");
        }

        public void Opened()
        {
            _isOpen = true;
            Level = Engine.Scene as Level;

            var cutscene = Cutscenes.AllCutscenes[_cutscene];
            if (cutscene != null)
            {
                cutscene.Level = Level;
                cutscene.Start();
                Level?.Add(cutscene);
            }
        }

        public void Closed()
        {
        }

        public override void Update()
        {
            base.Update();
            if (!_isOpen)
            {
                _hintVisible = (Player.Instance.Position - Position).Length() < 24f;
            }
            else
            {
                _hintVisible = false;
            }
            if (Input.Pressed("action") && _hintVisible)
            {
                Player.Instance.IgnoreAction = true;
                Opened();
                Player.Instance.IgnoreAction = false;
            }

            if (!_hintVisible) return;
            _timer += Engine.DeltaTime * 8f;
            _timer += Engine.DeltaTime * 8f;
            _hintPosition = new Vector2(Position.X, Position.Y + (float)Math.Sin(Math.Cos(_timer)) - 15);
        }

        public override void Render()
        {
            base.Render();
            if (_hintVisible)
                ButtonUI.Render(_hintPosition, "", Constants.DarkGreen, "action", 1f);
        }

    }
}
