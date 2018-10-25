using System.Collections;
using System.Collections.Generic;
using Invector;
using Pathfinding;
using UnityEditor;
using UnityEngine;

public class DebugAgentPathfinding : MonoBehaviour
{
    private float speed = 5.0f;
    public Transform targetPosition;
    
    private CharacterController cc;
    
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        Vector3 startPos = transform.position;
        startPos.y = 0;
        PathfindingManager.I.RequestPath(new PathfindingManager.PathRequest(startPos,targetPosition.position,OnPathFound,cc.radius));
        
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Follow Path"))
        {
            Vector3 startPos = transform.position;
            startPos.y = 0;
            PathfindingManager.I.RequestPath(new PathfindingManager.PathRequest(startPos,targetPosition.position,OnPathFound,cc.radius));
        }
    }

    public void OnPathFound(Vector3[] waypoints, bool isPathSuccessful)
    {
        if(isPathSuccessful)
            StartCoroutine(FollowPath(waypoints));
    }
    
    private IEnumerator FollowPath(Vector3[] waypoints)
    {            
        int currentIndex = 0;
        Vector3 destination = waypoints[0];
        
        while (true)
        {
            if (Vector3.Distance(transform.position, destination) < cc.radius * 2)
            {
                currentIndex++;
                
                if(currentIndex == waypoints.Length)
                    yield break;
                
                destination = waypoints[currentIndex];
            }

            cc.Move((destination - transform.position).normalized * speed * Time.deltaTime);
            yield return null;
        }

    }

}
