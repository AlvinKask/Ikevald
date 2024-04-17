/*
This script, Pipe, is a part of the Unity game engine and it’s used to control the behavior of a game object, referred to as “Pipe”. The script allows a player to enter the pipe and be transported to another connected pipe, similar to the mechanic seen in games like Super Mario.
Here are the key parts of the methods:
•	public Transform connection;: This line declares a public Transform variable connection that represents the other end of the pipe.
•	public KeyCode enterKeyCode = KeyCode.S;: This line sets the key (default is ‘S’) that the player needs to press to enter the pipe.
•	public Vector3 enterDirection = Vector3.down; and public Vector3 exitDirection = Vector3.zero;: These lines set the directions for entering and exiting the pipe.
•	public AudioClip enterSound;: This line declares a public AudioClip variable enterSound that stores the sound to play when the player enters the pipe.
•	private AudioSource audioSource;: This line declares a private AudioSource variable audioSource that will be used to play the enterSound.
•	private void Awake() { ... }: The Awake method is a Unity callback that is called when the script instance is being loaded. Here, it’s used to get a reference to the AudioSource component on the game object, or add one if it doesn’t exist.
•	private void OnTriggerStay2D(Collider2D other) { ... }: The OnTriggerStay2D method is a Unity callback that is called once per frame for every Collider2D other that is touching the trigger. In this method, the script checks if the player is in the trigger area and the enter key is pressed, then starts the Enter coroutine.
•	private IEnumerator Enter(Transform player) { ... }: The Enter coroutine is used to handle the player entering the pipe. It plays the enter sound, disables the player’s movement, moves the player to the entered position and scale, waits for a short duration, checks if the player is underground, moves the player to the exit position if there’s an exit direction, and finally enables the player’s movement.
•	private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale) { ... }: The Move coroutine is used to smoothly move the player from their current position and scale to the end position and scale over a duration of 1 second.
*/


using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    // Connection to another pipe
    public Transform connection;

    // Key to enter the pipe
    public KeyCode enterKeyCode = KeyCode.S;

    // Direction to enter and exit the pipe
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;

    // Sound to play when entering the pipe
    public AudioClip enterSound;

    // AudioSource to play the sound
    private AudioSource audioSource;

    private void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If there's no AudioSource component, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // If the player is in the trigger area and the enter key is pressed, start the Enter coroutine
        if (connection != null && other.CompareTag("Player") && Input.GetKey(enterKeyCode))
        {
            StartCoroutine(Enter(other.transform));
        }
    }

    private IEnumerator Enter(Transform player)
    {
        // Play the enter sound
        audioSource.PlayOneShot(enterSound, 0.5f);

        // Disable the player's movement
        player.GetComponent<PlayerMovement>().enabled = false;

        // Calculate the entered position and scale
        Vector3 enteredPosition = transform.position + enterDirection;
        Vector3 enteredScale = Vector3.one * 0.25f;

        // Move the player to the entered position and scale
        yield return Move(player, enteredPosition, enteredScale);

        // Wait for a short duration
        yield return new WaitForSeconds(0.75f);

        // Check if the player is underground
        var sideScrolling = Camera.main.GetComponent<HorizontalCamera>();
        sideScrolling.SetUnderground(connection.position.y < sideScrolling.undergroundThreshold);

        // If there's an exit direction, move the player to the exit position
        if (exitDirection != Vector3.zero)
        {
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection, Vector3.one);
        }
        else
        {
            // Otherwise, just set the player's position and scale
            player.position = connection.position;
            player.localScale = Vector3.one;
        }

        // Enable the player's movement
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
    {
        // Initialize elapsed time and duration
        float elapsed = 0f;
        float duration = 1f;

        // Record the start position and scale
        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;

        // Lerp the player's position and scale to the end position and scale over the duration
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            player.position = Vector3.Lerp(startPosition, endPosition, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;

            yield return null;
        }
        
        // Set the player's position and scale to the end position and scale
        player.position = endPosition;
        player.localScale = endScale;
    }
}