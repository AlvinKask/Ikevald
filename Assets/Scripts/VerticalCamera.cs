/*
This script, named VerticalCamera, controls the vertical movement of a camera in a 2D environment to follow the player vertically. Here's a breakdown of its functionality:
•	RequireComponent(typeof(Camera)):
    o	This attribute ensures that the GameObject this script is attached to has a Camera component. If not, it adds one automatically.
•	Private Variables:
    o	camera: A reference to the Camera component attached to the same GameObject.
    o	player: A reference to the player's transform.
•	Public Variables:
    o	minHeight and maxHeight: Define the minimum and maximum height for the camera to follow the player vertically.
•	Awake():
    o	Gets the Camera component attached to the same GameObject.
    o	Finds the GameObject with the tag "Player" and gets its transform.
•	LateUpdate():
    o	Executed every frame after all Update functions have been called.
    o	Calculates the target position for the camera based on the player's position.
    o	Clamps the targetY value between minHeight and maxHeight to restrict the camera's vertical movement.
    o	Updates the camera's position to match the target position, ensuring it follows the player vertically within the defined height bounds.
This script provides a simple solution for implementing vertical camera movement in a 2D game, ensuring that the camera follows the player's vertical movements while staying within specified height limits.
*/


using UnityEngine;

// This script requires a Camera component on the same GameObject
[RequireComponent(typeof(Camera))]
public class VerticalCamera : MonoBehaviour
{
    // Declare a new Camera object
    private new Camera camera;
    // Declare a Transform object to hold the player's transform
    private Transform player;

    // Define the minimum and maximum height for the camera
    public float minHeight = 7f;
    public float maxHeight = 40f;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the Camera component attached to the GameObject this script is attached to
        camera = GetComponent<Camera>();
        // Find the GameObject tagged as "Player" and get its transform
        player = GameObject.FindWithTag("Player").transform;
    }

    // LateUpdate is called every frame, if the Behaviour is enabled, after all Update functions have been called
    private void LateUpdate()
    {
        // Get the current position of the camera
        Vector3 cameraPosition = transform.position;
        // Clamp the y position of the player between the minHeight and maxHeight
        float targetY = Mathf.Clamp(player.position.y, minHeight, maxHeight);
        // Set the y position of the camera to the clamped y position of the player
        cameraPosition.y = targetY;
        // Update the position of the camera
        transform.position = cameraPosition;
    }
}