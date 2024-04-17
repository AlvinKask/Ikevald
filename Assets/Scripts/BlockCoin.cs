/*
This script is for a BlockCoin class in Unity. It’s used to animate a coin object in the game, moving it up and then back down, before destroying it. Here’s a breakdown of what each part does:
•	private void Start(): This method is called at the start of the script. It adds a coin to the game manager and starts the animation coroutine.
•	private IEnumerator Animate(): This coroutine is responsible for animating the coin. It first calculates the resting and animated positions of the coin. It then moves the coin from the resting position to the animated position, and then back down. After the animation, it destroys the coin game object.
•	private IEnumerator Move(Vector3 from, Vector3 to): This coroutine moves the coin from one position to another over a duration of 0.25 seconds. It uses linear interpolation (Vector3.Lerp) to smoothly transition the coin’s position from the start to the end position.
The GameManager.Instance.AddCoin(); line assumes that there is a GameManager class with a singleton instance and an AddCoin method. This method is likely responsible for updating the game’s state to reflect the new coin.
*/


using System.Collections;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    // This method is called at the start of the script
    private void Start()
    {
        // Adds a coin to the game manager
        GameManager.Instance.AddCoin();

        // Starts the animation coroutine
        StartCoroutine(Animate());
    }

    // Coroutine for animating the coin
    private IEnumerator Animate()
    {
        // The resting position of the coin
        Vector3 restingPosition = transform.localPosition;
        // The position of the coin when animated
        Vector3 animatedPosition = restingPosition + Vector3.up * 2f;

        // Move the coin from resting position to animated position
        yield return Move(restingPosition, animatedPosition);
        // Move the coin back from animated position to resting position
        yield return Move(animatedPosition, restingPosition);

        // Destroy the coin game object after the animation
        Destroy(gameObject);
    }

    // Coroutine for moving the coin from one position to another
    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        // Elapsed time since the start of the movement
        float elapsed = 0f;
        // Duration of the movement
        float duration = 0.25f;

        // While the elapsed time is less than the duration
        while (elapsed < duration)
        {
            // Calculate the fraction of the total duration that has elapsed
            float t = elapsed / duration;

            // Interpolate the position of the coin from the start position to the end position
            transform.localPosition = Vector3.Lerp(from, to, t);
            // Increase the elapsed time
            elapsed += Time.deltaTime;

            // Yield control to the next frame
            yield return null;
        }

        // Set the final position of the coin
        transform.localPosition = to;
    }
}