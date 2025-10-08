using UnityEngine;

public class runstate : MovementBaseState
{
    public override void EnterState(MovementStateManager swat)
    {
        swat.anime.SetBool("Run", true);
    }

    public override void UpdateState(MovementStateManager swat)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ExitState(swat, swat.walk);
        }
        else if (swat.dir.magnitude < 0.1f)
        {
            ExitState(swat, swat.idle);
        }
        
        if (swat.vtInput < 0)
        {
            swat.currentSpeed = swat.RunBackSpeed;
        }
        else
        {
            swat.currentSpeed = swat.RunSpeed;
        }
    }

    void ExitState(MovementStateManager swat, MovementBaseState State)
    {
        swat.anime.SetBool("Run", false);
        swat.SwitchState(State);
    } 
    
}
