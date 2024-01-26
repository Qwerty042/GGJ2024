using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnIndicatorText : MonoBehaviour
{
    public TextMeshProUGUI turnIndicatorText;

    void Update()
    {
        // Update the "Clowns Alive" text
        UpdateTurnIndicatorText();
    }

    void UpdateTurnIndicatorText()
    {
        if ((GameManager.gameState == "PLAYER TURN NO CHARACTER SELECTED") || (GameManager.gameState == "PLAYER TURN CHARACTER SELECTED"))
        {
            turnIndicatorText.text = "Clown Turn\nActions: " + GameManager.movesRemaining;
        }
        else if (GameManager.gameState == "ENEMY TURN")
        {
            turnIndicatorText.text = "Enemy Turn";
        }
    }
}
