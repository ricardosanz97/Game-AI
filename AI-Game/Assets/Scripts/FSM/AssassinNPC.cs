﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AssassinNPC : NPCStatesBehaviour 
{
    public List<Transform> assassinSpawnPoints;
    public bool appeared = false;
    public CommanderNPC commander;
    private void Start()
    {
        SetStates();
        SetTransitions();

        currentState = states.Find((x) => x.stateName == STATE.Hidden);
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);
    }

    public override void SetTransitions()
    {
        List<NextStateInfo> _nextStateInfo = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Hidden, STATE.None, GetComponent<TimeElapsedCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Alert, _nextStateInfo);

        List<NextStateInfo> _nextStateInfo2 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.None, commander.GetComponent<DetectPlayerCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Hidden, _nextStateInfo2);

        List<NextStateInfo> _nextStateInfo3 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.None, STATE.Alert, commander.GetComponent<DetectPlayerCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Attack, _nextStateInfo3);
       
    }

    public override void SetStates()
    {
        SetHiddenState();
        SetNoneState();
        SetAttackState();
        SetAlertState();
    }
    
    public void SetHiddenState()
    {
        FSMSystem.I.AddState(this, new State(STATE.Hidden));
    }

    public void SetAttackState()
    {
        List<SteeringBehaviour> behavioursAttackState = new List<SteeringBehaviour>()
        {
            //this.GetComponent<ChaseSteeringBehaviour>(),
            //this.GetComponent<ShootSteeringBehaviour>()
        };

        FSMSystem.I.AddState(this, new State(STATE.Attack));
        FSMSystem.I.AddBehaviours(this, behavioursAttackState, this.states.Find((x) => x.stateName == STATE.Attack));
    }

    public void SetNoneState()
    {
        FSMSystem.I.AddState(this, new State(STATE.None));
    }

    public void SetAlertState()
    {
        FSMSystem.I.AddState(this, new State(STATE.Alert));
    }

    public void Update()
    {
        CheckConditions();
        ActBehaviours();
    }

}
