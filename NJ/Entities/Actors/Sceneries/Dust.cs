using KTEngine;
using Microsoft.Xna.Framework;
using System.Timers;
using System.Xml;

namespace Chip
{
    public class Dust : Actor
    {
        private int _type = 0;
        private Animation _sprite;
        private Timer animationTimer;

        public Dust()
            : base(0, 0)
        {
            Depth = -4;
        }

        public override void CreateFromXml(XmlElement xml, Room room, Vector2 offset, Level level)
        {
            base.CreateFromXml(xml, room, offset, level);
            _type = xml.AttrInt("type");

            _sprite = new Animation(GFX.Game["sceneries/dust" + _type], 32, 32);

            Add(_sprite);

            _sprite.Add("idle", 3f, 0, 1, 2, 3);
            _sprite.Origin.X = 0f;
            _sprite.Origin.Y = 0f;
            animationTimer = new Timer(Rand.Instance.Next(500, 900));
            animationTimer.Elapsed += AnimationTimer_Elapsed;
            animationTimer.Enabled = true;
        }

        private void AnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _sprite.Play("idle");
            animationTimer.Stop();
        }
    }
}