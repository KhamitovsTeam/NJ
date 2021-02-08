using KTEngine;

namespace Chip
{
    public class FirstVillager : Script
    {
        public override void OnTriggerEnter()
        {
            // Player.Instance.Weaponless = true;
            if (Player.Instance.PlayerData.HasFire) return;
            OverlayDialog.Instance.DialogItem = new DialogItem(
                   "The Holy fire is gone!",
                   "The Dead Come to Life!",
                   "Help us!"
               );
            OverlayDialog.Instance.Show();
        }

        public override void OnTriggerExit()
        {
            //Player.Instance.Weaponless = false;
        }
    }
}