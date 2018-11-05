using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAssassinSteeringBehaviour : SteeringBehaviour
{

    public float speed = 10.0f;

    public GameObject player;
    private bool canEnterCorutine = true;
    private float timer = 0;
    public CharacterController cc;
    private Coroutine coroutine;

    private void Start()
    {
        player = PlayerController.I.gameObject;
        cc = GetComponent<CharacterController>();
        Vector3 startPos = transform.position;
    }

    public override void Act()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < cc.radius * 4)
            cc.Move(Vector3.zero);
        
    }

    public void OnPathFound(Vector3[] waypoints, bool isPathSuccessful)
    {
        if (isPathSuccessful)
        {
            if(coroutine != null)
                StopCoroutine(coroutine);

            coroutine = StartCoroutine(FollowPath(waypoints));
        }
    }

    private IEnumerator FollowPath(Vector3[] waypoints)
    {
        timer = 0;
        int currentIndex = 0;
        Vector3 destination = waypoints[0];

        while (true)
        {
            if (Vector3.Distance(transform.position, destination) < cc.radius * 2)
            {
                currentIndex++;

                if (currentIndex == waypoints.Length)
                {
                    yield break;
                }

                destination = waypoints[currentIndex];
                timer = 0;
            }

            timer += Time.deltaTime;
            cc.Move(((destination - transform.position).normalized) * speed * Time.deltaTime);
            transform.LookAt(player.transform);
            yield return null;
        }

    }

    [ContextMenu("DebugPathfinding")]
    public void RequestPathOnce()
    {
        InvokeRepeating("RequestPath", 0.01f, 1.2f);
    }

    public void RequestPath()
    {
        Vector3 startPos = transform.position;
        PathfindingManager.I.RequestPath(new PathfindingManager.PathRequest(transform.position, player.transform.position, OnPathFound, cc.radius));
        if (GetComponent<AssassinNPC>().currentState.stateName != STATE.Attack)
        {
            CancelInvoke("RequestPath");
        }
    }
}
