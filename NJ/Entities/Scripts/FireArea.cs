using KTEngine;

namespace Chip
{
    public class FireArea : Script
    {
        public override void OnTriggerEnter()
        {
            Player.Instance.Weaponless = true;
            OverlayDialog.Instance.DialogItem = new DialogItem(
                   "I must take it to the village."
               );
            OverlayDialog.Instance.Show();
        }

        public override void OnTriggerExit()
        {
            Player.Instance.Weaponless = false;
        }
    }
}