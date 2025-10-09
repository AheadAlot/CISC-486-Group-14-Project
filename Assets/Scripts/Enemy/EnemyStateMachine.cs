using System.Collections;
using System.Collections.Generic;
using UnityHFSM;
using UnityEngine;

public class EnemyStateMachine
{
    private StateMachine fsm;
    private DistanceService distanceService;
    private PathingService pathingService;
    private IEnemyBehavior wanderBehavior;
    private IEnemyBehavior chaseBehavior;
    private IEnemyBehavior attackBehavior;


    public EnemyStateMachine(
        PathingService pathingService,
        DistanceService distanceService,
        IEnemyBehavior wander,
        IEnemyBehavior chase,
        IEnemyBehavior attack)
    {
        this.pathingService = pathingService;
        this.distanceService = distanceService;
        this.wanderBehavior = wander;
        this.chaseBehavior = chase;
        this.attackBehavior = attack;

        InitFSM();
    }

    private void InitFSM()
    {
        fsm = new StateMachine();
        InitFSMStates();
        InitFSMTransitions();
        fsm.SetStartState("Wander");
        fsm.Init();
    }

    private void InitFSMStates()
    {
        fsm.AddState("Wander", new State(
            onEnter: state => wanderBehavior.OnEnter(),
            onLogic: state => wanderBehavior.OnUpdate(),
            onExit: state => wanderBehavior.OnExit()
        ));

        fsm.AddState("ChasePlayer", new State(
            onEnter: state => chaseBehavior.OnEnter(),
            onLogic: state => chaseBehavior.OnUpdate(),
            onExit: state => chaseBehavior.OnExit()
        ));

        fsm.AddState("AttackPlayer", new State(
            onEnter: state => attackBehavior.OnEnter(),
            onLogic: state => attackBehavior.OnUpdate(),
            onExit: state => attackBehavior.OnExit()
        ));
    }

    private void InitFSMTransitions()
    {
        // Start chase player if player in range and reachable
        fsm.AddTransition(new Transition(
            "Wander",
            "ChasePlayer",
            transition => distanceService.IsPlayerInChaseRange && pathingService.IsPlayerReachable
        ));

        // Turn back to wander if player left range
        fsm.AddTransition(new Transition(
            "ChasePlayer",
            "Wander",
            transition => !distanceService.IsPlayerInChaseRange
        ));

        // Start attack player if in attack range
        fsm.AddTransition(new Transition(
            "ChasePlayer",
            "AttackPlayer",
            transition => distanceService.IsPlayerInAttackRange
        ));

        // Turn back to chasing if player left attack range
        fsm.AddTransition(new Transition(
            "AttackPlayer",
            "ChasePlayer",
            transition => !distanceService.IsPlayerInAttackRange
        ));

        // Turn back to wander if player left attack and chase range
        fsm.AddTransition(new Transition(
            "AttackPlayer",
            "Wander",
            transition => !distanceService.IsPlayerInChaseRange
        ));

        fsm.AddTransition(new Transition(
            "ChasePlayer",
            "Wander",
            transition => !pathingService.IsPlayerReachable
        ));
    }

    public void Update()
    {
        fsm.OnLogic();
    }
}
