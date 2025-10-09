using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathingService
{
    private NavMeshAgent navMeshAgent;

    public PathingService(
        NavMeshAgent agent
    )
    {
        this.navMeshAgent = agent;
    }

    public bool IsPlayerReachable => IsPathExistToPlayer();

    private bool IsPathExistToPlayer()
    {
        Vector3 playerPos = MovementStateManager.Instance.transform.position;
        NavMeshPath navMeshPath = new NavMeshPath();
        bool hasPath = navMeshAgent.CalculatePath(playerPos, navMeshPath);
        return hasPath && navMeshPath.status == NavMeshPathStatus.PathComplete;
    }
}