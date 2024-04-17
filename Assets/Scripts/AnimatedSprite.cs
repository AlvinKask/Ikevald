/*
This script is for an AnimatedSprite class in Unity. It’s used to animate a sprite by changing its frames at a specified framerate. Here’s a breakdown of what each part does:
•	public Sprite[] sprites;: This is an array of sprites that will be used for the animation.
•	public float framerate = 1f / 6f;: This is the framerate at which the sprites will change. It’s set to 6 frames per second by default.
•	private SpriteRenderer spriteRenderer;: This is a reference to the SpriteRenderer component attached to the same GameObject.
•	private int frame;: This is the current frame index.
The methods in the script are:
•	Awake(): This method is called when the script instance is being loaded. It gets the SpriteRenderer component and assigns it to spriteRenderer.
•	OnEnable(): This method is called when the object becomes enabled and active. It starts the Animate() method as a coroutine with a delay equal to the framerate.
•	OnDisable(): This method is called when the behaviour becomes disabled or inactive. It cancels all Invoke calls.
•	Animate(): This method animates the sprite. It increments the frame index, resets it to 0 if it’s greater than or equal to the length of the sprites array, and sets the sprite to the current frame if the frame index is within the bounds of the sprites array.
The RequireComponent attribute at the top ensures that a SpriteRenderer component is attached to the GameObject. If it’s not, Unity will automatically add one. This is necessary because the script needs to change the sprite displayed by the SpriteRenderer to animate it.
*/


using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites; // Array of sprites to be used for the animation
    public float framerate = 1f / 6f; // Framerate at which the sprites will change

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private int frame; // Current frame index

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // OnEnable is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Start the Animate method as a coroutine with a delay equal to the framerate
        InvokeRepeating(nameof(Animate), framerate, framerate);
    }

    // OnDisable is called when the behaviour becomes disabled or inactive
    private void OnDisable()
    {
        // Cancel all Invoke calls
        CancelInvoke();
    }

    // Method to animate the sprite
    private void Animate()
    {
        frame++; // Increment the frame index

        // If the frame index is greater than or equal to the length of the sprites array, reset it to 0
        if (frame >= sprites.Length) {
            frame = 0;
        }

        // If the frame index is within the bounds of the sprites array, set the sprite to the current frame
        if (frame >= 0 && frame < sprites.Length) {
            spriteRenderer.sprite = sprites[frame];
        }
    }
}