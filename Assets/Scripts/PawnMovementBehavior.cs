using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovementBehavior : MonoBehaviour
{
    private bool _positionSet = false;

    // Start is called before the first frame update
    void Start()
    {
        NavigationGrid navigationGrid = GameObject.Find("NavigationGrid").GetComponent<NavigationGrid>();
        while (!_positionSet)
        {
            InitializePawnPosition(navigationGrid);
        }
    }

    private void InitializePawnPosition(NavigationGrid grid)
    {

        int xgrid = Random.Range(0, grid.GridSize.x-1);
        int ygrid = Random.Range(0, grid.GridSize.y-1);

        if (grid.Nodes[xgrid, ygrid].IsWalkable)
        {
            transform.position = grid.Nodes[xgrid, ygrid].WorldPos;
            _positionSet = true;
        }
    }
}
