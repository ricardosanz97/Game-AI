using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FacePlayerSteeringBehaviour : SteeringBehaviour
{
    private Transform player;
    private DetectPlayerCondition _detector;
    public float faceSpeed = 5f;
    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>().gameObject.transform;
        _detector = GetComponent<DetectPlayerCondition>();
    }

    public override void Act()
    {
        if (_detector.playerDetected)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), faceSpeed * Time.deltaTime);
            Quaternion.LookRotation(direction);
        }
       
        
    }
}
