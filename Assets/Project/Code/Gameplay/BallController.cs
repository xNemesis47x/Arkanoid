using UnityEngine;

public class BallController : IUpdatable
{
    private float speed = 8f;
    private Vector3 direction;
    private bool isLaunched;
    private Vector3 size;
    private Vector3 pos;
    private PaddleController paddleOwner;
    private System.Action onDestroyBall;

    private Transform ballTransform;

    public bool IsLaunched => isLaunched;

    public void SetDestroyCallback(System.Action _event)
    {
        onDestroyBall = _event;
    }

    public void Initialize(PaddleController owner, Vector3 sizeFake, Transform transform)
    {
        paddleOwner = owner;
        isLaunched = false;
        direction = Vector3.zero;
        size = sizeFake;
        pos = paddleOwner.Position + Vector3.up * 0.5f;
        ballTransform = transform;

        ballTransform.position = pos;

        UpdateManager.Instance.Register(this);
    }

    //Lo suma a la lista de objetos a eliminar 
    public void Dispose()
    {
        UpdateManager.Instance.Unregister(this);
        Debug.Log("Pelota eliminada y desregistrada del UpdateManager.");
        onDestroyBall?.Invoke();
    }

    public void Launch()
    {
        float randomX = Random.Range(-0.5f, 0.5f);
        direction = new Vector2(randomX, 1f).normalized;
        isLaunched = true;
        Debug.Log($"Pelota lanzada con dirección {direction}");
    }

    public void CustomUpdate(float deltaTime)
    {
        if (!isLaunched)
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
        Vector2 paddlePosRebote = paddleOwner.Position;
        Vector2 paddleSizeRebote = paddleOwner.Size;

        bool isOverlappingX = Mathf.Abs(ballPos.x - paddlePosRebote.x) < (size.x / 2f + paddleSizeRebote.x / 2f);
        bool isOverlappingY = Mathf.Abs(ballPos.y - paddlePosRebote.y) < (size.y / 2f + paddleSizeRebote.y / 2f);

        if (isOverlappingX && isOverlappingY && direction.y < 0f)
        {
            direction.y *= -1f;

            // Modificar dirección X según impacto
            float offset = (ballPos.x - paddlePosRebote.x) / (paddleSizeRebote.x / 2f);
            direction.x = offset;
            direction = direction.normalized;

            Debug.Log("¡Pelota rebotó con la paleta!");
        }

        HandleScreenBounds();
        HandleBrickCollision();

        ballTransform.position = pos;
    }

    private void HandleScreenBounds()
    {
        Vector2 pos = this.pos;

        if (pos.x <= -10f || pos.x >= 10f)
        {
            direction.x *= -1;
        }

        if (pos.y >= 5.7f)
        {
            direction.y *= -1;
        }

        if (pos.y <= -5f)
        {
            Debug.Log("La pelota salió por abajo. Se destruye y se spawnea una nueva.");
            Dispose();
            paddleOwner.SpawnNewBall();
        }
    }
    private void HandleBrickCollision()
    {
        foreach (Brick brick in BrickManager.Instance.Bricks)
        {
            if (brick != null && brick.CheckCollision(pos, size))
            {
                direction.y *= -1;
                brick.DestroyBrick();
                brick.OnDestroyBrick?.Invoke();
                break;
            }
        }
    }
}