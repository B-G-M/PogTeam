using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    private Vector3 _currentPos;
    private Vector3 _direction;
    private float _speed;
    private GameManager_Script _gameManagerScript;

    private void Awake()
    {
        _gameManagerScript = gameManager.GetComponent<GameManager_Script>();
    }
    public void SetInfo(Vector3 currentPos, Vector3 direction, float speed)
    {
        _currentPos = currentPos;
        _direction = direction;
        _speed = speed;
        //transform.position = _currentPos;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _direction, _speed*Time.deltaTime);
    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Barrier"))
        {
            _gameManagerScript.BallHasReachedCollider();
            //Debug.Log($"Ball has reaches collider. Ball pos {transform.position.x}:{transform.position.y}");
            //Debug.Log("Send ballDir");
        }
    }
}
