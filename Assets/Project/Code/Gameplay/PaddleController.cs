using UnityEngine;

public class PaddleController : IUpdatable
{
    [Header("Movimiento")]
    private float speed = 10f;

    [Header("Pelota")]
    private GameObject ballPrefab;
    private Transform ballSpawnPoint;

    private BallController currentBall;
    private Vector3 size;
    private Vector3 position;

    private Transform paddleTransform;

    private System.Action onDestroyCallback;

    public Vector3 Size => size;

    public Vector3 Position => position;

    public void Initialize(Renderer rendererFake, Transform transform)
    {
        ballPrefab = UpdateManager.Instance.ballPrefab;
        ballSpawnPoint = UpdateManager.Instance.ballSpawnPoint;
        Renderer rend = rendererFake;
        paddleTransform = transform;
        currentBall = new BallController();

        if (rend != null)
        {
            size = rend.bounds.size;
        }
        else
        {
            Debug.LogError("El Paddle no tiene un Renderer asignado.");
        }

        position = paddleTransform.position;

        UpdateManager.Instance.Register(this);
        SpawnNewBall();
    }

    public void Dispose()
    {
        UpdateManager.Instance.Unregister(this);

        if (currentBall != null)
        {
            currentBall.Dispose();
        }

        onDestroyCallback?.Invoke();
    }

    public void CustomUpdate(float deltaTime)
    {
        float input = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new (input * speed * deltaTime, 0f, 0f);
        position += movement;

        if (currentBall != null && Input.GetKeyDown(KeyCode.Space) && !currentBall.IsLaunched)
        {
            currentBall.Launch();
        }

        paddleTransform.position = position;
    }

    public void SpawnNewBall()
    {
        if (ballPrefab == null || ballSpawnPoint == null)
        {
            Debug.LogError("Falta asignar prefab o punto de spawn en el Paddle");
            return;
        }

        GameObject newBallGO = GameObject.Instantiate(ballPrefab, position, Quaternion.identity);
        Vector3 ballSize = new Vector3(0.5f, 0.5f, 0f); // o lo que uses
        currentBall.Initialize(this, ballSize, newBallGO.transform);

        currentBall.SetDestroyCallback(() => GameObject.Destroy(newBallGO));
    }
}