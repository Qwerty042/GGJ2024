using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ClownCounter : MonoBehaviour
{
    public TextMeshProUGUI clownsAliveText;
    private static bool gameOver = false;
    private float gameOverDelayCounter = 1.5f;

    void Update()
    {
        // Update the "Clowns Alive" text
        UpdateClownsAliveText();
        
        if (gameOver)
        {
            gameOverDelayCounter -= Time.deltaTime;
            if (gameOverDelayCounter <= 0.0f)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    void UpdateClownsAliveText()
    {
        // Count the number of '1's in the boardState
        int clownsAliveCount = CountClownsAlive();

        // Update the UI Text component
        clownsAliveText.text = "Clowns Alive: " + clownsAliveCount + "/10";
    }

    public static int CountClownsAlive()
    {
        int clownsAliveCount = 0;

        // Iterate through the boardState to count '1's
        for (int i = 0; i < GameManager.boardState.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.boardState.GetLength(1); j++)
            {
                if (GameManager.boardState[i, j] == 1)
                {
                    clownsAliveCount++;
                }
            }
        }

        if (clownsAliveCount == 0)
        {
            gameOver = true;
            //SceneManager.LoadScene("GameOver");
        }

        return clownsAliveCount;
    }
}
