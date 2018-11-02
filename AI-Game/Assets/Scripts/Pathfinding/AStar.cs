using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Pathfinding;
using Priority_Queue;
using UnityEngine;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

namespace CustomPathfinding
{
    public class AStar
    {
        private static List<float> mediciones = new List<float>();
        
        //by now it uses a GridDebugger Class. It should have as a paramenter a IAstarSearchableSurface or something like that
        public static void AStarSearch(PathfindingGrid pathfindingGrid, PathfindingManager.PathRequest request, Action<PathfindingManager.PathResult> callback)
        {
            //the camefrom path can be reconstructed using the parent field in the node itself
            Dictionary<Node, Node> pathSoFar = new Dictionary<Node, Node>();
            Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
            Stopwatch sw = new Stopwatch();
        
            //Debug.Log("Started search at thread number " + Thread.CurrentThread.ManagedThreadId);
            sw.Start();
            Profiler.BeginThreadProfiling("AStar", "Thread " + Thread.CurrentThread.ManagedThreadId);

            Node source = pathfindingGrid.GetNodeFromWorldPosition(request.PathStart);
            Node goal = pathfindingGrid.GetNodeFromWorldPosition(request.PathEnd);

            if (source.NodeType != Node.ENodeType.Walkable || goal.NodeType != Node.ENodeType.Walkable)
            {
                Debug.LogError("No se puede llegar hasta el nodo indicado");
                PathfindingManager.I.pathError = true;
                return;
            }

            pathSoFar.Clear();
            costSoFar.Clear();

            var frontier = new SimplePriorityQueue<Node>();
            frontier.Enqueue(source, 0);

            pathSoFar[source] = source;
            costSoFar[source] = 0;

            while (frontier.Count > 0)
            {
                Node currentNode = frontier.Dequeue();

                if (currentNode.Equals(goal))
                {
                    sw.Stop();
                    mediciones.Add(sw.ElapsedMilliseconds);
                    Debug.Log("Tiempo medio de pathfinding: " + GetAverageSearchTime() + " ms.");
                    Debug.Log("Finished search at thread number " + Thread.CurrentThread.ManagedThreadId + " in " + sw.ElapsedMilliseconds + "ms.");
                    Profiler.EndThreadProfiling();
                    break;
                }

                foreach (var next in pathfindingGrid.GetNeighbors(currentNode))
                {
                    //if(next == null)
                    //continue;

                    var newCost = costSoFar[currentNode] + pathfindingGrid.Cost(currentNode, next);

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        float priority = newCost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        pathSoFar[next] = currentNode;
                    }
                }
            }
            
            Vector3[] smoothedWaypoints = pathfindingGrid.SmoothPath(ReconstructPath(pathSoFar, goal), request.AgentRadius);
            bool succeded = smoothedWaypoints.Length > 0;
            callback( new PathfindingManager.PathResult(smoothedWaypoints,succeded,request.Callback, Thread.CurrentThread.ManagedThreadId));
        }

        public static Vector3[] ReconstructPath(Dictionary<Node,Node> pathSoFar, Node pathStartNode)
        {
            List<Vector3> path = new List<Vector3>();
            Node current = pathStartNode;
            Node next = pathSoFar[pathStartNode];
			
            path.Add(current.WorldPosition);
            path.Add(next.WorldPosition);

            while (!current.Equals(next))
            {
                current = next;
                next = pathSoFar[next];
				
                path.Add(next.WorldPosition);
            }

            path.Remove(path[path.Count - 1]);

            path.Reverse();
            return path.ToArray();
        }
        
        private static float Heuristic(Node node1, Node node2)
        {
            return Mathf.Abs(Vector3.Distance(node1.WorldPosition, node2.WorldPosition));
        }

        private static float GetAverageSearchTime()
        {
            float total = 0;
            foreach (var medicion in mediciones)
            {
                total += medicion;
            }

            return total / mediciones.Count;
        }
    }
}