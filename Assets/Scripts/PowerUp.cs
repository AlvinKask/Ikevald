/*
This script is for a PowerUp class in a Unity game. Here are the key aspects of its methods:
•	Enum Type: This enum represents the different types of power-ups that can be collected in the game, which include Coin, ExtraLife, and ArmorUp.
•	OnTriggerEnter2D(Collider2D other): This method is automatically called by Unity when a 2D collider enters the trigger. If the collider belongs to the player (checked using other.CompareTag("Player")), the Collect method is called.
•	Collect(GameObject player): This method handles the collection of the power-up. It performs an action based on the type of power-up:
    o	Coin: If the power-up is a coin, it adds a coin to the game manager.
    o	ExtraLife: If the power-up is an extra life, it adds a life to the game manager.
    o	ArmorUp: If the power-up is armor, it grows the player.
After performing the action, the power-up is destroyed using Destroy(gameObject).
*/


using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Enum to represent the different types of power-ups
    public enum Type
    {
        Coin,
        ExtraLife,
        ArmorUp,
    }

    // Variable to hold the type of power-up
    public Type type;

    // Function that is called when a 2D collider enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the collider belongs to the player, collect the power-up
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    // Function to handle the collection of the power-up
    private void Collect(GameObject player)
    {
        // Perform an action based on the type of power-up
        switch (type)
        {
            case Type.Coin:
                // If the power-up is a coin, add a coin to the game manager
                GameManager.Instance.AddCoin();
                break;

            case Type.ExtraLife:
                // If the power-up is an extra life, add a life to the game manager
                GameManager.Instance.AddLife();
                break;

            case Type.ArmorUp:
                // If the power-up is armor, grow the player
                player.GetComponent<Player>().Grow();
                break;
        }

        // Destroy the power-up
        Destroy(gameObject);
    }
}