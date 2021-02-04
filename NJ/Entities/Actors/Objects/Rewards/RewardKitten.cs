using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    class RewardKitten : Actor
    {
        public bool IsCatched = false;

        private Animation _sprite;
        private int _type = 0;
        private StateMachine _state;
        private LightLine _lightLine;
        private float _timer;
        private float _speed;
        private float _accleration;

        private const float LIGHT_TIME = 0.3f;
        private const float CAT_TIME = 1.7f;

        public RewardKitten()
            : base(0, 0)
        {
            _sprite = new Animation(GFX.Game["objects/kitten" + _type], 16, 16);
            Add(_sprite);
            _sprite.Add("idle", 1.5f, 0, 1, 2, 3);
            _sprite.Add("empty", 1.5f, 4);
            _sprite.Add("fly", 1.5f, 5);
            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;
            _sprite.Play("idle");

            Depth = -6;

            _state = new StateMachine();
            _state.Add("idle", IdleBegin, IdleUpdate, IdleEnd);
            _state.Add("light_show", LightShowBegin, LightShowUpdate);
            _state.Add("cat_up", CatUpBegin, CatUpUpdate);
            _state.Add("light_hide", LightHideBegin, LightHideUpdate, LightHideEnd);
            Add(_state);

            _state.Set("idle");

            _lightLine = Add(new LightLine());
            _lightLine.Width = 0f;
            _lightLine.Height = 0f;

            MoveCollider = Add(new Hitbox(-5, -4, 10, 12));
            MoveCollider.Tag((int)Tags.Sceneries);

            Fall(3000f); // падение моне
        }

        private void IdleBegin()
        {

        }

        private void IdleUpdate()
        {

        }

        private void IdleEnd()
        {

        }

        private void LightShowBegin()
        {
            SFX.Play("cat");
            MoveCollider.Collidable = false;
            _timer = LIGHT_TIME;
            _speed = 10f / _timer;
            _lightLine.Width = 0f;
            _lightLine.Height = Math.Min(Position.Y + 16f, Constants.GameHeight * 1.75f);
        }

        private void LightShowUpdate()
        {
            _timer -= Engine.DeltaTime;
            _accleration = _speed * Engine.DeltaTime;
            _lightLine.Width += _accleration;
            if (_timer > 0f)
                return;
            _state.Set("cat_up");
        }

        private void CatUpBegin()
        {
            _sprite.Play("fly");
            _timer = CAT_TIME;
            _speed = Constants.GameHeight * 1.75f / _timer;
        }

        private void CatUpUpdate()
        {
            _timer -= Engine.DeltaTime;
            _accleration = _speed * Engine.DeltaTime;
            _sprite.Position.Y -= _accleration;
            if (_timer > 0f)
                return;
            _state.Set("light_hide");
        }

        private void LightHideBegin()
        {
            _timer = LIGHT_TIME;
            _speed = 10f / _timer;
        }

        private void LightHideUpdate()
        {
            _timer -= Engine.DeltaTime;
            _accleration = _speed * Engine.DeltaTime;
            _lightLine.Width -= _accleration;
            if (_timer > 0f)
                return;
            _state.Set("idle");
        }

        private void LightHideEnd()
        {
            // Добавляем в список собранных котиков
            if (Level != null)
            {
                var levelData = Level.Session.GetLevelData(Level.ID);
                if (levelData == null)
                {
                    levelData = new LevelData(Level.ID);
                    Level.Session.LevelsData.Add(levelData);
                }
                levelData.Kittens.Add(new EntityID(Level.ID, ID));
            }
            Scene.Remove(this);
            Player.Instance.PlayerData.Kittens += 1;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            
            // Если котик был сохранён, то не показываем 
           /* var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Kittens == null) return;
            foreach (var kitten in levelData.Kittens)
            {
                if (ID != kitten.ID) continue;
                _sprite.Play("empty");
                MoveCollider.Collidable = false;
            }*/
        }

        public void Define(Vector2 position)
        {
            Position = position;
            MoveCollider.Reset(-8, -8, 16, 16);
            _sprite.Visible = true;
        }

        public override void Update()
        {
            base.Update();

            if (IsCatched)
            {
                _sprite.Play("empty");
                MoveCollider.Collidable = false;
                return;
            }

            if (MoveCollider != null && MoveCollider.Check((int)Tags.Player))
            {
                if (_state.State == "idle")
                {
                    _state.Set("light_show");
                }
            }
            Move();
        }

        public override void Render()
        {

            base.Render();
            _sprite.Render();
        }

        public static void Born(Room room, Vector2 position)
        {

            RewardKitten entity = new RewardKitten();
            entity.Room = room;
            entity.Define(new Vector2(position.X, position.Y - 2));
            Engine.Scene.Add(entity, "default");
        }
    }
}
