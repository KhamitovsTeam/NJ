using KTEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace Chip
{
    public class Kitten : Actor
    {
        public bool IsCatched = false;

        private Animation _sprite;
        private int _type;
        private StateMachine _state;
        private LightLine _lightLine;
        private float _timer;
        private float _speed;
        private float _accleration;

        private const float LIGHT_TIME = 0.3f;
        private const float CAT_TIME = 1.7f;

        public Kitten()
            : base(0, 0)
        {
            Depth = -6;
            
            CreateSprite();
            
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
        }

        public Kitten(int type, Vector2 position)
            : this()
        {
            Remove(_sprite);
            CreateSprite(type);
            
            Position = position;
            Depth = -6;
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
            _type = xml.AttrInt("type");

            Remove(_sprite);
            CreateSprite(_type);
            
            // Если котик был сохранён, то не показываем 
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Kittens == null) return;
            foreach (var kitten in levelData.Kittens)
            {
                if (ID != kitten.ID) continue;
                _sprite.Play("empty");
                MoveCollider.Collidable = false;
            }
        }

        public override void Update()
        {
            base.Update();

            if (IsCatched)
            {
                _sprite?.Play("empty");
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
        }

        public override void Render()
        {

            base.Render();
            _sprite?.Render();
        }

        private void CreateSprite(int type = 0)
        {
            _sprite = new Animation(GFX.Game["objects/kitten" + type], 16, 16);
            _sprite.Add("idle", 1.5f, 0, 1, 2, 3);
            _sprite.Add("empty", 1.5f, 4);
            _sprite.Add("fly", 1.5f, 5);
            _sprite.Origin.X = 8f;
            _sprite.Origin.Y = 8f;
            _sprite.Play("idle");
            Add(_sprite);
        }
    }
}
