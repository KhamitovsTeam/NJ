using KTEngine;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Xml;

namespace Chip
{
    public class Rock : Actor
    {
        public string Type;

        private int _height;
        private int _dust = 16;

        public Rock()
            : base(0, 0)
        {
            Depth = 5;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            Type = xml.Attr("type");

            // 
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Rocks != null)
            {
                foreach (var rock in levelData.Rocks)
                {
                    if (ID == rock.ID) return;
                }
            }

            _height = xml.AttrInt("height");

            MoveCollider = Add(new Hitbox(-8, -8, 16, _height));
            MoveCollider.Tag((int)Tags.Solid);

            int count = _height / 16;
            for (int i = 0; i < count; i++)
            {
                Graphic graphic = new Graphic(GFX.Game["objects/rock"]);
                graphic.X = -MoveCollider.Width / 2f;
                graphic.Y = i * 16 - 8;
                Add(graphic);
            }
        }

        public IEnumerator Destroy()
        {
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData == null)
            {
                levelData = new LevelData(Level.ID);
                Level.Session.LevelsData.Add(levelData);
            }
            levelData.Rocks.Add(new EntityID(Level.ID, ID));
            Scene.Remove(this);
            Engine.Instance.CurrentCamera.Shake(4f, 0.5f);
            Input.Rumble(Input.RumbleStrength.Strong, Input.RumbleLength.Short);
            SFX.Play("destroy");
            Smoke.Burst(X, Y + _height, 8f, 0.0f, Calc.TAU, _dust);
            StonePieces.Burst(Position + Vector2.UnitY * _height / 2f, 150);
            yield return 2f;
        }
    }
}