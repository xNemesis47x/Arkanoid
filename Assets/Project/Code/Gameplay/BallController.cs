using UnityEngine;

public class BallController : MonoBehaviour, IUpdatable
{
    [SerializeField] private float speed = 8f;
    private Vector2 direction;
    private bool isLaunched;
    private Vector2 size;
    private PaddleController paddleOwner;
    private Transform ballSpawnPoint;

    public void Initialize(PaddleController owner)
    {
        paddleOwner = owner;
        isLaunched = false;
        direction = Vector2.zero;
        size = GetComponent<Renderer>().bounds.size;

        //Esto asegura que la pelota empiece en el lugar correcto
        transform.position = paddleOwner.transform.position + Vector3.up * 0.5f;

        UpdateManager.Instance.Register(this);
    }

    public void Dispose()
    {
        UpdateManager.Instance.Unregister(this);
        Debug.Log("Pelota eliminada y desregistrada del UpdateManager.");
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
            Vector2 paddlePos = paddleOwner.transform.position;
            Vector2 paddleSize = paddleOwner.Size;

            transform.position = new Vector2(paddlePos.x, paddlePos.y + (paddleSize.y / 2f) + (size.y / 2f));
            return;
        }

        // Movimiento de la pelota si ya fue lanzada
        Vector3 movement = (Vector3)(direction * speed * deltaTime);
        transform.position += movement;

        // Colisión con la paleta (rebote)
        Vector2 ballPos = transform.position;
        Vector2 paddlePosRebote = paddleOwner.transform.position;
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
    }
    //public void CustomUpdate(float deltaTime)
    //{
    //    if (!isLaunched)
    //    {
    //        Debug.Log("Esperando a que se lance la pelota...");
    //        return;
    //    }

    //    transform.position += (Vector3)(direction * speed * deltaTime);
    //    Debug.Log("Pelota moviéndose. Posición actual: " + transform.position);

    //    HandleScreenBounds();
    //}

    private void HandleScreenBounds()
    {
        Vector2 pos = transform.position;

        if (pos.x <= -10f || pos.x >= 10f)
        {
            direction.x *= -1;
            Debug.Log("Rebote horizontal. Nueva dirección: " + direction);
        }

        if (pos.y >= 5.7f)
        {
            direction.y *= -1;
            Debug.Log("Rebote superior. Nueva dirección: " + direction);
        }

        if (pos.y <= -5f)
        {
            Debug.Log("La pelota salió por abajo. Se destruye y se spawnea una nueva.");
            Dispose(); // Primero me desregistro
            Destroy(gameObject);
            paddleOwner.SpawnNewBall();
        }
    }
    private void HandleBrickCollision()
    {
        foreach (var brick in BrickManager.Instance.Bricks)
        {
            if (brick != null && brick.CheckCollision(transform.position, size))
            {
                direction.y *= -1;
                brick.DestroyBrick();
                break;
            }
        }
    }
}