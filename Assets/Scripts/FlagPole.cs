/*
This script, FlagPole, is a Unity script written in C# that manages the behavior of a flagpole in a game. Here’s a breakdown of its key parts:
•	Public Variables: The script has several public variables, including flag, poleBottom, castle, speed, nextWorld, nextStage, and victorySound. These are used to control the appearance, speed, sound effects, and level progression of the game.
•	Private Variables: The script has a private variable, audioSource, which is a reference to the AudioSource component of the game object.
•	Awake Method: This method is called when the script instance is being loaded. It gets the AudioSource component attached to the game object, or adds one if it doesn’t exist.
•	OnTriggerEnter2D Method: This method is called when another Collider2D enters the trigger attached to the game object this script is attached to. If the other collider is the player, it starts the coroutines to move the flag and complete the level.
•	LevelCompleteSequence Method: This is a coroutine that handles the sequence of events when the level is completed. It plays the victory sound, disables the PlayerMovement component, moves the player to the bottom of the pole, then to the right, then down, then to the castle, deactivates the player game object, waits for 2 seconds, and loads the next level.
•	MoveTo Method: This is a coroutine that moves a Transform to a destination. While the subject is not close enough to the destination, it moves the subject towards the destination. Once the subject is close enough, it sets the subject’s position to the destination.
*/


using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag; // The flag on the pole
    public Transform poleBottom; // The bottom of the pole
    public Transform castle; // The castle the player moves to after reaching the flag
    public float speed = 6f; // The speed at which the player and flag move
    public int nextWorld = 1; // The next world to load after completing the level
    public int nextStage = 1; // The next stage to load after completing the level
    public AudioClip victorySound; // The sound to play when the level is completed
    private AudioSource audioSource; // The AudioSource to play the victory sound

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the AudioSource component attached to the game object, or add one if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the other collider is the player
        if (other.CompareTag("Player"))
        {
            // Start the coroutines to move the flag and complete the level
            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(other.transform));
        }
    }

    // Coroutine to handle the sequence of events when the level is completed
    private IEnumerator LevelCompleteSequence(Transform player)
    {
        // Play the victory sound
        audioSource.PlayOneShot(victorySound, 0.5f);
        // Disable the PlayerMovement component
        player.GetComponent<PlayerMovement>().enabled = false;

        // Move the player to the bottom of the pole, then to the right, then down, then to the castle
        yield return MoveTo(player, poleBottom.position);
        yield return MoveTo(player, player.position + Vector3.right);
        yield return MoveTo(player, player.position + Vector3.right + Vector3.down);
        yield return MoveTo(player, castle.position);

        // Deactivate the player game object
        player.gameObject.SetActive(false);

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Load the next level
        GameManager.Instance.LoadLevel(nextWorld, nextStage);
    }

    // Coroutine to move a Transform to a destination
    private IEnumerator MoveTo(Transform subject, Vector3 destination)
    {
        // While the subject is not close enough to the destination
        while (Vector3.Distance(subject.position, destination) > 0.125f)
        {
            // Move the subject towards the destination
            subject.position = Vector3.MoveTowards(subject.position, destination, speed * Time.deltaTime);
            yield return null;
        }

        // Set the subject's position to the destination
        subject.position = destination;
    }
}