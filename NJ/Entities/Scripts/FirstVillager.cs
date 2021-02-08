using KTEngine;

namespace Chip
{
    public class FirstVillager : Script
    {
        public override void OnTriggerEnter()
        {
            Player.Instance.Weaponless = true;
            OverlayDialog.Instance.DialogItem = new DialogItem(
                   "The holy fire is gone!",
                   "The holy fire is gone!",
                   "Help us!"
               );
            OverlayDialog.Instance.Show();
        }

        public override void OnTriggerExit()
        {
            Player.Instance.Weaponless = false;
        }
    }
}