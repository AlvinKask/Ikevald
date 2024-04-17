/*
This script, MainMenu, is a part of a Unity game project and it’s used to control the main menu of the game. Here are the key parts of the script:
•	Playgame Method: This method is called when the player chooses to start the game from the main menu. It loads the next scene in the build order, effectively starting the game. The SceneManager.GetActiveScene().buildIndex + 1 expression gets the build index of the current scene (which is the main menu), adds 1 to it, and loads the scene with the resulting index. This assumes that the scenes are arranged in the build order in such a way that the game scene immediately follows the main menu scene.
•	Quitgame Method: This method is called when the player chooses to quit the game from the main menu. It quits the application using the Application.Quit() command. This command only works in a built game, so it won’t do anything in the Unity Editor.
*/


using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Function to start the game
    public void Playgame()
    {
        // Load the next scene in the build order
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Function to quit the game
    public void Quitgame()
    {
        // Quit the application
        Application.Quit();
    }
}