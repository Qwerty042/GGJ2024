using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject clownPrefab;
    public GameObject soldierPrefab;
    public GameObject soldierPrefab2;
    public GameObject tankPrefab;
    public GameObject bombPrefab;
    public GameObject ghostClownPrefab;
    public GameObject ghostSoldierPrefab;
    public GameObject ghostSoldierPrefab2;
    public BoardManager boardManager;
    public static Vector2Int currentGridPosition;
    public List<GameObject> clowns = new List<GameObject>();
    public List<GameObject> soldiers = new List<GameObject>();
    private static List<Vector2Int> deadClowns = new List<Vector2Int>();
    private List<GameObject> ghostClowns = new List<GameObject>();
    private static List<Vector2Int> deadSoldiers = new List<Vector2Int>();
    private static List<Vector2Int> deadSoldiers2 = new List<Vector2Int>();
    private List<GameObject> ghostSoldiers= new List<GameObject>();
    public List<Bomb> bombs = new List<Bomb>();
    public float enemyTurnDelay;

    public GameObject bloodScreen;

    public AudioClip enemyMoveSound;
    public AudioClip explodeSound;
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
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0},
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
        OpacityManager.ChangeSpriteOpacity(bloodScreen, 0f);
        audioSourceSoundEffects = gameObject.AddComponent<AudioSource>();
        UpdateGrid();
    }
    void Update()
    {
        OpacityManager.DecrementOpacityOverTime(bloodScreen, 2.5f);

        if (gameState == "ENEMY TURN")
        {
            enemyTurnDelay += Time.deltaTime;

            if (enemyTurnDelay >= 1.0f)
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

        //foreach (GameObject item in collection)
        //{

        //}
        //ghostClowns.RemoveAll(ghostClown => ghostClown.alpha == 0.0f);
        //ghostSoldiers.RemoveAll(ghostSoldier => ghostSoldier.alpha == 0.0f); 
    }

    public static void ResetStaticStuff() // don't you love static variables
    {
        currentGridPosition = new Vector2Int();
        deadClowns = new List<Vector2Int>();
        deadSoldiers = new List<Vector2Int>();
        deadSoldiers2 = new List<Vector2Int>();

        boardState = new int[,] // 1 is clown, 2 is soldier
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };

        gameState = "PLAYER TURN NO CHARACTER SELECTED";
        movesRemaining = 3;
        score = 0;
}


    int SoldiersCount()
    {
        int soldiersAliveCount = 0;

        // Iterate through the boardState to count '2's
        for (int i = 0; i < GameManager.boardState.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.boardState.GetLength(1); j++)
            {
                if (GameManager.boardState[i, j] >= 2)
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
        int numDeadClowns = 10 - ClownCounter.CountClownsAlive();
        int numberSoldiersToMove = (int)((float)SoldiersCount() * (0.3 + 0.07 * numDeadClowns));
        for (int i = 0; i < numberSoldiersToMove; i++)
        {
            MoveOneEnemy();
        }

        for (int i = 0; i < 3 + numDeadClowns; i++)
        {
            if (Random.value < 0.7f)
            {
                SpawnNewEnemy();
            }
        }

        if (Random.value < 0.5f + (float)numDeadClowns * 0.05)
        {
            SpawnBomb();
        }

        foreach (Bomb bomb in bombs)
        {
            bomb.Update(boardManager);
        }
        bombs.RemoveAll(bomb => bomb.Obsolete());

        foreach (Vector2Int deadClown in deadClowns)
        {
            OpacityManager.ChangeSpriteOpacity(bloodScreen, 1.0f);
            float newX = -15.2f + (15.2f / 19f) * (deadClown.y + deadClown.x);
            float newY = 0.7f + (7.6f / 19f) * deadClown.x - (7.6f / 19f) * deadClown.y;
            Vector3 spawnPosition = new Vector3(newX, newY, 0f);
            GameObject newGhostClown = Instantiate(ghostClownPrefab, spawnPosition, Quaternion.identity);
            newGhostClown.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newY * -1000f) - 1;
        }
        foreach (Vector2Int deadSoldier in deadSoldiers)
        {
            float newX = -15.2f + (15.2f / 19f) * (deadSoldier.y + deadSoldier.x);
            float newY = 0.7f + (7.6f / 19f) * deadSoldier.x - (7.6f / 19f) * deadSoldier.y;
            Vector3 spawnPosition = new Vector3(newX, newY, 0f);
            GameObject newSoldierClown = Instantiate(ghostSoldierPrefab, spawnPosition, Quaternion.identity);
            newSoldierClown.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newY * -1000f) - 1;
        }
        foreach (Vector2Int deadSoldier2 in deadSoldiers2)
        {
            float newX = -15.2f + (15.2f / 19f) * (deadSoldier2.y + deadSoldier2.x);
            float newY = 0.7f + (7.6f / 19f) * deadSoldier2.x - (7.6f / 19f) * deadSoldier2.y;
            Vector3 spawnPosition = new Vector3(newX, newY, 0f);
            GameObject newSoldierClown = Instantiate(ghostSoldierPrefab2, spawnPosition, Quaternion.identity);
            newSoldierClown.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newY * -1000f) - 1;
        }
        boardManager.DeathTiles(deadClowns.Concat(deadSoldiers).Concat(deadSoldiers2).ToList<Vector2Int>());
        deadClowns.Clear();
        deadSoldiers.Clear();
        deadSoldiers2.Clear();

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

            // Set the randomly selected space to 2, 3, or 4
            CheckForDeathBeforeClearingCellWhyAmIDoingThisThisWay(rowIndex, columns - 1);
            if (Random.value < 0.5)
                boardState[rowIndex, columns - 1] = 2;
            else if (Random.value < 0.8)
                boardState[rowIndex, columns - 1] = 3;
            else
                boardState[rowIndex, columns - 1] = 4;
        }
        else
        {
            Debug.Log("No empty space in the rightmost column.");
        }
    }

    void SpawnBomb()
    {
        Vector2Int spawnPos = new Vector2Int(Random.Range(0, boardState.GetLength(0) - 1), Random.Range(0, boardState.GetLength(1) - 1));
        bombs.Add(new Bomb(spawnPos, bombPrefab, audioSourceSoundEffects,explodeSound));
    }

    void MoveOneEnemy()
    {
        Vector2 randomEnemy;
        int enemyType;

        if (Random.value < 0.5)
        {
            randomEnemy = GetRandomPositionOfValue(2);
            enemyType = 2;
        }
        else if (Random.value < 0.5)
        {
            randomEnemy = GetRandomPositionOfValue(3);
            enemyType = 3;
        }
        else
        {
            randomEnemy = GetRandomPositionOfValue(4);
            enemyType = 4;
        }
            

        // randomly move enemy by one or two squares
        if ((int)randomEnemy.y == 0) // is the enemy at the end of the board? if so they just disappear
        {
            boardState[(int)randomEnemy.x, (int)randomEnemy.y] = 0;
        }
        else if ((int)randomEnemy.y == 1)
        {
            if (boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] < 2) // only move one square to reach the edge
            {
                boardState[(int)randomEnemy.x, (int)randomEnemy.y] = 0;
                CheckForDeathBeforeClearingCellWhyAmIDoingThisThisWay((int)randomEnemy.x, (int)randomEnemy.y - 1);
                boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] = enemyType;
            }
        }
        else
        {
            if (Random.value < 0.5f)
            {
                if (boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] < 2) // stops soldiers from smashing into each other
                {
                    boardState[(int)randomEnemy.x, (int)randomEnemy.y] = 0;
                    CheckForDeathBeforeClearingCellWhyAmIDoingThisThisWay((int)randomEnemy.x, (int)randomEnemy.y - 1);
                    boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] = enemyType;
                }
            }
            else
            {
                if (boardState[(int)randomEnemy.x, (int)randomEnemy.y - 1] < 2 && boardState[(int)randomEnemy.x, (int)randomEnemy.y - 2] < 2) // stops soldiers from smashing into each other
                {
                    boardState[(int)randomEnemy.x, (int)randomEnemy.y] = 0;
                    CheckForDeathBeforeClearingCellWhyAmIDoingThisThisWay((int)randomEnemy.x, (int)randomEnemy.y - 1);
                    CheckForDeathBeforeClearingCellWhyAmIDoingThisThisWay((int)randomEnemy.x, (int)randomEnemy.y - 2);
                    boardState[(int)randomEnemy.x, (int)randomEnemy.y - 2] = enemyType;
                }
            }
        }
        
    }

    private void CheckForDeathBeforeClearingCellWhyAmIDoingThisThisWay(int x, int y)
    {
        if (boardState[x, y] == 1)
        {
            ClownDied(new Vector2Int(y, x));
        }
        else if ((boardState[x, y] == 2) || (boardState[x, y] == 4))
        {
            SoldierDied(new Vector2Int(y, x));
        }
        if (boardState[x, y] == 1)
        {
            SoldierDied2(new Vector2Int(y, x));
        }
        boardState[x, y] = 0;
    }

    public static void ClownDied(Vector2Int pos)
    {
        deadClowns.Add(pos);
    }

    public static void SoldierDied(Vector2Int pos)
    {
        deadSoldiers.Add(pos);
    }

    public static void SoldierDied2(Vector2Int pos)
    {
        deadSoldiers2.Add(pos);
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

                if (boardState[i, j] == 3)
                {
                    GameObject newSoldier = Instantiate(soldierPrefab2, spawnPosition, Quaternion.identity);
                    newSoldier.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newY * -1000f);
                    soldiers.Add(newSoldier);
                }

                if (boardState[i, j] == 4)
                {
                    GameObject newTank = Instantiate(tankPrefab, spawnPosition, Quaternion.identity);
                    newTank.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newY * -1000f);
                    soldiers.Add(newTank);
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
    private AudioClip explodeSound;
    private AudioSource audioSourceExplosions;
    

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

    public Bomb(Vector2Int spawnPos, GameObject bombPrefab, AudioSource audioSourceSoundEffects, AudioClip boomSound)
    {
        prefab = bombPrefab;
        pos = spawnPos;
        state = BombState.INIT;
        audioSourceExplosions = audioSourceSoundEffects;
        explodeSound = boomSound;
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
                audioSourceExplosions.clip = explodeSound;
                audioSourceExplosions.Play();
                List<Vector2Int> explodedTiles = new List<Vector2Int>();
                foreach (Vector2Int relativeAoePos in relativeAoePositions)
                {
                    Vector2Int gridPos = new Vector2Int(relativeAoePos.x + pos.x, relativeAoePos.y + pos.y);
                    if (gridPos.x >= 0 && gridPos.x < 20 && gridPos.y >= 0 && gridPos.y < 20)
                    {
                        if (GameManager.boardState[gridPos.y, gridPos.x] == 1)
                        {
                            GameManager.ClownDied(gridPos);
                        }
                        else if (GameManager.boardState[gridPos.y, gridPos.x] == 2)
                        {
                            GameManager.SoldierDied(gridPos);
                        }
                        else if (GameManager.boardState[gridPos.y, gridPos.x] == 3)
                        {
                            GameManager.SoldierDied2(gridPos);
                        }
                        if (GameManager.boardState[gridPos.y, gridPos.x] != 4)
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
