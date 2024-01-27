using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject clownPrefab;
    public GameObject soldierPrefab;
    public GameObject bombPrefab;
    public BoardManager boardManager;
    public static Vector2Int currentGridPosition;
    public List<GameObject> clowns = new List<GameObject>();
    public List<GameObject> soldiers = new List<GameObject>();
    public List<Bomb> bombs = new List<Bomb>();
    public int enemyTurnDelay;

    public AudioClip enemyMoveSound;
    AudioSource audioSourceSoundEffects;

    public static int[,] boardState = new int[,] // 1 is clown, 2 is soldier
    {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

    private int[,] lastBoardState = boardState;

    public static string gameState = "PLAYER TURN NO CHARACTER SELECTED";
    public static int movesRemaining = 3;
    public static int score = 0;

    void Start()
    {
        audioSourceSoundEffects = gameObject.AddComponent<AudioSource>();
        UpdateGrid();
    }
    void Update()
    {
        if (gameState == "ENEMY TURN")
        {
            enemyTurnDelay += 1;

            if (enemyTurnDelay == 400)
            {
                audioSourceSoundEffects.clip = enemyMoveSound;
                audioSourceSoundEffects.Play();
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

        if (Random.value < 0.5f)
        {
            SpawnBomb();
        }

        //if (bombs.Count == 0)
        //{
        //    SpawnBomb();
        //}

        foreach (Bomb bomb in bombs)
        {
            bomb.Update(boardManager);
        }
        bombs.RemoveAll(bomb => bomb.Obsolete());

        UpdateGrid();
        enemyTurnDelay = 0;
        gameState = "PLAYER TURN NO CHARACTER SELECTED";
        FindAllTheDeadBodiesAndMakeSomeCoolEffects();
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

    void SpawnBomb()
    {
        Vector2Int spawnPos = new Vector2Int(Random.Range(0, boardState.GetLength(0) - 1), Random.Range(0, boardState.GetLength(1) - 1));
        //Vector2Int spawnPos = new Vector2Int(2, 2);
        bombs.Add(new Bomb(spawnPos, bombPrefab));
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

    private void FindAllTheDeadBodiesAndMakeSomeCoolEffects()
    {
        List<Vector2Int> deathTiles = new List<Vector2Int>();
        // Iterate through the boardState to count '1's
        for (int i = 0; i < GameManager.boardState.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.boardState.GetLength(1); j++)
            {
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!");
                if (boardState[i,j] == 0)
                {
                    if (lastBoardState[i,j] == 1) //clown died
                    {
                        // do some stuff to show a clown died
                        Debug.Log("!!! clown died");
                        deathTiles.Add(new Vector2Int(j,i));
                    }
                    else if (lastBoardState[i,j] == 2) // soldier died
                    {
                        // do some stuff to show a soldier died
                        deathTiles.Add(new Vector2Int(j, i));
                    }
                }
            }
        }
        boardManager.DeathTiles(deathTiles);
        lastBoardState = boardState;
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
                    newClown.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newY * -1000f);
                    clowns.Add(newClown);
                }

                if (boardState[i, j] == 2)
                {
                    GameObject newSoldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
                    newSoldier.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newY * -1000f);
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

public class Bomb
{
    private GameObject prefab;
    private List<GameObject> bombSprites = new List<GameObject>();
    private Vector2Int pos;
    private BombState state;
    
    /*    -2-1 0 1 2
     * 
     * -2  0 0 1 0 0
     * -1  0 1 1 1 0
     *  0  1 1 x 1 1
     *  1  0 1 1 1 0
     *  2  0 0 1 0 0
     */
    
    private Vector2Int[] relativeAoePositions = new Vector2Int[]
    {
        new Vector2Int( 0, -2),
        new Vector2Int(-1, -1),
        new Vector2Int( 0, -1),
        new Vector2Int( 1, -1),
        new Vector2Int(-2, 0),
        new Vector2Int(-1, 0),
        new Vector2Int( 0, 0),
        new Vector2Int( 1, 0),
        new Vector2Int( 2, 0),
        new Vector2Int(-1, 1),
        new Vector2Int( 0, 1),
        new Vector2Int( 1, 1),
        new Vector2Int(0, 2),
    };

    enum BombState
    {
        INIT,
        EXPLODE,
        DESTROY,
        OBSOLETE,
    }

    public Bomb(Vector2Int spawnPos, GameObject bombPrefab)
    {
        prefab = bombPrefab;
        pos = spawnPos;
        state = BombState.INIT;
    }

    public void Update(BoardManager boardManager)
    {
        switch (state)
        {
            case BombState.INIT:
                foreach (Vector2Int relativeAoePos in relativeAoePositions)
                {
                    Vector2Int gridPos = new Vector2Int(relativeAoePos.x + pos.x, relativeAoePos.y + pos.y);
                    if (gridPos.x >= 0 && gridPos.x < 20 && gridPos.y >= 0 && gridPos.y < 20)
                    {
                        float newX = -15.2f + (15.2f / 19f) * ((gridPos.y + 1) + (gridPos.x - 1));
                        float newY = 0.7f + (7.6f / 19f) * (gridPos.x - 1) - (7.6f / 19f) * (gridPos.y + 1);
                        Vector3 spawnPosition = new Vector3(newX, newY, 0f);
                        GameObject newBombSprite = MonoBehaviour.Instantiate(prefab, spawnPosition, Quaternion.identity);
                        float jankY = 0.7f + (7.6f / 19f) * (gridPos.x) - (7.6f / 19f) * (gridPos.y);
                        newBombSprite.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(jankY * -1000f) + 1;
                        bombSprites.Add(newBombSprite);
                    }
                }
                state = BombState.EXPLODE;
                break;
            case BombState.EXPLODE:
                List<Vector2Int> explodedTiles = new List<Vector2Int>();
                foreach (Vector2Int relativeAoePos in relativeAoePositions)
                {
                    Vector2Int gridPos = new Vector2Int(relativeAoePos.x + pos.x, relativeAoePos.y + pos.y);
                    if (gridPos.x >= 0 && gridPos.x < 20 && gridPos.y >= 0 && gridPos.y < 20)
                    {
                        GameManager.boardState[gridPos.y, gridPos.x] = 0;
                        explodedTiles.Add(gridPos);
                    }
                }
                boardManager.ExplodedTiles(explodedTiles);
                foreach (GameObject bomb in bombSprites)
                {
                    MonoBehaviour.Destroy(bomb);
                }
                state = BombState.DESTROY;
                break;
            case BombState.DESTROY:
                boardManager.ExplodedTiles(new List<Vector2Int>());
                state = BombState.OBSOLETE;
                break;
            case BombState.OBSOLETE:
                // Wait to be deleted
                break;
        }
    }

    public bool Obsolete()
    {
        return state == BombState.OBSOLETE;
    }
}
