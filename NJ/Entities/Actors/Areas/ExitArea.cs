using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    /// <summary>
    /// Автоматическая зона для выхода. Используется на уровне в местах, где игрок должен перейти из одного уровня
    /// в другой. Работает в автоматическом режиме, при касании переключает на другой уровень заданный в параметре "to".
    /// В параметр "from" записываем текущий уровень, чтобы знать откуда мы пришли и заспавнили игрока в нужной точке.
    /// </summary>
    public class ExitArea : Actor
    {
        public bool Active;
        public string Type;

        private int width;
        private int height;
        private string next;
        private string prev;

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            width = xml.AttrInt("width");
            height = xml.AttrInt("height");
            next = xml.Attr("to");
            prev = xml.Attr("from");

            MoveCollider = Add(new Hitbox(-8, -height / 2, width, height));

            Active = xml.AttrBool("active", false);
            Type = xml.Attr("type");
        }

        public override void Update()
        {
            base.Update();
            Collider collider = MoveCollider.Collide((int)Tags.Player);
            if (collider == null)
                return;
            if (next.Equals("end"))
            {
                Player.Instance.StateMachine.Set(Player.StateEndGame);
            }
            else
            {
                if (UserIO.Saving) return;
                SaveData.Instance.CurrentSession.LastCheckpoint = Position;
                SaveData.Instance.CurrentSession.ToLevel = next;
                SaveData.Instance.CurrentSession.FromLevel = prev;
                UserIO.SaveHandler(true, false, () =>
                {
                    Engine.Scene = new Loader(Level.Session/*, Player.Instance*/);
                });
            }
        }
    }
}
