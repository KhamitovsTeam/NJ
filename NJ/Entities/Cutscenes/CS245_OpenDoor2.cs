using KTEngine;
using System.Collections;

namespace Chip
{
    public class CS245_OpenDoor2 : CutsceneEntity
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
            // Open #2 door (02_02_02 room)
            // Move camera
            EndCutscene(Level);
            yield return 0f;
        }
    }
}