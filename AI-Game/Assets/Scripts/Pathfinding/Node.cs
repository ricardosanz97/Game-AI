using System;
using UnityEngine;

namespace CustomPathfinding
{
    public struct Node : IEquatable<Node>
    {
        private int _cost;
        public Vector3 WorldPosition { get; set; }
        public int GridX { get; set; }
        public int GridZ { get; set; }
        public bool Walkable { get; set; }

        public Node(int cost, Vector3 worldPosition, int gridX, int gridZ, bool walkable) : this()
        {
            _cost = cost;
            WorldPosition = worldPosition;
            GridX = gridX;
            GridZ = gridZ;
            Walkable = walkable;
        }

        public int Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public bool Equals(Node other)
        {
            return GridX == other.GridX && GridZ == other.GridZ;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (GridX * 397) ^ GridZ;
            }
        }
    
    }
}
