using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatcherNPC : NPCStatesBehaviour
{
    private void Start()
    {
        SetStates();
        SetTransitions();

        currentState = states.Find((x) => x.stateName == STATE.Idle);
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);
    }

    public override void SetStates()
    {
        SetNoneState();
        SetAttackState();
        SetIdleState();
        SetAlertState();
        SetRecolocateState();
    }

    public override void SetTransitions()
    {
        List<NextStateInfo> _nextStateInfo = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.None, GetComponent<DetectPlayerCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Idle, _nextStateInfo);

        List<NextStateInfo> _nextStateInfo2 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.None, STATE.Alert, GetComponent<DetectPlayerCondition>())
        };       
        FSMSystem.I.AddTransition(this, STATE.Attack, _nextStateInfo2);

        List<NextStateInfo> _nextStateInfo3 = new List<NextStateInfo>(){
            new NextStateInfo(this, STATE.None, STATE.Recolocate, GetComponent<PlayerAliveCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Attack, _nextStateInfo3);

        List<NextStateInfo> _nextStateInfo4 = new List<NextStateInfo>(){
            new NextStateInfo(this, STATE.None, STATE.Recolocate, GetComponent<PlayerAliveCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Alert, _nextStateInfo4);

        List<NextStateInfo> _nextStateInfo5 = new List<NextStateInfo>(){
            new NextStateInfo(this, STATE.Attack, STATE.None, GetComponent<DetectPlayerCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Alert, _nextStateInfo5);

        List<NextStateInfo> _nextStateInfo6 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Idle, STATE.None, GetComponent<TimeElapsedCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Alert, _nextStateInfo6);

        List<NextStateInfo> _nextStateInfo7 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Idle, STATE.None, GetComponent<TimeElapsedCondition>())
        };
        FSMSystem.I.AddTransition(this, STATE.Recolocate, _nextStateInfo7);
        //falta, entre otras, la transición de recolocate a idle (cuando ha vuelto a su punto inicial de patrulla).
        //la transicion de alert a recolocate cuando pasa X tiempo.
    }

    public void SetNoneState()
    {
        FSMSystem.I.AddState(this, new State(STATE.None));
    }

    public void SetAttackState(){
        List<SteeringBehaviour> behavioursAttackState = new List<SteeringBehaviour>(){
            this.GetComponent<ShootSteeringBehaviour>()
        };
        FSMSystem.I.AddState(this, new State(STATE.Attack));
        FSMSystem.I.AddBehaviours(this, behavioursAttackState, this.states.Find((x) => x.stateName == STATE.Attack));
    }

    public void SetIdleState(){
        FSMSystem.I.AddState(this, new State(STATE.Idle));
    }

    public void SetAlertState()
    {
        List<SteeringBehaviour> behavioursAlertState = new List<SteeringBehaviour>()
        {
            this.GetComponent<MoveLastPositionPlayerSteeringBehaviour>()
        };
        FSMSystem.I.AddState(this, new State(STATE.Alert));
        FSMSystem.I.AddBehaviours(this, behavioursAlertState, this.states.Find((x) => x.stateName == STATE.Alert));
    }

    public void SetRecolocateState()
    {
        FSMSystem.I.AddState(this, new State(STATE.Recolocate));
    }

    public void Update()
    {
        CheckConditions();
        ActBehaviours();
    }
}
