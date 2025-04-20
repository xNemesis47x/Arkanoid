using UnityEngine;

public class LevelController 
{
    [Header("Referencias")]
    private GameObject paddlePrefab;
    private Transform paddleSpawnPoint;

    private PaddleController currentPaddle;
    public PaddleController CurrentPaddle => currentPaddle;

    UpdateManager updateManager;

    public void Start(GameObject paddleGM, Transform paddleSpawn, UpdateManager currentUM)
    {
        updateManager = currentUM;
        paddlePrefab = paddleGM;
        paddleSpawnPoint = paddleSpawn;
        Initialize();
        BrickManager.Instance.Initialize(currentUM);
    }

    public void Initialize()
    {
        if (paddlePrefab != null && paddleSpawnPoint != null)
        {
            GameObject paddleGO = GameObject.Instantiate(paddlePrefab, paddleSpawnPoint.position, Quaternion.identity);
            Renderer paddleRenderer = paddleGO.GetComponent<Renderer>();

            currentPaddle = new PaddleController();
            currentPaddle.Initialize(paddleRenderer, paddleGO.transform, updateManager);
        }
        else
        {
            Debug.LogError("Faltan referencias asignadas en el LevelController");
        }
    }
}