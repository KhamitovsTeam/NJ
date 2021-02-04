using KTEngine;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Chip
{
    public class World : MenuItem
    {
        public string Name { get; set; }

        public string File { get; set; }

        private Coroutine _coroutine;
        //private Vector2 _namePosition;
        private Animation _animation;

        const float PADDING = 4f;

        const float BOTTOM_PADDING = PADDING * 2f + PADDING / 2f;
        const float END_PADDING = PADDING * 2f + PADDING / 2f;
        const float START_PADDING = PADDING;
        const float TOP_PADDING = PADDING;

        public World(string name, string file)
        {
            _coroutine = new Coroutine(false);
            Add(_coroutine);

            _animation = XML.LoadSprite<Animation>(GFX.Gui, GFX.Sprites, name);
            Add(_animation);

            Name = name;
            File = file;
        }

        protected override void OnConfirm()
        {

        }

        protected override void OnDeselect()
        {

        }

        protected override void OnSelect()
        {

        }

        public override void Render()
        {
            base.Render();
            if (Selected)
            {
                Draw.HollowRect(Position.X - _animation.Width / 2f - START_PADDING, Position.Y - _animation.Height / 2f - TOP_PADDING, _animation.Width + END_PADDING, _animation.Height + BOTTOM_PADDING, 1f, Color.Aqua);
            }
        }

        public void Move(Vector2 from, Vector2 to)
        {
            _coroutine.Replace(MoveTo(from, to));
        }

        private IEnumerator MoveTo(Vector2 from, Vector2 to)
        {
            Ease.Easer ease = Ease.Linear;
            var p = 0.0f;
            while (p < 1.0)
            {
                Position = from + (to - from) * ease(p);
                p += Engine.DeltaTime;
                yield return Engine.DeltaTime;
            }
        }
    }
}
