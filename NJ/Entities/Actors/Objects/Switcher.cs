using System;
using System.Xml;
using Microsoft.Xna.Framework;
using KTEngine;

namespace Chip
{
    public class Switcher : Actor
    {
        private Animation sprite;
        private string cutscene;
        private float timer;
        private bool isOpen;
        private bool hintVisible;
        private Vector2 hintPosition;

        public Switcher()
            : base(0, 0)
        {
            Depth = 1;
            
            sprite = Add(new Animation(GFX.Game["objects/switcher"], 16, 16));
            sprite.Add("idle", 16f, 0, 1, 2, 3, 4, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            sprite.Add("moving", 8f, false, 0, 7, 8, 7);
            sprite.Add("off", 0f, 7);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 10f;
            sprite.Play("idle");
            sprite.OnFinish += animation =>
            {
                if (animation.Equals("moving"))
                {
                    sprite.Play("off");
                }
            };
            
            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int) Tags.Computer);
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            cutscene = xml.Attr("cutscene");
        }

        public void Opened()
        {
            if (!isOpen)
            {
                sprite.Play("moving");
            }
            isOpen = true;
            Level = Engine.Scene as Level;
            
            CutsceneEntity cutsceneEntity = Cutscenes.AllCutscenes[this.cutscene];
            if (cutsceneEntity != null)
            {
                cutsceneEntity.Level = Level;
                cutsceneEntity.Start();
                Level?.Add(cutsceneEntity);
            }
        }

        public void Closed()
        {
            
        }

        public override void Update()
        {
            base.Update();
            if (!isOpen)
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
                Opened();
                Player.Instance.IgnoreAction = false;
            }

            if (!hintVisible) return;
            timer += Engine.DeltaTime * 8f;
            timer += Engine.DeltaTime * 8f;
            hintPosition = new Vector2(Position.X, Position.Y + (float) Math.Sin(Math.Cos(timer)) - 15);
        }

        public override void Render()
        {
            base.Render();
            if (hintVisible)
                ButtonUI.Render(hintPosition, "", Constants.Dark, "action", 1f);
        }
    }
}
