/*
This script, UIScoring, manages the UI elements related to displaying lives and coins in a game. Here's a breakdown of its functionality:
•	Serialized Fields:
    o	Private Text fields _livesText and _coinsText to reference the Text components for displaying lives and coins, respectively. These are serialized fields, meaning they can be set from the Unity editor.
•	Start():
    o	Subscribes to the OnLivesChanged and OnCoinsChanged events of the GameManager singleton instance.
    o	Updates the UI with the initial values of lives and coins from the GameManager.
•	OnDestroy():
    o	Unsubscribes from the OnLivesChanged and OnCoinsChanged events when the object is destroyed. This prevents memory leaks and errors.
•	UpdateLivesUI(int lives):
    o	Updates the _livesText with the current lives count received as a parameter.
•	UpdateCoinsUI(int coins):
    o	Updates the _coinsText with the current coins count received as a parameter.
This script ensures that the UI elements reflecting lives and coins are updated whenever there's a change in the corresponding values managed by the GameManager. It follows the observer pattern by subscribing to events from the GameManager and updating the UI accordingly, promoting loose coupling between UI and game logic.
*/


using UnityEngine;
using UnityEngine.UI;

public class UIScoring : MonoBehaviour
{
    // References to the Text components for displaying lives and coins
    [SerializeField] private Text _livesText;
    [SerializeField] private Text _coinsText;

    private void Start()
    {
        // Subscribe to the OnLivesChanged and OnCoinsChanged events of the GameManager
        GameManager.Instance.OnLivesChanged += UpdateLivesUI;
        GameManager.Instance.OnCoinsChanged += UpdateCoinsUI;

        // Update the UI with the initial lives and coins values
        UpdateLivesUI(GameManager.Instance.lives);
        UpdateCoinsUI(GameManager.Instance.coins);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnLivesChanged and OnCoinsChanged events when this object is destroyed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLivesChanged -= UpdateLivesUI;
            GameManager.Instance.OnCoinsChanged -= UpdateCoinsUI;
        }
    }

    // Function to update the lives UI
    private void UpdateLivesUI(int lives)
    {
        _livesText.text = "Lives: " + lives.ToString();
    }

    // Function to update the coins UI
    private void UpdateCoinsUI(int coins)
    {
        _coinsText.text = "Coins: " + coins.ToString();
    }
}