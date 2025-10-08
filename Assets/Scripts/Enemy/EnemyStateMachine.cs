using System.Collections;
using System.Collections.Generic;
using UnityHFSM;
using UnityEngine;

public class EnemyStateMachine
{
    private StateMachine fsm;
    private DistanceService distanceService;
    private IEnemyBehavior wanderBehavior;
    private IEnemyBehavior chaseBehavior;
    private IEnemyBehavior attackBehavior;


    public EnemyStateMachine(
        DistanceService distanceService,
        IEnemyBehavior wander,
        IEnemyBehavior chase,
        IEnemyBehavior attack)
    {
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
        fsm.AddState("Idle");
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
        fsm.AddTransition(new Transition(
            "Wander",
            "ChasePlayer",
            transition => distanceService.IsPlayerInChaseRange
        ));

        fsm.AddTransition(new Transition(
            "ChasePlayer",
            "Wander",
            transition => !distanceService.IsPlayerInChaseRange
        ));

        fsm.AddTransition(new Transition(
            "ChasePlayer",
            "AttackPlayer",
            transition => distanceService.IsPlayerInAttackRange
        ));

        fsm.AddTransition(new Transition(
            "AttackPlayer",
            "ChasePlayer",
            transition => !distanceService.IsPlayerInAttackRange
        ));
    }

    public void Update()
    {
        fsm.OnLogic();
    }
}
