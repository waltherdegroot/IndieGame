using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public variables
    public float movementSpeed = 5f;

    //private variables
    Rigidbody rb;
    Vector3 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        var verticalMovement = Input.GetAxis("Vertical");

        moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;

        if (Input.GetMouseButtonDown(0))
        {
            print("Shoot");
        }

        if (Input.GetMouseButtonDown(1))
        {
            print("Zoom");
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 yFix = new Vector3(0, rb.velocity.y, 0);

        rb.velocity = moveDirection * movementSpeed * (Time.deltaTime / 2);
        rb.velocity += yFix;
    }

    void Shoot()
    {
        
    }
}
