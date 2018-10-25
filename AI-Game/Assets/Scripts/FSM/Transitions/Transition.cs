using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition {

    public State currentState;
    public List<NextStateInfo> nextStateInfo;

    public Transition (State current, List<NextStateInfo> nextStateInfo)
    {
        this.currentState = current;
        this.nextStateInfo = nextStateInfo;
    }

}
