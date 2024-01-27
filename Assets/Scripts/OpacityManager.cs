using UnityEngine;

public class OpacityManager : MonoBehaviour
{
    // Function to change the opacity of a sprite
    public static void ChangeSpriteOpacity(GameObject spriteObject, float alphaValue)
    {
        // Check if the spriteObject is not null
        if (spriteObject != null)
        {
            // Check if the spriteObject has a SpriteRenderer component
            SpriteRenderer spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Get the current color of the sprite
                Color currentColor = spriteRenderer.color;

                // Set the new alpha value
                currentColor.a = alphaValue;

                // Apply the new color to the sprite
                spriteRenderer.color = currentColor;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer component not found on the provided GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Provided GameObject is null.");
        }
    }

    // Function to decrement sprite opacity over time
    public static void DecrementOpacityOverTime(GameObject spriteObject, float decrementFactor)
    {
        // Check if the spriteObject is not null
        if (spriteObject != null)
        {
            // Check if the spriteObject has a SpriteRenderer component
            SpriteRenderer spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Get the current color of the sprite
                Color currentColor = spriteRenderer.color;

                // Calculate the new alpha value based on deltaTime and decrementFactor
                float newAlpha = currentColor.a - (decrementFactor * Time.deltaTime);

                // Ensure the new alpha value stays within [0, 1] range. The lower clamp bound is affected by the number of clowns there are.
                newAlpha = Mathf.Clamp(newAlpha, (float)(0.01 * (double)(10 - ClownCounter.CountClownsAlive())), 1);

                // Set the new alpha value
                currentColor.a = newAlpha;

                // Apply the new color to the sprite
                spriteRenderer.color = currentColor;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer component not found on the provided GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Provided GameObject is null.");
        }
    }
}