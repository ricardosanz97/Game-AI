using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderNPC : NPCStatesBehaviour
{
    private DebugStateSystem dss;
    private AudioSource audio;
	public AudioClip risaMalvada;

    private void Awake()
    {
        dss = GetComponent<DebugStateSystem>();
    }

    public override void Start()
    {
		audio = GetComponent<AudioSource> ();

        SetStates();
        SetTransitions();

        currentState = states.Find((x) => x.stateName == STATE.Idle);
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);

        SetDebugs();

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
		audio.clip = risaMalvada;
		audio.Play ();
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
