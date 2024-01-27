using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUpdate : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        scoreText.text = "Score: " + GameManager.score;
    }
}
