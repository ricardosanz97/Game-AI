using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAliveCondition : Condition
{
    public override bool Check()
    {
        return GameController.I.playerAlive;
    }
}
