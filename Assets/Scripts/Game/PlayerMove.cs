using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
     // Start is called before the first frame update

    public Vector2 speed = new Vector2(50,50);
    public static bool canMove = true;

    public float xBarrier = 8.5f;
    public float yBarrier = 4f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);

        movement *= Time.deltaTime;

        if(canMove){
            transform.Translate(movement);
        }

        if(transform.position.x > xBarrier){
            transform.position = new Vector3(xBarrier, transform.position.y, transform.position.z);
        }
        if(transform.position.x < -1*xBarrier){
            transform.position = new Vector3(-1*xBarrier, transform.position.y, transform.position.z);
        }

        if(transform.position.y > yBarrier){
            transform.position = new Vector3(transform.position.x, yBarrier, transform.position.z);
        }
        if(transform.position.y < -1*yBarrier){
            transform.position = new Vector3(transform.position.x, -1*yBarrier, transform.position.z);
        }
        
    }
}
