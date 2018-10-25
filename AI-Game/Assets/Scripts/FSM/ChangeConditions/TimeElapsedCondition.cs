using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeElapsedCondition : Condition
{
    public float timeToRecolocate;
    private float currentTime;
    public override bool Check()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeToRecolocate)
        {
            currentTime = 0f;
            return true;
        }
        return false;
    }
}
