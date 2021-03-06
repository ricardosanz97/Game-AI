﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChaserNPC : NPCStatesBehaviour 
{
    private DebugStateSystem dss;
    private void Awake()
    {
        dss = GetComponent<DebugStateSystem>();
    }

    public override void Start()
    {
        SetStates();
        SetTransitions();

        currentState = states.Find((x) => x.stateName == STATE.Patrol);
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);

        SetDebugs();

        base.Start();
    }

    public override void SetTransitions()
    {
        List<NextStateInfo> _nextStateInfo = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.None, GetComponent<DetectPlayerCondition>()),
        };

        FSMSystem.I.AddTransition(this, STATE.Patrol, _nextStateInfo);

        List<NextStateInfo> _nextStateInfo2 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.None, STATE.Alert, GetComponent<DetectPlayerCondition>()),
        };

        FSMSystem.I.AddTransition(this, STATE.Attack, _nextStateInfo2);

        List<NextStateInfo> _nextStateInfo3 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.None, GetComponent<DetectPlayerCondition>()),
            new NextStateInfo(this, STATE.Patrol, STATE.None, GetComponent<TimeElapsedCondition>()),
        };

        FSMSystem.I.AddTransition(this, STATE.Alert, _nextStateInfo3);
    }

    public override void SetStates()
    {
        SetNoneState();
        SetAttackState();
        SetPatrolState();
        SetAlertState();
    }

    public void SetAttackState()
    {
        List<SteeringBehaviour> behavioursAttackState = new List<SteeringBehaviour>()
        {
            this.GetComponent<ChaseSteeringBehaviour>(),
            this.GetComponent<HitSteeringBehaviour>()
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

    public void SetAlertState()
    {
        List<SteeringBehaviour> behavioursAlertState = new List<SteeringBehaviour>()
        {
            this.GetComponent<MoveLastPositionPlayerSteeringBehaviour>(),
        };

        FSMSystem.I.AddState(this, new State(STATE.Alert));
        FSMSystem.I.AddBehaviours(this, behavioursAlertState, this.states.Find((x) => x.stateName == STATE.Alert));
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

    private void SetDebugs()
    {
        dss.SetNPCName(this.gameObject.name.ToString());
        dss.SetCurrentState(currentState.stateName.ToString());

    }

}
