using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    public class Box : Actor
    {
        public Contents Content;

        private Animation sprite = new Animation(GFX.Game["objects/box"], 16, 16);
        private Powerup powerup;
        private float closeWait;
        private int _content;
        private int _type;
        private string _value;
        private float timer;
        private bool isOpen;

        //   private Graphic icon;
        private bool _hintVisible;
        private Vector2 _hintPosition;

        public enum Contents
        {
            Powerup,
            Life,
            Money
        }

        public Box()
            : base(0, 0)
        {
            Depth = 1;

            MoveCollider = Add(new Hitbox(-8, -8, 16, 16));
            MoveCollider.Tag((int)Tags.Box);
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _content = xml.AttrInt("content");
            _type = xml.AttrInt("type");
            _value = xml.Attr("value");

            Content = (Contents)_content;

            Add(sprite);
            sprite.Add("idle", 9f, 0, 1, 2, 3, 4, 5);
            sprite.Add("open", 9f, 7);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");

            // Если сундук был сохранён, то открываем 
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Boxes == null) return;
            foreach (var box in levelData.Boxes)
            {
                if (ID != box.ID) continue;
                isOpen = true;
                sprite.Play("idle");
                break;
            }
        }

        public void Opened()
        {
            isOpen = true;
            if (Content == Contents.Powerup)
            {
                switch (_value)
                {
                    case "claws":
                        powerup = Powerups.Claws;
                        break;
                    case "rocket_boots":
                        powerup = Powerups.RocketBoots;
                        break;
                    case "spiked_boots":
                        powerup = Powerups.SpikedBoots;
                        break;
                    case "levitation_boots":
                        powerup = Powerups.LevitationBoots;
                        break;
                    case "jetpack":
                        powerup = Powerups.Jetpack;
                        break;
                    case "wallbreaker_gun":
                        powerup = Powerups.WallBreakerGun;
                        break;
                }

                Player.Instance.AnimationGotItem();

                OverlayPowerup.Instance.Title = powerup.Name;
                OverlayPowerup.Instance.Powerup = powerup;
                OverlayPowerup.Instance.Show();

                switch (powerup.Type)
                {
                    /*case PowerupType.Weapon:
                        Player.Instance.Weapon = powerup;
                        break;*/
                    case PowerupType.Body:
                        Player.Instance.PlayerData.Body.Add(powerup);
                        break;
                    case PowerupType.Footwear:
                        Player.Instance.PlayerData.Footwear.Add(powerup);
                        break;
                    case PowerupType.Armwear:
                        Player.Instance.PlayerData.Armwear.Add(powerup);
                        break;

                }

                /*if (powerup.Description != null)
                    description = new Text(Fonts.MainFont, powerup.Description, Position, Constants.DarkGreen, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);*/
            }
            SFX.Play("chest");

            // Добавляем в список ящиков
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData == null)
            {
                levelData = new LevelData(Level.ID);
                Level.Session.LevelsData.Add(levelData);
            }

        }

        public void Closed()
        {
            OverlayPowerup.Instance.Hide();
        }

        public override void Update()
        {
            base.Update();
            closeWait -= Engine.DeltaTime;
            if (closeWait <= 0.0 && isOpen)
            {
                if (!Input.Pressed("cancel"))
                {
                    goto update;
                }
                Closed();
            }
        update:
            if (!isOpen)
            {
                _hintVisible = (Player.Instance.Position - Position).Length() < 24.0;
            }
            else
            {
                sprite.Play("open");
                _hintVisible = false;
            }
            if (Input.Pressed("action") && _hintVisible)
            {
                Player.Instance.IgnoreAction = true;
                Opened();
                Player.Instance.IgnoreAction = false;
            }

            if (!_hintVisible) return;
            timer += Engine.DeltaTime * 8f;
            _hintPosition = new Vector2(Position.X, Position.Y + (float)Math.Sin(Math.Cos(timer)) - 12);
        }

        public override void Render()
        {
            base.Render();
            if (_hintVisible)
                ButtonUI.Render(_hintPosition, "", Constants.LightGreen, "action", 1f);
        }
    }
}
