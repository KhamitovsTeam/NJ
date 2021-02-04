using KTEngine;
using System.Collections;

namespace Chip
{
    public class CS225_OpenDoor1 : CutsceneEntity
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
            // Open #1 door (02_01_02 room)
            // Move camera
            EndCutscene(Level);
            yield return 0f;
        }
    }
}