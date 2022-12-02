using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] private BallMovement ballMovement;

    private void Awake()
    {
        ballMovement.enabled = false;
    }
    public void SetPosition(float x, float y)
    {
        ballMovement.enabled = false;
        transform.position = new Vector3(x, y, 0);
    }

    public void StartMove(Vector3 currentPos, Vector3 direction, float speed)
    {
        //Debug.Log($"Dir: ");
        ballMovement.SetInfo(currentPos, direction, speed);
        ballMovement.enabled = true;
    }
}
