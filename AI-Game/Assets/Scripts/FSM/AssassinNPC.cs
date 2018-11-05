using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AssassinNPC : NPCStatesBehaviour 
{
    private DebugStateSystem dss;
    public List<Transform> assassinSpawnPoints;
    public GameObject assassinSpawnPointsGO;
    public bool appeared = false;
    public CommanderNPC commander;

    private void Awake()
    {
        dss = GetComponent<DebugStateSystem>();
    }

    public override void Start()
    {
        SetStates();
        SetTransitions();

        currentState = states.Find((x) => x.stateName == STATE.Hidden);
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);

        for (int i = 0; i < assassinSpawnPointsGO.transform.childCount; i++)
        {
            assassinSpawnPoints.Add(assassinSpawnPointsGO.transform.GetChild(i));
        }

        SetDebugs();

        base.Start();
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

        List<NextStateInfo> _nextStateInfo4 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.None, commander.GetComponent<DetectPlayerCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Alert, _nextStateInfo4);

        List<NextStateInfo> _nextStateInfo5 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.None, STATE.Alert, GetComponent<PlayerAliveCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Attack, _nextStateInfo5);
       
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
            this.GetComponent<ChaseSteeringBehaviour>(),
            this.GetComponent<AppearSteeringBehaviour>()
            
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

    private void SetDebugs()
    {
        dss.SetNPCName(this.gameObject.name.ToString());
        dss.SetCurrentState(currentState.stateName.ToString());

    }

}
