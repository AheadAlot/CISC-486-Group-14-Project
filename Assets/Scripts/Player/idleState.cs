using UnityEngine;

public class IdleState : MovementBaseState
{
    public override void EnterState(MovementStateManager swat)
    {

    }

    public override void UpdateState(MovementStateManager swat)
    {
        if (swat.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                swat.SwitchState(swat.run);
            }
            else
            {
                swat.SwitchState(swat.walk);
            }
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            swat.SwitchState(swat.crouch);
        }
    }
    
}
