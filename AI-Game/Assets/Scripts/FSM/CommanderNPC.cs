using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderNPC : NPCStatesBehaviour
{
    private void Start()
    {
        SetStates();
        SetTransitions();

        currentState = states.Find((x) => x.stateName == STATE.Idle);
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);
    }

    public override void SetTransitions()
    {
        

    }

    public override void SetStates()
    {
        SetNoneState();
        SetIdleState();
    }

    public void SetNoneState()
    {
        FSMSystem.I.AddState(this, new State(STATE.None));
    }

    public void SetIdleState(){
        FSMSystem.I.AddState(this, new State(STATE.Idle));
    }

    public void Update()
    {
        CheckConditions();
        ActBehaviours();
    }
   
}
