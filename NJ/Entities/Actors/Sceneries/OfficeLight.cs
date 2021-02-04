using KTEngine;
using System.Timers;

namespace Chip
{
    public class OfficeLight : Actor
    {
        private Animation sprite = new Animation(GFX.Game["sceneries/office_light"], 28, 64);
        private Timer animationTimer;

        public OfficeLight()
            : base(0, 0)
        {
            Add(sprite);
            sprite.Add("start", 2f, 0);
            sprite.Add("idle", 8f, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            sprite.Origin.X = 8f;
            sprite.Origin.Y = 8f;
            sprite.Play("start");
            animationTimer = new Timer(Rand.Instance.Next(1000, 7000));
            animationTimer.Elapsed += AnimationTimer_Elapsed;
            animationTimer.Enabled = true;

            Depth = -2;
        }

        private void AnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            sprite.Play("idle");
            animationTimer.Stop();
        }
    }
}