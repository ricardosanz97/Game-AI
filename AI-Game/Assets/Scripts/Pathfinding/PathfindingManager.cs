using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using CustomPathfinding;
using UnityEngine;

namespace Pathfinding
{
    public class PathfindingManager : Singleton<PathfindingManager>
    {
        private const int RESULTS_QUEUE_CAPACITY = 64;
        private readonly Queue<PathResult> results = new Queue<PathResult>(RESULTS_QUEUE_CAPACITY);
        private readonly Queue<PathRequest> requestsToProcess = new Queue<PathRequest>(RESULTS_QUEUE_CAPACITY);

        [Range(1,4)]
        public int _maxThreadCount;
        private PathfindingGrid _pathfindingGraph;
        private WaitForSeconds _wfs;
        private Thread[] _threads;

        private void Awake()
        {
           _pathfindingGraph = GetComponent<PathfindingGrid>();
            _wfs = new WaitForSeconds(0.16f);
            _threads = new Thread[_maxThreadCount];
        }

        private void Start()
        {
            StartCoroutine(ReadPathfindingResults());  
        }

        private IEnumerator ReadPathfindingResults()
        {
            while (true)
            {
                yield return _wfs;

                lock (results)
                {
                    //read only in case that we have a result queued up.
                    if (results.Count > 0)
                    {
                        PathResult result = results.Dequeue();
                        result.callback(result.path, result.success);

                    }
                }
            }


        }

        /* Non multithreading version
        public Vector3[] RequestPath(Vector3 source, Vector3 target, float agentRadius)
        {
            Node sourceNode = _pathfindingGraph.GetNodeFromWorldPosition(source);
            Node targetNode = _pathfindingGraph.GetNodeFromWorldPosition(target);

            if (!targetNode.Walkable)
            {
                Debug.LogError("No se puede llegar hasta el nodo indicado");
                return null;
            }
            
            _pathfindingGraph.AStar.AStarSearch(_pathfindingGraph,sourceNode,targetNode);
            
            return _pathfindingGraph.SmoothPath(_pathfindingGraph.ReconstructPath(targetNode), agentRadius);
        }*/


        public void FinishedProcessingPath(PathResult result)
        {
            lock (results)
            {
                results.Enqueue(result);
            }
        }
        
        public void RequestPath(PathRequest request)
        {
            requestsToProcess.Enqueue(request); 
        }

        //TODO hacer que haya threads background no que se cree uno cada vez
        private void Update()
        {
            if (requestsToProcess.Count > 0)
            {
                for (var index = 0; index < _threads.Length; index++)
                {
                    if (_threads[index] == null || !_threads[index].IsAlive)
                    {
                        PathRequest request = requestsToProcess.Dequeue();
                        ThreadStart threadStart = new ThreadStart(() =>
                            AStar.AStarSearch(_pathfindingGraph, request, FinishedProcessingPath));

                        _threads[index] = new Thread(threadStart);

                        _threads[index].Start();
                    }
                }
            }
        }


        public struct PathResult
        {
            public Vector3[] path;
            public bool success;
            public int ThreadId;
            public Action<Vector3[], bool> callback;

            public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback, int threadId    )
            {
                this.path = path;
                this.success = success;
                this.callback = callback;
                this.ThreadId = threadId;
            }
        }
        
        public struct PathRequest
        {
            public Vector3 PathStart;
            public Vector3 PathEnd;
            public readonly Action<Vector3[], bool> Callback;
            public readonly float AgentRadius;

            public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback, float agentRadius)
            {
                this.PathStart = pathStart;
                this.PathEnd = pathEnd;
                this.Callback = callback;
                this.AgentRadius = agentRadius;
            }
        }
    }
}
