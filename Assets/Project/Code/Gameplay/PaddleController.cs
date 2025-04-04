using UnityEngine;

public class PaddleController : MonoBehaviour, IUpdatable
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 10f;

    [Header("Pelota")]
    [SerializeField] private BallController ballPrefab;
    [SerializeField] private Transform ballSpawnPoint;

    private BallController currentBall;
    private Vector2 size;

    public Vector2 Size => size;

    public void Initialize()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            size = rend.bounds.size;
        }
        else
        {
            Debug.LogError("El Paddle no tiene un Renderer asignado.");
        }

        UpdateManager.Instance.Register(this);
        SpawnNewBall();
    }

    public void Dispose()
{
    UpdateManager.Instance.Unregister(this);

    if (currentBall != null)
    {
        currentBall.Dispose();
        GameObject.Destroy(currentBall.gameObject);
    }

    GameObject.Destroy(this.gameObject); // <-- agregá esto si se va a destruir desde fuera
}

    public void CustomUpdate(float deltaTime)
    {
        float input = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(input * speed * deltaTime, 0f, 0f);
        transform.position += movement;

        if (currentBall != null && Input.GetKeyDown(KeyCode.Space))
        {
            currentBall.Launch();
        }
    }

    public void SpawnNewBall()
    {
        if (ballPrefab == null || ballSpawnPoint == null)
        {
            Debug.LogError("Falta asignar prefab o punto de spawn en el Paddle");
            return;
        }

        BallController newBall = GameObject.Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
        currentBall = newBall;
        newBall.Initialize(this);
    }
}