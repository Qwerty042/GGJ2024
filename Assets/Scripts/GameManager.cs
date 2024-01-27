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
    public static int movesRemaining = 3;
    public static int score = 0;

    void Start()
    {
        UpdateGrid();
    }
    void Update()
    {
        if (gameState == "ENEMY TURN")
        {
            enemyTurnDelay += 1;

            if (enemyTurnDelay == 400)
            {
                EnemyTurn();
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }




    int SoldiersCount()
    {
        int soldiersAliveCount = 0;

        // Iterate through the boardState to count '1's
        for (int i = 0; i < GameManager.boardState.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.boardState.GetLength(1); j++)
            {
                if (GameManager.boardState[i, j] == 2)
                {
                    soldiersAliveCount++;
                }
            }
        }

        return soldiersAliveCount;
    }

    void EnemyTurn()
    {

        Debug.Log("Enemy move made");
        int numberSoldiersToMove = SoldiersCount() / 3;
        for (int i = 0; i < numberSoldiersToMove; i++)
        {
            MoveOneEnemy();
        }

        for (int i = 0; i < 3; i++)
        {
            if (Random.value < 0.7f)
            {
                SpawnNewEnemy();
            }
        }

        UpdateGrid();
        enemyTurnDelay = 0;
        gameState = "PLAYER TURN NO CHARACTER SELECTED";
    }

    void SpawnNewEnemy()
    {
        int columns = boardState.GetLength(1);

        // Check if anything in the rightmost column is 0
        bool hasEmptySpace = false;
        for (int i = 0; i < boardState.GetLength(0); i++)
        {
            if ((boardState[i, columns - 1] == 0) || (boardState[i, columns - 1] == 1))
            {
                hasEmptySpace = true;
                break;
            }
        }

        // If there is an empty space, randomly set one of them to 2
        if (hasEmptySpace)
        {
            // Find all available empty spaces in the rightmost column
            List<int> emptyIndices = new List<int>();
            for (int i = 0; i < boardState.GetLength(0); i++)
            {
                if ((boardState[i, columns - 1] == 0) || (boardState[i, columns - 1] == 1))
                {
                    emptyIndices.Add(i);
                    
                }
            }
            // Convert the list to an array of strings
            string[] indexStrings = emptyIndices.ConvertAll(i => i.ToString()).ToArray();

            // Use string.Join to concatenate the elements with a separator (e.g., ", ")
            string result = string.Join(", ", indexStrings);
            Debug.Log(result);

            // Randomly select one of the empty spaces
            int randomIndex = Random.Range(0, emptyIndices.Count);
            int rowIndex = emptyIndices[randomIndex];

            // Set the randomly selected space to 2
            boardState[rowIndex, columns - 1] = 2;
        }
        else
        {
            Debug.Log("No empty space in the rightmost column.");
        }
    }


    void MoveOneEnemy()
    {
        Vector2 randomEnemy = GetRandomPositionOfValue(2);
        
        // randomly move enemy by one or two squares
        if ((int)randomEnemy.y == 0)
        {
            boardState[(int)randomEnemy.x, (int)randomEnemy.y] = 0;
        }
        else if ((int)randomEnemy.y == 1)
        {
            if (boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] != 2) // stops soldiers from smashing into each other
            {
                boardState[(int)randomEnemy.x, (int)randomEnemy.y] = 0;
                boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] = 2;
            }
        }
        else
        {
            if (Random.value < 0.5f)
            {
                if (boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] != 2) // stops soldiers from smashing into each other
                {
                    boardState[(int)randomEnemy.x, (int)randomEnemy.y] = 0;
                    boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] = 2;
                }
            }
            else
            {
                if (boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] != 2) // stops soldiers from smashing into each other
                {
                    boardState[(int)randomEnemy.x, (int)randomEnemy.y] = 0;
                    boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] = 0;
                    boardState[(int)randomEnemy.x, (int)randomEnemy.y - 2] = 2;
                }
            }
        }
        
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
                float newX = -15.2f + (15.2f / 19f) * (i + j);
                float newY = 0.7f + (7.6f / 19f) * j - (7.6f / 19f) * i;
                Vector3 spawnPosition = new Vector3(newX, newY, 0f);

                if (boardState[i, j] == 1)
                {
                    GameObject newClown = Instantiate(clownPrefab, spawnPosition, Quaternion.identity);
                    newClown.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newY * -100f);
                    clowns.Add(newClown);
                }

                if (boardState[i, j] == 2)
                {
                    GameObject newSoldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
                    newSoldier.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newY * -100f);
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
        // Iterate through the list and destroy each soldier
        foreach (GameObject soldierObject in soldiers)
        {
            Destroy(soldierObject);
        }

        // Clear the list after destroying all soliders
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
