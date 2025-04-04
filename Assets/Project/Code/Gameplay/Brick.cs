using UnityEngine;

public class Brick : MonoBehaviour
{
    private Vector2 size;
    private Vector2 position;

    public Vector2 Position => position;
    public Vector2 Size => size;

    public void Initialize()
    {
        size = GetComponent<Renderer>().bounds.size;
        position = transform.position;
    }

    public void DestroyBrick()
    {
        BrickManager.Instance.RemoveBrick(this);
        Destroy(gameObject);
    }
    public bool CheckCollision(Vector2 ballPos, Vector2 ballSize)
    {
        bool overlapX = Mathf.Abs(ballPos.x - position.x) < (ballSize.x / 2 + size.x / 2);
        bool overlapY = Mathf.Abs(ballPos.y - position.y) < (ballSize.y / 2 + size.y / 2);

        return overlapX && overlapY;
    }
}