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

    public void ChasePlayer(float speed)
    {
        if (PlayerController.Instance == null)
        {
            Debug.LogWarning("ChasePlayer has null PlayerController instance.");
            return;
        }
        Vector3 playerPos = PlayerController.Instance.transform.position;

        navMeshAgent.speed = speed;
        navMeshAgent.SetDestination(playerPos);
    }

    public void OnEnter()
    {
        Debug.Log("In ChasePlayer State.");
        animator.SetTrigger("Walk_Cycle_1");
        animator.speed = 1.5f;
    }

    public void OnExit()
    {
        animator.speed = 1.0f;
    }

    public void OnUpdate()
    {
        ChasePlayer(chaseSpeed);
    }
}
