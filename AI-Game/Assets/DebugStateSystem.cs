using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugStateSystem : MonoBehaviour {

    public Text NPCName;
    public Text currentState;
    public void SetNPCName(string name)
    {
        NPCName.text = name;
    }

    public void SetCurrentState(string state)
    {
        this.currentState.text = state;
    }
}
