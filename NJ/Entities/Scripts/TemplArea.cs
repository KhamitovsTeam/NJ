using KTEngine;

namespace Chip
{
    public class TemplArea : Script
    {
        public override void OnTriggerEnter()
        {
            Player.Instance.Weaponless = true;
            OverlayDialog.Instance.DialogItem = new DialogItem(
                   "Hmm..",
                   "The temple is empty..."
               );
            OverlayDialog.Instance.Show();
        }

        public override void OnTriggerExit()
        {
            Player.Instance.Weaponless = false;
        }
    }
}