using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") / 7;
        float moveVertical = Input.GetAxis("Vertical") / 7;

        Vector3 newPosition = new Vector3(rb.position.x + moveHorizontal, rb.position.y + 0.0f, rb.position.z + moveVertical);

        rb.position = newPosition;
    }
}
