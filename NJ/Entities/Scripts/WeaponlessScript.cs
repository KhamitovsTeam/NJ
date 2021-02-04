namespace Chip
{
    public class WeaponlessScript : Script
    {
        public override void OnTriggerEnter()
        {
            Player.Instance.Weaponless = true;
        }

        public override void OnTriggerExit()
        {
            Player.Instance.Weaponless = false;
        }
    }
}