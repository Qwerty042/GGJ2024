using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandleHighScore : MonoBehaviour
{

    public TextMeshProUGUI bestScoreText;

    // Start is called before the first frame update
    void Start()
    {
        int existingHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (GameManager.score > existingHighScore)
        {
            PlayerPrefs.SetInt("HighScore", GameManager.score);
            PlayerPrefs.Save();
            bestScoreText.text = "New Best Score";
        }
        else
        {
            bestScoreText.text = "Best: " + existingHighScore;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
