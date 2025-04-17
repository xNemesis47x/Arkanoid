using UnityEngine;

public class Brick
{
    private Vector2 size;
    private Vector2 position;

    public Vector2 Size => size;
    public Vector2 Position => position;

    private GameObject brickGO;

    private System.Action onDestroyBrick;
    public System.Action OnDestroyBrick => onDestroyBrick;

    bool isPowerUp;

    public void Initialize(Vector3 renderer, Transform transform, bool containsPowerUp = false)
    {
        size = renderer;
        position = transform.position;
        brickGO = transform.gameObject;
        isPowerUp = containsPowerUp;
    }

    public void DestroyBrick()
    {
        if (isPowerUp)
        {
            SpawnPowerUp();
        }

        BrickManager.Instance.RemoveBrick(this);
        SetDestroyCallback(() => GameObject.Destroy(brickGO));
    }

    private void SpawnPowerUp()
    {
        GameObject powerUpPrefab = UpdateManager.Instance.powerUpPrefab; 
        GameObject powerUpGO = GameObject.Instantiate(powerUpPrefab, position, Quaternion.identity);
        PowerUp powerUp = new PowerUp(powerUpGO.transform, new Vector2(0.5f, 0.5f)); 

        powerUp.SetOnCollectedCallback(() => {
            MultiBall multiBall = new MultiBall(UpdateManager.Instance.GetPaddle());
            multiBall.SpawnMultiBall();
        });
    }

    public bool CheckCollision(Vector2 ballPos, Vector2 ballSize)
    {
        bool overlapX = Mathf.Abs(ballPos.x - position.x) < (ballSize.x / 2 + size.x / 2);
        bool overlapY = Mathf.Abs(ballPos.y - position.y) < (ballSize.y / 2 + size.y / 2);

        return overlapX && overlapY;
    }

    public void SetDestroyCallback(System.Action _event)
    {
        onDestroyBrick = _event;
    }
}