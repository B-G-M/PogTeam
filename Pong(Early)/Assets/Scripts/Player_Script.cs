using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Script : MonoBehaviour
{
    [SerializeField] private float speedDelete = 5f;
    [SerializeField] private GameObject gM;
    [SerializeField] private GameManager_Script gameManager;
    private float _x ;
    private void Awake()
    {
        gameManager = gM.GetComponent<GameManager_Script>();
        _x = gameObject.transform.position.x;
    }

    public void SetX_Pos(float pos)
    {
        //_x = pos;
    }

    public void StartListenForKeys()
    {
        StartCoroutine(ListenForKeys());
    }

    public void EndListenForKey()
    {
        StopCoroutine(ListenForKeys());
    }
    public void Move(float position, float target, float speed)
    {
        Vector3 currentPos = new Vector3(_x,position);
        Vector3 direction = new Vector3(0, target);
        transform.position = Vector3.MoveTowards(currentPos, direction, speed * Time.deltaTime);
    }

    IEnumerator ListenForKeys()
    {
        while (true)
        {
            if (Input.GetButton("Vertical"))
            {
                if (Input.GetAxis("Vertical") <= 0)
                {
                    gameManager.Send_Request_For_Move_Down();
                }

                if (Input.GetAxis("Vertical") >= 0)
                {
                    gameManager.Send_Request_For_Move_Up();
                }
            }
            yield return new WaitForSeconds(0.07f);
        }
    }
    
    
}
