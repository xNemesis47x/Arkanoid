using UnityEngine;

public class Brick
{
    private bool isPowerUp;

    public int Life { get; set; }
    public Vector2 Size { get; private set; }
    public Vector2 Position { get; private set; }
    public GameObject BrickObject { get; private set; }
    public System.Action OnDesactivateBrick { get; private set; }
    public bool IsActive { get; private set; }

    private BrickManager brickManager;

    private UpdateManager updateManager;

    private Renderer brickRenderer;
    private MaterialPropertyBlock block;
    private AdressableInstantiator currentAdressable;

    public void Initialize(Vector3 renderer, Transform transform, BrickManager currentBrickManager, UpdateManager currentUM, AdressableInstantiator adressable, bool containsPowerUp = false)
    {
        Life = Random.Range(1,5);
        IsActive = true;
        Size = renderer;
        Position = transform.position;
        BrickObject = transform.gameObject;
        isPowerUp = containsPowerUp;
        brickManager = currentBrickManager;
        updateManager = currentUM;
        currentAdressable = adressable;

        brickRenderer = BrickObject.GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        UpdateColorByLife();
    }

    public void DesactivateBrick()
    {
        if (isPowerUp)
        {
            SpawnPowerUp();
        }

        updateManager.LevelController.CountPoints += 10;
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
        GameObject powerUpPrefab = currentAdressable.GetInstance("PowerUp");
        GameObject newPowerUp = GameObject.Instantiate(powerUpPrefab, Position, Quaternion.identity);
        PowerUp powerUp = new PowerUp(newPowerUp.transform, new Vector2(0.5f, 0.5f), updateManager);
        powerUp.ActivePowerUps.Add(newPowerUp);

        powerUp.SetOnCollectedCallback(() => EventPowerUp(powerUp, newPowerUp));
    }

    private void EventPowerUp(PowerUp powerUp, GameObject newPowerUp)
    {
        powerUp.ActivePowerUps.Remove(newPowerUp);
        MultiBall multiBall = new MultiBall(updateManager.GetPaddle());
        multiBall.SpawnMultiBall();
    }

    public bool CheckCollision(Vector2 ballPos, Vector2 ballSize)
    {
        if (BrickObject.activeInHierarchy)
        {
            bool overlapX = Mathf.Abs(ballPos.x - Position.x) < (ballSize.x / 2 + Size.x / 2);
            bool overlapY = Mathf.Abs(ballPos.y - Position.y) < (ballSize.y / 2 + Size.y / 2);

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

    private void UpdateColorByLife()
    {
        if (brickRenderer == null) return;

        Color color;

        switch (Life)
        {
            case 4: 
                color = Color.magenta; 
                break;
            case 3:
                color = Color.green;
                break;
            case 2:
                color = Color.blue;
                break;
            case 1:
            default:
                color = Color.cyan;
                break;
        }

        brickRenderer.GetPropertyBlock(block);
        block.SetColor("_Color", color);
        brickRenderer.SetPropertyBlock(block);
    }

    public void UpdateColor()
    {
        UpdateColorByLife();
    }
}