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

    public Action<float> OnMovePaddle;

    public Queue<BallController> InactiveLogicBalls { get; private set; } = new Queue<BallController>();
    public int PaddleHits { get; set; }

    private UpdateManager updateManager;
    private BallController ballController;

    public void Initialize(Renderer rendererFake, Transform transform, UpdateManager currentUM, AdressableInstantiator adressable)
    {
        Lives = 3;
        ballContainer = currentUM.BallContainer;
        ballPrefab = adressable.GetInstancePrefabs("ballPrefab");
        Renderer rend = rendererFake;
        paddleTransform = transform;

        if (rend != null)
        {
            Size = rend.bounds.size;
        }

        Position = paddleTransform.position;

        currentUM.Register(this);
        updateManager = currentUM;
        SpawnNewBall();

        updateManager.OnRestartGame -= RestartBalls;
        updateManager.OnRestartGame += RestartBalls;
        updateManager.OnNextLevel -= RestartBalls;
        updateManager.OnNextLevel += RestartBalls;
    }

    public void CustomUpdate(float deltaTime)
    {
        Vector3 oldPosition = Position;

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

        HandleScreenBounds();

        float deltaMovement = Position.x - oldPosition.x;
        if (Mathf.Abs(deltaMovement) > 0.001f)
        {
            OnMovePaddle?.Invoke(deltaMovement);
        }

        paddleTransform.position = Position;
    }

    public GameObject SpawnNewBall()
    {
        if (ballPrefab == null)
        {
            return null;
        }

        if (ballController != null)
            ballController.Dispose();

        GameObject newBallGO = GetBall();
        Vector3 ballSize = new Vector3(0.5f, 0.5f, 0f);
        ballController = GetLogic();
        ballController.Initialize(this, ballSize, newBallGO.transform, updateManager);


        ballController.SetDestroyCallback(() => EventBall(newBallGO, ballController));

        ActiveBalls.Add(ballController);

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
        PaddleHits = 0;
    }

    public void DestroyReference()
    {
        GameObject.Destroy(paddleTransform.gameObject);
    }
}