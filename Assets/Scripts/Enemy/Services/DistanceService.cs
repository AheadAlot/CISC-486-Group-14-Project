using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceService
{
    private Transform npcTransform;
    private float sqrEnemyChaseRange;
    private float sqrEnemyAttackRange;

    public DistanceService(
        Transform npcTransform,
        float chaseRange,
        float attackRange)
    {
        this.npcTransform = npcTransform;
        sqrEnemyChaseRange = chaseRange * chaseRange;
        sqrEnemyAttackRange = attackRange * attackRange;
    }

    private float DistanceToPlayer => (npcTransform.position - PlayerController.Instance.transform.position).sqrMagnitude;
    public bool IsPlayerInChaseRange => DistanceToPlayer <= sqrEnemyChaseRange;
    public bool IsPlayerInAttackRange => DistanceToPlayer <= sqrEnemyAttackRange;
}
