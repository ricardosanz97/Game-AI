using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State{

    public STATE stateName;
    public List<SteeringBehaviour> behaviours;
    public State (STATE stateName)
    {
        this.stateName = stateName;
    }

}
