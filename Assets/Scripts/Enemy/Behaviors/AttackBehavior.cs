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
        navMeshAgent.isStopped = true;
        // Immediately clear all velocity otherwise will silde a little
        navMeshAgent.velocity = Vector3.zero;
        animator.SetBool("isAttacking", true);

        int[] validAnim = { 1, 2, 3, 5 }; // Attack 4 glitchy
        int index = validAnim[Random.Range(0, validAnim.Length)];
        animator.SetTrigger($"Attack_{index}");
    }

    public void OnExit()
    {
        navMeshAgent.isStopped = false;
        animator.SetBool("isAttacking", false);
    }

    public void OnUpdate()
    {
        Vector3 targetPos = MovementStateManager.Instance.transform.position;
        // only rotate on Y
        targetPos.y = npcTransform.position.y;

        // get vector from npc to player
        Vector3 direction = targetPos - npcTransform.position;
        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        // get target rotation on vector
        Quaternion targetRot = Quaternion.LookRotation(direction);
        // spheracle interpolate instead of LookAt to avoid sudden rotation change
        npcTransform.rotation = Quaternion.Slerp(
            npcTransform.rotation,
            targetRot,
            5f * Time.deltaTime
        );
    }
}
