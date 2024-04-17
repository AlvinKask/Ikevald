/*
This script, Extensions, is a Unity script written in C# that provides extension methods for Rigidbody2D and Transform classes. Here’s a breakdown of its key parts:
•	LayerMask: A LayerMask for the “Default” layer is defined. This is used in the Raycast method to specify which layer to hit.
•	Raycast Method: This is an extension method for the Rigidbody2D class. It performs a raycast in a specified direction. If the Rigidbody2D is kinematic, it returns false. It defines the radius and distance for the CircleCast, performs a CircleCast, and returns true if the CircleCast hit a collider that is not attached to the same Rigidbody2D.
•	DotTest Method: This is an extension method for the Transform class. It performs a dot product test with another Transform. It calculates the direction from this Transform to the other Transform and returns true if the dot product of the normalized directions is greater than 0.25.
*/


using UnityEngine;

public static class Extensions
{
    // Define a LayerMask for the "Default" layer
    private static LayerMask layerMask = LayerMask.GetMask("Default");

    // Extension method for Rigidbody2D to perform a raycast in a specified direction
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        // If the Rigidbody2D is kinematic, return false
        if (rigidbody.isKinematic) {
            return false;
        }

        // Define the radius and distance for the CircleCast
        float radius = 0.25f;
        float distance = 0.375f;

        // Perform a CircleCast and store the result in a RaycastHit2D
        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);
        // Return true if the CircleCast hit a collider that is not attached to the same Rigidbody2D
        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    // Extension method for Transform to perform a dot product test with another Transform
    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        // Calculate the direction from this Transform to the other Transform
        Vector2 direction = other.position - transform.position;
        // Return true if the dot product of the normalized directions is greater than 0.25
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }
}