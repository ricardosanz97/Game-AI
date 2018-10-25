using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SteeringBehaviour : MonoBehaviour{

    public abstract void Act();

    [HideInInspector]
    protected Rigidbody player;

    protected enum Deceleration { slow = 3, normal = 2, fast = 1 };

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>(); 
    }

    
    protected Vector3 Seek(Vector3 targetPos, Vector3 pos, float maxSpeed)
    {
        Vector3 desiredVelocity = Vector3.Normalize(targetPos - pos) * maxSpeed;
        return desiredVelocity;
    }

    protected Vector3 Arrive(Vector3 targetPos, Vector3 pos, float maxSpeed, Deceleration d, float modifierDeceleration)
    {
        Vector3 toTarget = targetPos - pos;
        float dist = toTarget.magnitude;
        float speed = dist / (modifierDeceleration * (float)d);

        speed = Mathf.Min(speed, maxSpeed);

        Vector3 desiredVelocity = toTarget * speed / dist;
        desiredVelocity.y = 0;

        return desiredVelocity;      
    }
    


}
