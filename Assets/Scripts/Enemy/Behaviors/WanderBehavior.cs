using UnityEngine;
using UnityEngine.AI;

public class WanderBehavior : IEnemyBehavior
{
    private float wanderRadius = 50f;
    private float timer = 0f;
    private float changeDirTime = 5f;
    private float wanderSpeed = 1f;
    private Transform npcTransform;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public WanderBehavior(
        Transform npcTransform,
        NavMeshAgent agent,
        Animator animator,
        float wanderRadius,
        float changeDirTime,
        float wanderSpeed
        )
    {
        this.npcTransform = npcTransform;
        this.navMeshAgent = agent;
        this.animator = animator;
        this.wanderRadius = wanderRadius;
        this.changeDirTime = changeDirTime;
        this.wanderSpeed = wanderSpeed;
    }

    private void GetNewWanderPosition()
    {
        timer = 0f;
        // Get a new random position
        Vector3 rand = Random.insideUnitCircle * wanderRadius;
        rand += npcTransform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rand, out hit, wanderRadius, NavMesh.AllAreas))
        {
            navMeshAgent.speed = wanderSpeed;
            navMeshAgent.SetDestination(hit.position);
        }
    }


    public void OnEnter()
    {
        Debug.Log("In Wander State.");
        GetNewWanderPosition();
        animator.SetTrigger("Sneak_Cycle_1");
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        // Get a new wander position when timer is up
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f || timer >= changeDirTime)
        {
            GetNewWanderPosition();
        }
    }

    public void OnExit()
    {
        
    }
}