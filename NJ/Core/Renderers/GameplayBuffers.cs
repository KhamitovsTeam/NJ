using System.Collections.Generic;
using KTEngine;

namespace Chip
{
    public static class GameplayBuffers
    {
        private static List<VirtualRenderTarget> all = new List<VirtualRenderTarget>();
        
        public static VirtualRenderTarget Gameplay;
        public static VirtualRenderTarget Level;

        public static void Create()
        {
            Unload();
            Gameplay = Create(320, 180);
            Level = Create(320, 180);
        }

        private static VirtualRenderTarget Create(int width, int height)
        {
            VirtualRenderTarget renderTarget = VirtualContent.CreateRenderTarget("gameplay-buffer-" + all.Count, width, height);
            all.Add(renderTarget);
            return renderTarget;
        }

        public static void Unload()
        {
            foreach (VirtualAsset virtualAsset in all)
                virtualAsset.Dispose();
            all.Clear();
        }
    }
}