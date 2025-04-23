using System.Collections.Generic;
using UnityEngine;

public class PaddleController : IUpdatable
{
    public int Lives { get; set; }

    [Header("Movimiento")]
    private float speed = 10f;

    [Header("Pelota")]
    private GameObject ballPrefab;
    private Transform ballContainer;

    private Vector3 size;
    private Vector3 position;

    private Transform paddleTransform;

    public Vector3 Size => size;
    public Vector3 Position => position;

    private List<BallController> activeBalls = new List<BallController>();
    public List<BallController> ActiveBalls { get => activeBalls; set => activeBalls = value; }

    public Queue<GameObject> InactiveObjectBalls { get; private set; }

    public UpdateManager updateManager { get; private set; }

    public void Initialize(Renderer rendererFake, Transform transform, UpdateManager currentUM)
    {
        Lives = 3;
        ballContainer = currentUM.ballContainer;
        ballPrefab = currentUM.ballPrefab;
        Renderer rend = rendererFake;
        paddleTransform = transform;
        InactiveObjectBalls = new Queue<GameObject>();

        if (rend != null)
        {
            size = rend.bounds.size;
        }
        else
        {
            Debug.LogError("El Paddle no tiene un Renderer asignado.");
        }

        position = paddleTransform.position;

        currentUM.Register(this);
        updateManager = currentUM;
        SpawnNewBall();

        updateManager.OnRestartGame += RestartBalls;
    }

    public void CustomUpdate(float deltaTime)
    {
        float input = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new(input * speed * deltaTime, 0f, 0f);
        position += movement;

        if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale != 0f)
        {
            foreach (BallController ball in ActiveBalls)
            {
                if (!ball.IsLaunched)
                    ball.Launch();
            }
        }

        paddleTransform.position = position;
        HandleScreenBounds();
    }

    public GameObject SpawnNewBall()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("Falta asignar prefab");
            return null;
        }

        GameObject newBallGO = GetBall();
        Vector3 ballSize = new Vector3(0.5f, 0.5f, 0f);

        BallController ball = new BallController();
        ball.Initialize(this, ballSize, newBallGO.transform);


        ball.SetDestroyCallback(() =>
        {
            newBallGO.SetActive(false);
            InactiveObjectBalls.Enqueue(newBallGO);
            activeBalls.Remove(ball);
        });

        ActiveBalls.Add(ball);

        return newBallGO;
    }

    public GameObject GetBall()
    {
        if (InactiveObjectBalls.Count > 0)
        {
            foreach (GameObject ball in InactiveObjectBalls)
            {
                if (!ball.activeInHierarchy)
                {
                    ball.SetActive(true);
                    return ball;
                }
            }
        }

        return GameObject.Instantiate(ballPrefab, position, Quaternion.identity, ballContainer);
    }

    private void HandleScreenBounds()
    {
        Vector2 pos = this.position;

        if (pos.x <= -8.5f)
        {
            position.x = -8.5f;
        }

        if (pos.x >= 8.5f)
        {
            position.x = 8.5f;
        }
    }

    public void CheckDefeat()
    {
        if (Lives == 0)
        {
            updateManager.PauseGame();
            UIManager.Instance.ShowDefeat();
        }
    }

    private void RestartBalls()
    {
        foreach (BallController ball in new List<BallController>(ActiveBalls))
        {
            ball.Dispose();
        }
        SpawnNewBall();
    }
}