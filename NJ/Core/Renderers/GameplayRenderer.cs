using KTEngine;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Chip
{
    public class GameplayRenderer : Renderer
    {
        public Camera Camera;
        private static GameplayRenderer instance;

        public GameplayRenderer()
        {
            instance = this;
            Camera = new Camera(320, 180);
        }

        public static void Begin()
        {
            Draw.SpriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, instance.Camera.Matrix);
        }

        public override void Render(Scene scene)
        {
            Begin();
            //scene.GetEntities().RenderExcept((int) Tags.HUD);
            List<Entity> entities = scene.GetEntities();
            foreach (var entity in entities)
            {
                entity.Render();
            }
            //if (Engine.Commands.Open)
            //scene.Entities.DebugRender(Camera);
            End();
        }

        public static void End()
        {
            Draw.SpriteBatch.End();
        }
    }
}