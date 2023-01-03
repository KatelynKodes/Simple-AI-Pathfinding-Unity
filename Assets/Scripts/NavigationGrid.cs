using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class NavigationGrid : MonoBehaviour
{
    [SerializeField]
    private Vector3 _gridPosition;
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
            foreach (NeighborNode neighbor in currentNode.Neighbors)
            {
                //If the current node's neighbor is walkable
                if (neighbor.target.IsWalkable)
                {
                    //and isn't included in the closed list
                    if (!ClosedList.Contains(neighbor.target))
                    {
                        //This is the cost of the movement, aka the Gscore
                        int movementCost = (int)currentNode.GScore + DistanceBetweenNodes(currentNode, neighbor.target);

                        //If the open list doesn't contain the neighbor or if the neighbor's gscore
                        //is less than the movement cost
                        if (!OpenList.Contains(neighbor.target) || neighbor.target.GScore < movementCost)
                        {
                            //Set the neighbprs gscore to the movement cost
                            neighbor.target.GScore = movementCost;
                            //Calculate the hscore
                            neighbor.target.HScore = DistanceBetweenNodes(neighbor.target, TargetNode);
                            //Set the neighbors previous node to be the current node
                            neighbor.target.PreviousNode = currentNode;

                            //if the openlist does not contain the neighbor
                            if (!OpenList.Contains(neighbor.target))
                            {
                                //Add it
                                OpenList.Add(neighbor.target);
                            }
                        }
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
                        Node.AddNeighborNode(_nodeGrid[checkX, checkY]);
                }
            }
        }
    }

    /// <summary>
    /// Gets the distance between two nodes
    /// </summary>
    /// <param name="startingNode">The starting node</param>
    /// <param name="endingNode">The ending node</param>
    /// <returns>integer</returns>
    private int DistanceBetweenNodes(Node startingNode, Node endingNode)
    {
        return (int)Math.Abs(Math.Sqrt(startingNode.GridPosX - endingNode.GridPosX) +
            Math.Sqrt(startingNode.GridPosY - endingNode.GridPosY));
    }

    /// <summary>
    /// Constructs the path from the end node to the starting node
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    /// <returns></returns>
    private List<Node> ConstructPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        //While the current node is not the start's previous
        while (currentNode != startNode.PreviousNode)
        {
            //Insert the current node at the first index
            path.Insert(0, currentNode);

            //set the current node to the current node's previous
            currentNode = currentNode.PreviousNode;
        }

        return path;
    }

    /// <summary>
    /// Gets a node according to it's grid position
    /// </summary>
    /// <param name="gridX">grid position x</param>
    /// <param name="gridY">grid position y</param>
    /// <returns>Node object</returns>
    public Node GetNode(int gridX, int gridY)
    {
        return _nodeGrid[gridX, gridY];
    }

    /// <summary>
    /// Grabs a node based on it's world position
    /// </summary>
    /// <param name="worldPos">the world position being checked</param>
    /// <returns>a Node</returns>
    public Node GetNode(Vector3 worldPos)
    {
        Node node = null;

        for (int i = 0; i < _gridSize.x; i++)
        {
            for (int j = 0; j < _gridSize.y; j++)
            {
                if (_nodeGrid[i, j].WorldPos == worldPos)
                {
                    node = _nodeGrid[i, j];
                }
            }
        }

        return node;
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
