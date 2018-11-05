using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public abstract class NPCStatesBehaviour : MonoBehaviour
{

    private DebugStateSystem dss;

    public List<Transition> transitions;
    public List<State> states;
    public abstract void SetTransitions();
    public abstract void SetStates();

    public State currentState;
    public State initialState;

    public List<Transition> currentTransitions;

    public virtual void Start()
    {
        initialState = currentState;
    }

    public virtual void ActBehaviours()
    {
        if (currentState.behaviours != null)
        {
            foreach (SteeringBehaviour behaviour in currentState.behaviours)
            {
                behaviour.Act();
            }
        }
    }

    public virtual void CheckConditions()
    {
        foreach (Transition trans in currentTransitions)
        {
            foreach (NextStateInfo nsi in trans.nextStateInfo)
            {
                bool result = nsi.changeCondition.Check();
                //Debug.Log(nsi.changeCondition.ToString() + "= " + result);
                if (result == true)
                {
                    if (nsi.stateCaseTrue.stateName == STATE.None)
                    {
                        continue;
                    }
                    currentState = nsi.stateCaseTrue;
                }
                else
                {
                    if (nsi.stateCaseFalse.stateName == STATE.None)
                    {
                        continue;
                    }
                    currentState = nsi.stateCaseFalse;
                }
                currentTransitions = transitions.FindAll(x => x.currentState == currentState);
                return;
            }
        }
    }

    public void SetInitialState()
    {
        this.currentState = this.initialState;
        currentTransitions = this.transitions.FindAll((x) => x.currentState == this.currentState);
    }

    
}
