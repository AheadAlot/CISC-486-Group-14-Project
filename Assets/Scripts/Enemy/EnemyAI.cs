using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float wanderRadius = 50f;
    public float speed = 25f;
    public float changeDirTime = 15f;
    public float chaseRange = 25f;
    public float attackRange = 4f;

    private EnemyStateMachine fsm;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        DistanceService distanceService = new DistanceService(transform, chaseRange, attackRange);
        WanderBehavior wanderBehavior = new WanderBehavior(transform, navMeshAgent, animator, wanderRadius, changeDirTime);
        ChaseBehavior chaseBehavior = new ChaseBehavior(navMeshAgent, animator);
        AttackBehavior attackBehavior = new AttackBehavior(transform, navMeshAgent, animator);

        fsm = new EnemyStateMachine(distanceService, wanderBehavior, chaseBehavior, attackBehavior);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }
}
