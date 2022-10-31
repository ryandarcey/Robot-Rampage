using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraMovement : MonoBehaviour
{
    // Public values for mouse sensitivity
    public float xSensitivity = 300f;
    public float ySensitivity = 300f;

    // Value for rotation along the x-axis in response to mouse y-axis movement (mouseY)
    float xRotation = 0f;

    // Use player for rotation along the y-axis in response to mouse x-axis movement (mouseX)
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        // Set cursor to be invisible and locked to the center of the screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Handles the movement of the mouse on the X and Y axis with the set sensitivity. Not dependent on framerate
        float mouseX = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

        // Subtract from xRotation value with current mouseY value
        xRotation -= mouseY;
        // Limit rotation along x-axis to 90 and -90 degrees
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate the camera along the x-axis
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Rotate the camera along the y-axis
        player.Rotate(mouseX * Vector3.up);
    }
}
