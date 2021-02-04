using KTEngine;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Chip
{
    public class CS121_StartElevator : CutsceneEntity
    {
        private Coroutine _coroutine;

        public override void OnBegin(Level level)
        {
            Player.Instance.StateMachine.Set("cutscene");
            _coroutine = new Coroutine(Cutscene());
            Add(_coroutine);
            Music.Play("boss", true);
        }

        public override void OnEnd(Level level)
        {
            level.LockCamera = true;
            Player.Instance.StateMachine.Set("normal");
        }

        private IEnumerator Cutscene()
        {
            yield return 2.0f;
            yield return MoveCamera();
            //yield return ShakeCamera();
            //yield return SpawnBoss();
            OnComplete();
        }

        private IEnumerator ShakeCamera()
        {
            Engine.Instance.CurrentCamera.Shake(4f, 1f);
            Input.Rumble(Input.RumbleStrength.Strong, Input.RumbleLength.FullSecond);
            yield return 0f;
        }

        private IEnumerator MoveCamera()
        {
            Engine.Instance.CurrentCamera.Position = new Vector2(328f, 295f);
            yield return 0.5f;
        }

        /*private IEnumerator SpawnBoss()
        {
            var boss = new Boss1(Player.Instance.PlayerData.Facing);
            if (Player.Instance.PlayerData.Facing > 0)
                boss.Position = new Vector2(544, 324);
            else
                boss.Position = new Vector2(434, 324);
            boss.Room = Level.GetPlayerRoom();
            Level.Add(boss);
            yield return 0f;
        }*/

        public void OnComplete()
        {
            EndCutscene(Level);
        }
    }
}