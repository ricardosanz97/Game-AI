using System.Collections;
using System.Collections.Generic;
using CustomPathfinding;
using UnityEngine;

[RequireComponent(typeof(PathfindingGrid))]
public class PathfindingGridDebug : MonoBehaviour
{
    public float DistanceBetweenCubes = 0.4f;
    
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
						
                    if(!n.Walkable)
                        Gizmos.color = Color.red;
                    else if(n.Walkable)
                        Gizmos.color = Color.green;
						
                    Gizmos.DrawCube(n.WorldPosition,Vector3.one * (_grid.NodeRadius * 2 - DistanceBetweenCubes));
                }
            }
				
            if(_grid.LastPath != null)
            {
                //draw recorded path
                Gizmos.color = Color.cyan;
                foreach (Vector3 pos in _grid.LastPath)
                {
                    Gizmos.DrawCube(pos,Vector3.one * (_grid.NodeRadius * 2 - DistanceBetweenCubes));
	
                }
            }
        }
    }
}
