using UnityEngine;

public class Brick
{
    private Vector2 size;
    private Vector2 position;

    private GameObject brickGO;

    private System.Action onDestroyBrick;

    public System.Action OnDestroyBrick => onDestroyBrick;

    public void Initialize(Vector3 renderer, Transform transform)
    {
        size = renderer;
        position = transform.position;
        brickGO = transform.gameObject;
    }

    public void DestroyBrick()
    {
        BrickManager.Instance.RemoveBrick(this);
        SetDestroyCallback(() => GameObject.Destroy(brickGO));
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