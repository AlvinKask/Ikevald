/*
This script, PlayerMovement,  is a part of the Unity game engine and it’s used to control the movement of a game object, referred to as “Player”. The script allows the player to move horizontally, jump, and interact with enemies and power-ups.
Here are the key parts of the methods:
•	public float moveSpeed = 8f;, public float maxJumpHeight = 5f;, public float maxJumpTime = 1f;: These lines set the player’s movement speed, maximum jump height, and maximum jump time.
•	public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);, public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);: These lines calculate the jump force and gravity based on the maximum jump height and maximum jump time.
•	public bool grounded { get; private set; }, public bool jumping { get; private set; }, public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;, public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);, public bool falling => velocity.y < 0f && !grounded;: These lines declare public properties that indicate the player’s current state (grounded, jumping, running, sliding, falling).
•	public AudioClip runningSound;, public AudioClip jumpingSound;: These lines declare public AudioClip variables that store the sounds to play when the player is running or jumping.
•	private void Awake() { ... }: The Awake method is a Unity callback that is called when the script instance is being loaded. Here, it’s used to get references to the player’s Camera, Rigidbody2D, and Collider2D components, and get or add an AudioSource component on the game object.
•	private void OnEnable() { ... }, private void OnDisable() { ... }: The OnEnable and OnDisable methods are Unity callbacks that are called when the script is enabled or disabled. Here, they’re used to enable or disable the player’s Rigidbody2D and Collider2D components, reset the player’s velocity, and set the jumping state to false.
•	private void Update() { ... }: The Update method is a Unity callback that is called once per frame. Here, it’s used to handle the player’s horizontal movement, check if the player is grounded, handle the player’s grounded movement, and apply gravity.
•	private void FixedUpdate() { ... }: The FixedUpdate method is a Unity callback that is called at a fixed interval. Here, it’s used to update the player’s position based on the velocity and clamp the player’s x position within the screen bounds.
•	private void HorizontalMovement() { ... }, private void GroundedMovement() { ... }, private void ApplyGravity() { ... }: These methods are used to handle the player’s horizontal movement, grounded movement, and gravity.
•	private void OnCollisionEnter2D(Collision2D collision) { ... }: The OnCollisionEnter2D method is a Unity callback that is called when the player’s Collider2D starts touching another Collider2D. Here, it’s used to handle the player’s interaction with enemies and power-ups.
*/


using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // References to the camera, rigidbody, and collider
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    // Player's velocity and input axis
    private Vector2 velocity;
    private float inputAxis;

    // Player's movement parameters
    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    // Player's state properties
    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);
    public bool falling => velocity.y < 0f && !grounded;

    // Sounds for different player actions
    public AudioClip runningSound;
    public AudioClip jumpingSound;
    private AudioSource audioSource;

    private void Awake()
    {
        // Get the required components
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        // Enable the player's physics and reset their state
        rigidbody.isKinematic = false;
        collider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        // Disable the player's physics and reset their state
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void Update()
    {
        // Handle the player's horizontal movement and check if they're grounded
        HorizontalMovement();
        grounded = rigidbody.Raycast(Vector2.down);
        if (grounded) {
            GroundedMovement();
        }
        ApplyGravity();
    }

    private void FixedUpdate()
    {
        // Move the player based on their velocity and clamp their position within the screen bounds
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;
        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);
        rigidbody.MovePosition(position);
    }

    private void HorizontalMovement()
    {
        // Accelerate or decelerate the player and check if they're running into a wall
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);
        if (grounded && Mathf.Abs(inputAxis) > 0 && !sliding && !jumping && !audioSource.isPlaying)
        {
            // Play the running sound
            audioSource.PlayOneShot(runningSound, 0.5f);
        }
        if (rigidbody.Raycast(Vector2.right * velocity.x)) {
            velocity.x = 0f;
        }
        // Flip the player's sprite to face the direction of movement
        if (velocity.x > 0f) {
            transform.eulerAngles = Vector3.zero;
        } else if (velocity.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        // Prevent gravity from infinitely building up and check if the player is jumping
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // Perform a jump if the jump button is pressed
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;

            // Play the jumping sound
            audioSource.PlayOneShot(jumpingSound, 0.5f);
        }
    }

    private void ApplyGravity()
    {
        // Check if the player is falling
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        // Apply gravity and terminal velocity
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with an enemy
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // If the player is above the enemy, perform a jump
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        // If the player collided with something other than a power-up, perform a dot test
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(collision.transform, Vector2.up)) {
                velocity.y = 0f;
            }
        }
    }
}