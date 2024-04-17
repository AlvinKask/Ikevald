/*
This script, HorizontalCamera, is a Unity script written in C# that manages the behavior of a camera in a 2D game. Here’s a breakdown of its key parts:
•	Private Variables: The script has two private variables, camera and player. camera is a reference to the Camera component of the game object, and player is a reference to the Transform component of the player game object.
•	Public Variables: The script has several public variables, height, undergroundHeight, and undergroundThreshold, which control the vertical position of the camera based on whether it’s underground or not.
•	Awake Method: This method is called when the script instance is being loaded. It gets the Camera component attached to the game object and finds the player game object by its tag.
•	LateUpdate Method: This method is called every frame, after all Update functions have been called. It gets the current position of the camera, sets the x position of the camera to be the maximum of its current x position and the player’s x position, and updates the position of the camera.
•	SetUnderground Method: This method sets whether the camera is underground or not. It gets the current position of the camera, sets its y position to undergroundHeight if the camera is underground or height otherwise, and updates the position of the camera.
*/


using UnityEngine;

public class HorizontalCamera : MonoBehaviour
{
    // Declare a new Camera object and a Transform object for the player
    private new Camera camera;
    private Transform player;

    // Set the default height for the camera and the height when it's underground
    public float height = 7f;
    public float undergroundHeight = -9f;
    public float undergroundThreshold = 0f;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the Camera component attached to the same GameObject this script is attached to
        camera = GetComponent<Camera>();
        // Find the GameObject with the tag "Player" and get its Transform component
        player = GameObject.FindWithTag("Player").transform;
    }

    // LateUpdate is called every frame, after all Update functions have been called
    private void LateUpdate()
    {
        // Get the current position of the camera
        Vector3 cameraPosition = transform.position;
        // Set the x position of the camera to be the maximum of its current x position and the player's x position
        cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
        // Update the position of the camera
        transform.position = cameraPosition;
    }

    // Function to set whether the camera is underground or not
    public void SetUnderground(bool underground)
    {
        // Get the current position of the camera
        Vector3 cameraPosition = transform.position;
        // If the camera is underground, set its y position to undergroundHeight, otherwise set it to height
        cameraPosition.y = underground ? undergroundHeight : height;
        // Update the position of the camera
        transform.position = cameraPosition;
    }
}