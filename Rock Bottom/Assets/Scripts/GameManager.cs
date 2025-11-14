using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    private MapGenerator mapGenerator;
    private GridToTilemap gridToTilemap;
    private PlayerStats playerStats;
    private ScoreManager scoreManager;

    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();

        mapGenerator = FindAnyObjectByType<MapGenerator>();
        gridToTilemap = FindAnyObjectByType<GridToTilemap>();

        playerStats = FindAnyObjectByType<PlayerStats>();
        scoreManager = FindAnyObjectByType<ScoreManager>();

        GenerateNewMap();
    }

    private void GenerateNewMap()
    {
        int seed = Random.Range(0, int.MaxValue);
        mapGenerator.GenerateNow(seed);
        gridToTilemap.Back();
    }

    public void RestartGame()
    {
        // reset player
        playerController.Reset();

        // regenerate map
        GenerateNewMap();

        //reset UI
        playerStats.ResetPlayer();
        scoreManager.UpdateStatus();
    }
}