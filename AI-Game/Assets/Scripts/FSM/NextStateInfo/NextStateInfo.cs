using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class NextStateInfo {

    public State stateCaseTrue;
    public State stateCaseFalse;
    public Condition changeCondition;
    public NextStateInfo (NPCStatesBehaviour npc, STATE stateTrue, STATE stateFalse, Condition changeCondition)
    {
        this.stateCaseTrue = FSMSystem.I.FindState(npc, stateTrue);
        this.stateCaseFalse = FSMSystem.I.FindState(npc, stateFalse);
        this.changeCondition = changeCondition;
    }
	
}
