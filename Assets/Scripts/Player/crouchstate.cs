using UnityEngine;
public class CrouchState : MovementBaseState
{
    public override void EnterState(MovementStateManager swat)
    {
        swat.anime.SetBool("Crouch", true);
    }

    public override void UpdateState(MovementStateManager swat)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ExitState(swat, swat.crouch);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (swat.dir.magnitude < 0.1f)
            {
                ExitState(swat, swat.idle);
            }
            else
            {
                ExitState(swat, swat.walk);
            }
        }
        
        if (swat.vtInput < 0)
        {
            swat.currentSpeed = swat.CrouchBackSpeed;
        }
        else
        {
            swat.currentSpeed = swat.CrouchSpeed;
        }
    
    }

    void ExitState(MovementStateManager swat, MovementBaseState State)
    {
        swat.anime.SetBool("Crouch", false);
        swat.SwitchState(State);
    } 

}

