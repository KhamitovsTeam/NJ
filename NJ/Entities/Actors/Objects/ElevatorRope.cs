using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class ElevatorRope : Actor
    {
        private int _height;
        private int _direction = -1;
        private float _timer;
        private float _walkSpeed;
        private bool _cutsceneIsShowed = false;

        private Elevator _elevator;
        private StateMachine _state;

        /// <summary>  
        /// Gets whether or not the player's feet are on the MovableTile.  
        /// </summary>  
        public bool PlayerIsOn { get; set; }

        public ElevatorRope()
            : base(0, 0)
        {
            Depth = 5;

            // states
            _state = Add(new StateMachine());
            _state.Add("work", BeginWork, UpdateWork);
            _state.Add("turn", BeginTurn, UpdateTurn);
            _state.Add("cutscene", BeginCutscene, UpdateCutscene, EndCutscene);
            _state.Add("idle");

            _state.Set("idle");
            _walkSpeed = 24f;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _height = xml.AttrInt("height");
            MoveCollider = Add(new Hitbox(-8, -_height / 2, 16, _height));
            MoveCollider.Tag((int)Tags.Rope);

            var count = _height / 16;
            for (var i = 0; i < count; i++)
            {
                Add(new Graphic(GFX.Game["objects/moveplatform_rope"])
                {
                    X = -MoveCollider.Width / 2f,
                    Y = i * 16 - MoveCollider.Height / 2f
                });
            }

            // Если лифт был сохранён, то не отображаем платформу 
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Elevators != null)
            {
                foreach (var elevator in levelData.Elevators)
                {
                    if (ID == elevator.ID) return;
                }
            }

            _elevator = new Elevator
            {
                X = X,
                Y = Y
            };
            Scene.Add(_elevator);
        }

        public override void Update()
        {
            if (_elevator != null)
            {
                PlayerIsOn = _elevator.MoveCollider.Check(0, -1, (int)Tags.Player);
                if (PlayerIsOn && !_cutsceneIsShowed)
                {
                    Level.InCutscene = true;
                    Player.Instance.Speed = Vector2.Zero;
                    Player.Instance.Position.X = _elevator.Position.X;
                    Player.Instance.Position.Y = _elevator.Position.Y;
                    _cutsceneIsShowed = true;
                    _state.Set("cutscene");
                }
                foreach (var collider in _elevator.MoveCollider.CollideAll(0, -1, (int)Tags.Player))
                {
                    if (!(collider.Entity is Actor)) continue;
                    var entity = (Actor)collider.Entity;
                    entity.Position.Y = _elevator.MoveCollider.ScenePosition.Y - _elevator.MoveCollider.Height;
                }
            }
            base.Update();
        }

        private void BeginWork()
        {
            _direction = -_direction;
            SFX.Play("elevator");
        }

        private void UpdateWork()
        {
            if (_elevator == null) return;
            _elevator.Speed.Y = _direction * _walkSpeed;
            _elevator.Move();

            if (_elevator.Position.Y >= MoveCollider.ScenePosition.Y + MoveCollider.Height && _direction >= 0)
            {
                if (PlayerIsOn)
                    _state.Set("turn");
            }
            else if (_elevator.Position.Y < MoveCollider.ScenePosition.Y + _elevator.MoveCollider.Height + 1 &&
                     _direction <= 0)
            {
                _state.Set("turn");
            }
        }

        private void BeginTurn()
        {
            _timer = 2f;
        }

        private void UpdateTurn()
        {
            _timer -= Engine.DeltaTime;
            if (_timer <= 0.0)
                _state.Set("work");
        }

        private void BeginCutscene()
        {
            _direction = -1;
            var cut = new CS121_StartElevator
            {
                Level = Engine.Scene as Level
            };
            cut.Start();
            Engine.Scene.Add(cut);
        }

        private void UpdateCutscene()
        {
            if (_elevator == null) return;
            _elevator.Speed.Y = _direction * _walkSpeed;
            _elevator.Move();
            if (_elevator.Position.Y < MoveCollider.ScenePosition.Y + _elevator.MoveCollider.Height + 1 &&
                _direction <= 0)
            {
                _state.Set("idle");
            }
        }

        private void EndCutscene()
        {

        }

        public void TurnOn()
        {
            Active = true;
            _state.Set("work");
        }

        public void TurnOff()
        {
            _state.Set("idle");
        }
    }
}