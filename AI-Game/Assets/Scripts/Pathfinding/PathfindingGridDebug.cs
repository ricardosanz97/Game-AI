using System.Collections;
using System.Collections.Generic;
using CustomPathfinding;
using UnityEngine;

[RequireComponent(typeof(PathfindingGrid))]
public class PathfindingGridDebug : MonoBehaviour
{
    [Range(0.3f,0.9f)]
    public float cubeSeparation = 0.4f;
    
    private PathfindingGrid _grid;

    private void Start()
    {
        _grid = GetComponent<PathfindingGrid>();
    }

    private void OnDrawGizmos()
    {
        if (_grid != null)
        {
            //draw obstacles and no obstacles
            for (int i = 0; i < _grid.GridSizeX; i++)
            {
                for (int j = 0; j < _grid.GridSizeZ; j++)
                {
                    Node n = _grid.Grid[i, j];
						
                    if(n.NodeType == Node.ENodeType.NonWalkable)
                        Gizmos.color = Color.red;
                    else if(n.NodeType == Node.ENodeType.Walkable)
                        Gizmos.color = Color.green;
                    else if(n.NodeType == Node.ENodeType.Invisible)
                        continue;
						
                    Gizmos.DrawCube(n.WorldPosition,Vector3.one * cubeSeparation);
                }
            }
        }
    }
}
