using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class NavigationGrid : MonoBehaviour
{
    [SerializeField]
    private Vector3 _position;
    [SerializeField]
    private Vector2Int _gridSize;
    [SerializeField]
    private LayerMask _unwalkableArea;
    Node[,] _nodeGrid;

    public Vector2Int GridSize { get { return _gridSize; } }
    public Node[,] Nodes { get { return _nodeGrid; } }

    private void Awake()
    {
        CreateGrid(_gridSize.x, _gridSize.y);
    }

    
    private void CreateGrid(int gridX, int gridY)
    {
        //Resizes the node grid to be the grid size
        _nodeGrid = new Node[gridX,gridY];

        //The bottom left of the grid
        Vector3 worldBottomLeft = transform.position - (Vector3.right * gridX / 2.16f) -
            (Vector3.up * gridY/2.16f);

        //Iterate through the x axis
        for (int i = 0; i < gridX; i++)
        {
            //Iterate through the y axis
            for (int j = 0; j < gridY; j++)
            {
                //The position of the node
                Vector3 nodeWorldPos = worldBottomLeft + (Vector3.right * i) + (Vector3.up * j);
                bool walkable = !(Physics2D.OverlapCircle(nodeWorldPos, 0.2f, _unwalkableArea));
                //Create a node at that spot on the grid
                _nodeGrid[i, j] = new Node(walkable, nodeWorldPos);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_nodeGrid != null)
        {
            foreach (Node n in _nodeGrid)
            {
                if (n.IsWalkable)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(n.WorldPos, new Vector3(1, 1, 0));
                    Gizmos.color = Color.yellow;
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(n.WorldPos, new Vector3(1, 1, 0));
                }
            }
        }
    }
}
