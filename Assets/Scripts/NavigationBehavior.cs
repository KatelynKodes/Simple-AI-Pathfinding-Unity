using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class NavigationBehavior : MonoBehaviour
{

    private void Start()
    {
    }


    private void Update()
    {
    }

    /// <summary>
    /// Finds and returns a path of node objects for the pawn to navigate through
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        return new List<Node>();
    }

    /// <summary>
    /// Constructs and returns a path
    /// </summary>
    /// <param name="startNode">the starting node</param>
    /// <param name="endNode"> the ending node</param>
    /// <returns>a list of node behaviors</returns>
    private List<Node> ConstructPath(Node startNode, Node endNode)
    {
        return new List<Node>();
    }
}
