using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolSteeringBehaviour : SteeringBehaviour
{

    public Transform[] patrolPoints;
    public bool randomPatrol;
    public float maxSpeed = 4f;
    public GameObject SphereDebug;
    public bool patrol = false;

    private int actualPatrolPoint = 0;
    private int previousPatrolPoint = 0;
    private CharacterController ccPlayer;
    private CharacterController ccEnemy;
    private bool isPathSuccesfull;
    private GameObject player;
    private Vector3[] path;
    Vector3 destination;
    int currentPointInPath = 0;
    Vector3 previousNodePosition;
    Vector3 nodePosition;
    float magnitude;
    float realSpeed;
    Vector3 startPos;
    private bool canPatrol = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ccPlayer = player.GetComponent<CharacterController>();
        ccEnemy = GetComponent<CharacterController>();
        startPos = ccEnemy.transform.position;
    }

    public override void Act()
    {
        //Debug.Log(actualPatrolPoint);
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
        canPatrol = isPathSuccessfull;
        path = wayPoints;
        currentPointInPath = 0;
        destination = path[currentPointInPath];
        //Instantiate(SphereDebug, destination, Quaternion.identity);

    }

    public void Patrol()
    {
        if (!canPatrol)
        {
            recalculatePath();
        }

        if (patrol && patrolPoints != null && path != null)
        {
                        
            if (Vector3.Distance(ccEnemy.transform.position, destination) < ccEnemy.radius * 2)
            {

                currentPointInPath++;
                Debug.Log("current " + currentPointInPath + "length " + path.Length);

                if (path != null && currentPointInPath == path.Length)
                {
                    path = null;
                    currentPointInPath = 0;
                    if (randomPatrol)
                    {
                        actualPatrolPoint = RandomPoint(actualPatrolPoint);
                    }
                    else
                    {
                        actualPatrolPoint += 1;
                        if (actualPatrolPoint >= patrolPoints.Length) actualPatrolPoint = 0;
                    }

                    patrol = false;
                    return;
                }

                destination = path[currentPointInPath];
                //Instantiate(SphereDebug, destination, Quaternion.identity);
                ccEnemy.Move(Vector3.zero);
                return;
            }

            else 
            {
                Vector3 direction = destination - transform.position;
                direction = direction.normalized;
                ccEnemy.Move(direction * maxSpeed * Time.deltaTime);
                ccEnemy.transform.LookAt(new Vector3(transform.position.x, transform.position.y, transform.position.z) + new Vector3(direction.x, 0, direction.z));
                //ccEnemy.transform.rotation = Quaternion.Slerp(ccEnemy.transform.rotation, Quaternion.LookRotation(patrolPoints[actualPatrolPoint].position - patrolPoints[previousPatrolPoint].position), .2f);
                return;
            }
        }
        ccEnemy.Move(Vector3.zero);
    }

    public void recalculatePath()
    {
        if (randomPatrol)
        {
            actualPatrolPoint = RandomPoint(actualPatrolPoint);
        }
        else
        {
            actualPatrolPoint += 1;
            if (actualPatrolPoint >= patrolPoints.Length) actualPatrolPoint = 0;
        }

        Pathfinding.PathfindingManager.I.RequestPath(new Pathfinding.PathfindingManager.PathRequest(ccEnemy.transform.position, patrolPoints[actualPatrolPoint].position, PathReceived, ccPlayer.radius));

    }

    private int RandomPoint(int actualPoint)
    {
        int point = actualPoint;
        while(actualPoint == point)
        {
            point = Random.Range(0, patrolPoints.Length);
        }
        return point;
    }
}
