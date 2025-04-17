using System.Collections.Generic;
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

    private List<BallController> activeBalls = new List<BallController>();

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

    public void CustomUpdate(float deltaTime)
    {
        float input = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new (input * speed * deltaTime, 0f, 0f);
        position += movement;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (BallController ball in activeBalls)
            {
                if (!ball.IsLaunched)
                    ball.Launch();
            }
        }

        paddleTransform.position = position;
        HandleScreenBounds();
    }

    private void SpawnNewBall()
    {
        if (ballPrefab == null || ballSpawnPoint == null)
        {
            Debug.LogError("Falta asignar prefab o punto de spawn en el Paddle");
            return;
        }

        GameObject newBallGO = GameObject.Instantiate(ballPrefab, position, Quaternion.identity);
        Vector3 ballSize = new Vector3(0.5f, 0.5f, 0f); 
        currentBall.Initialize(this, ballSize, newBallGO.transform);
        currentBall.CountBalls++;
        activeBalls.Add(currentBall);
    }

    public void SpawnMultiBall()
    {
        GameObject newBallGO = GameObject.Instantiate(ballPrefab, position, Quaternion.identity);
        Vector3 ballSize = new Vector3(0.5f, 0.5f, 0f);

        BallController ball = new BallController();
        ball.Initialize(this, ballSize, newBallGO.transform);
        ball.Launch();
        ball.CountBalls = activeBalls.Count + 1; // o manejalo según quieras contar

        ball.SetDestroyCallback(() => {
            GameObject.Destroy(newBallGO);
            activeBalls.Remove(ball);
        });

        activeBalls.Add(ball);
    }

    private void HandleScreenBounds()
    {
        Vector2 pos = this.position;

        if (pos.x <= -9f)
        {
            position.x = -9f;
        }

        if (pos.x >= 9f)
        {
            position.x = 9f;
        }
    }
}