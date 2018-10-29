using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PatrolSteeringBehaviour : SteeringBehaviour
{

    public Transform[] patrolPoints;
    public bool randomPatrol;
    public float maxSpeed = 4f;

    private int actualPatrolPoint = 0;
    private int previousPatrolPoint = 0;
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
        Debug.Log(actualPatrolPoint);
        Debug.Log(patrolPoints.Length);
        if (!patrol && patrolPoints != null)
        {
            Pathfinding.PathfindingManager.I.RequestPath(new Pathfinding.PathfindingManager.PathRequest(ccEnemy.transform.position, patrolPoints[actualPatrolPoint].position, PathReceived, ccPlayer.radius));
            patrol = true;
        }
        Patrol();
        return;
    }

    public void PathReceived(Vector3[] wayPoints, bool isPathSuccessfull)
    {
        patrol = true;
        path = wayPoints;
        destination = path[0];
    }

    public void Patrol()
    {
        if (Pathfinding.PathfindingManager.I.pathError == true)
        {
            errorController();
            Pathfinding.PathfindingManager.I.pathError = false;
        }

        if (patrol && patrolPoints != null && path != null)
        {
                        
            realSpeed = maxSpeed / magnitude;
            if (Vector3.Distance(ccEnemy.transform.position, destination) < ccEnemy.radius * 4)
            {
                previousNodePosition = path[currentPointInPath];

                currentPointInPath++;

                if (path != null && currentPointInPath == path.Length)
                {
                    Debug.Log("ImIN");
                    path = null;
                    currentPointInPath = 0;
                    if (randomPatrol)
                    {
                        previousPatrolPoint = actualPatrolPoint;
                        actualPatrolPoint = randomPoint(actualPatrolPoint);
                    }
                    else
                    {
                        previousPatrolPoint = actualPatrolPoint;
                        actualPatrolPoint += 1;
                        if (actualPatrolPoint >= patrolPoints.Length) actualPatrolPoint = 0;
                    }

                    Debug.Log(actualPatrolPoint);
                    patrol = false;
                    return;
                }

                nodePosition = path[currentPointInPath];
                magnitude = Vector3.Distance(nodePosition, previousNodePosition);

                destination = path[currentPointInPath];
            }
            //Debug.Log(Quaternion.FromToRotation(patrolPoints[previousPatrolPoint].position, patrolPoints[actualPatrolPoint].position));
            ccEnemy.transform.DOMove(destination, maxSpeed).SetEase(Ease.Linear);
            ccEnemy.transform.rotation = Quaternion.Slerp(ccEnemy.transform.rotation, Quaternion.LookRotation(patrolPoints[actualPatrolPoint].position - patrolPoints[previousPatrolPoint].position), .2f);
;        }

        /*else if(path == null)
        {
            Debug.Log("pathnull");
            ccEnemy.transform.DOMove(points[currentPointInPath].transform.position, maxSpeed*4);
        }*/
    }

    private void errorController()
    {
        if (randomPatrol)
        {
            previousPatrolPoint = actualPatrolPoint;
            actualPatrolPoint = randomPoint(actualPatrolPoint);
        }
        else
        {
            previousPatrolPoint = actualPatrolPoint;
            actualPatrolPoint += 1;
            if (actualPatrolPoint >= patrolPoints.Length) actualPatrolPoint = 0;
        }
        Pathfinding.PathfindingManager.I.RequestPath(new Pathfinding.PathfindingManager.PathRequest(ccEnemy.transform.position, patrolPoints[actualPatrolPoint].position, PathReceived, ccPlayer.radius));

    }

    private int randomPoint(int actualPoint)
    {
        int point = actualPoint;
        Debug.Log(patrolPoints.Length);
        while(actualPoint == point)
        {
            point = Random.Range(0, patrolPoints.Length);
        }
        return point;
    }
}
