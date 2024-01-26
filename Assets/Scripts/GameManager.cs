using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject clownPrefab;
    public GameObject soldierPrefab;
    public static Vector2Int currentGridPosition;
    public List<GameObject> clowns = new List<GameObject>();
    public List<GameObject> soldiers = new List<GameObject>();
    public int enemyTurnDelay;

    public static int[,] boardState = new int[,]
    {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

    public static string gameState = "PLAYER TURN NO CHARACTER SELECTED";

    void Start()
    {
        UpdateGrid();
    }
    void Update()
    {
        if (gameState == "ENEMY TURN")
        {
            enemyTurnDelay += 1;

            if (enemyTurnDelay == 1000)
            {
                EnemyMove();
            }
        }
    }

    void EnemyMove()
    {
        Debug.Log("Enemy move made");
        Vector2 randomEnemy = GetRandomPositionOfValue(2);
        boardState[(int)randomEnemy.x, (int)randomEnemy.y] = 0;
        boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] = 2;
        
        UpdateGrid();
        enemyTurnDelay = 0;
        gameState = "PLAYER TURN NO CHARACTER SELECTED";
    }

    public void UpdateGrid()
    {
        DestroyAllSoldiers();
        DestroyAllClowns();
        // you need to reorder things somehow to make things render in the right order
        for (int i = 0; i < boardState.GetLength(0); i++)
        {
            for (int j = 0; j < boardState.GetLength(1); j++)
            {
                if (boardState[i, j] == 1)
                {

                    float newX = -15.2f + (15.2f / 19f) * (i + j);
                    float newY = 0.7f + (7.6f / 19f) * j - (7.6f / 19f) * i;

                    Vector3 spawnPosition = new Vector3(newX, newY, 0f);
                    GameObject newClown = Instantiate(clownPrefab, spawnPosition, Quaternion.identity);
                    clowns.Add(newClown);
                }

                if (boardState[i, j] == 2)
                {

                    float newX = -15.2f + (15.2f / 19f) * (i + j);
                    float newY = 0.7f + (7.6f / 19f) * j - (7.6f / 19f) * i;

                    Vector3 spawnPosition = new Vector3(newX, newY, 0f);
                    GameObject newSoldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
                    soldiers.Add(newSoldier);
                }
            }
        }
    }

    public void DestroyAllClowns()
    {
        // Iterate through the list and destroy each clown
        foreach (GameObject clownObject in clowns)
        {
            Destroy(clownObject);
        }

        // Clear the list after destroying all clowns
        clowns.Clear();

    }

    public void DestroyAllSoldiers()
    {
        // Iterate through the list and destroy each clown
        foreach (GameObject soldierObject in soldiers)
        {
            Destroy(soldierObject);
        }

        // Clear the list after destroying all clowns
        soldiers.Clear();

    }

    Vector2 GetRandomPositionOfValue(int targetValue)
    {
        // Create a list to store positions with the target value
        System.Collections.Generic.List<Vector2> targetPositions = new System.Collections.Generic.List<Vector2>();

        // Iterate through the array to find positions with the target value
        for (int i = 0; i < boardState.GetLength(0); i++)
        {
            for (int j = 0; j < boardState.GetLength(1); j++)
            {
                if (boardState[i, j] == targetValue)
                {
                    targetPositions.Add(new Vector2(i, j));
                }
            }
        }

        // Check if there are positions with the target value
        if (targetPositions.Count > 0)
        {
            // Pick a random position from the list
            int randomIndex = Random.Range(0, targetPositions.Count);
            return targetPositions[randomIndex];
        }
        else
        {
            // No positions with the target value found
            Debug.LogWarning("No positions with the target value (" + targetValue + ") found.");
            return Vector2.zero; // You can return some default value or handle it based on your needs
        }
    }

    void PrintBoardState()
    {
        for (int i = 0; i < boardState.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < boardState.GetLength(1); j++)
            {
                row += boardState[i, j] + " ";
            }
            Debug.Log("Row " + i + ": " + row);
        }
    }
}
