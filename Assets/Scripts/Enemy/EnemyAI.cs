using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float wanderRadius = 50f;
    public float wanderSpeed = 5f;
    public float chaseSpeed = 7.5f;
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

        PathingService pathingService = new PathingService(navMeshAgent);
        DistanceService distanceService = new DistanceService(transform, chaseRange, attackRange);
        WanderBehavior wanderBehavior = new WanderBehavior(transform, navMeshAgent, animator, wanderRadius, changeDirTime, wanderSpeed);
        ChaseBehavior chaseBehavior = new ChaseBehavior(navMeshAgent, animator, chaseSpeed);
        AttackBehavior attackBehavior = new AttackBehavior(transform, navMeshAgent, animator);

        fsm = new EnemyStateMachine(pathingService, distanceService, wanderBehavior, chaseBehavior, attackBehavior);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }
}
