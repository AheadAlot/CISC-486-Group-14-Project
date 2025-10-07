using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class EnemyStateManager : MonoBehaviour
{
    private StateMachine fsm;
    private JobDriverWander jobDriverWander;
    private Animator animator;

    // TODO: Adjust
    public float playerChaseRange = 10f;

    /// <summary>
    /// Start chasing player, set animator and NavMeshAgent pathing.
    /// </summary>
    /// <param name="speed"></param>
    private void ChasePlayer(float speed)
    {
        // TODO: Chase player with position from PlayerController
    }

    private void InitJobDrivers()
    {
        jobDriverWander = GetComponent<JobDriverWander>();
    }

    private void InitFSM()
    {
        fsm = new StateMachine();

        fsm.AddState("Idle");
        fsm.AddState("Wander", new State(
            onEnter: state =>
            {
                Debug.Log("In Wander State.");
                animator.SetTrigger("Walk_Cycle_1");
            },
            onLogic: state =>
            {
                jobDriverWander.OnWanderUpdate();
            }
        ));
        // TODO: Add chase player logic and enter code
        fsm.AddState("ChasePlayer");
        // TODO: Add attack player logic and enter code
        fsm.AddState("AttackPlayer");

        fsm.SetStartState("Wander");

        fsm.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        InitJobDrivers();
        InitFSM();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
    }
}
