using System;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : IUpdatable
{
    [Header("Movimiento")]
    private float speed = 10f;

    [Header("Pelota")]
    private GameObject ballPrefab;
    private Transform ballContainer;


    private Transform paddleTransform;

    public Vector3 Size { get; private set; }
    public Vector3 Position { get; private set; }
    public int Lives { get; set; }
    public List<BallController> ActiveBalls { get; private set; } = new List<BallController>();
    public Queue<GameObject> InactiveObjectBalls { get; private set; } = new Queue<GameObject>();
    public Queue<BallController> InactiveLogicBalls { get; private set; } = new Queue<BallController>();
    public UpdateManager UpdateManager { get; private set; }

    public void Initialize(Renderer rendererFake, Transform transform, UpdateManager currentUM)
    {
        Lives = 3;
        ballContainer = currentUM.BallContainer;
        ballPrefab = currentUM.BallPrefab;
        Renderer rend = rendererFake;
        paddleTransform = transform;

        if (rend != null)
        {
            Size = rend.bounds.size;
        }

        Position = paddleTransform.position;

        currentUM.Register(this);
        UpdateManager = currentUM;
        SpawnNewBall();

        UpdateManager.OnRestartGame += RestartBalls;
    }

    public void CustomUpdate(float deltaTime)
    {
        float input = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new(input * speed * deltaTime, 0f, 0f);
        Position += movement;

        if (Input.GetKeyDown(KeyCode.Space) && Math.Abs(Time.timeScale) > 0.01f)
        {
            foreach (BallController ball in ActiveBalls)
            {
                if (!ball.IsLaunched)
                    ball.Launch();
            }
        }

        paddleTransform.position = Position;
        HandleScreenBounds();
    }

    public GameObject SpawnNewBall()
    {
        if (ballPrefab == null)
        {
            return null;
        }

        GameObject newBallGO = GetBall();
        Vector3 ballSize = new Vector3(0.5f, 0.5f, 0f);
        BallController ball = GetLogic();
        ball.Initialize(this, ballSize, newBallGO.transform);


        ball.SetDestroyCallback(() => EventBall(newBallGO, ball));

        ActiveBalls.Add(ball);

        return newBallGO;
    }

    private void EventBall(GameObject newBallGO, BallController ball)
    {
        newBallGO.SetActive(false);
        InactiveObjectBalls.Enqueue(newBallGO);
        InactiveLogicBalls.Enqueue(ball);
        ActiveBalls.Remove(ball);
    }

    public BallController GetLogic()
    {
        if (InactiveLogicBalls.Count > 0)
        {
            return InactiveLogicBalls.Dequeue();
        }
        else
        {
            return new BallController();
        }
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

        return GameObject.Instantiate(ballPrefab, Position, Quaternion.identity, ballContainer);
    }

    private void HandleScreenBounds()
    {
        Vector2 pos = this.Position;

        if (pos.x <= -8.5f)
        {
            pos.x = -8.5f;
        }

        if (pos.x >= 8.5f)
        {
            pos.x = 8.5f;
        }

        this.Position = pos;
    }

    public void CheckDefeat()
    {
        if (Lives == 0)
        {
            UpdateManager.PauseGame();
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