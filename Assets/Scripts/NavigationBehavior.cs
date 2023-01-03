using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NavigationBehavior : MonoBehaviour
{
    private NavigationGrid _grid;
    private PawnMovementBehavior _pawnMovement;
    private Node _target;
    private bool _needsPath = true;
    private List<Node> _path;

    public Node Target { get { return _target; } set { _target = value; } }


    private void Start()
    {
        _grid = GameObject.Find("NavigationGrid").GetComponent<NavigationGrid>();
        _pawnMovement = GetComponent<PawnMovementBehavior>();
    }

    private void Update()
    {
        //If there is no target
        if(_target == null)
        {
            //Get a random target, primarily for testing purposes
            _target = GetRandomTarget();
        }

        //if it needs a path
        if (_needsPath)
        {
            //Update the current path
            UpdatePath();
        }

        Vector3 nextPosition = transform.position;
        //if the path count is less not less than or equal to 0
        if (!(_path.Count <= 0))
        {
            nextPosition = _path[0].WorldPos;
        }
    }

    /// <summary>
    /// Returns a random node from the grid to set as the target
    /// </summary>
    /// <returns></returns>
    private Node GetRandomTarget()
    {
        Node targetNode = null;
        

        while (targetNode == null)
        {
            int xPos = Random.Range(0, _grid.GridSize.x - 1);
            int yPos = Random.Range(0, _grid.GridSize.y - 1);

            if (_grid.Nodes[xPos, yPos].IsWalkable)
            {
                targetNode = _grid.Nodes[xPos, yPos];
            }
        }

        return targetNode;
    }

    private void UpdatePath()
    {
        UpdatePath(Target.WorldPos); 
    }

    /// <summary>
    /// Updates the path based on the world position
    /// </summary>
    /// <param name="worldPos"></param>
    private void UpdatePath(Vector3 worldPos)
    {
        Node startNode = _grid.GetNode(this.transform.position);
        Node endNode = _grid.GetNode(worldPos);

        _path = _grid.FindPath(startNode, endNode);
        _needsPath = false;
    }
}
