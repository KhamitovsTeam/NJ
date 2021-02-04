using KTEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Chip
{
    public class Sky : Actor
    {
        private int _type = 0;  //type sprite from XML
        private int _cnt = 3;   // count clouds from XML
        private float orignX = 8f;
        private float orignY = 8f;
        private Animation _sprite;
        private readonly StateMachine _state = new StateMachine();
        private int _speed = 16;
        private int _dropping = 1;
        private int _direction = 1;
        private readonly float _yAmplitude = 16f;
        private float _yAxis;
        private bool _turning;
        private float _turnTimer;

        public Sky()
            : base(0, 0)
        {

            Add(_state);
            _state.Add("normal", NormalBegin, NormalUpdate, NormalEnd);
            _state.Set("normal");


        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");
            _cnt = xml.AttrInt("count");

            for (int i = 0; i <= _cnt; i++)
            {
                switch (_type)
                {
                    case 0:
                        orignX = 8f - i * 64;
                        orignY = 12f;
                        break;
                    case 1:
                        orignX = 4f - i * 64;
                        orignY = 4f;
                        break;
                    case 2:
                        orignX = 12f - i * 64;
                        orignY = -4f;
                        break;
                    default:
                        orignX = 8f - i * 64;
                        orignY = 8f;
                        break;
                }

                _sprite = new Animation(GFX.Game["sceneries/sky" + _type], 64, 18);
                Add(_sprite);
                _sprite.Add("idle", 0f, 0);
                _sprite.Origin.X = orignX;
                _sprite.Origin.Y = orignY;
                _sprite.Play("idle");
            }

            MoveCollider = Add(new Hitbox(16, -4, 64 * _cnt - 18, 8));
            MoveCollider.Tag((int)Tags.Sceneries);

            switch (_type)
            {
                case 0:
                    Depth = 0;
                    _speed = 12;
                    break;
                case 1:
                    Depth = -1;
                    _speed = 8;
                    break;
                case 2:
                    Depth = -2;
                    _speed = 4;
                    break;
                default:
                    Depth = 0;
                    _speed = 12;
                    break;
            }
        }

        private void NormalBegin()
        {
        }

        private void NormalUpdate()
        {
            Speed.X = _speed * _direction;
            Move();

            if (_turning)
            {
                _turnTimer -= Engine.DeltaTime;
                if (_turnTimer <= 0.0)
                    _direction = -_direction;
                if (!MoveCollider.Check(_direction, 0, Solids))
                    _turning = false;
            }

            if ((X >= (double)(Room.SceneX - 16) || _direction >= 0) &&
                (X <= (double)(Room.SceneX + Room.SceneWidth + 16) || _direction <= 0))
                return;
            _direction = -_direction;
        }
        private void NormalEnd()
        {
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Horizontal)
            {
                if (!_turning)
                {
                    _turning = true;
                    _turnTimer = 0.3f;
                }
            }
            else if (Speed.Y < 0.0)
            {
                _dropping = 1;
                _yAxis = Y + _yAmplitude;
            }
            else
            {
                _dropping = -1;
                _yAxis = Y - _yAmplitude;
            }

            base.HitSolid(axis, ref velocity, collision);
        }
    }
}
