using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PatrolSteeringBehaviour : SteeringBehaviour
{

    public Transform[] points;
    public float maxSpeed = 4f;

    private int actualPatrolPoint = 0;
    private CharacterController ccPlayer;
    private CharacterController ccEnemy;
    private bool isPathSuccesfull;
    private bool patrol = false;
    private Vector3[] path;
    Vector3 destination;
    int currentPointInPath = 0;
    Vector3 previousNodePosition;
    Vector3 nodePosition;
    float magnitude;
    float realSpeed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        ccPlayer = player.GetComponent<CharacterController>();
        ccEnemy = GetComponent<CharacterController>();
    }

    public override void Act()
    {
        if (!patrol && points != null)
        {
            Pathfinding.PathfindingManager.I.RequestPath(new Pathfinding.PathfindingManager.PathRequest(ccEnemy.transform.position, points[actualPatrolPoint].position, PathReceived, ccPlayer.radius));
            patrol = true;
        }
        Patrol();
        return;
    }

    public void PathReceived(Vector3[] wayPoints, bool isPathSuccessfull)
    {
        Debug.Log("pathReceived");
        path = wayPoints;
        destination = path[0];
    }

    public void Patrol()
    {
        if (Pathfinding.PathfindingManager.I.pathError == true)
        {
            Pathfinding.PathfindingManager.I.RequestPath(new Pathfinding.PathfindingManager.PathRequest(ccEnemy.transform.position, ccPlayer.transform.position, PathReceived, ccPlayer.radius));
            Pathfinding.PathfindingManager.I.pathError = false;
        }

        if (patrol && points != null && path != null)
        {
            //Trying to smooth velocity  
            
           

            realSpeed = maxSpeed / magnitude;
            Debug.Log(magnitude);

            if (Vector3.Distance(ccEnemy.transform.position, destination) < ccEnemy.radius * 2)
            {
                previousNodePosition = path[currentPointInPath];

                currentPointInPath++;

                if (currentPointInPath == path.Length)
                {
                    Debug.Log("ImIN");
                    path = null;
                    currentPointInPath = 0;
                    actualPatrolPoint = randomPoint(actualPatrolPoint);
                    patrol = false;
                    return;
                }

                nodePosition = path[currentPointInPath];
                magnitude = Vector3.Distance(nodePosition, previousNodePosition);
                Debug.Log(magnitude);

                destination = path[currentPointInPath];
            }
            //ccEnemy.Move((destination - trans.position).normalized * maxSpeed * Time.deltaTime);
            ccEnemy.transform.DOMove(destination, maxSpeed* magnitude);
        }

        /*else if(path == null)
        {
            Debug.Log("pathnull");
            ccEnemy.transform.DOMove(points[currentPointInPath].transform.position, maxSpeed*4);
        }*/
    }

    private int randomPoint(int actualPoint)
    {
        int point = actualPoint;
        Debug.Log(points.Length);
        while(actualPoint == point)
        {
            point = Random.Range(0, points.Length);
        }
        return point;
    }
}
