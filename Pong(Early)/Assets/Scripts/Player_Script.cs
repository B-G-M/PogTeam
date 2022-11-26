using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Script : MonoBehaviour
{
    [SerializeField] private float speedDelete = 5f;
    [SerializeField] private GameObject gM;
    [SerializeField] private GameManager_Script gameManager;
    private void Awake()
    {
        gameManager = gM.GetComponent<GameManager_Script>();
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
        Vector3 pos = new Vector3(11, position);
        Vector3 direction = new Vector3(0, target);
        transform.position = Vector3.MoveTowards(pos, direction, speed * Time.deltaTime);
    }

    IEnumerator ListenForKeys()
    {
        while (true)
        {
            if (Input.GetButton("Vertical"))
            {
                if (Input.GetAxis("Vertical") <= 0)
                {
                    gameManager.Send_Request_For_Move_Up();
                    yield return new WaitForSeconds(0.5f);
                }

                if (Input.GetAxis("Vertical") >= 0)
                {
                    gameManager.Send_Request_For_Move_Down();
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }
    
    
}
