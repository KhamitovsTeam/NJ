using KTEngine;
using System.Collections;
using System.Linq;

namespace Chip
{
    public class CS310_DisableLaser : CutsceneEntity
    {
        private Coroutine _coroutine;

        public override void OnBegin(Level level)
        {
            _coroutine = Add(new Coroutine(DisableLaser()));
        }

        public override void OnEnd(Level level)
        {

        }

        private IEnumerator DisableLaser()
        {
            // Disable laser
            Entity laserEntity = null;
            foreach (var entity in Scene.GetEntities().Where(entity => entity.GetType() == typeof(Laser) && ((Laser)entity).ID == 0))
            {
                laserEntity = entity;
            }
            ((Laser)laserEntity)?.Disable();
            EndCutscene(Level);
            yield return 0f;
        }
    }
}