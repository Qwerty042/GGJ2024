using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HandleMouseover : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase newTile;

    private void Update()
    {
        HandleMouseoverMethod();
    }

    private void HandleMouseoverMethod()
    {
        // Get the mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

        Vector2Int gridPosition = new Vector2Int(
            cellPosition.x + 5,
            14 - cellPosition.y
            );

        Debug.Log("Cell Position: " + cellPosition);
        Debug.Log("Grid Position: " + gridPosition);
    }
}
