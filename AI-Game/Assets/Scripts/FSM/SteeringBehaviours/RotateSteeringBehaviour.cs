using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateSteeringBehaviour : SteeringBehaviour
{
    public float speedRotation = 5f;
    private DetectPlayerCondition _detector;
    private void Awake()
    {
        _detector = GetComponent<DetectPlayerCondition>();
    }
    public override void Act()
    {
        if (_detector.playerDetected)
        {
            return;
        }
        this.transform.Rotate(Vector3.up * Time.deltaTime * speedRotation);
    }
}
