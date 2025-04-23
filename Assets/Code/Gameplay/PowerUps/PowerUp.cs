using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : IUpdatable
{
    private Transform transform;
    private Vector2 size;
    private float fallSpeed = 3f;
    private System.Action onCollected;
    private bool isCollected = false;

    UpdateManager updateManager;

    public List<GameObject> activePowerUps { get; private set; }

    public PowerUp(Transform powerUpTransform, Vector2 powerUpSize, UpdateManager currentUM)
    {
        activePowerUps = new List<GameObject>();
        transform = powerUpTransform;
        size = powerUpSize;
        currentUM.Register(this);
        updateManager = currentUM;

        updateManager.OnRestartGame += RestartPowerUps;
    }

    public void CustomUpdate(float deltaTime)
    {
        if (isCollected || transform == null) return;

        transform.position += Vector3.down * fallSpeed * deltaTime;

        PaddleController paddle = updateManager.GetPaddle(); 
        if (paddle != null)
        {
            if (CheckCollision(transform.position, size, paddle.Position, paddle.Size))
            {
                isCollected = true;
                onCollected?.Invoke();
                GameObject.Destroy(transform.gameObject);
                transform = null;
                updateManager.Unregister(this);
            }
            else if(transform.position.y < -5f)
            {
                GameObject.Destroy(transform.gameObject);
                transform = null;
                updateManager.Unregister(this);
            }
        }
    }

    private bool CheckCollision(Vector2 posA, Vector2 sizeA, Vector2 posB, Vector2 sizeB)
    {
        bool overlapX = Mathf.Abs(posA.x - posB.x) < (sizeA.x / 2 + sizeB.x / 2);
        bool overlapY = Mathf.Abs(posA.y - posB.y) < (sizeA.y / 2 + sizeB.y / 2);

        return overlapX && overlapY;
    }

    public void SetOnCollectedCallback(System.Action callback)
    {
        onCollected = callback;
    }

    private void RestartPowerUps()
    {
        foreach (GameObject powerUp in activePowerUps)
        {
            GameObject.Destroy(powerUp);
        }
    }
}
