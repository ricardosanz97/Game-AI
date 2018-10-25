using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPathfinding
{
    //interface for constructing Graphs
    public interface IWeightedGraph
    {
        float Cost(Node a, Node b);
        IEnumerable<Node> GetNeighbors(Node current);
    }
}