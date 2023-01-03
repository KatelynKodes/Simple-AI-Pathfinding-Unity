using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovementBehavior : MonoBehaviour
{
    private bool _startPositionSet = false;
    private Vector3 _velocity;
    private Vector3 _direction;
    private float _speed;
    private bool _isMoving = false;

    /// <summary>
    /// The direction the pawn needs to go in
    /// </summary>
    public Vector3 Direction { get { return _direction; } set { _direction = value; } }


    // Start is called before the first frame update
    void Start()
    {
        NavigationGrid navigationGrid = GameObject.Find("NavigationGrid").GetComponent<NavigationGrid>();
        while (!_startPositionSet)
        {
            InitializePawnPosition(navigationGrid);
        }
    }

    private void Update()
    {
        if (_isMoving)
        {
            //Calculate the velocity
            _velocity = _direction.normalized * _speed * Time.deltaTime;

            //Move the pawn
            transform.position += _velocity;
        }
    }

    private void InitializePawnPosition(NavigationGrid grid)
    {

        int xgrid = Random.Range(0, grid.GridSize.x-1);
        int ygrid = Random.Range(0, grid.GridSize.y-1);

        if (grid.Nodes[xgrid, ygrid].IsWalkable)
        {
            transform.position = grid.Nodes[xgrid, ygrid].WorldPos;
            _startPositionSet = true;
        }
    }
}
