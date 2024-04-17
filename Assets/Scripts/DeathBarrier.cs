/*
This script, DeathBarrier  manages the behavior of a “death barrier” in a game. Here’s a breakdown of its key parts:
•	OnTriggerEnter2D Method: This method is called when another Collider2D enters the trigger attached to the game object this script is attached to. This is typically used in Unity to implement behavior when a game object enters a certain area (the “trigger”).
    o	If the game object that entered the trigger is tagged as “Player”, it calls the Death method of the Player component attached to the game object. This is typically used to implement some kind of “death” or “game over” behavior when the player character enters the death barrier.
    o	If the game object that entered the trigger is not tagged as “Player”, it destroys the game object that entered the trigger. This could be used to clean up other types of game objects that should be destroyed when they enter the death barrier.
*/


using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the game object that entered the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Call the Death method of the Player component attached to the game object
            other.GetComponent<Player>().Death();
        }
        else
        {
            // Destroy the game object that entered the trigger
            Destroy(other.gameObject);
        }
    }
}