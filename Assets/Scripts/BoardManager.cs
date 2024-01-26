using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private Vector2Int mouseoverTile;
    private Vector2Int selectedTile;
    private List<Vector2Int> validTileList;
    private bool isSelected;
    private bool isMouseOverBoard;
    public int boardSize;
    public GameObject mouseoverTilePrefab;
    public GameObject selectedTilePrefab;
    public GameObject validTilePrefab;
    private List<GameObject> specialTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyAllSpecialTiles();
        if (isMouseOverBoard)
        {
            CreateTileAtBoardPosition(mouseoverTilePrefab, mouseoverTile, 1);
        }

        if (isSelected)
        {
            CreateTileAtBoardPosition(selectedTilePrefab, selectedTile);

            foreach (Vector2Int validTile in validTileList) 
            {
                CreateTileAtBoardPosition(validTilePrefab, validTile);
            }
        }
    }

    public void MouseoverPosition(Vector2Int mouseoverTilePos)
    {
        if (mouseoverTilePos.x < 0 || mouseoverTilePos.x >= boardSize || mouseoverTilePos.y < 0 || mouseoverTilePos.y >= boardSize)
        {
            isMouseOverBoard = false;
        }
        else
        {
            isMouseOverBoard = true;
            mouseoverTile = mouseoverTilePos;
        }
    }

    public void SelectTile(Vector2Int selectedTilePos)
    {
        selectedTile = selectedTilePos;
        isSelected = true;
    }

    public void Deselect() 
    {
        isSelected = false;
    }

    public void ValidTiles(List<Vector2Int> validTiles)
    {
        validTileList = validTiles;
    }

    private void DestroyAllSpecialTiles()
    {
        foreach (GameObject tile in specialTiles) 
        {
            Destroy(tile);
        }
        specialTiles.Clear();
    }

    private void CreateTileAtBoardPosition(GameObject tileType, Vector2Int boardPosition, int sortingOrderModifier = 0)
    {
        float newX = -15.2f + (15.2f / 19f) * ((boardPosition.y + 1) + (boardPosition.x - 1));
        float newY = 0.7f + (7.6f / 19f) * (boardPosition.x - 1) - (7.6f / 19f) * (boardPosition.y + 1);
        Vector3 spawnPosition = new Vector3(newX, newY, 0f);

        GameObject newSpecialTile = Instantiate(tileType, spawnPosition, Quaternion.identity);
        newSpecialTile.GetComponent<SpriteRenderer>().sortingOrder = -32768 + 1 + sortingOrderModifier; // at least one level above the normal board tiles
        specialTiles.Add(newSpecialTile);
    }
}
