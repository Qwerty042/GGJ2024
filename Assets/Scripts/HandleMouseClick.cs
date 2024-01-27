using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class HandleMouseClick : MonoBehaviour
{
    public Tilemap tilemap;
    public GameManager gameManager;
    public BoardManager boardManager;

    public AudioClip selectSound;
    public AudioClip moveSound;
    public AudioClip failedAttackSound;
    

    AudioSource audioSourceSoundEffects;
    

    private void Start()
    {
        audioSourceSoundEffects = gameObject.AddComponent<AudioSource>();
        audioSourceSoundEffects.volume = 0.5f;

    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0)) // 0 represents the left mouse button
        {
            // Handle the click
            HandleMouseClickMethod();
        }
    }

    List<Vector2Int> GetValidTiles(Vector2Int gridPosition)
    {
        List<Vector2Int> validTiles = new List<Vector2Int>();
        for (int i = 0; i < GameManager.boardState.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.boardState.GetLength(1); j++)
            {
                if (((GameManager.boardState[j, i] == 0) && (Mathf.Abs(j - gridPosition.y) <= 2f && Mathf.Abs(i - gridPosition.x) <= 2f)))
                {
                    validTiles.Add(new Vector2Int(i, j));
                }
            }
        }
        return validTiles;
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
            boardManager.SelectTile(gridPosition);
            List<Vector2Int> validTiles = GetValidTiles(gridPosition);
            boardManager.ValidTiles(validTiles);
            Debug.Log("Character selected");
            audioSourceSoundEffects.clip = selectSound;
            audioSourceSoundEffects.Play();


        }
        else if ((GameManager.boardState[gridPosition.y, gridPosition.x] == 1) && (GameManager.gameState == "PLAYER TURN CHARACTER SELECTED"))
        {
            GameManager.currentGridPosition = gridPosition;
            boardManager.SelectTile(gridPosition);
            List<Vector2Int> validTiles = GetValidTiles(gridPosition);
            boardManager.ValidTiles(validTiles);
            Debug.Log("Character reselected");

            audioSourceSoundEffects.clip = selectSound;
            audioSourceSoundEffects.Play();

        }
        else if ((GameManager.boardState[gridPosition.y, gridPosition.x] == 2) && (GameManager.gameState == "PLAYER TURN CHARACTER SELECTED"))
        {
            audioSourceSoundEffects.clip = failedAttackSound;
            audioSourceSoundEffects.Play();
        }
        else if ((GameManager.boardState[gridPosition.y, gridPosition.x] == 0) && (GameManager.gameState == "PLAYER TURN CHARACTER SELECTED"))
        {
            if (Mathf.Abs(GameManager.currentGridPosition.y - gridPosition.y) <= 2f && Mathf.Abs(GameManager.currentGridPosition.x - gridPosition.x) <= 2f)
            {
                GameManager.boardState[gridPosition.y, gridPosition.x] = 1;
                GameManager.boardState[GameManager.currentGridPosition.y, GameManager.currentGridPosition.x] = 0;
                boardManager.Deselect();
                Debug.Log("Character moved");
                gameManager.UpdateGrid();
                GameManager.movesRemaining -= 1;
                GameManager.gameState = "PLAYER TURN NO CHARACTER SELECTED";

                audioSourceSoundEffects.clip = moveSound;
                audioSourceSoundEffects.Play();

                if (GameManager.movesRemaining <= 0)
                {
                    GameManager.score += ClownCounter.CountClownsAlive() * 100;
                    GameManager.gameState = "ENEMY TURN";
                    GameManager.movesRemaining = 3;
                }
                
            }
            
            
        }
    }
}
