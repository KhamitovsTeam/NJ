using KTEngine;

namespace Chip
{
    public class ThirdVillager : Script
    {
        public override void OnTriggerEnter()
        {
            // Player.Instance.Weaponless = true;
            if (Player.Instance.PlayerData.HasFire) return;
            OverlayDialog.Instance.DialogItem = new DialogItem(
                   "Spirit of the forest",
                   "stole the holy fire.",
                   "Protect yourself..",
                   "from Zombies"
               );
            OverlayDialog.Instance.Show();
        }

        public override void OnTriggerExit()
        {
            //Player.Instance.Weaponless = false;
        }
    }
}