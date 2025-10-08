using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehavior : IEnemyBehavior
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public ChaseBehavior(
        NavMeshAgent navMeshAgent,
        Animator animator
    )
    {
        this.navMeshAgent = navMeshAgent;
        this.animator = animator;
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
        animator.SetBool("isAttacking", false);
        animator.SetTrigger("Walk_Cycle_1");
        navMeshAgent.isStopped = false;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        ChasePlayer(5f);
    }
}
