using KTEngine;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Linq;

namespace Chip
{
    public class CS120_ShowElevator : CutsceneEntity
    {
        private Coroutine _coroutine;

        public override void OnBegin(Level level)
        {
            _coroutine = new Coroutine(Cutscene());
            Add(_coroutine);
        }

        public override void OnEnd(Level level)
        {
            Player.Instance.StateMachine.Set("normal");
        }

        private IEnumerator Cutscene()
        {
            Player.Instance.StateMachine.Set("cutscene");
            Entity elevator = null;
            foreach (var entity in Scene.GetEntities().Where(entity => entity.GetType() == typeof(ElevatorRope)))
            {
                elevator = entity;
            }
            Vector2 center = new Vector2(Engine.Instance.Screen.Width / 2f, Engine.Instance.Screen.Height / 2f);
            yield return CameraTo(elevator.Position - center, 2f);
            yield return 0.2f;
            ((ElevatorRope)elevator).TurnOn();
            yield return 2f;
            yield return CameraTo(Player.Instance.Position - center, 2f);
            EndCutscene(Level);
        }
    }
}