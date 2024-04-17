/*
This script is for a BlockHit class in Unity. It’s used to handle the behavior of a block when it’s hit by the player. Here’s a breakdown of what each part does:
•	public GameObject item;: This is the item that will be instantiated when the block is hit.
•	public Sprite emptyBlock;: This is the sprite that will be displayed when the block is empty (i.e., when it has been hit the maximum number of times).
•	public int maxHits = -1;: This is the maximum number of times the block can be hit. If it’s -1, the block can be hit an unlimited number of times.
•	private bool animating;: This is a flag that indicates whether the block is currently animating.
•	public AudioClip blockSound;: This is the sound that will be played when the block is hit.
•	private AudioSource audioSource;: This is a reference to the AudioSource component attached to the same GameObject.
The methods in the script are:
•	Awake(): This method is called when the script instance is being loaded. It gets the AudioSource component and adds one if it doesn’t exist.
•	OnCollisionEnter2D(Collision2D collision): This method is called when the block collides with another object. If the block is not animating, has not been hit the maximum number of times, and the other object is the player, it calls the Hit() method.
•	Hit(): This method handles the behavior when the block is hit. It plays the block sound, enables the sprite renderer, decreases the hit count, changes the sprite to the empty block sprite if the block has been hit the maximum number of times, instantiates the item if it exists, and starts the animation coroutine.
•	Animate(): This coroutine animates the block by moving it up and then back down.
•	Move(Vector3 from, Vector3 to): This coroutine moves the block from one position to another over a duration of 0.125 seconds.
*/


using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item; // The item to be instantiated when the block is hit
    public Sprite emptyBlock; // The sprite to be displayed when the block is empty
    public int maxHits = -1; // The maximum number of hits the block can take

    private bool animating; // Flag to check if the block is currently animating

    public AudioClip blockSound; // Audio clip for the sound when the block is hit
    private AudioSource audioSource; // Reference to the AudioSource component

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get AudioSource component from the same GameObject or add one if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the block is not animating, has hits left, and the collision is with the player
        if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            // If the player is below the block
            if (collision.transform.DotTest(transform, Vector2.up))
            {
                // Hit the block
                Hit();
            }
        }
    }

    // Function to handle the block being hit
    private void Hit()
    {
        // Play the block hit sound
        audioSource.PlayOneShot(blockSound, 0.5f);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;

        // Decrease the number of hits left
        maxHits--;

        // If the block has no hits left
        if (maxHits == 0)
        {
            // Change the sprite to the empty block sprite
            spriteRenderer.sprite = emptyBlock;
        }

        // If there is an item to instantiate
        if (item != null)
        {
            // Instantiate the item at the block's position
            Instantiate(item, transform.position, Quaternion.identity);
        }

        // Start the block hit animation
        StartCoroutine(Animate());
    }

    // Coroutine to animate the block being hit
    private IEnumerator Animate()
    {
        // Set the animating flag to true
        animating = true;

        // The resting position of the block
        Vector3 restingPosition = transform.localPosition;
        // The position of the block when hit
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        // Move the block from resting position to hit position
        yield return Move(restingPosition, animatedPosition);
        // Move the block back from hit position to resting position
        yield return Move(animatedPosition, restingPosition);

        // Set the animating flag to false
        animating = false;
    }

    // Coroutine to move the block from one position to another
    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        // Elapsed time since the start of the movement
        float elapsed = 0f;
        // Duration of the movement
        float duration = 0.125f;

        // While the elapsed time is less than the duration
        while (elapsed < duration)
        {
            // Calculate the fraction of the total duration that has elapsed
            float t = elapsed / duration;

            // Interpolate the position of the block from the start position to the end position
            transform.localPosition = Vector3.Lerp(from, to, t);
            // Increase the elapsed time
            elapsed += Time.deltaTime;

            // Yield control to the next frame
            yield return null;
        }

        // Set the final position of the block
        transform.localPosition = to;
    } 
}