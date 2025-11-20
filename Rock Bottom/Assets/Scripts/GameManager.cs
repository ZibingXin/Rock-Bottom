using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    private MapGenerator mapGenerator;
    private GridToTilemap gridToTilemap;
    private PlayerStats playerStats;
    private ScoreManager scoreManager;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


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
        //int seed = Random.Range(0, int.MaxValue);
        mapGenerator.RandomSeed();
        mapGenerator.GenerateNow();
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

    public void Win()
    {
        Debug.Log("Win");
        // Show win screen or perform win actions
    }
}