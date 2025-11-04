using UnityEngine;

public class GamaManager : MonoBehaviour
{
    private MapGenerator mapGenerator;
    private GridToTilemap gridToTilemap;
    private TileType[,] grid;



    void Start()
    {
        mapGenerator = FindAnyObjectByType<MapGenerator>();
        gridToTilemap = FindAnyObjectByType<GridToTilemap>();

        StartNewGame();
    }

    private void StartNewGame()
    {
        int seed = Random.Range(0, int.MaxValue);
        mapGenerator.GenerateNow();
        gridToTilemap.Back();
    }



}
