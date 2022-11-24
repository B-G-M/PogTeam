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
    private void Send_request_For_Move()
    {
        gameManager.Send_Request_For_Move_Up();
        //Vector3 dir = transform.up * Input.GetAxis("Vertical");
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speedDelete * Time.deltaTime);
    }

    public void Move(float position, float dir, float speed)
    {
        Vector3 pos = new Vector3(11, position);
        Vector3 direction = new Vector3(0, dir);
        transform.position = Vector3.MoveTowards(pos, pos + direction, speed * Time.deltaTime);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Vertical"))
        {
            Send_request_For_Move();
        }
    }
}
