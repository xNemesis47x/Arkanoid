using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public static BrickManager Instance;
    [SerializeField] private GameObject brickPrefab; // Asignalo en el inspector

    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 10;

    [SerializeField] private float brickWidth = 1.5f;
    [SerializeField] private float brickHeight = 0.7f;
    [SerializeField] private float spacing = 0.2f;

    [Header("Bricks")]
    private List<Brick> allBricks = new List<Brick>();

    public List<Brick> Bricks => allBricks;

    private void Awake()
    {
        Instance = this;
    }

  public void Initialize()
{
    InitializeBricks();
}

    public void RemoveBrick(Brick brick)
    {
        if (allBricks.Contains(brick))
        {
            allBricks.Remove(brick);
            Debug.Log("Ladrillo destruido");
        }
    }
    public void InitializeBricks()
    {
        int columns = 10;
        int rows = 5;
        float spacingX = 1.1f;
        float spacingY = 0.6f;

        Vector2 brickSize = brickPrefab.GetComponent<Renderer>().bounds.size;
        Vector2 startPosition = new Vector2(-((columns - 1) * spacingX) / 2f, 4f);

        allBricks = new List<Brick>();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 spawnPos = startPosition + new Vector2(x * spacingX, -y * spacingY);
                GameObject brickGO = Instantiate(brickPrefab, spawnPos, Quaternion.identity);
                Brick brick = brickGO.GetComponent<Brick>();
                brick.Initialize();
                allBricks.Add(brick);
            }
        }

        Debug.Log($"Bricks instanciados: {allBricks.Count}");
    }

}