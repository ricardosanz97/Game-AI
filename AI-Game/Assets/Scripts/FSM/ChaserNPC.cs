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
            new NextStateInfo(this, STATE.Attack, STATE.None, GetComponent<DetectPlayerCondition>())
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
