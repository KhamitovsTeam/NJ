using KTEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chip
{
    public class HiresRenderer : Renderer
    {
        public static VirtualRenderTarget Buffer
        {
            get
            {
                return NJGame.HudTarget;
            }
        }

        public static bool DrawToBuffer
        {
            get
            {
                if (Buffer == null)
                    return false;
                return Engine.ViewWidth < 1920 || Engine.ViewHeight < 1080;
            }
        }

        public static void BeginRender(BlendState blend = null, SamplerState sampler = null)
        {
            if (blend == null)
                blend = BlendState.AlphaBlend;
            if (sampler == null)
                sampler = SamplerState.LinearClamp;
            Matrix matrix = DrawToBuffer ? Matrix.Identity : Engine.ScreenMatrix;
            Draw.SpriteBatch.Begin(0, blend, sampler, DepthStencilState.Default, RasterizerState.CullNone, null, matrix);
        }

        public static void EndRender()
        {
            Draw.SpriteBatch.End();
        }

        public override void BeforeRender(Scene scene)
        {
            if (!DrawToBuffer)
                return;
            Engine.Graphics.GraphicsDevice.SetRenderTarget(Buffer);
            Engine.Graphics.GraphicsDevice.Clear(Color.Transparent);
            RenderContent(scene);
        }

        public override void Render(Scene scene)
        {
            if (DrawToBuffer)
            {
                Draw.SpriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Engine.ScreenMatrix);
                Draw.SpriteBatch.Draw(Buffer, new Vector2(-1f, -1f), Color.White);
                Draw.SpriteBatch.End();
            }
            else
                RenderContent(scene);
        }

        public virtual void RenderContent(Scene scene)
        {
        }
    }
}