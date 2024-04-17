/*
This script is for a BlockItem class in Unity. It’s used to animate a block item in the game. Here’s a breakdown of what each part does:
•	private void Start(): This method is called at the start of the script. It starts the animation coroutine.
•	private IEnumerator Animate(): This coroutine is responsible for animating the block item. It first gets the necessary components attached to the GameObject (Rigidbody2D, CircleCollider2D, BoxCollider2D, SpriteRenderer) and disables the physics and rendering of the block item. After waiting for a quarter of a second, it enables the rendering of the block item and starts a movement animation that lasts for half a second. The block item moves upwards during this animation. After the animation, it enables the physics of the block item.
*/


using System.Collections;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    // This method is called at the start of the script
    private void Start()
    {
        // Starts the animation coroutine
        StartCoroutine(Animate());
    }

    // Coroutine for animating the block item
    private IEnumerator Animate()
    {
        // Get the components attached to the game object
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        CircleCollider2D physicsCollider = GetComponent<CircleCollider2D>();
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Disable the physics and rendering of the block item
        rigidbody.isKinematic = true;
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
        spriteRenderer.enabled = false;

        // Wait for a quarter of a second
        yield return new WaitForSeconds(0.25f);

        // Enable the rendering of the block item
        spriteRenderer.enabled = true;

        // Initialize the elapsed time and the duration of the animation
        float elapsed = 0f;
        float duration = 0.5f;

        // The start and end positions of the block item
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + Vector3.up;

        // While the elapsed time is less than the duration
        while (elapsed < duration)
        {
            // Calculate the fraction of the total duration that has elapsed
            float t = elapsed / duration;

            // Interpolate the position of the block item from the start position to the end position
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            // Increase the elapsed time
            elapsed += Time.deltaTime;

            // Yield control to the next frame
            yield return null;
        }

        // Enable the physics of the block item
        rigidbody.isKinematic = false;
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;
    }
}