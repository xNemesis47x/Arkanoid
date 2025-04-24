using UnityEngine;

public class MultiBall
{
    private PaddleController paddleController;
    private int ballsAmount;

    public MultiBall(PaddleController currentPaddle)
    {
        ballsAmount = 2;
        paddleController = currentPaddle;
    }

    public void SpawnMultiBall()
    {
        for (int i = 0; i < ballsAmount; i++)
        {
            GameObject newBallGO = paddleController.GetBall();
            Vector3 ballSize = new Vector3(0.5f, 0.5f, 0f);

            BallController ball = paddleController.GetLogic();
            ball.Initialize(paddleController, ballSize, newBallGO.transform);
            ball.Launch();

            ball.SetDestroyCallback(() => EventBall(newBallGO, ball));

            paddleController.ActiveBalls.Add(ball);
        }
    }

    private void EventBall(GameObject newBallGO, BallController ball)
    {
        newBallGO.SetActive(false);
        paddleController.InactiveObjectBalls.Enqueue(newBallGO);
        paddleController.InactiveLogicBalls.Enqueue(ball);
        paddleController.ActiveBalls.Remove(ball);
    }
}
