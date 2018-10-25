using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLastPositionPlayerSteeringBehaviour : SteeringBehaviour
{
    public override void Act()
    {
        Debug.Log("Going to the last position where I last saw the player. ");
    }
}
