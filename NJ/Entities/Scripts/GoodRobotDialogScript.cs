using KTEngine;

namespace Chip
{
    public class GoodRobotDialogScript : Script
    {
        public override void OnTriggerEnter()
        {
            Player.Instance.Weaponless = true;
            OverlayDialog.Instance.DialogItem = new DialogItem(Texts.MainText["dialog_robot_no_kitten1"], Texts.MainText["dialog_robot_no_kitten2"]);
            //OverlayDialog.Instance.Avatar = new Graphic(GFX.Gui["goodrobot_face"]);
            OverlayDialog.Instance.Show();
        }

        public override void OnTriggerExit()
        {
            Player.Instance.Weaponless = false;
        }
    }
}