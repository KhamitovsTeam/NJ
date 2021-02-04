using KTEngine;
using System.Collections;
using System.Linq;

namespace Chip
{
    public class CS123_KittensSaved : CutsceneEntity
    {
        public override void OnBegin(Level level)
        {
            Level.InCutscene = false;
            Add(new Coroutine(Cutscene()));
        }

        public override void OnEnd(Level level)
        {
        }

        private IEnumerator Cutscene()
        {
            Engine.Instance.CurrentCamera.Shake(4f, 0.5f);
            Input.Rumble(Input.RumbleStrength.Strong, Input.RumbleLength.Short);
            Elevator elevator = null;
            ElevatorRope elevatorRope = null;
            foreach (var entity in Level.GetEntities().Where(entity => entity.GetType() == typeof(ElevatorRope)))
            {
                elevatorRope = (ElevatorRope)entity;
            }
            if (elevatorRope != null)
            {
                // Добавляем в список лифтов
                var levelData = Level.Session.GetLevelData(Level.ID);
                if (levelData == null)
                {
                    levelData = new LevelData(Level.ID);
                    Level.Session.LevelsData.Add(levelData);
                }
                levelData.Elevators.Add(new EntityID(Level.ID, elevatorRope.ID));
            }

            foreach (var entity in Level.GetEntities().Where(entity => entity.GetType() == typeof(Elevator)))
            {
                elevator = (Elevator)entity;
                elevator.Sprite.Visible = false;
                elevator.MoveCollider.Collidable = false;
            }
            if (elevator != null)
            {
                ExplodePieces.Burst(elevator.Position, 30);
                SFX.Play("destroy");
            }
            EndCutscene(Level);
            yield return 0f;
        }
    }
}