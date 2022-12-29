using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehavior : MonoBehaviour
{
    public struct Edge
    {
        public NodeBehavior targetNode;
        public float cost;
    }

    /// <summary>
    /// Distance from the starting node
    /// </summary>
    private float _gScore;
    /// <summary>
    /// Distance from the target node
    /// </summary>
    private float _hScore;

    /// <summary>
    /// Gscore + Hscore
    /// </summary>
    private float _fScore;

    public List<Edge> edges = new List<Edge>();

    public void CalculateGScore(NodeBehavior startNode)
    {
        Vector2 startNodePosition = startNode.transform.position;
        //distance formula
        _gScore = Mathf.Sqrt(Mathf.Pow(startNodePosition.x - transform.position.x, 2)
                             + Mathf.Pow(startNodePosition.y - transform.position.y, 2));
    }

    public void CalculateHScore(NodeBehavior targetNode)
    {
        Vector2 TargetNodePosition = targetNode.transform.position;
        //distance formula
        _hScore = Mathf.Sqrt(Mathf.Pow(TargetNodePosition.x - transform.position.x, 2)
                             + Mathf.Pow(TargetNodePosition.y - transform.position.y, 2));
    }
}
