/*
This script, EntityMovement, is a Unity script written in C# that manages the movement of a game entity. Here’s a breakdown of its key parts:
•	Public Variables: The script has two public variables, speed and direction, which control the speed and direction of the entity’s movement.
•	Private Variables: The script has two private variables, rigidbody and velocity. rigidbody is a reference to the Rigidbody2D component of the game object, and velocity is the velocity of the entity.
•	Awake Method: This method is called when the script instance is being loaded. It gets the Rigidbody2D component attached to the game object and disables the script.
•	OnBecameVisible and OnBecameInvisible Methods: These methods are called when the game object becomes visible or invisible. The script is enabled when the game object becomes visible and disabled when it becomes invisible, unless the game object is a spirit.
•	OnEnable and OnDisable Methods: These methods are called when the script becomes enabled or disabled. When the script is enabled, the Rigidbody2D is woken up (made non-kinematic). When the script is disabled, the velocity of the Rigidbody2D is set to zero and the Rigidbody2D is put to sleep (made kinematic).
•	FixedUpdate Method: This method is called every fixed framerate frame. It calculates the x component of the velocity based on the direction and speed, adds gravity to the y component of the velocity, moves the position of the Rigidbody2D, checks for obstacles in the direction of movement and below the entity, and flips the sprite based on the direction of movement.
*/


using UnityEngine;

// This script requires a Rigidbody2D component to be attached to the same game object
[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
    public float speed = 1f; // The speed of the entity
    public Vector2 direction = Vector2.left; // The direction of the entity's movement

    private new Rigidbody2D rigidbody; // Reference to the Rigidbody2D component
    private Vector2 velocity; // The velocity of the entity

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the Rigidbody2D component attached to the game object
        rigidbody = GetComponent<Rigidbody2D>();
        // Disable the script
        enabled = false;
    }

    // OnBecameVisible is called when the renderer became visible by any camera
    private void OnBecameVisible()
    {
        // Enable the script when the game object becomes visible
        enabled = true;
    }

    // OnBecameInvisible is called when the renderer is no longer visible by any camera
    private void OnBecameInvisible()
    {
        // If the game object is a spirit, don't disable the script
        if(gameObject.layer == LayerMask.NameToLayer("Spirit"))
            return;
            
        // Disable the script when the game object becomes invisible
        enabled = false;
    }

    // OnEnable is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Wake up the Rigidbody2D (make it non-kinematic)
        rigidbody.WakeUp();
    }

    // OnDisable is called when the behaviour becomes disabled or inactive
    private void OnDisable()
    {
        // Set the velocity of the Rigidbody2D to zero
        rigidbody.velocity = Vector2.zero;
        // Put the Rigidbody2D to sleep (make it kinematic)
        rigidbody.Sleep();
    }

    // FixedUpdate is called every fixed framerate frame
    private void FixedUpdate()
    {
        // Calculate the x component of the velocity based on the direction and speed
        velocity.x = direction.x * speed;
        // Add gravity to the y component of the velocity
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        // Move the position of the Rigidbody2D
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        // If there's an obstacle in the direction of movement, reverse the direction
        if (rigidbody.Raycast(direction)) {
            direction = -direction;
        }

        // If there's an obstacle below, stop falling
        if (rigidbody.Raycast(Vector2.down)) {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }

        // Flip the sprite based on the direction of movement
        if (direction.x > 0f) {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        } else if (direction.x < 0f) {
            transform.localEulerAngles = Vector3.zero;
        }
    }
}