using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ClownCounter : MonoBehaviour
{
    public TextMeshProUGUI clownsAliveText;

    void Update()
    {
        // Update the "Clowns Alive" text
        UpdateClownsAliveText();
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
            SceneManager.LoadScene("GameOverScene");
        }

        return clownsAliveCount;
    }
}
