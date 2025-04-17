using UnityEngine;

public class LevelController 
{
    [Header("Referencias")]
    private GameObject paddlePrefab;
    private Transform paddleSpawnPoint;

    private PaddleController currentPaddle;
    public PaddleController CurrentPaddle => currentPaddle;

    public void Start(GameObject paddleGM, Transform paddleSpawn)
    {
        paddlePrefab = paddleGM;
        paddleSpawnPoint = paddleSpawn;
        Initialize();
        BrickManager.Instance.Initialize();
    }

    public void Initialize()
    {
        if (paddlePrefab != null && paddleSpawnPoint != null)
        {
            GameObject paddleGO = GameObject.Instantiate(paddlePrefab, paddleSpawnPoint.position, Quaternion.identity);
            Renderer paddleRenderer = paddleGO.GetComponent<Renderer>();

            currentPaddle = new PaddleController();
            currentPaddle.Initialize(paddleRenderer, paddleGO.transform);
        }
        else
        {
            Debug.LogError("Faltan referencias asignadas en el LevelController");
        }
    }
}