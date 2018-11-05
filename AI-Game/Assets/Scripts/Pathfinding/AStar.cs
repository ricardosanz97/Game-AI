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
        private static int MaxBfsSteps = 10;
        public static bool isDebugMode = true;
        
        //by now it uses a GridDebugger Class. It should have as a paramenter a IAstarSearchableSurface or something like that
        public static void AStarSearch(PathfindingGrid pathfindingGrid, PathfindingManager.PathRequest request, Action<PathfindingManager.PathResult> callback)
        {
            //estos diccionarios se resetean cada vez porque estan dentro de un metodo estatico. Solo hay una instancia de el en memoria
            //the camefrom path can be reconstructed using the parent field in the node itself
            Dictionary<Node, Node> pathSoFar = new Dictionary<Node, Node>();
            Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
            Stopwatch sw = new Stopwatch();

            if (isDebugMode)
            {
                Debug.Log("Started search at thread number " + Thread.CurrentThread.ManagedThreadId);
                sw.Start();
                Profiler.BeginThreadProfiling("AStar", "Thread " + Thread.CurrentThread.ManagedThreadId);                
            }

            Node source = pathfindingGrid.GetNodeFromWorldPosition(request.PathStart);
            Node goal = pathfindingGrid.GetNodeFromWorldPosition(request.PathEnd);

            if (source.NodeType != Node.ENodeType.Walkable || goal.NodeType != Node.ENodeType.Walkable)
            {
                if (goal.NodeType != Node.ENodeType.Walkable)
                {
                    Debug.LogError("No se puede llegar hasta el nodo indicado");
                    callback( new PathfindingManager.PathResult(null,false,request.Callback, Thread.CurrentThread.ManagedThreadId));
                    return;
                }
                else
                {
                    Debug.LogError("No se puede inciar un camino desde este nodo, buscando uno nuevo... ");
                    source = Bfs(pathfindingGrid, source, MaxBfsSteps);
                }
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
                    if (isDebugMode)
                    {
                        sw.Stop();
                        mediciones.Add(sw.ElapsedMilliseconds);
                        Debug.Log("Tiempo medio de pathfinding: " + GetAverageSearchTime() + " ms.");
                        Debug.Log("Finished search at thread number " + Thread.CurrentThread.ManagedThreadId + " in " + sw.ElapsedMilliseconds + "ms.");                        
                        Profiler.EndThreadProfiling();
                    }
                    break;
                }

                foreach (var next in pathfindingGrid.GetNeighbors(currentNode))
                {
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

            Vector3[] path = ReconstructPath(pathSoFar, goal);
            Vector3[] smoothedWaypoints = pathfindingGrid.SmoothPath(path, request.AgentRadius);
            callback( new PathfindingManager.PathResult(smoothedWaypoints,true,request.Callback, Thread.CurrentThread.ManagedThreadId));
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
            //return Mathf.Abs(Vector3.Distance(node1.WorldPosition, node2.WorldPosition));
            
            return Math.Abs(node2.GridX - node1.GridX) + Math.Abs(node2.GridZ - node1.GridZ);
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

        public static Node Bfs(PathfindingGrid grid, Node start, int maxSearchSteps)
        {
            //diccionario de los visitado, no contiene duplicados
            Dictionary<Node,bool>  visited = new Dictionary<Node, bool>(grid.NodeCount);

            for (int i = 0; i < grid.GridSizeX; i++)
            {
                for (int j = 0; j < grid.GridSizeZ; j++)
                {
                    visited.Add(grid.Grid[i,j],false);
                }
            }

            Queue<Node> queue = new Queue<Node>(grid.NodeCount/4);
            queue.Enqueue(start);
            visited[start] = true;
            int steps = 0;

            while (queue.Count > 0 || steps < maxSearchSteps)
            {
                Node front = queue.Dequeue();

                if (front.NodeType == Node.ENodeType.Walkable)
                    return front;
                
                foreach (var node in grid.GetNeighbors(front))
                {
                    if (node.NodeType == Node.ENodeType.Walkable)
                        return node;
                    
                    if (!visited[node])
                    {
                        visited[node] = true;
                        queue.Enqueue(node);
                    }
                }

                steps += 1;
            }

            return start;
        }

        public static void DebugPath(Vector3[] path)
        {
            for (int i = 0; i < path.Length - 1; i++)
            {
                Debug.DrawLine(path[i],path[i + 1],Color.yellow, 10.0f);
            }
        }
    }
    
}