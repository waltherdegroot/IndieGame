using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Public variables
    public float minX = -60f;
    public float maxX = 60f;
    public float minY = -360f;
    public float maxY = 360f;

    public float sensitivityX = 15f;
    public float sensitivityY = 15f;

    public Camera cam;

    // Private Variables
    float rotationY = 0f;
    float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * sensitivityY;
        rotationX += Input.GetAxis("Mouse Y") * sensitivityX;

        rotationX = Mathf.Clamp(rotationX, minX, maxX);

        transform.localEulerAngles = new Vector3(0, rotationY, 0);
        cam.transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);

        Vector3 desiredPosition = transform.position;
        cam.transform.position = desiredPosition;

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
