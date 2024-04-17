/*
This script defines a Shaman class in Unity. Here's a breakdown of its key methods:
1.	Start():
•	Retrieves or adds an AudioSource component to the GameObject.
2.	OnCollisionEnter2D(Collision2D collision):
•	Checks collision with the player.
•	If hit from above, flattens the Shaman; otherwise, the player is hit.
3.	OnTriggerEnter2D(Collider2D other):
•	Handles collision with a spirit object.
4.	Flatten():
•	Plays a sound when the Shaman is flattened.
•	Disables collider, movement, and animation.
•	Changes the sprite to the dead sprite.
•	Destroys the Shaman after a delay.
5.	Hit():
•	Disables the Shaman's animation.
•	Enables the death animation.
•	Destroys the Shaman after a delay.
Overall, the script manages collision events between the Shaman and the player or spirit objects, triggering appropriate actions such as flattening or hitting the Shaman, along with sound and sprite changes.

*/


using UnityEngine;

public class Shaman : MonoBehaviour
{
    // Sprite to use when the shaman is dead
    public Sprite deadSprite;

    // Sound to play when the shaman is hit
    public AudioClip shamanSound;
    private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        // Get the AudioSource component from the same GameObject or add one if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the shaman collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            // If the player hits the shaman from above, flatten the shaman
            if (collision.transform.DotTest(transform, Vector2.down))
            {
                Flatten();
            }
            // Otherwise, the player is hit
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the shaman is hit by a spirit, hit the shaman
        if (other.gameObject.layer == LayerMask.NameToLayer("Spirit"))
        {
            Hit();
        }
    }

    private void Flatten()
    {
        // Play the shaman sound
        audioSource.PlayOneShot(shamanSound, 0.5f);

        // Disable the shaman's collider, movement, and animation
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;

        // Change the shaman's sprite to the dead sprite
        GetComponent<SpriteRenderer>().sprite = deadSprite;

        // Destroy the shaman after a delay
        Destroy(gameObject, 0.5f);
    }

    private void Hit()
    {
        // Disable the shaman's animation
        GetComponent<AnimatedSprite>().enabled = false;

        // Enable the shaman's death animation
        GetComponent<DeathAnimation>().enabled = true;

        // Destroy the shaman after a delay
        Destroy(gameObject, 3f);
    }
}