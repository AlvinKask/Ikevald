/*
This script is for an AudioPlayer class in Unity. It’s used to play an audio clip when the game starts and ensures that the audio continues to play even when the scene changes. Here’s a breakdown of what each part does:
•	private AudioSource audioSource;: This is a reference to the AudioSource component attached to the same GameObject.
The method in the script is:
•	Awake(): This method is called when the script instance is being loaded. It does several things:
    o	DontDestroyOnLoad(gameObject);: This line ensures that the GameObject with this script persists across scene changes. This is useful for background music or sound effects that should continue to play even when the player moves to a different scene.
    o	audioSource = GetComponent<AudioSource>();: This line gets the AudioSource component and assigns it to audioSource.
    o	The if statement checks if the AudioSource and the audio clip are not null before playing. If they are not null, it plays the audio clip automatically when the game starts.
*/


using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource; // Reference to the AudioSource component

    private void Awake()
    {
        // Ensure the GameObject with this script persists across scene changes
        DontDestroyOnLoad(gameObject);

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Play the audio clip automatically when the game starts
        // Check if the AudioSource and the audio clip are not null before playing
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}