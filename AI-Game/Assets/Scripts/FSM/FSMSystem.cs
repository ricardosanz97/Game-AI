using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum STATE
{
    Default,
    Attack,
    Patrol,
    Idle,
    Alert,
    Recolocate,
    Hidden,
    Hit,
    None
}

public enum BEHAVIOUR
{
    Patrol,
    Hit,
    Shoot,
    Rotate,
}

public class FSMSystem : Singleton<FSMSystem>{

	public void AddTransition(NPCStatesBehaviour npc, STATE currentStateName, List<NextStateInfo> nextStateInfos)
    {
        State _currentState = npc.states.Find((x) => x.stateName == currentStateName);
        List<NextStateInfo> _nextStateInfos = nextStateInfos;

        Transition _transition = new Transition(_currentState, _nextStateInfos);
        foreach (Transition trans in npc.transitions)
        {
            if (trans == _transition) //TODO: falta delimitarlo más.
            {
                Debug.Log("Can't add it because there already is a transition like this");
            }
        }
        npc.transitions.Add(_transition);
    }

    public void DeleteTransition(NPCStatesBehaviour npc, State currentState, List<NextStateInfo> nextStatesInfos)
    {
        Transition _transition = new Transition(currentState, nextStatesInfos);
        foreach (Transition trans in npc.transitions)
        {
            if (trans == _transition)
            {
                npc.transitions.Remove(trans);
            }
        }
    }

    public void AddState(NPCStatesBehaviour npc, State newState)
    {
        npc.states.Add(newState);
    }

    public void AddBehaviours(NPCStatesBehaviour npc, List<SteeringBehaviour> _behaviours, State _state)
    {
        foreach (State state in npc.states)
        {
            if (state == _state)
            {
                state.behaviours = _behaviours;
            }
        }
    }

    public State GetNextState(NPCStatesBehaviour npc, bool _bool, State currentState, Condition condition)
    {
        foreach (Transition trans in npc.transitions)
        {
            if (trans.currentState == currentState)
            {
                foreach (NextStateInfo nsi in trans.nextStateInfo)
                {
                    if (condition == nsi.changeCondition && _bool)
                    {
                        return nsi.stateCaseTrue;
                    }
                    else if (condition == nsi.changeCondition && !_bool)
                    {
                        return nsi.stateCaseFalse;
                    }
                }
            }
        }
        return null;
    }

    public State FindState(NPCStatesBehaviour npc, STATE stateName)
    {
        State returnState = npc.states.Find((x) => x.stateName == stateName);
        return returnState;
    }
}
