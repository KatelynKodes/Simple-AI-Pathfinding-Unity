using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private bool _isWalkable;
    private Vector3 _worldPosition;
    private int _gridPosX, _gridPosY;
    private float _gScore;
    private float _hScore;
    private List<Node> _neighbors;


    /// <summary>
    /// The nodes position in relation to the grid
    /// </summary>
    public int GridPosX { get { return _gridPosX; } set { _gridPosX = value; } }

    /// <summary>
    /// The nodes position in relation to the grid
    /// </summary>
    public int GridPosY { get { return _gridPosY; } set { _gridPosY = value; } }

    /// <summary>
    /// Determines if the node is able to be walked through
    /// </summary>
    public bool IsWalkable { get { return _isWalkable; } set { _isWalkable = value; } }

    /// <summary>
    /// Distance from starting node
    /// </summary>
    public float GScore { get { return _gScore; } set { _gScore = value; } }

    /// <summary>
    /// Distance from target node
    /// </summary>
    public float HScore { get { return _hScore; } set { _hScore = value; } }

    /// <summary>
    /// Gscore + Hscore
    /// </summary>
    public float FScore { get { return _gScore + _hScore; } }

    /// <summary>
    /// The position of the node
    /// </summary>
    public Vector3 WorldPos { get { return _worldPosition; } }

    /// <summary>
    /// The neighbors of the node
    /// </summary>
    public List<Node> Neighbors { get { return _neighbors; } set { _neighbors = value; } }

    /// <summary>
    /// The base constructor for a node
    /// </summary>
    /// <param name="walkable"></param>
    /// <param name="worldPos"></param>
    public Node(bool walkable, Vector3 worldPos)
    {
        _isWalkable = walkable;
        _worldPosition = worldPos;
    }
}
