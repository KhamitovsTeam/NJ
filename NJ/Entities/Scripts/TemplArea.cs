using KTEngine;

namespace Chip
{
    public class TemplArea : Script
    {
        public override void OnTriggerEnter()
        {
            Player.Instance.Weaponless = true;
            if (!Player.Instance.PlayerData.HasFire)
            {
                OverlayDialog.Instance.DialogItem = new DialogItem("Hmm..", "The temple is empty...");  
            }
            else
            {
                OverlayDialog.Instance.DialogItem = new DialogItem("Holy fire returned!");
                OverlayDialog.Instance.Show();
            }
            OverlayDialog.Instance.Show();
        }

        public override void OnTriggerExit()
        {
            Player.Instance.Weaponless = false;
            Engine.Scene = new End();
        }
    }
}