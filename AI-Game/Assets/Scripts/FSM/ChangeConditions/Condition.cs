using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Condition : MonoBehaviour {
    public abstract bool Check();
}
