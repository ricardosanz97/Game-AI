using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChaseSteeringBehaviour : SteeringBehaviour
{

    public float anticipationMultiplier;
    public float maxSpeed = 4f;
    public float distanceToHit = 3f;

    private Rigidbody rbEnemy;
    private GameObject player;
    private CharacterController ccPlayer;
    private CharacterController ccEnemy;
    private Vector3[] path;
    private int pathIndex;
    float timer;


    void Start()
    {
        player = PlayerController.I.gameObject;
        rbEnemy = GetComponent<Rigidbody>();
        ccPlayer = player.GetComponent<CharacterController>();
        ccEnemy = GetComponent<CharacterController>();

    }
    public override void Act()
    {
        timer += Time.deltaTime;
        if (timer > 0.2f)
        {
            Pathfinding.PathfindingManager.I.RequestPath(new Pathfinding.PathfindingManager.PathRequest(ccEnemy.transform.position, ccPlayer.transform.position, PathReceived, ccPlayer.radius));
            timer = 0;
        }
    }

    public void PathReceived(Vector3[] wayPoints, bool isPathSuccessfull)
    {
        path = wayPoints;
        Pursuit(isPathSuccessfull);
    }
    
    public void Pursuit(bool isPathSuccesfull)
    {

        if (!isPathSuccesfull)
        {
            Pathfinding.PathfindingManager.I.RequestPath(new Pathfinding.PathfindingManager.PathRequest(ccEnemy.transform.position, ccPlayer.transform.position, PathReceived, ccPlayer.radius));
        }

        if (path != null)
        {
            Debug.Log("he entrado");
            Vector3 toEvader = ccPlayer.transform.position - rbEnemy.position;

            float relativeHeading = Vector3.Dot(ccPlayer.transform.forward, ccEnemy.transform.forward);

            float lookAheadTime = toEvader.magnitude / (maxSpeed + rbEnemy.velocity.magnitude);

            if (Vector3.Distance(ccEnemy.transform.position, ccPlayer.transform.position) < distanceToHit) rbEnemy.velocity = Vector3.zero;
            else
            {
                rbEnemy.velocity = (((path[1] + rbEnemy.velocity * (lookAheadTime * anticipationMultiplier)) - ccEnemy.transform.position).normalized * maxSpeed);
            }
            ccEnemy.transform.LookAt(ccPlayer.transform);
        }
    }

}
