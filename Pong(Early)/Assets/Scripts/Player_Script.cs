using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Script : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private void Move()
    {
        Vector3 dir = transform.up * Input.GetAxis("Vertical");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Vertical"))
        {
            Move();
        }
    }
}
