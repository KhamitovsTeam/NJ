using KTEngine;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Chip
{
    public class KittenInCage : Actor
    {
        private Animation sprite = new Animation(GFX.Game["objects/cats"], 16, 16);
        private string cutscene;
        private int cats;
        private bool kittensSaved = false;
        private Collider playerCollider;
        private List<Kitten> kittens = new List<Kitten>();

        public KittenInCage()
            : base(0, 0)
        {
            Depth = 1;

            Add(sprite);
            sprite.Add("idle", 3f, 0, 1, 2, 1);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("idle");
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);

            cutscene = xml.Attr("cutscene");
            cats = xml.AttrInt("cats");

            // Если клетка была сохранена, то не показываем 
            var levelData = Level.Session.GetLevelData(Level.ID);
            if (levelData?.Cages == null) return;
            foreach (var cage in levelData.Cages)
            {
                if (ID != cage.ID) continue;
                NeedToRemove = true;
            }

            if (NeedToRemove)
            {
                // Создаём собранных котиков
                var half = cats * 16 / 2f;
                playerCollider = Add(new Hitbox(-(int)half, -8, cats * 16, 16));

                var kittenPosX = Position.X - (cats / 2) * 16;
                for (var i = 0; i < cats; i++)
                {
                    var kitten = new Kitten();
                    kitten.IsCatched = true;
                    kitten.Position.X = kittenPosX;
                    kitten.Position.Y = Position.Y;
                    kittens.Add(Scene.Add(kitten));
                    kittenPosX += 16;
                }
            }
        }

        public override void Update()
        {
            base.Update();
            if (playerCollider != null && playerCollider.Check((int)Tags.Player))
            {
                kittensSaved = true;
            }
            if (kittensSaved)
            {
                playerCollider = null;
                kittensSaved = false;
                // Добавляем в список собранных котиков
                if (Level != null)
                {
                    var levelData = Level.Session.GetLevelData(Level.ID);
                    if (levelData == null)
                    {
                        levelData = new LevelData(Level.ID);
                        Level.Session.LevelsData.Add(levelData);
                    }
                    levelData.Cages.Add(new EntityID(Level.ID, ID));
                }
                var cutscene = Cutscenes.AllCutscenes[this.cutscene];
                if (cutscene == null) return;
                cutscene.Level = Level;
                cutscene.Start();
                Level?.Add(cutscene);
            }
        }

        public IEnumerator ReleaseKittens()
        {
            Smoke.Burst(X, Y, 8f, 0.0f, Calc.TAU, 4);
            Engine.Instance.CurrentCamera.Shake(4f, 0.5f);
            //Scene.Remove(this);
            sprite.Visible = false;
            CreateKittens();
            yield return 2f;
        }

        private void CreateKittens()
        {
            var half = cats * 16 / 2f;
            playerCollider = Add(new Hitbox(-(int)half, -8, cats * 16, 16));

            var kittenPosX = Position.X - (cats / 2) * 16;
            for (var i = 0; i < cats; i++)
            {
                //Kitten.Born(0, new Vector2(kittenPosX, Position.Y));
                var kitten = new Kitten(0, new Vector2(kittenPosX, Position.Y));
                kittens.Add(Scene.Add(kitten));
                //kitten.Position.X = kittenPosX;
                //kitten.Position.Y = Position.Y;
                kittenPosX += 16;
            }
        }
    }
}
