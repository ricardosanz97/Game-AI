using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChaseSteeringBehaviour : SteeringBehaviour
{

    public float anticipationMultiplier;
    public float maxSpeed = 4f;

    private CharacterController ccPlayer;
    private CharacterController ccEnemy;
    private Vector3[] path;
    private Vector3 destination;
    private int pathIndex;
    float timer;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
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

        return;
    }

    public void PathReceived(Vector3[] wayPoints, bool isPathSuccessfull)
    {
        path = wayPoints;
        Pursuit();
    }
    public void Pursuit()
    {

        if (Pathfinding.PathfindingManager.I.pathError == true)
        {
            Pathfinding.PathfindingManager.I.RequestPath(new Pathfinding.PathfindingManager.PathRequest(ccEnemy.transform.position, ccPlayer.transform.position, PathReceived, ccPlayer.radius));
            Pathfinding.PathfindingManager.I.pathError = false;
        }

        if (path != null)
        {
            /*Vector3 toEvader = base.player.position - trans.position;

            float relativeHeading = Vector3.Dot(base.player.transform.forward, trans.forward);

            float lookAheadTime = toEvader.magnitude / (maxSpeed + ccPlayer.velocity.magnitude);

            Debug.Log(base.player.velocity);
            Debug.DrawLine(trans.position, trans.position + ccPlayer.velocity * 50, Color.red);

            ccEnemy.Move(((destination + controller.velocity * (lookAheadTime * anticipationMultiplier)) - trans.position).normalized * maxSpeed * Time.deltaTime);

            if(Vector3.Distance(trans.position, destination) < ccPlayer.radius * 2)
            {
                pathIndex++;
            }
            */
           
            ccEnemy.transform.DOMove(path[1]-(ccPlayer.transform.forward*2), maxSpeed);          
            ccEnemy.transform.LookAt(ccPlayer.transform);
        }
    }

}
