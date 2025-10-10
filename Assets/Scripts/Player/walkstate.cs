using UnityEngine;

public class WalkState : MovementBaseState
{
    public override void EnterState(MovementStateManager swat)
    {
        swat.anime.SetBool("Walk", true);
    }

    public override void UpdateState(MovementStateManager swat)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ExitState(swat, swat.run);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ExitState(swat, swat.crouch);
        }
        else if (swat.dir.magnitude < 0.1f)
        {
            ExitState(swat, swat.idle);
        }

        if (swat.vtInput < 0)
        {
            swat.currentSpeed = swat.WalkBackSpeed;
        }
        else
        {
            swat.currentSpeed = swat.WalkSpeed;
        }
    }

    void ExitState(MovementStateManager swat, MovementBaseState State)
    {
        swat.anime.SetBool("Walk", false);
        swat.SwitchState(State);
    }   
}

