using KTEngine;
using System.Collections;

namespace Chip
{
    public class CS100_RocketLanding : CutsceneEntity
    {
        private Player player;
        private Coroutine _coroutine;
        //private Level _level;

        public CS100_RocketLanding(Player player)
        {
            this.player = player;
        }

        public override void OnBegin(Level level)
        {
            _coroutine = new Coroutine(Cutscene());
            Add(_coroutine);
        }

        public override void OnEnd(Level level)
        {
            player.StateMachine.Set("normal");
        }

        private IEnumerator Cutscene()
        {
            player.StateMachine.Set("cutscene");
            player.Visible = false;
            yield return 1.0f;
            player.Visible = true;
            yield return 0f;
            EndCutscene(Level);
        }
    }
}