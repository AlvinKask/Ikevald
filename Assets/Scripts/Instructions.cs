/*
This script, Instructions,  is a part of a Unity game project and it’s used to control the visibility of in-game instructions. The instructions are initially visible and then gradually fade out over time. Here are the key parts of the script:
•	Public Variables: visibleDuration and fadeSpeed are public float variables that control how long the instructions stay visible and how quickly they fade out, respectively.
•	Private Variables: uiElement is a reference to the UI Text or Image component that displays the instructions. startTime records when the instructions started appearing. fadingOut is a flag that checks whether the instructions are currently fading out.
•	Start Method: This method is called at the beginning of the game. It gets the UI Text or Image component and records the start time.
•	Update Method: This method is called once per frame. It checks if it’s time to start fading out the instructions. If it is, it calculates the new alpha value for the UI element’s color, applies it, and deactivates the GameObject if the instructions are fully faded out.
*/


using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    // Duration for the instructions to stay visible before fading out
    public float visibleDuration = 12f;
    
    // Speed at which the instructions fade out
    public float fadeSpeed = 1f;

    // Reference to the UI Text or Image component
    private MaskableGraphic uiElement;

    // Time when the instructions started appearing
    private float startTime;

    // Flag to check whether the instructions are currently fading out
    private bool fadingOut = false;

    void Start()
    {
        // Get the UI Text or Image component
        uiElement = GetComponent<MaskableGraphic>();

        // Record the start time
        startTime = Time.time;
    }

    void Update()
    {
        // Check if it's time to start fading out
        if (!fadingOut && Time.time - startTime >= visibleDuration)
        {
            fadingOut = true;
        }

        // Fade out the instructions
        if (fadingOut)
        {
            // Calculate the alpha value to fade out
            float alpha = Mathf.Lerp(1f, 0f, (Time.time - (startTime + visibleDuration)) * fadeSpeed);

            // Set the alpha value of the UI element
            Color color = uiElement.color;
            color.a = alpha;
            uiElement.color = color;

            // If the instructions are fully faded out, deactivate the GameObject
            if (alpha <= 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}