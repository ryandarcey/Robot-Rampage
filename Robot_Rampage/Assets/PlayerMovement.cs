using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // CharacterController attatched to player
    public CharacterController controller;
    // GameObject used for checking ground below player
    public Transform groundCheck;
    // Layer for checking ground collisions
    public LayerMask groundLayer;

    // Public attributes for player movement
    public float walkSpeed = 15f;
    public float gravity = -5f;
    public float jumpHeight = 3f;

    // Vertical velocity vector for gravity and movement
    Vector3 verticalVelocity;
    // Check whether or not a player is on the ground
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is touching the ground
        // isGrounded = Physics.Raycast(transform.position, Vector3.down, (controller.height * 0.5f) - 0.1f, groundLayer);
        isGrounded = Physics.CheckSphere(groundCheck.position, .2f, groundLayer);

        // If the player is supposed to be grounded, set the velocity to a low value to force the player to the ground
        if (verticalVelocity.y < 0 && isGrounded)
        {
            verticalVelocity.y = -.1f;
        }

        // Get forward/backward movement (zMovement) and left/right movement (xMovement)
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        // Set the vector for movement based on xMovement and zMovement
        Vector3 movement = (xMovement * transform.right) + (zMovement * transform.forward);

        // Move the player independent of the framerate
        controller.Move(movement * walkSpeed * Time.deltaTime);

        // If the player is grounded and presses the Jump button, increase velocity by set jumpHeight
        if (Input.GetButton("Jump") && isGrounded)
        {
            verticalVelocity.y = jumpHeight;
        }

        // Update player's position based on gravity, independent of framerate
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        /*        updateGravity();
                updateMove();*/
    }

/*    private void updateGravity()
    {
        // If the player is supposed to be grounded, set the velocity to a low value to force the player to the ground
        if(velocity.y < 0 && isGrounded)
        {
            velocity.y = -.1f;
        }

        // If the player is grounded and presses the Jump button, increase velocity by set jumpHeight
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpHeight;
        }

        // Update player's position based on gravity, independent of framerate
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void updateMove()
    {
        // Get forward/backward movement (zMovement) and left/right movement (xMovement)
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        // Set the vector for movement based on xMovement and zMovement
        Vector3 movement = (xMovement * transform.right) + (zMovement * transform.forward);

        // Move the player independent of the framerate
        controller.Move(movement * walkSpeed * Time.deltaTime);
    }*/
}
