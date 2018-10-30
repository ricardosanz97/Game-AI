using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSteeringBehaviour : SteeringBehaviour
{
    public float shootRate = 2f;
    public float maxDistance;
    public GameObject bullet;
    public Transform barrelGun;

    private float currentTime = 0f;

    private void Start()
    {
        maxDistance = GetComponent<DetectPlayerCondition>().viewRadius;
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
            GameObject _bullet = Instantiate(bullet, barrelGun.position, barrelGun.rotation);
            Vector3 direction = (PlayerController.I.targetObjectRef.transform.position - barrelGun.transform.position).normalized;
            _bullet.GetComponent<BulletBehaviour>().Move(direction);
        }

    }

    public bool TimeRateElapsed()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= shootRate)
        {
            currentTime = 0f;
            return true;
        }
        return false;
    }
}
