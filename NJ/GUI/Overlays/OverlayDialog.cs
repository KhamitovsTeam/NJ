using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class OverlayDialog : OverlayComponent
    {
        public static OverlayDialog Instance;
        public DialogItem DialogItem;

        private float closeButtonPadding;
        private const float DialogOffset = 1f;

        private Vector2 dialogPosition;
        private Vector2 dialogSize;

        private readonly Text text = new Text(Fonts.MainFont, "text", Vector2.One, Constants.Dark, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);

        private string message = "";

        public OverlayDialog(Level level = null)
            : base(level)
        {
            Instance = this;

            dialogPosition = new Vector2(DialogOffset, Engine.Instance.Screen.Height - 50);
            dialogSize = Vector2.Zero;
        }

        public override void Show()
        {
            base.Show();
            Level.IsGamePaused = true;
            if (DialogItem == null || !DialogItem.HasNext()) return;
            message = DialogItem.Next();
            UpdateFrame();
        }

        public override void Hide()
        {
            base.Hide();
            Level.IsGamePaused = false;
        }

        public override void Update()
        {
            closeButtonPadding = Engine.Instance.Screen.Width - ButtonUI.Width("", "cancel") / 2f - DialogOffset;

            if (Input.Pressed("cancel"))
            {
                if (DialogItem.HasNext())
                {
                    message = DialogItem.Next();
                    UpdateFrame();
                }
                else
                {
                    Hide();
                }
            }
            base.Update();
        }

        public override void Render()
        {
            Draw.SetOffset(Engine.Instance.CurrentCamera.Render);

            Draw.Rect(dialogPosition.X, dialogPosition.Y - DialogOffset, dialogSize.X, dialogSize.Y, Constants.Light);
            Draw.HollowRect(dialogPosition.X, dialogPosition.Y - DialogOffset, dialogSize.X, dialogSize.Y, 1f, Constants.Dark);

            text.DrawText = Calc.WrapText(Fonts.MainFont, message, dialogSize.X - DialogOffset * 4);
            text.Color = Constants.Dark;
            text.Position.X = Engine.Instance.CurrentCamera.Render.X + DialogOffset * 4;
            text.Position.Y = Engine.Instance.CurrentCamera.Render.Y + dialogPosition.Y + DialogOffset;
            text.Alpha = Alpha;
            text.Render();

            //ButtonUI.Render(new Vector2(closeButtonPadding, dialogPosition.Y + dialogSize.Y - ButtonUI.Height("", "cancel")), "", Constants.Dark, "cancel", 1f);

            Draw.ResetOffset();
        }

        private void UpdateFrame()
        {
            text.DrawText = Calc.WrapText(Fonts.MainFont, message, Engine.Instance.Screen.Width - DialogOffset * 4);
            dialogSize = new Vector2(Engine.Instance.Screen.Width - DialogOffset * 2, text.Height + DialogOffset * 4);
            dialogPosition = new Vector2(DialogOffset, Engine.Instance.Screen.Height - dialogSize.Y);

            /*topMiddle.Scale = new Vector2((dialogSize.X - 8) / 8, 1);
            middleLeft.Scale = new Vector2(1, (dialogSize.Y - 8) / 8);
            middleRight.Scale = new Vector2(1, (dialogSize.Y - 8) / 8);
            bottomMiddle.Scale = new Vector2((dialogSize.X - 8) / 8, 1);*/
        }
    }
}