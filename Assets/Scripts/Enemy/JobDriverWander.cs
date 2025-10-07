using UnityEngine;
using UnityEngine.AI;

public class JobDriverWander : MonoBehaviour
{
    // Maximum radius to find a wander position
    public float wanderRadius = 50f;
    // Speed of wander
    public float speed = 10f;
    // How long to next destination
    public float changeDirTime = 3f;

    private NavMeshAgent navMeshAgent;
    private float timer;

    /// <summary>
    /// Get a new wandering position for NPC
    /// </summary>
    private void GetNewWanderPosition()
    {
        timer = 0f;
        // Get a new random position
        Vector3 rand = Random.insideUnitCircle * wanderRadius;
        rand += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rand, out hit, wanderRadius, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }

    /// <summary>
    /// Update function called per frame for NavMeshAgent pathing
    /// </summary>
    public void OnWanderUpdate()
    {
        timer += Time.deltaTime;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f || timer >= changeDirTime)
        {
            GetNewWanderPosition();
        }
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GetNewWanderPosition();
    }
}
