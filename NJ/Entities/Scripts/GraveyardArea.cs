using KTEngine;

namespace Chip
{
    public class GraveyardArea : Script
    {
        public override void OnTriggerEnter()
        {
            Player.Instance.Weaponless = true;
            if (Player.Instance.PlayerData.HasFire) return;
            OverlayDialog.Instance.DialogItem = new DialogItem(
                   "Seems like I can go..",
                   " under a ground."
               );
            OverlayDialog.Instance.Show();
        }

        public override void OnTriggerExit()
        {
            Player.Instance.Weaponless = false;
        }
    }
}