using Microsoft.Xna.Framework;
using System;

namespace Chip
{
    public class MenuButton : MenuItem
    {
        private string name;
        private Action confirmAction;
        private Color activeColor = Color.Black;
        private Color disabledColor = Color.Gray;

        public MenuButton(int x, int y, string name, Action confirmAction)
        {
            this.name = name;
            this.confirmAction = confirmAction;
            Position = new Vector2(x, y);
        }

        public override void Render()
        {
            base.Render();
            //Draw.Text(font, this.name, this.Position * Engine.Scale, this.Active ? activeColor : disabledColor, new Vector2(0,0), Engine.Scale, 0);
            //Draw.Text(Position, Selected ? ">> " + name : name, );
        }

        protected override void OnConfirm()
        {
            if (confirmAction != null)
                confirmAction();
        }

        protected override void OnSelect()
        {

        }

        protected override void OnDeselect()
        {

        }

        public override void Update()
        {
            base.Update();
            if (!Selected)
                return;
            if (Selected/* && Confirm.Released*/)
            {
                OnConfirm();
            }
        }
    }
}