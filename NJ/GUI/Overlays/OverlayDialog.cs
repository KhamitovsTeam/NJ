using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class OverlayDialog : OverlayComponent
    {
        public static OverlayDialog Instance;
        public DialogItem DialogItem;
        public Graphic Avatar = new Graphic(GFX.Gui["player_face"]);

        private float closeButtonPadding;
        private const float DialogOffset = 5f;

        private Vector2 dialogPosition;
        private Vector2 dialogSize;

        private readonly Text text = new Text(Fonts.MainFont, "text", Vector2.One, Constants.Dark, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
        
        private readonly Graphic topLeft;
        private readonly Graphic topMiddle;
        private readonly Graphic topRight;
        private readonly Graphic middleLeft;
        private readonly Graphic middleRight;
        private readonly Graphic bottomLeft;
        private readonly Graphic bottomMiddle;
        private readonly Graphic bottomRight;

        private string message = "";

        public OverlayDialog(Level level = null)
            : base(level)
        {
            Instance = this;

            dialogPosition = new Vector2(DialogOffset, Engine.Instance.Screen.Height - 50);
            dialogSize = Vector2.Zero;

            topLeft = new Graphic(GFX.Gui["dialog_frame"], new Rectangle(0, 0, 8, 8));
            topMiddle = new Graphic(GFX.Gui["dialog_frame"], new Rectangle(8, 0, 8, 8));
            topRight = new Graphic(GFX.Gui["dialog_frame"], new Rectangle(16, 0, 8, 8));

            middleLeft = new Graphic(GFX.Gui["dialog_frame"], new Rectangle(0, 8, 8, 8));
            middleRight = new Graphic(GFX.Gui["dialog_frame"], new Rectangle(16, 8, 8, 8));

            bottomLeft = new Graphic(GFX.Gui["dialog_frame"], new Rectangle(0, 16, 8, 8));
            bottomMiddle = new Graphic(GFX.Gui["dialog_frame"], new Rectangle(8, 16, 8, 8));
            bottomRight = new Graphic(GFX.Gui["dialog_frame"], new Rectangle(16, 16, 8, 8));

            //avatar = new Graphic(GFX.Gui["player_face"]);
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

            Draw.Rect(dialogPosition.X + 2, dialogPosition.Y - DialogOffset + 2, dialogSize.X - 4, dialogSize.Y - 4, Constants.Dark);

            topLeft.RenderAt(dialogPosition.X, dialogPosition.Y - DialogOffset);
            topMiddle.RenderAt(dialogPosition.X + topLeft.Width / 2f, dialogPosition.Y - DialogOffset);
            topRight.RenderAt(dialogPosition.X + dialogSize.X - topRight.Width, dialogPosition.Y - DialogOffset);

            middleLeft.RenderAt(dialogPosition.X, dialogPosition.Y + topLeft.Height / 2f - DialogOffset);
            middleRight.RenderAt(dialogPosition.X + dialogSize.X - middleRight.Width, dialogPosition.Y + topRight.Height / 2f - DialogOffset);

            bottomLeft.RenderAt(dialogPosition.X, dialogPosition.Y + dialogSize.Y - bottomLeft.Height - DialogOffset);
            bottomMiddle.RenderAt(dialogPosition.X + bottomLeft.Width / 2f, dialogPosition.Y + dialogSize.Y - bottomMiddle.Height - DialogOffset);
            bottomRight.RenderAt(dialogPosition.X + dialogSize.X - bottomRight.Width, dialogPosition.Y + dialogSize.Y - bottomRight.Height - DialogOffset);

            text.DrawText = Calc.WrapText(Fonts.MainFont, message, dialogSize.X - Avatar.Width - DialogOffset * 4);
            text.Color = Constants.Dark;
            text.Position.X = Engine.Instance.CurrentCamera.Render.X + Avatar.Width + DialogOffset * 4;
            text.Position.Y = Engine.Instance.CurrentCamera.Render.Y + dialogPosition.Y + DialogOffset;
            text.Alpha = Alpha;
            text.Render();

            Avatar.RenderAt(new Vector2(dialogPosition.X + DialogOffset * 2, dialogPosition.Y - Avatar.Height / 2f));

            ButtonUI.Render(new Vector2(closeButtonPadding, dialogPosition.Y + dialogSize.Y - ButtonUI.Height("", "cancel")), "", Constants.Dark, "cancel", 1f);

            Draw.ResetOffset();
        }

        private void UpdateFrame()
        {
            text.DrawText = Calc.WrapText(Fonts.MainFont, message, Engine.Instance.Screen.Width - DialogOffset * 4);
            dialogSize = new Vector2(Engine.Instance.Screen.Width - DialogOffset * 2, text.Height + DialogOffset * 4);
            dialogPosition = new Vector2(DialogOffset, Engine.Instance.Screen.Height - dialogSize.Y);

            topMiddle.Scale = new Vector2((dialogSize.X - 8) / 8, 1);
            middleLeft.Scale = new Vector2(1, (dialogSize.Y - 8) / 8);
            middleRight.Scale = new Vector2(1, (dialogSize.Y - 8) / 8);
            bottomMiddle.Scale = new Vector2((dialogSize.X - 8) / 8, 1);
        }
    }
}