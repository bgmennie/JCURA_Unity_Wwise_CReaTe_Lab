using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Look : MonoBehaviour
{
    [Header("OSC Headtracker")] // Code used by https://www.youtube.com/watch?v=8lWxxFKZTiQ&ab_channel=IssacThomas

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;

    //public OscIn oscIn; // 

    // Start is called before the first frame update
    void Start()
    {
        // Hide and lock cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
