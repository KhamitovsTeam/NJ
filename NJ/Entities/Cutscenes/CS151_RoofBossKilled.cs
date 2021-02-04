using KTEngine;
using System.Collections;

namespace Chip
{
    public class CS151_RoofBossKilled : CutsceneEntity
    {
        private Coroutine _coroutine;
        //private States State;

        public override void OnBegin(Level level)
        {
            level.LockCamera = false;

            _coroutine = new Coroutine(Cutscene());
            Add(_coroutine);
        }

        public override void OnEnd(Level level)
        {
            Music.Play("world_1", true);
        }

        private IEnumerator Cutscene()
        {
            EndCutscene(Level);
            yield return 0f;
        }

        private enum States
        {
            ShowExit,
            ShakeCamera,
            ShowPlayer
        }
    }
}