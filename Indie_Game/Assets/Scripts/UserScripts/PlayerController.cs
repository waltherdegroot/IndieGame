using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //public variables
    public float movementSpeed = 5f;
    public float jumpHeight = 5f;

    public Text healthText;

    //private variables
    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool grounded = false;
    private bool doubleJump = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        var verticalMovement = Input.GetAxis("Vertical");

        healthText.text = string.Format("health: {0}", PlayerManager.instance.playerHealth);

        if (PlayerManager.instance.playerHealth <= 0)
            Application.LoadLevel(Application.loadedLevel);

        moveDirection = new Vector3(0f, 0f, 0f);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        }

        if (grounded  && Input.GetKey(KeyCode.LeftShift) || grounded && Input.GetKey(KeyCode.RightShift))
        {
            moveDirection = moveDirection * 1.5f;
        }

        if (Input.GetMouseButtonDown(1))
        {
            print("Zoom");
        }
    }

    void FixedUpdate()
    {
        Move();

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (2.5f - 1) * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded || Input.GetKeyDown(KeyCode.Space) && doubleJump)
        {
            if (rb.velocity.y < 0)
            {
                rb.AddForce(new Vector3(1f, jumpHeight * 2f, 1f), ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(new Vector3(1f, jumpHeight, 1f), ForceMode.Impulse);
            }

            if (!grounded && doubleJump)
            {
                doubleJump = false;
            }
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "grenade")
        {
            Physics.IgnoreCollision(hit.collider, GetComponent<Collider>());
            print("player hit");
        }

        if (hit.gameObject.tag == "Ground")
        {
            grounded = true;
            doubleJump = true;
        }
    }

    void OnCollisionExit(Collision hit)
    {
        if (hit.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    void Move()
    {
        Vector3 yFix = new Vector3(0, rb.velocity.y, 0);
        
        rb.velocity = moveDirection * movementSpeed * (Time.deltaTime / 2);
        rb.velocity += yFix;
    }
}
