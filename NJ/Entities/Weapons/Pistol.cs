namespace Chip
{
    public class Pistol : Weapon
    {
        public Pistol()
        {
            Sound = "shoot";
            BulletCount = 1;
            BulletSpeed = 1f;
        }
    }
}