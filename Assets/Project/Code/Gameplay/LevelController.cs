using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PaddleController paddlePrefab;
    [SerializeField] private Transform paddleSpawnPoint;

    private PaddleController currentPaddle;

    private void Start()
    {
        Initialize();
        BrickManager.Instance.Initialize();
    }

    public void Initialize()
    {
        if (paddlePrefab != null && paddleSpawnPoint != null)
        {
            currentPaddle = Instantiate(paddlePrefab, paddleSpawnPoint.position, Quaternion.identity);
            currentPaddle.Initialize();
        }
        else
        {
            Debug.LogError("Faltan referencias asignadas en el LevelController");
        }
    }
}