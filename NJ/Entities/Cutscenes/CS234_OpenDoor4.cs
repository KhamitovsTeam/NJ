using KTEngine;
using System.Collections;

namespace Chip
{
    public class CS234_OpenDoor4 : CutsceneEntity
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
            // Open #4 door (02_01_04 room)
            // Move camera
            EndCutscene(Level);
            yield return 0f;
        }
    }
}