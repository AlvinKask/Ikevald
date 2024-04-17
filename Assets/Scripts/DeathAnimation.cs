/*
This script, DeathAnimation manages the death animation of a game object. Here’s a breakdown of its key parts:
•	Public Variables: The script has two public variables, spriteRenderer and deadSprite. spriteRenderer is a reference to the SpriteRenderer component of the game object, and deadSprite is the sprite that will be displayed when the game object is “dead” or deactivated.
•	Reset Method: This method is called when the user hits the Reset button in the Inspector’s context menu or when adding the component the first time. It gets the SpriteRenderer component attached to the game object.
•	OnEnable Method: This method is called when the object becomes enabled and active. It updates the sprite, disables the physics, and starts the death animation.
•	OnDisable Method: This method is called when the behaviour becomes disabled or inactive. It stops all coroutines when the game object is disabled.
•	UpdateSprite Method: This method updates the sprite of the game object. It enables the sprite renderer, sets its sorting order, and if there is a dead sprite, sets it as the sprite of the sprite renderer.
•	DisablePhysics Method: This method disables the physics of the game object. It gets all the Collider2D components attached to the game object and disables them, makes the Rigidbody2D component kinematic, and gets the PlayerMovement and EntityMovement components and disables them.
•	Animate Method: This is a coroutine for animating the death of the game object. It initializes the elapsed time, duration of the animation, jump velocity, and gravity. While the elapsed time is less than the duration, it moves the game object according to the velocity and increases the elapsed time. It yields control to the next frame
*/


using System.Collections;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // The sprite renderer of the game object
    public Sprite deadSprite; // The sprite to be displayed when the game object is dead

    // Reset is called when the user hits the Reset button in the Inspector's context menu or when adding the component the first time
    private void Reset()
    {
        // Get the SpriteRenderer component attached to the game object
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // OnEnable is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Update the sprite, disable the physics, and start the death animation
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animate());
    }

    // OnDisable is called when the behaviour becomes disabled or inactive
    private void OnDisable()
    {
        // Stop all coroutines when the game object is disabled
        StopAllCoroutines();
    }

    // Update the sprite of the game object
    private void UpdateSprite()
    {
        // Enable the sprite renderer and set its sorting order
        spriteRenderer.enabled = true;
        spriteRenderer.sortingOrder = 10;

        // If there is a dead sprite, set it as the sprite of the sprite renderer
        if (deadSprite != null) {
            spriteRenderer.sprite = deadSprite;
        }
    }

    // Disable the physics of the game object
    private void DisablePhysics()
    {
        // Get all the Collider2D components attached to the game object and disable them
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders) {
            collider.enabled = false;
        }

        // Make the Rigidbody2D component kinematic
        GetComponent<Rigidbody2D>().isKinematic = true;

        // Get the PlayerMovement and EntityMovement components and disable them
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        EntityMovement entityMovement = GetComponent<EntityMovement>();
        if (playerMovement != null) {
            playerMovement.enabled = false;
        }
        if (entityMovement != null) {
            entityMovement.enabled = false;
        }
    }

    // Coroutine for animating the death of the game object
    private IEnumerator Animate()
    {
        // Initialize the elapsed time, duration of the animation, jump velocity, and gravity
        float elapsed = 0f;
        float duration = 4f;
        float jumpVelocity = 10f;
        float gravity = -36f;

        // The velocity of the game object
        Vector3 velocity = Vector3.up * jumpVelocity;

        // While the elapsed time is less than the duration
        while (elapsed < duration)
        {
            // Move the game object according to the velocity and increase the elapsed time
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime;

            // Yield control to the next frame
            yield return null;
        }
    }
}