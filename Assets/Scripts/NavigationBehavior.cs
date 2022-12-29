using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationBehavior : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    private GameObject[] _allNodes;

    private void Start()
    {
        _allNodes = GameObject.FindGameObjectsWithTag("Node");
        int startingNode = Random.Range(0, _allNodes.Length);
        Vector3 startingPos = _allNodes[startingNode].transform.position;
        transform.position = startingPos;
        _spriteRenderer.enabled = true;
    }
}
