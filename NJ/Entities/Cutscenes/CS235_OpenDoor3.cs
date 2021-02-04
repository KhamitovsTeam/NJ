using KTEngine;
using System.Collections;

namespace Chip
{
    public class CS235_OpenDoor3 : CutsceneEntity
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
            // Open #3 door to boss in deep (02_03_05 room)
            // Move camera
            EndCutscene(Level);
            yield return 0f;
        }
    }
}