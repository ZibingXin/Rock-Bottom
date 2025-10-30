using UnityEngine;

public class GamaManager : MonoBehaviour
{
    private MapGenerator mapGenerator;
    private TileType[,] grid;



    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.GenerateNow();
        grid = mapGenerator.Grid;

    }



}
