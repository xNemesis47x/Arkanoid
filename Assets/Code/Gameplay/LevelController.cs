using UnityEngine;

public class LevelController
{
    [Header("Referencias")]
    private GameObject paddlePrefab;
    private Transform paddleSpawnPoint;

    private UpdateManager updateManager;

    public PaddleController CurrentPaddle { get; private set; }

    public int CountLevels { get; private set; }
    public int CountPoints { get; set; }

    public void Start(GameObject paddleGM, Transform paddleSpawn, UpdateManager currentUM)
    {
        CountLevels = 1;
        CountPoints = 0;
        updateManager = currentUM;
        paddlePrefab = paddleGM;
        paddleSpawnPoint = paddleSpawn;
        Initialize();
        BrickManager.Instance.Initialize(currentUM);
    }

    public void Initialize()
    {
        if (paddlePrefab != null && paddleSpawnPoint != null && CurrentPaddle == null)
        {
            GameObject paddleGO = GameObject.Instantiate(paddlePrefab, paddleSpawnPoint.position, paddleSpawnPoint.rotation);
            Renderer paddleRenderer = paddleGO.GetComponent<Renderer>();

            CurrentPaddle = new PaddleController();
            CurrentPaddle.Initialize(paddleRenderer, paddleGO.transform, updateManager);
        }
    }
}