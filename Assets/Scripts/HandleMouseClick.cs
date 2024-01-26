using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HandleMouseClick : MonoBehaviour
{
    public Tilemap tilemap;
    public GameManager gameManager;

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0)) // 0 represents the left mouse button
        {
            // Handle the click
            HandleMouseClickMethod();
        }
    }

    void HandleMouseClickMethod()
    {
        // Get the mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
        Vector2Int gridPosition = new Vector2Int(
            cellPosition.x + 5,
            14 - cellPosition.y
            );

        // Optionally, you can print the mouse position to the console
        Debug.Log("Mouse Clicked at: " + gridPosition);
        Debug.Log(GameManager.boardState[gridPosition.y, gridPosition.x]);

        if ((GameManager.boardState[gridPosition.y, gridPosition.x] == 1) && (GameManager.gameState == "PLAYER TURN NO CHARACTER SELECTED"))
        {
            GameManager.gameState = "PLAYER TURN CHARACTER SELECTED";
            GameManager.currentGridPosition = gridPosition;
            Debug.Log("Character selected");
            
        }
        else if ((GameManager.boardState[gridPosition.y, gridPosition.x] == 1) && (GameManager.gameState == "PLAYER TURN CHARACTER SELECTED"))
        {
            GameManager.currentGridPosition = gridPosition;
            Debug.Log("Character reselected");
        }
        else if ((GameManager.boardState[gridPosition.y, gridPosition.x] == 0) && (GameManager.gameState == "PLAYER TURN CHARACTER SELECTED"))
        {
            GameManager.boardState[gridPosition.y, gridPosition.x] = 1;
            GameManager.boardState[GameManager.currentGridPosition.y, GameManager.currentGridPosition.x] = 0;
            Debug.Log("Character moved");
            gameManager.UpdateGrid();

            GameManager.gameState = "ENEMY TURN";
            
            
        }
    }
}
