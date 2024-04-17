/*
This script, Moon, is a part of the Unity game engine and it’s used to control the position of a game object, referred to as “Moon”, relative to the main camera in the scene. The script makes the “Moon” object maintain a certain distance from the camera, with an additional offset, creating a dynamic and immersive visual effect as if the “Moon” is always in the view of the camera.
Here are the key parts of the methods:
•	public float distanceFromCamera = 10.0f;: This line sets the distance from the camera at which the “Moon” object should stay. It’s set to 10 units by default.
•	public Vector3 offset;: This line declares a Vector3 variable offset that can be used to adjust the position of the “Moon” object relative to the camera’s forward direction.
•	private Camera mainCamera;: This line declares a private variable mainCamera of type Camera. This will hold a reference to the main camera in the scene.
•	void Start() { ... }: The Start method is a Unity callback that is called before the first frame update. Here, it’s used to get a reference to the main camera in the scene.
•	void Update() { ... }: The Update method is another Unity callback that is called once per frame. In this method, the script checks if the main camera is not null, calculates the target position for the “Moon” object, and then sets the “Moon” object’s position to this target position. This makes the “Moon” object appear to follow the camera around in the game scene.
*/


using UnityEngine;

public class Moon : MonoBehaviour
{
    // Distance from the camera
    public float distanceFromCamera = 10.0f;
    
    // Additional offset from the camera's forward direction
    public Vector3 offset;

    // Reference to the main camera in the scene
    private Camera mainCamera;

    void Start()
    {
        // Get the main camera in the scene
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the main camera is not null
        if (mainCamera != null)
        {
            // Calculate the position where the moon should stay
            Vector3 targetPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera + offset;

            // Set the position of the moon
            transform.position = targetPosition;
        }
    }
}