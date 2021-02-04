using KTEngine;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Linq;

namespace Chip
{
    public class CS325_BossKilled : CutsceneEntity
    {
        private Coroutine _coroutine;

        public override void OnBegin(Level level)
        {
            level.LockCamera = false;

            _coroutine = new Coroutine(Cutscene());
            Add(_coroutine);
            
        }

        public override void OnEnd(Level level)
        {
            Music.Play("world_3", true);
        }

        private IEnumerator Cutscene()
        {
            yield return 1f;
            Blocker blockerEntity = null;
            foreach (Blocker entity in Engine.Scene.GetEntities().Where(entity => entity.GetType() == typeof(Blocker)))
            {
                blockerEntity = entity;
                if (blockerEntity != null)
                {
                    blockerEntity.Y -= 48f;
                }
            }
            EndCutscene(Level);
            yield return 0f;
        }
    }
}