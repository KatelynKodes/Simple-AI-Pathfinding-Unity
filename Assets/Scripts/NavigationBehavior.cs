using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class NavigationBehavior : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    private NodeBehavior[] _allNodes;

    [SerializeField]
    private NodeBehavior _destinationNode;

    private void Start()
    {
        _allNodes = FindObjectsOfType<NodeBehavior>();
        int startingNode = Random.Range(0, _allNodes.Length);
        Vector3 startingPos = _allNodes[startingNode].transform.position;
        transform.position = startingPos;
        _spriteRenderer.enabled = true;
    }


    private void Update()
    {
        if (_destinationNode)
        {
            //FindPath
        }
    }

    /// <summary>
    /// Finds and returns a path of node objects for the pawn to navigate through
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    public List<NodeBehavior> FindPath(NodeBehavior startNode, NodeBehavior targetNode)
    {
        //Initialize the open list
        List<NodeBehavior> openList = new List<NodeBehavior>();
        //Initialize the closed list
        List<NodeBehavior> closedList = new List<NodeBehavior>();

        //Add the starting node to the open list
        openList.Add(startNode);

        //Create a currentNode variable and set it to the index of the start node in the open list
        NodeBehavior CurrentNode = openList[0];

        //while open list count is greater than 0
        while (openList.Count > 0)
        {
            //set the current node to be the first item in the open list
            CurrentNode = openList[0];

            //Remove the current node from the open list
            openList.Remove(CurrentNode);
            //Move the current node over to the closed list
            closedList.Add(CurrentNode);

            //if the current node is equal to the target node
            if(CurrentNode == targetNode)
                return ConstructPath(startNode, targetNode);

            //for every neighbor of the current node
            for (int i = 1; i < CurrentNode.edges.Count; i++)
            {
                //If the closed list does not contain the target node of the current
                //edge
                if (!closedList.Contains(CurrentNode.edges[i].targetNode))
                {
                    //Calculate the Gscore of the current edge
                    CurrentNode.edges[i].targetNode.CalculateGScore(CurrentNode);
                    //Calculate the Hscore of the current edge
                    CurrentNode.edges[i].targetNode.CalculateHScore(targetNode);
                }
                //Else
                else
                {
                    
                }
            }
        }

        return new List<NodeBehavior>();
    }

    /// <summary>
    /// Constructs and returns a path
    /// </summary>
    /// <param name="startNode">the starting node</param>
    /// <param name="endNode"> the ending node</param>
    /// <returns>a list of node behaviors</returns>
    private List<NodeBehavior> ConstructPath(NodeBehavior startNode, NodeBehavior endNode)
    {
        return new List<NodeBehavior>();
    }
}
