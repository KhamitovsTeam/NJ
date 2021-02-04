using KTEngine;

namespace Chip
{
    public class GlobalDialog : Entity
    {
        private static GlobalDialog instance;
        
        public static void Show()
        {
            if (instance != null)
                Engine.Scene.Remove(instance);
            Engine.Scene.Add((Entity)(instance = new GlobalDialog()));
        }

        public static void Hide()
        {
            instance?.RemoveSelf();
            instance = null;
        }

        public override void Render()
        {
            if (instance == null) return;
            base.Render();
        }
    }
}