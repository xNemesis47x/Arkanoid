using UnityEngine;

public class Brick
{
    private Vector2 size;
    private Vector2 position;

    public Vector2 Size => size;
    public Vector2 Position => position;

    private GameObject brickGO;
    public System.Action OnDesactivateBrick { get; private set;} 

    bool isPowerUp;

    BrickManager brickManager;

    UpdateManager updateManager;

    public void Initialize(Vector3 renderer, Transform transform, BrickManager currentBrickManager, UpdateManager currentUM, bool containsPowerUp = false)
    {
        size = renderer;
        position = transform.position;
        brickGO = transform.gameObject;
        isPowerUp = containsPowerUp;
        brickManager = currentBrickManager;

        updateManager = currentUM;
    }

    public void DesactivateBrick()
    {
        if (isPowerUp)
        {
            SpawnPowerUp();
        }

        brickManager.RemoveBrick(this);
        brickGO.SetActive(false);
        brickManager.InactiveBrick.Enqueue(brickGO);
    }

    private void SpawnPowerUp()
    {
        GameObject powerUpPrefab = updateManager.powerUpPrefab; 
        GameObject powerUpGO = GameObject.Instantiate(powerUpPrefab, position, Quaternion.identity);
        PowerUp powerUp = new PowerUp(powerUpGO.transform, new Vector2(0.5f, 0.5f), updateManager); 

        powerUp.SetOnCollectedCallback(() => {
            MultiBall multiBall = new MultiBall(updateManager.GetPaddle());
            multiBall.SpawnMultiBall();
        });
    }

    public bool CheckCollision(Vector2 ballPos, Vector2 ballSize)
    {
        bool overlapX = Mathf.Abs(ballPos.x - position.x) < (ballSize.x / 2 + size.x / 2);
        bool overlapY = Mathf.Abs(ballPos.y - position.y) < (ballSize.y / 2 + size.y / 2);

        return overlapX && overlapY;
    }
}