using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackBehavior : IEnemyBehavior
{
    private Transform npcTransform;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public AttackBehavior(
        Transform npcTransform,
        NavMeshAgent navMeshAgent,
        Animator animator
    )
    {
        this.npcTransform = npcTransform;
        this.navMeshAgent = navMeshAgent;
        this.animator = animator;
    }

    public void OnEnter()
    {
        Debug.Log("In AttackPlayer State.");
        animator.SetBool("isAttacking", true);
        // TODO: random attack animation?
        animator.SetTrigger($"Attack_1");
        navMeshAgent.isStopped = true;
    }

    public void OnExit()
    {
        animator.SetBool("isAttacking", false);
        navMeshAgent.isStopped = false;
    }

    public void OnUpdate()
    {
        npcTransform.LookAt(PlayerController.Instance.transform.position);
    }
}
