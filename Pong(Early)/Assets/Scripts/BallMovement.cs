using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Vector3 _currentPos;
    private Vector3 _direction;
    private float _speed;

    public void SetInfo(Vector3 currentPos, Vector3 direction, float speed)
    {
        _currentPos = currentPos;
        _direction = direction;
        _speed = speed;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(_currentPos, _direction, _speed);
    }

    private void Update()
    {
        Move();
    }
}
