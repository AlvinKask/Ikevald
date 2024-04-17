/*
This script, Demon, is a Unity script written in C# that manages the behavior of a “demon” character in a game. Here’s a breakdown of its key parts:
•	Public Variables: The script has several public variables, including spiritSprite, spiritSpeed, demonhitsound, and spirithitsound. These are used to control the appearance, speed, and sound effects of the demon character.
•	Private Variables: The script has two private boolean variables, spirited and pushed, which track the state of the demon character.
•	Awake Method: This method is called when the script instance is being loaded. It gets the AudioSource component attached to the game object. If there is no AudioSource component, it adds one.
•	OnCollisionEnter2D Method: This method is called when the demon character collides with another game object. If the demon is not spirited and the other game object is the player, it checks the direction of the collision. If the player is above the demon, it turns the demon into a spirit. Otherwise, it calls the Hit method on the player.
•	OnTriggerEnter2D Method: This method is called when the demon character enters a trigger. If the demon is spirited and the other game object is the player, it checks if the demon has been pushed. If not, it pushes the spirit in the opposite direction of the player. Otherwise, it calls the Hit method on the player. If the demon is not spirited and the other game object is a spirit, it calls the Hit method on the demon. If the demon is spirited and the other game object is untagged, it plays the spirit hit sound.
•	TurnToSpirit Method: This method turns the demon into a spirit. It sets the spirited variable to true, plays the demon hit sound, disables the EntityMovement and AnimatedSprite components, and changes the sprite to the spirit sprite.
•	PushSpirit Method: This method pushes the spirit in a specified direction. It sets the pushed variable to true, plays the spirit hit sound, makes the Rigidbody2D component non-kinematic, sets the direction and speed of the EntityMovement component, enables the EntityMovement component, and changes the layer of the game object to “Spirit”.
•	Hit Method: This method is called when the demon is hit. It disables the AnimatedSprite component, enables the DeathAnimation component, and destroys the game object after 10 seconds.
•	OnBecameInvisible and OnBecameVisible Methods: These methods are called when the game object becomes invisible or visible. If the demon has been pushed and becomes invisible, it invokes the DestroyDemon method after 10 seconds. If the demon becomes visible again, it cancels the invocation of the DestroyDemon method.
•	DestroyDemon Method: This method destroys the game object.
*/


using UnityEngine;

public class Demon : MonoBehaviour
{
    public Sprite spiritSprite; // The sprite that represents the spirit form of the demon
    public float spiritSpeed = 12f; // The speed at which the spirit moves

    private bool spirited; // Flag to check if the demon is in spirit form
    private bool pushed; // Flag to check if the spirit has been pushed

    public AudioClip demonhitsound; // Audio clip for demon hit sound
    public AudioClip spirithitsound; // Audio clip for spirit hit sound
    private AudioSource audioSource; // Reference to AudioSource component

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the AudioSource component attached to the game object
        audioSource = GetComponent<AudioSource>();
        // If there's no AudioSource component attached, add one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the demon is not in spirit form and it collides with the player
        if (!spirited && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            // If the player is above the demon
            if (collision.transform.DotTest(transform, Vector2.down))
            {
                // Turn the demon into a spirit
                TurnToSpirit();
            }
            else
            {
                // The player is hit by the demon
                player.Hit();
            }
        }
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the demon is in spirit form and it collides with the player
        if (spirited && other.CompareTag("Player"))
        {
            // If the spirit has not been pushed
            if (!pushed)
            {
                // Calculate the direction to push the spirit away from the player
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                // Push the spirit in the calculated direction
                PushSpirit(direction);
            }
            else
            {
                // The player is hit by the spirit
                Player player = other.GetComponent<Player>();
                player.Hit();
            }
        }
        // If the demon is not in spirit form and it collides with a spirit
        else if (!spirited && other.gameObject.layer == LayerMask.NameToLayer("Spirit"))
        {
            // The demon is hit
            Hit();
        }
        // If the demon is in spirit form and it collides with an untagged object
        else if (spirited && other.CompareTag("Untagged")) 
        {
            // Play the spirit hit sound
            audioSource.PlayOneShot(spirithitsound, 0.1f);
        }
    }

    // Method to turn the demon into a spirit
    private void TurnToSpirit()
    {
        spirited = true;
        // Play the demon hit sound
        audioSource.PlayOneShot(demonhitsound, 0.5f);
        // Disable the EntityMovement and AnimatedSprite components
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        // Change the sprite to the spirit sprite
        GetComponent<SpriteRenderer>().sprite = spiritSprite;
    }

    // Method to push the spirit in a specified direction
    private void PushSpirit(Vector2 direction)
    {
        pushed = true;

        // Play the spirit hit sound
        audioSource.PlayOneShot(spirithitsound, 0.5f);

        // Make the Rigidbody2D non-kinematic (affected by physics)
        GetComponent<Rigidbody2D>().isKinematic = false;

        // Set the direction and speed of the EntityMovement component and enable it
        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = spiritSpeed;
        movement.enabled = true;

        // Change the layer of the game object to "Spirit"
        gameObject.layer = LayerMask.NameToLayer("Spirit");
    }

    // Method to handle the demon being hit
    private void Hit()
    {
        // Disable the AnimatedSprite component and enable the DeathAnimation component
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        // Destroy the game object after 10 seconds
        Destroy(gameObject, 10f);
    }

    // Method to handle the game object becoming invisible
    private void OnBecameInvisible()
    {
        // If the spirit has been pushed
        if (pushed)
            // Invoke the DestroyDemon method after 10 seconds
            Invoke(nameof(DestroyDemon), 10.0f);
    }

    // Method to handle the game object becoming visible
    private void OnBecameVisible()
    {
        // If the spirit has been pushed
        if(pushed)
            // Cancel the invocation of the DestroyDemon method
            CancelInvoke(nameof(DestroyDemon));
    }

    // Method to destroy the demon
    private void DestroyDemon()
    {
        Destroy(gameObject);
    }
}