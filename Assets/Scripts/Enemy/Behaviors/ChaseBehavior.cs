using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehavior : IEnemyBehavior
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float chaseSpeed;

    public ChaseBehavior(
        NavMeshAgent navMeshAgent,
        Animator animator,
        float chaseSpeed
    )
    {
        this.navMeshAgent = navMeshAgent;
        this.animator = animator;
        this.chaseSpeed = chaseSpeed;
    }

    private void ChasePlayer(float speed)
    {
        if (MovementStateManager.Instance == null)
        {
            Debug.LogWarning("ChasePlayer has null PlayerController instance.");
            return;
        }
        Vector3 playerPos = MovementStateManager.Instance.transform.position;

        // Use NavMeshAgent for pathing and moving
        navMeshAgent.speed = speed;
        navMeshAgent.SetDestination(playerPos);
    }

    public void OnEnter()
    {
        Debug.Log("In ChasePlayer State.");
        animator.SetTrigger("Walk_Cycle_1");
        // Faster animation to simulate run
        animator.speed = 1.5f;
    }

    public void OnExit()
    {
        // Reset animator speed
        animator.speed = 1.0f;
    }

    public void OnUpdate()
    {
        ChasePlayer(chaseSpeed);
    }
}
