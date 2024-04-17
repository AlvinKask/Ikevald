/*
This script, Player,  is a part of the Unity game engine and it’s used to control the behavior of a game object, referred to as “Player”. The script allows the player to grow, shrink, and die, with corresponding visual and audio effects.
Here are the key parts of the methods:
•	public PlayerSpriteRenderer smallRenderer;, public PlayerSpriteRenderer bigRenderer;, public PlayerSpriteRenderer hugeRenderer;: These lines declare public PlayerSpriteRenderer variables that represent the player in different sizes (small, big, huge).
•	public CapsuleCollider2D capsuleCollider { get; private set; }, public DeathAnimation deathAnimation { get; private set; }: These lines declare public properties for the player’s CapsuleCollider2D and DeathAnimation components.
•	public bool big => bigRenderer.enabled;, public bool dead => deathAnimation.enabled;: These lines declare public properties that indicate whether the player is big or dead.
•	public AudioClip deathSound;, public AudioClip armorupSound;, public AudioClip losingarmorSound;: These lines declare public AudioClip variables that store the sounds to play when the player dies, grows, or shrinks.
•	private void Awake() { ... }: The Awake method is a Unity callback that is called when the script instance is being loaded. Here, it’s used to get references to the player’s CapsuleCollider2D and DeathAnimation components, set the active renderer to the small renderer, and get or add an AudioSource component on the game object.
•	public void Hit() { ... }: The Hit method is called when the player is hit by an enemy. It checks if the player is not dead, then shrinks the player or kills the player based on the player’s current size.
•	public void Death() { ... }: The Death method is called to kill the player. It plays the death sound, disables the renderers, enables the death animation, and resets the level.
•	public void Grow() { ... }, public void ShrinkToBig() { ... }, public void ShrinkToSmall() { ... }: These methods are called to grow or shrink the player. They play the corresponding sound, enable the renderer for the new size, set the active renderer, adjust the size and offset of the capsule collider, and start the scale animation.
•	private IEnumerator ScaleAnimation() { ... }: The ScaleAnimation coroutine is used to animate the player’s size change. It alternates the enabled state of the small and big renderers every 4 frames over a duration of 0.5 seconds, then sets the active renderer to be the only one enabled.
*/


using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Different renderers for different player sizes
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    public PlayerSpriteRenderer hugeRenderer;
    private PlayerSpriteRenderer activeRenderer; // The currently active renderer

    // Reference to the player's collider and death animation
    public CapsuleCollider2D capsuleCollider { get; private set; }
    public DeathAnimation deathAnimation { get; private set; }

    // Properties to check if the player is big or dead
    public bool big => bigRenderer.enabled;
    public bool dead => deathAnimation.enabled;

    // Sounds for different events
    public AudioClip deathSound;
    public AudioClip armorupSound;
    public AudioClip losingarmorSound;
    private AudioSource audioSource; // Reference to the AudioSource component

    private void Awake()
    {
        // Get the required components
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        deathAnimation = GetComponent<DeathAnimation>();
        activeRenderer = smallRenderer;

        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Function to handle when the player is hit
    public void Hit()
    {
        if (!dead)
        {
            // Shrink the player or kill them based on their current size
            if (activeRenderer == hugeRenderer) {
                ShrinkToBig();
            } else if (activeRenderer == bigRenderer) {
                ShrinkToSmall();
            } else {
                Death();
            }
        }
    }

    // Function to handle the player's death
    public void Death()
    {
        // Play the death sound and start the death animation
        audioSource.PlayOneShot(deathSound, 0.3f);
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        // Reset the level after a delay
        GameManager.Instance.ResetLevel(3f);
    }

    // Function to grow the player
    public void Grow()
    {
        // If the player is already at maximum size, do nothing
        if (activeRenderer == hugeRenderer) {
            return;
        }

        // Play the armor up sound and grow the player
        audioSource.PlayOneShot(armorupSound, 0.5f);
        if (activeRenderer == smallRenderer) {
            // Grow from small to big
            smallRenderer.enabled = false;
            bigRenderer.enabled = true;
            activeRenderer = bigRenderer;
            capsuleCollider.size = new Vector2(0.85f, 2f);
            capsuleCollider.offset = new Vector2(0f, 0.5f);
        } else if (activeRenderer == bigRenderer) {
            // Grow from big to huge
            bigRenderer.enabled = false;
            hugeRenderer.enabled = true;
            activeRenderer = hugeRenderer;
            capsuleCollider.size = new Vector2(0.85f, 2f);
            capsuleCollider.offset = new Vector2(0f, 0.5f);
        }
        // Start the scale animation
        StartCoroutine(ScaleAnimation());
    }

    // Function to shrink the player to big size
    public void ShrinkToBig()
    {
        // Play the losing armor sound and shrink the player
        audioSource.PlayOneShot(losingarmorSound, 0.5f);
        hugeRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;
        capsuleCollider.size = new Vector2(0.85f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);
        StartCoroutine(ScaleAnimation());
    }

    // Function to shrink the player to small size
    public void ShrinkToSmall()
    {
        // Play the losing armor sound and shrink the player
        audioSource.PlayOneShot(losingarmorSound, 0.5f);
        bigRenderer.enabled = false;
        smallRenderer.enabled = true;
        activeRenderer = smallRenderer;
        capsuleCollider.size = new Vector2(0.85f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);
        StartCoroutine(ScaleAnimation());
    }

    // Coroutine to animate the player's scaling
    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        // Animate the player's scaling over the duration
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Toggle the visibility of the renderers every 4 frames
            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        // Ensure the correct renderer is enabled at the end of the animation
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }
}