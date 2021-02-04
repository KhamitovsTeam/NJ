using KTEngine;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Linq;

namespace Chip
{
    public class CS122_BossKilled : CutsceneEntity
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
            yield return 1f;

            Vector2 topRight = new Vector2(Engine.Instance.Screen.Width - 9, 9);
            Vector2 center = new Vector2(Engine.Instance.Screen.Width / 2f, Engine.Instance.Screen.Height / 2f);

            // Show rock
            Entity doorEntity = null;
            foreach (var entity in Scene.GetEntities().Where(entity => entity.GetType() == typeof(Rock) && ((Rock)entity).Type.Equals("end")))
            {
                doorEntity = entity;
            }
            if (doorEntity != null)
            {
                yield return CameraTo(doorEntity.Position - topRight, 2f);
                yield return ((Rock)doorEntity).Destroy();
            }

            // Show kittens
            Entity cageEntity = null;
            foreach (var entity in Scene.GetEntities().Where(entity => entity.GetType() == typeof(KittenInCage)))
            {
                cageEntity = entity;
            }
            if (cageEntity != null)
            {
                yield return CameraTo(cageEntity.Position - center, 2f);
                yield return ((KittenInCage)cageEntity).ReleaseKittens();
            }

            // Show player
            yield return CameraTo(Player.Instance.Position - center, 2f);
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