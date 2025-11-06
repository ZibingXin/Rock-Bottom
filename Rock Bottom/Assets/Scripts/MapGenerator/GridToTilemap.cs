using UnityEngine;
using UnityEngine.Tilemaps;

public class GridToTilemap : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public Tilemap tilemap;
    public TileBase dirt, rock, iron, gold, oil;
    public Vector3Int origin = Vector3Int.zero;

    [ContextMenu("Convert Grid to Tilemap")]
    public void Back()
    {
        Clear();

        var grid = mapGenerator.Grid;
        if (grid == null)
        {
            mapGenerator.GenerateNow();
            grid = mapGenerator.Grid;
        }
        int width = grid.GetLength(0), height = grid.GetLength(1);
        tilemap.ClearAllTiles();
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var pos = new Vector3Int(origin.x + x, origin.y - y, origin.z);
                tilemap.SetTile(pos, ToTile(grid[x, y]));
            }
        }
    }

    TileBase ToTile(TileType type)
    {
        return type switch
        {
            TileType.Dirt => dirt,
            TileType.Rock => rock,
            TileType.Iron => iron,
            TileType.Gold => gold,
            TileType.Oil => oil,
            _ => null,
        };
    }

    [ContextMenu("Clear Tilemap")]
    public void Clear()
    {
        tilemap.ClearAllTiles();
    }
}
