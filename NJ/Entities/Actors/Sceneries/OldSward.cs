using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    public class OldSward : Actor
    {
        private Animation _sprite;
        private float timer;
        private bool isShown;
        private bool hintVisible;
        private Vector2 hintPosition;

        public OldSward()
            : base(0, 0)
        {
            Depth = 6;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            _sprite = new Animation(GFX.Game["sceneries/old_sward"], 23, 23);
            Add(_sprite);

            _sprite.Add("idle", 11f, 0);
            _sprite.Origin.X = 11f;
            _sprite.Origin.Y = 15f;
            _sprite.Play("idle");
        }
        public override void Update()
        {
            base.Update();
            if (!isShown)
            {
                hintVisible = (Player.Instance.Position - Position).Length() < 24f;
            }
            else
            {
                hintVisible = false;
            }
            if (Input.Pressed("action") && hintVisible)
            {
                Player.Instance.IgnoreAction = true;
                ShowDialog();
                Player.Instance.IgnoreAction = false;
            }

            if (!hintVisible) return;
            timer += Engine.DeltaTime * 8f;
            timer += Engine.DeltaTime * 8f;
            hintPosition = new Vector2(Position.X, Position.Y + (float)Math.Sin(Math.Cos(timer)) - 20);
            
        }

        private void ShowDialog()
        {
            OverlayDialog.Instance.DialogItem = new DialogItem(Texts.MainText["dialog_old_sward"]);
            //OverlayDialog.Instance.Avatar = new Graphic(GFX.Gui["sward_face"]);
            OverlayDialog.Instance.Show();
            isShown = true;
        }

        public override void Render()
        {
            base.Render();
            if (hintVisible)
                ButtonUI.Render(hintPosition, "", Constants.Dark, "action", 1f);
        }
    }
}
