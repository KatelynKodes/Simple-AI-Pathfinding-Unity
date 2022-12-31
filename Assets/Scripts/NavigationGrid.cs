using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

        //Set each node's neighbors
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                setNodeNeighbors(_nodeGrid[i, j], gridX, gridY);
            }
        }
    }

    /// <summary>
    /// Finds a path using the A* method
    /// </summary>
    /// <param name="startNode">The node the path is starting from</param>
    /// <param name="TargetNode">The node the path is trying to get to</param>
    /// <returns>A List of nodes indicating a path</returns>
    public List<Node> FindPath(Node startNode, Node TargetNode)
    {
        //Defining the open and closed lists
        List<Node> OpenList = new List<Node>();
        List<Node> ClosedList = new List<Node>();
        //Add the starting node to the open list
        OpenList.Add(startNode);
        //Set the current node to be equal to the node at the start of the list
        Node currentNode = OpenList[0];

        while (OpenList.Count > 0)
        {
            //Replace the start node at the index of 0 with the current node
            OpenList[0] = currentNode;

            //Remove the current node from the open list
            OpenList.Remove(currentNode);
            //Add the current node to the closed list
            ClosedList.Add(currentNode);

            //if the current node is equal to the target node
            if(currentNode == TargetNode)
                return ConstructPath(startNode, TargetNode);

            //For every neighbor of the current node
            for (int i = 1; i < currentNode.Neighbors.Count; i++)
            {
                //If the current node's neighbor is walkable
                if (currentNode.Neighbors[i].IsWalkable)
                {
                    //and isn't included in the closed list
                    if (!ClosedList.Contains(currentNode.Neighbors[i]))
                    {
                        
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// A method that sets the given node's neighbors list
    /// </summary>
    /// <param name="Node"> The node being set</param>
    /// <param name="GridSizeX"> The X size of the grid </param>
    /// <param name="GridSizeY"> The Y size of the grid </param>
    private void setNodeNeighbors(Node Node, int GridSizeX, int GridSizeY)
    {
        List<Node> NeighborList = new List<Node>();

        //Search a 3 by 3 grid around the current node
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                //if i and j are not both equal to 0
                if (!(i == 0 && j == 0))
                {
                    //check the x
                    int checkX = Node.GridPosX + i;
                    //Check the y
                    int checkY = Node.GridPosY + j;

                    //If the x and y are both greater than or equal to 0 and less than the grid size
                    if ((checkX >= 0 && checkX < GridSizeX) && (checkY >= 0 && checkY < GridSizeY))
                        NeighborList.Add(_nodeGrid[checkX, checkY]);
                }
            }
        }

        Node.Neighbors = NeighborList;
    }

    private List<Node> ConstructPath(Node startNode, Node endNode)
    {
        return new List<Node>();
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
