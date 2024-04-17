/*
This script defines a Terminator class in Unity. Here's a summary of its functionality:
•	Start():
    o	Finds the player GameObject by its tag and gets its Transform component.
    o	Retrieves the EntityMovement script from the same GameObject.
•	Update():
    o	Moves the Terminator towards the player only in the x direction.
    o	Flips the sprite based on the direction of movement.
•	OnCollisionEnter2D(Collision2D collision):
    o	If the Terminator collides with the player, triggers the player's Hit() function.
•	OnTriggerEnter2D(Collider2D other):
    o	If the Terminator is hit by a spirit, changes the layer of the Terminator to the default layer.
•	OnTriggerExit2D(Collider2D other):
    o	If the spirit leaves the Terminator, changes the layer of the Terminator back to the Enemy layer.
•	OnBecameInvisible() & OnBecameVisible():
    o	If the Terminator becomes invisible, schedules its destruction after a delay. Cancels the destruction if it becomes visible again.
•	DestroyTerminator():
    o	Creates a new GameObject to play the Terminator destruction sound.
    o	Destroys the Terminator GameObject and the audio player after the sound finishes playing.
This script essentially controls the behavior of the Terminator character in the game, including movement towards the player, collision handling, and destruction upon becoming invisible. Additionally, it plays a sound effect when the Terminator is destroyed.
*/


using UnityEngine;

public class Terminator : MonoBehaviour
{
    // The sound that will be played when the Terminator is destroyed
    public AudioClip terminatorSound;
    // Reference to AudioSource component
    private AudioSource audioSource;
    // Reference to the player's transform
    public Transform player;
    // Reference to the EntityMovement script
    private EntityMovement entityMovement;

    private void Start()
    {
        // Find the player GameObject by its tag and get its Transform component
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the EntityMovement script from the same GameObject
        entityMovement = GetComponent<EntityMovement>();
    }

    private void Update()
    {
        // Move the Terminator towards the player only in the x direction
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        transform.position += new Vector3(direction * entityMovement.speed * Time.deltaTime, 0, 0);

        // Flip the sprite based on the direction
        if (direction > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Facing right
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Facing left
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the Terminator collides with the player, hit the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.Hit();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the Terminator is hit by a spirit, change the layer of the Terminator to the default layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Spirit"))
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If the spirit leaves the Terminator, change the layer of the Terminator back to the Enemy layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Spirit"))
        {
            gameObject.layer = LayerMask.NameToLayer("Terminator");
        }
    }

    private void OnBecameInvisible()
    {
        // If the Terminator becomes invisible, destroy it after a delay
        Invoke(nameof(DestroyTerminator), 4.0f);
    }

    private void OnBecameVisible()
    {
        // If the Terminator becomes visible again, cancel the destruction
        CancelInvoke(nameof(DestroyTerminator));
    }

    private void DestroyTerminator()
    {
        // Create a new GameObject to play the sound
        GameObject audioPlayer = new GameObject("AudioPlayer");
        AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();
        audioSource.clip = terminatorSound;
        audioSource.volume = 0.3f;
        audioSource.Play();

        // Destroy the audio player after the clip finishes
        Destroy(audioPlayer, terminatorSound.length);

        // Destroy the Terminator
        Destroy(gameObject);
    }
}