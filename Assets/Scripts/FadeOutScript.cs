using UnityEngine;
using TMPro;

public class FadeOutScript : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public SpriteRenderer poppiesSprite;

    public float fadeDuration = 4f;
    public float delayBeforeFade = 8f;

    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Set the alpha of the TextMeshPro and SpriteRenderer components to 1 (fully visible) at the start
        SetAlpha(1f);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        // Wait for the specified delay before starting the fade-out
        if (elapsedTime < delayBeforeFade)
        {
            Debug.Log(elapsedTime);
        }
        else
        {
            // Calculate the normalized progress of the fade-out
            float t = Mathf.Clamp01((elapsedTime - delayBeforeFade) / fadeDuration);

            // Lerping alpha from 1 to 0 for the TextMeshPro and SpriteRenderer components
            float lerpedAlpha = Mathf.Lerp(1f, 0f, t);

            Debug.Log("Setting alpha to "+ lerpedAlpha);
            // Set the alpha for TextMeshPro components
            SetAlpha(lerpedAlpha);

            // Check if the fade-out is complete
            if (t >= 1f)
            {
                Debug.Log("Fade out theoretically complete?");
            }
        }
    }

    // Helper function to set alpha for TextMeshPro and SpriteRenderer components
    private void SetAlpha(float alpha)
    {
        // Set alpha for TextMeshPro components
        if (gameOverText != null)
        {
            Color gameOverColor = gameOverText.color;
            gameOverColor.a = alpha;
            gameOverText.color = gameOverColor;
        }

        if (scoreText != null)
        {
            Color scoreColor = scoreText.color;
            scoreColor.a = alpha;
            scoreText.color = scoreColor;
        }

        // Set alpha for the SpriteRenderer component
        if (poppiesSprite != null)
        {
            Color spriteColor = poppiesSprite.color;
            spriteColor.a = alpha;
            poppiesSprite.color = spriteColor;
        }
    }
}
