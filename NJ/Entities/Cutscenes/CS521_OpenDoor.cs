using KTEngine;
using System.Collections;
using System.Linq;

namespace Chip
{
    public class CS521_OpenDoor : CutsceneEntity
    {
        private Coroutine _coroutine;

        public override void OnBegin(Level level)
        {
            _coroutine = Add(new Coroutine(OpenDoor()));
        }

        public override void OnEnd(Level level)
        {

        }

        private IEnumerator OpenDoor()
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