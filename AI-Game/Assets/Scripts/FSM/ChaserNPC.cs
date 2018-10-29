using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChaserNPC : NPCStatesBehaviour 
{
    private void Start()
    {
        SetStates();
        SetTransitions();

        currentState = states.Find((x) => x.stateName == STATE.Patrol);
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);
    }

    public override void SetTransitions()
    {
        List<NextStateInfo> _nextStateInfo = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.None, GetComponent<DetectPlayerCondition>()),
            new NextStateInfo(this, STATE.Hit, STATE.Attack, GetComponent<DetectPlayerHit>())

        };

        FSMSystem.I.AddTransition(this, STATE.Patrol, _nextStateInfo);
    }

    public override void SetStates()
    {
        SetNoneState();
        SetAttackState();
        SetPatrolState();
    }

    public void SetAttackState()
    {
        List<SteeringBehaviour> behavioursAttackState = new List<SteeringBehaviour>()
        {
            this.GetComponent<ChaseSteeringBehaviour>(),
            //this.GetComponent<ShootSteeringBehaviour>()
        };

        FSMSystem.I.AddState(this, new State(STATE.Attack));
        FSMSystem.I.AddBehaviours(this, behavioursAttackState, this.states.Find((x) => x.stateName == STATE.Attack));
    }

    public void SetPatrolState()
    {
        List<SteeringBehaviour> behavioursPatrolState = new List<SteeringBehaviour>()
        {
            this.GetComponent<PatrolSteeringBehaviour>(),
        };

        FSMSystem.I.AddState(this, new State(STATE.Patrol));
        FSMSystem.I.AddBehaviours(this, behavioursPatrolState, this.states.Find((x) => x.stateName == STATE.Patrol));
    }

    public void SetHitState()
    {
        List<SteeringBehaviour> behavioursHitState = new List<SteeringBehaviour>()
        {
            this.GetComponent<HitSteeringBehaviour>(),
        };

        FSMSystem.I.AddState(this, new State(STATE.Hit));
        FSMSystem.I.AddBehaviours(this, behavioursHitState, this.states.Find((x) => x.stateName == STATE.Hit));
    }

    public void SetNoneState()
    {
        FSMSystem.I.AddState(this, new State(STATE.None));
    }

    public void Update()
    {
        CheckConditions();
        ActBehaviours();
    }

}
