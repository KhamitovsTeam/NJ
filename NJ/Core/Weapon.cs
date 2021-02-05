using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class Weapon : Component
    {
        public WeaponType Type;
        public string Sound;
        public int BulletCount;
        public float BulletSpeed;

        public virtual void Shoot(Vector2 pointing)
        {
            Bullet.Burst(ScenePosition + Vector2.UnitY + pointing, pointing, BulletCount, BulletSpeed, Vector2.Zero);
            SFX.Play(Sound);
            SaveData.Instance.TotalShots++;
        }
    }
}