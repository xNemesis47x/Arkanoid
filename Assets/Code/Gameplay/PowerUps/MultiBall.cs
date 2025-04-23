using UnityEngine;

public class MultiBall
{
    PaddleController paddleController;

    public MultiBall(PaddleController currentPaddle)
    {
        paddleController = currentPaddle;
    }

    public void SpawnMultiBall()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject newBallGO = paddleController.GetBall();
            Vector3 ballSize = new Vector3(0.5f, 0.5f, 0f);

            BallController ball = new BallController();
            ball.Initialize(paddleController, ballSize, newBallGO.transform);
            ball.Launch();

            ball.SetDestroyCallback(() =>
            {
                newBallGO.SetActive(false);
                paddleController.InactiveObjectBalls.Enqueue(newBallGO);
                paddleController.ActiveBalls.Remove(ball);
            });

            paddleController.ActiveBalls.Add(ball);
        }
    }
}
