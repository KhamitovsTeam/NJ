using KTEngine;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Chip
{
    public abstract class CutsceneEntity : Entity
    {
        public Level Level;

        public bool Running { get; private set; }

        public void Start()
        {
            Running = true;
            Level.StartCutscene();
            OnBegin(Level);
        }

        public void EndCutscene(Level level, bool removeSelf = true)
        {
            Running = false;
            OnEnd(level);
            level.EndCutscene();
            if (!removeSelf)
                return;
            Scene.Remove(this);
        }

        public abstract void OnBegin(Level level);

        public abstract void OnEnd(Level level);

        public static IEnumerator CameraTo(Vector2 target, float duration, Ease.Easer ease = null, float delay = 0.0f)
        {
            if (ease == null)
                ease = Ease.CubeInOut;
            if (delay > 0.0)
                yield return delay;
            var from = Engine.Instance.CurrentCamera.Position;
            var p = 0.0f;
            while (p < 1.0)
            {
                Engine.Instance.CurrentCamera.Position = from + (target - from) * ease(p);
                p += Engine.DeltaTime;
                yield return Engine.DeltaTime;
            }
        }
    }
}