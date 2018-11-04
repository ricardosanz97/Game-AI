using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSteeringBehaviour : SteeringBehaviour
{
    public float hitRate = 1f;
    public float maxDistance = 2f;
    public int hitDamage = 20;
    public bool hitEnabled = false;

    private float currentTime = 0f;
    

    private void Start()
    {
    }

    public override void Act()
    {
        float distanceToPlayer = Vector3.Distance(PlayerController.I.targetObjectRef.transform.position, this.transform.position);
        if (distanceToPlayer > maxDistance)
        {
            Debug.Log("distanceToPlayer is higher than maxDistance");
            return;
        }

        if (TimeRateElapsed())
        {
            hitEnabled = true;
            PlayerController.I.GetComponent<PlayerHealth>().ReceiveDamage(hitDamage);
        }

    }

    public bool TimeRateElapsed()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= hitRate)
        {
            currentTime = 0f;
            return true;
        }
        hitEnabled = false;
        return false;
    }
}
    

