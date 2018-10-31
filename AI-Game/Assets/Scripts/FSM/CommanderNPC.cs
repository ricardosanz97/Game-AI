using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderNPC : NPCStatesBehaviour
{
    public override void Start()
    {
        SetStates();
        SetTransitions();

        currentState = states.Find((x) => x.stateName == STATE.Idle);
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);

        base.Start();
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
        List<SteeringBehaviour> behavioursIdleState = new List<SteeringBehaviour>()
        {
            this.GetComponent<RotateSteeringBehaviour>(),
            this.GetComponent<FacePlayerSteeringBehaviour>()
        };
        FSMSystem.I.AddState(this, new State(STATE.Idle));
        FSMSystem.I.AddBehaviours(this, behavioursIdleState, this.states.Find((x)=>x.stateName == STATE.Idle));
    }

    public void Update()
    {
        CheckConditions();
        ActBehaviours();
    }
   
}
