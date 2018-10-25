using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolSteeringBehaviour : SteeringBehaviour
{

    public Transform[] points;
    public bool arriveActive;
    public float maxSpeed = 4f;
    public float modifierDeceleration = 0.8f;

    private int actualPatrolPoint = 0;
    private Rigidbody rb;
    private Transform trans;
    private CharacterController ccPlayer;
    private CharacterController ccEnemy;
    private bool isPathSuccesfull;
    private float distance;
    float realDistance;
    float distanceBetweenNodes = 0;
    Vector3 startPos;
    [SerializeField]
    private Deceleration deceleration;
    private bool patrol = false;
    private Vector3[] path;
    Vector3 destination;
    int currentPointInPath = 0;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        ccPlayer = player.GetComponent<CharacterController>();
        ccEnemy = GetComponent<CharacterController>();
        startPos = trans.position;
        startPos.y = 0;
    }

    public override void Act()
    {
        
        if (!patrol && points != null) Pathfinding.PathfindingManager.I.RequestPath(new Pathfinding.PathfindingManager.PathRequest(startPos, points[actualPatrolPoint].position, PathReceived, ccPlayer.radius));
        Patrol();
        return;
    }

    public void PathReceived(Vector3[] wayPoints, bool isPathSuccessfull)
    {
        patrol = true;
        Debug.Log("pathReceived");
        path = wayPoints;
        destination = path[1];
    }

    public void Patrol()
    {

        if (patrol && points != null && path != null)
        {   
            if (Vector3.Distance(trans.position, destination) < ccEnemy.radius * 2)
            {
                currentPointInPath++;
                Debug.Log("currentPoint "+currentPointInPath);

                if (currentPointInPath == path.Length)
                {
                    path = null;
                    currentPointInPath = 0;
                    startPos = trans.position;
                    startPos.y = 0;
                    actualPatrolPoint = randomPoint(actualPatrolPoint);
                    patrol = false;
                    return;
                }
                    
                destination = path[currentPointInPath];                   
            }
            ccEnemy.Move((destination - trans.position).normalized * maxSpeed * Time.deltaTime);
        }
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
