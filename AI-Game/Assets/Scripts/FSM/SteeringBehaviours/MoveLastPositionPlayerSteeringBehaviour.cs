using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLastPositionPlayerSteeringBehaviour : SteeringBehaviour
{
    private Rigidbody rbEnemy;
    void Start()
    {
        rbEnemy = GetComponent<Rigidbody>();
    }
    public override void Act()
    {
        rbEnemy.velocity = Vector3.zero;
        //Debug.Log("entro");
    }
}
