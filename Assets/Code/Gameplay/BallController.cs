using UnityEngine;

public class BallController : IUpdatable
{
    private float speed = 8f;
    private Vector3 direction;
    private Vector3 size;
    private Vector3 pos;
    private PaddleController paddleOwner;
    private Transform ballTransform;

    public event System.Action onDestroyBall;
    public bool IsLaunched { get; private set; }

    public void SetDestroyCallback(System.Action _event)
    {
        onDestroyBall = _event;
    }

    public void Initialize(PaddleController owner, Vector3 sizeFake, Transform transform)
    {
        paddleOwner = owner;
        IsLaunched = false;
        direction = Vector3.zero;
        size = sizeFake;
        pos = paddleOwner.Position + Vector3.up * 0.5f;
        ballTransform = transform;

        ballTransform.position = pos;

        paddleOwner.UpdateManager.Register(this);
    }

    //Lo suma a la lista de objetos a eliminar 
    public void Dispose()
    {
        paddleOwner.UpdateManager.Unregister(this);
        onDestroyBall?.Invoke();
    }

    public void Launch()
    {
        float randomX = Random.Range(-0.5f, 0.5f);
        direction = new Vector2(randomX, 1f).normalized;
        IsLaunched = true;
    }

    public void CustomUpdate(float deltaTime)
    {
        if (!IsLaunched && paddleOwner.ActiveBalls.Count <= 1)
        {
            // Seguir la posición de la paleta (centrado en X, justo arriba en Y)
            Vector2 paddlePos = paddleOwner.Position;
            Vector2 paddleSize = paddleOwner.Size;

            pos = new Vector2(paddlePos.x, paddlePos.y + (paddleSize.y / 2f) + (size.y / 2f));
            ballTransform.position = pos;
            return;
        }

        // Movimiento de la pelota si ya fue lanzada
        Vector3 movement = (Vector3)(direction * (speed * deltaTime));
        pos += movement;

        // Colisión con la paleta (rebote)
        Vector2 ballPos = pos;
        Vector2 paddlePosRebound = paddleOwner.Position;
        Vector2 paddleSizeRebound = paddleOwner.Size;

        bool isOverlappingX = Mathf.Abs(ballPos.x - paddlePosRebound.x) < (size.x / 2f + paddleSizeRebound.x / 2f);
        bool isOverlappingY = Mathf.Abs(ballPos.y - paddlePosRebound.y) < (size.y / 2f + paddleSizeRebound.y / 2f);

        if (isOverlappingX && isOverlappingY && direction.y < 0f)
        {
            Vector2 paddlePos = paddleOwner.Position;
            Vector2 paddleSize = paddleOwner.Size;

            float overlapX = (paddleSize.x / 2f + size.x / 2f) - Mathf.Abs(pos.x - paddlePos.x);
            float overlapY = (paddleSize.y / 2f + size.y / 2f) - Mathf.Abs(pos.y - paddlePos.y);

            if (overlapX < overlapY)
                direction.x *= -1;
            else
            {
                direction.y *= -1f;

                // Modificar dirección X según impacto
                float offset = (ballPos.x - paddlePosRebound.x) / (paddleSizeRebound.x / 2f);
                direction.x = offset;
                direction = direction.normalized;
            }
        }

        HandleScreenBounds();
        HandleBrickCollision();

        ballTransform.position = pos;
    }

    private void HandleScreenBounds()
    {
        Vector2 pos = this.pos;

        if (pos.x <= -9.7f || pos.x >= 9.7f)
        {
            direction.x *= -1;
        }

        if (pos.y >= 5f)
        {
            direction.y *= -1;
        }

        if (pos.y <= -5f && paddleOwner.ActiveBalls.Count <= 1)
        {
            this.IsLaunched = false;
            paddleOwner.Lives--;
            UIManager.Instance.LoseLife();
            paddleOwner.CheckDefeat();
        }
        else if (pos.y <= -5f && paddleOwner.ActiveBalls.Count > 1)
        {
            Dispose();
        }
    }

    private void HandleBrickCollision()
    {
        foreach (Brick brick in BrickManager.Instance.AllBricks)
        {
            if (brick != null && brick.CheckCollision(pos, size))
            {
                Vector2 brickPos = brick.Position;
                Vector2 brickSize = brick.Size;

                float overlapX = (brickSize.x / 2f + size.x / 2f) - Mathf.Abs(pos.x - brickPos.x);
                float overlapY = (brickSize.y / 2f + size.y / 2f) - Mathf.Abs(pos.y - brickPos.y);

                if (overlapX < overlapY)
                {
                    direction.x *= -1;
                }
                else
                {
                    direction.y *= -1;
                }

                brick.Life--;
                brick.UpdateColor();

                if (brick.Life == 0)
                {
                    brick.DesactivateBrick();
                    brick.OnDesactivateBrick?.Invoke();
                    UIManager.Instance.AddPoints();
                    brick.CheckWin();
                }

                break;
            }
        }
    }
}