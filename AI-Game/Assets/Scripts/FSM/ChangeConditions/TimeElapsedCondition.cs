using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeElapsedCondition : Condition
{
    public float timeToRecolocate;
    private float currentTime = 0f;
    public override bool Check()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeToRecolocate)
        {
            if (this.name == "EnemyChaser")
            {
                this.GetComponent<PatrolSteeringBehaviour>().patrol = false;
                Debug.Log("he entrado alert");
            }
            currentTime = 0f;
            return true;
        }
        return false;
    }
}
