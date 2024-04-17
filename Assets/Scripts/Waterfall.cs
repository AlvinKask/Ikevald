/*
This script, named Waterfall, controls the animation of a waterfall by alternating between two provided sprites. Here's a breakdown of its functionality:
•	Public Variables:
    o	waterfall1 and waterfall2: Sprite objects for the waterfall sprites.
    o	alternateTime: Time interval to alternate between the sprites.
•	Private Variables:
    o	spriteRenderer: A reference to the SpriteRenderer component attached to the same GameObject.
•	Start():
    o	Gets the SpriteRenderer component attached to the same GameObject.
    o	Starts the Alternate() coroutine.
•	Alternate() Coroutine:
    o	Alternates between the waterfall sprites indefinitely using a while loop.
    o	Sets the sprite to waterfall1, waits for alternateTime seconds, then sets the sprite to waterfall2, and waits for alternateTime seconds again.
This script provides a simple way to create a continuous animation for a waterfall effect, with the sprites alternating at a specified time interval.
*/


using System.Collections;
using UnityEngine;

public class Waterfall : MonoBehaviour
{
    // Declare two public Sprite objects for the waterfall sprites
    public Sprite waterfall1;
    public Sprite waterfall2;
    // Define the time to alternate between the sprites
    public float alternateTime = 1f;

    // Declare a private SpriteRenderer object
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer component attached to the GameObject this script is attached to
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Start the Alternate coroutine
        StartCoroutine(Alternate());
    }

    // Define a coroutine to alternate between the waterfall sprites
    IEnumerator Alternate()
    {
        while (true)
        {
            // Set the sprite to waterfall1
            spriteRenderer.sprite = waterfall1;
            // Wait for alternateTime seconds
            yield return new WaitForSeconds(alternateTime);
            // Set the sprite to waterfall2
            spriteRenderer.sprite = waterfall2;
            // Wait for alternateTime seconds
            yield return new WaitForSeconds(alternateTime);
        }
    }
}