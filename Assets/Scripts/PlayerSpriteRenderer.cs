/*
This script, PlayerSpriteRenderer,  is a part of a Unity game project and is responsible for managing the visual representation of a player character in the game. It changes the player’s sprite based on their current state (idle, jumping, sliding, or running). Here are the key methods:
•	Awake(): This method is called when the script instance is being loaded. It initializes the movement and spriteRenderer variables by getting the PlayerMovement script and SpriteRenderer component attached to the parent game object.
•	LateUpdate(): This method is called every frame, after all Update methods have been completed. It checks the player’s state and updates the sprite accordingly. If the player is running, it enables the running animation. If the player is jumping or sliding, it changes the sprite to the jump or slide sprite. If the player is not running, it changes the sprite to the idle sprite.
•	OnEnable(): This method is called when the object becomes enabled and active. It enables the SpriteRenderer component, making the player visible.
•	OnDisable(): This method is called when the behaviour becomes disabled or inactive. It disables the SpriteRenderer component and the running animation, making the player invisible and stopping the running animation.
*/


using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteRenderer : MonoBehaviour
{
    // Reference to the PlayerMovement script
    private PlayerMovement movement;

    // Reference to the SpriteRenderer component
    public SpriteRenderer spriteRenderer { get; private set; }

    // Different sprites for different player states
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public AnimatedSprite run;

    private void Awake()
    {
        // Get the PlayerMovement script and SpriteRenderer component
        movement = GetComponentInParent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        // Enable the running animation if the player is running
        run.enabled = movement.running;

        // Set the sprite based on the player's state
        if (movement.jumping) {
            spriteRenderer.sprite = jump;
        } else if (movement.sliding) {
            spriteRenderer.sprite = slide;
        } else if (!movement.running) {
            spriteRenderer.sprite = idle;
        }
    }

    private void OnEnable()
    {
        // Enable the SpriteRenderer component
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        // Disable the SpriteRenderer component and running animation
        spriteRenderer.enabled = false;
        run.enabled = false;
    }
}