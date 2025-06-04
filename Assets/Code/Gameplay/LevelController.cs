using UnityEngine;

public class LevelController
{
    [Header("Referencias")]
    private GameObject paddlePrefab;
    private Transform paddleSpawnPoint;

    private AdressableInstantiator adressable;
    private UpdateManager updateManager;

    public PaddleController CurrentPaddle { get; private set; }

    public int CountLevels { get; private set; }
    public int CountPoints { get; set; }

    public void Start(UpdateManager currentUM, Transform paddleSpawn, AdressableInstantiator currentAdress)
    {
        CountLevels = 1;
        CountPoints = 0;
        updateManager = currentUM;
        adressable = currentAdress;
        paddlePrefab = adressable.GetInstance("paddlePrefab");
        paddleSpawnPoint = paddleSpawn;
        Initialize();
        BrickManager.Instance.Initialize(currentUM, currentAdress);
        currentUM.OnRestartGame += () => Start(updateManager, paddleSpawnPoint, adressable);
    }

    public void Initialize()
    {
        if (paddlePrefab != null && paddleSpawnPoint != null && CurrentPaddle == null)
        {
            GameObject paddleGO = GameObject.Instantiate(paddlePrefab, paddleSpawnPoint.position, paddleSpawnPoint.rotation);
            Renderer paddleRenderer = paddleGO.GetComponent<Renderer>();

            CurrentPaddle = new PaddleController();
            CurrentPaddle.Initialize(paddleRenderer, paddleGO.transform, updateManager, adressable);
        }
    }
}