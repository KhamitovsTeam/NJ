using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    /// <summary>
    /// Зона для выхода через дверь. Используется на уровне в местах, где игрок должен войти в помещение или секретную
    /// комнату. Срабатывает при нажатии на кнопку действия. В активной зоне показывает подсказку с клавишей активации.
    /// В параметре "to" указываем уровень, в который нужно перейти. В параметр "from" указываем текущий уровень, чтобы
    /// знать откуда мы пришли и заспавнили игрока в нужной точке.
    /// </summary>
    public class DoorArea : Actor
    {
        private int height;
        private float timer;
        private string next;
        private string prev;
        private bool hintVisible;
        private Vector2 hintPosition;

        public DoorArea()
        {
            Depth = -12;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            height = xml.AttrInt("height");

            MoveCollider = Add(new Hitbox(-8, -height / 2, 16, height));
            MoveCollider.Tag((int)Tags.Door);

            next = xml.Attr("to");
            prev = xml.Attr("from");
        }

        public override void Update()
        {
            base.Update();

            hintVisible = (Player.Instance.Position - Position).Length() < 12f;

            if (Input.Pressed("action") && hintVisible)
            {
                Player.Instance.IgnoreAction = true;
                if (UserIO.Saving) return;
                SaveData.Instance.CurrentSession.LastCheckpoint = Position;
                SaveData.Instance.CurrentSession.ToLevel = next;
                SaveData.Instance.CurrentSession.FromLevel = prev;
                UserIO.SaveHandler(true, false, () =>
                {
                    Player.Instance.IgnoreAction = false;
                    Engine.Scene = new Loader(Level.Session/*, Player.Instance*/);
                });
            }

            if (!hintVisible) return;
            timer += Engine.DeltaTime * 8f;
            hintPosition = new Vector2(Position.X, Position.Y + (float)Math.Sin(Math.Cos(timer)) - height);
        }

        public override void Render()
        {
            base.Render();
            if (hintVisible)
                ButtonUI.Render(hintPosition, "", Constants.Dark, "action", 1f, 0f, 0f, 1f);
        }
    }
}