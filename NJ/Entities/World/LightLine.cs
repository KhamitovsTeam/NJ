using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class LightLine : Graphic
    {
        public float Width;
        public float Height;
        public LightLine()
            : base(null, new Rectangle(0, 0, 0, 0))
        {

        }

        public override void Render()
        {
            var renderPos = ScenePosition;
            renderPos.X = renderPos.X - Width / 2f;
            renderPos.Y = renderPos.Y - Height + 9f;
            //renderPos.Y = 0f;
            RenderAt(renderPos);
        }

        public override void RenderAt(Vector2 position)
        {
            //Alpha = 0.3f;
            Draw.Rect((position - Origin * Scale).X, (position - Origin * Scale).Y, Width * Scale.X, Height * Scale.Y, Constants.Dark, 0.9f);
            Draw.Rect((position - Origin * Scale).X, (position - Origin * Scale).Y, 1f, Height * Scale.Y, Constants.Dark, 1f);
            Draw.Rect((position - Origin * Scale).X + Width * Scale.X, (position - Origin * Scale).Y, 1f, Height * Scale.Y, Constants.Dark, 1f);
        }
    }
}