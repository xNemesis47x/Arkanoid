using UnityEngine;

public class LevelController
{
    [Header("Referencias")]
    private GameObject paddlePrefab;
    private Transform paddleSpawnPoint;

    private PaddleController currentPaddle;
    public PaddleController CurrentPaddle => currentPaddle;

    UpdateManager updateManager;

    private int countLevels;
    public int countPoints { get; set; }

    public void Start(GameObject paddleGM, Transform paddleSpawn, UpdateManager currentUM)
    {
        countLevels = 1;
        countPoints = 0;
        updateManager = currentUM;
        paddlePrefab = paddleGM;
        paddleSpawnPoint = paddleSpawn;
        Initialize();
        BrickManager.Instance.Initialize(currentUM);
    }

    public void Initialize()
    {
        if (paddlePrefab != null && paddleSpawnPoint != null && currentPaddle == null)
        {
            GameObject paddleGO = GameObject.Instantiate(paddlePrefab, paddleSpawnPoint.position, paddleSpawnPoint.rotation);
            Renderer paddleRenderer = paddleGO.GetComponent<Renderer>();

            currentPaddle = new PaddleController();
            currentPaddle.Initialize(paddleRenderer, paddleGO.transform, updateManager);
        }
    }

    public int GetPoints()
    {
        return countPoints;
    }

    public int GetLevels()
    {
        return countLevels;
    }

    public int GetLives()
    {
        return currentPaddle.Lives;
    }
}