using UnityEngine;

public class Brick
{
    private Vector2 size;
    private Vector2 position;

    public Vector2 Size => size;
    public Vector2 Position => position;

    public GameObject BrickObject { get; private set; }
    public System.Action OnDesactivateBrick { get; private set; }

    bool isPowerUp;
    public bool IsActive { get; private set; }

    BrickManager brickManager;

    UpdateManager updateManager;

    public void Initialize(Vector3 renderer, Transform transform, BrickManager currentBrickManager, UpdateManager currentUM, bool containsPowerUp = false)
    {
        IsActive = true;
        size = renderer;
        position = transform.position;
        BrickObject = transform.gameObject;
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

        IsActive = false;
        BrickObject.SetActive(false);
        brickManager.InactiveBrick.Enqueue(BrickObject);
        brickManager.InactiveBricksLogic.Enqueue(this);
        CheckWin();
    }

    public void Disable()
    {
        if (BrickObject != null)
            BrickObject.SetActive(false);
    }

    private void SpawnPowerUp()
    {
        GameObject powerUpPrefab = updateManager.powerUpPrefab;
        GameObject newPowerUp = GameObject.Instantiate(powerUpPrefab, position, Quaternion.identity);
        PowerUp powerUp = new PowerUp(newPowerUp.transform, new Vector2(0.5f, 0.5f), updateManager);
        powerUp.activePowerUps.Add(newPowerUp);

        powerUp.SetOnCollectedCallback(() =>
        {
            powerUp.activePowerUps.Remove(newPowerUp);
            MultiBall multiBall = new MultiBall(updateManager.GetPaddle());
            multiBall.SpawnMultiBall();
        });
    }

    public bool CheckCollision(Vector2 ballPos, Vector2 ballSize)
    {
        if (BrickObject.activeInHierarchy)
        {
            bool overlapX = Mathf.Abs(ballPos.x - position.x) < (ballSize.x / 2 + size.x / 2);
            bool overlapY = Mathf.Abs(ballPos.y - position.y) < (ballSize.y / 2 + size.y / 2);

            return overlapX && overlapY;
        }

        return false;
    }

    public void CheckWin()
    {
        if (brickManager.GetActiveBricks() == 0)
        {
            UIManager.Instance.ShowWin();
            updateManager.PauseGame();
        }
    }
}