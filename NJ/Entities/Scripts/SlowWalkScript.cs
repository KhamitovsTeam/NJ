namespace Chip
{
    public class SlowWalkScript : Script
    {
        public override void OnTriggerEnter()
        {
            Player.Instance.SlowMoving = true;
        }

        public override void OnTriggerExit()
        {
            Player.Instance.StateMachine.Set(Player.StateNormal);
            Player.Instance.SlowMoving = false;
        }
    }
}