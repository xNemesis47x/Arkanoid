using UnityEngine;

public class MultiBall 
{
    PaddleController paddleController;
    GameObject ballPrefab;

    public MultiBall(PaddleController currentPaddle)
    {
        paddleController = currentPaddle;
        ballPrefab = UpdateManager.Instance.ballPrefab;
    }

    public void SpawnMultiBall()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject newBallGO = GameObject.Instantiate(ballPrefab, paddleController.Position, Quaternion.identity);
            Vector3 ballSize = new Vector3(0.5f, 0.5f, 0f);

            BallController ball = new BallController();
            ball.Initialize(paddleController, ballSize, newBallGO.transform);
            ball.Launch();
            ball.CountBalls = paddleController.ActiveBalls.Count + 1; // o manejalo según quieras contar

            ball.SetDestroyCallback(() =>
            {
                GameObject.Destroy(newBallGO);
                paddleController.ActiveBalls.Remove(ball);
            });

            paddleController.ActiveBalls.Add(ball);
        }
    }
}
