/*
This script defines a Spike class in Unity. Here's what it does:
•	OnCollisionEnter2D(Collision2D collision):
•	Detects collisions with other 2D objects.
•	If the collision is with an object tagged as "Player":
•	Retrieves the Player script from the player object.
•	Calls the Hit() function on the player.
This script essentially triggers the player's Hit() function when a collision occurs between the spike and the player.
*/


using UnityEngine;

public class Spike : MonoBehaviour
{
    // Function that is called when a 2D collision enters
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the spike collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Player script from the player object
            Player player = collision.gameObject.GetComponent<Player>();

            // Call the Hit function on the player
            player.Hit();
        }
    }
}