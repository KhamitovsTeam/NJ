using KTEngine;

namespace Chip
{
    public class SecondVillager : Script
    {
        public override void OnTriggerEnter()
        {
            Player.Instance.Weaponless = true;
            OverlayDialog.Instance.DialogItem = new DialogItem(
                   "The holy fire is gone!",
                   "The temple is empty!"
               );
            OverlayDialog.Instance.Show();
        }

        public override void OnTriggerExit()
        {
            Player.Instance.Weaponless = false;
        }
    }
}