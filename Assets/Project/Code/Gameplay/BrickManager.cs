using System.Collections.Generic;
using UnityEngine;

public class BrickManager 
{
    public static BrickManager Instance = new ();
    [SerializeField] private GameObject brickPrefab; // Asignalo en el inspector

    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 10;

    [SerializeField] private float brickWidth = 1.5f;
    [SerializeField] private float brickHeight = 0.7f;
    [SerializeField] private float spacing = 0.2f;

    [Header("Bricks")]
    private List<Brick> allBricks = new List<Brick>();

    public List<Brick> Bricks => allBricks;

    public void Initialize()
    {
        brickPrefab = UpdateManager.Instance.brickPrefab;
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

        Vector2 brickSize = brickPrefab.GetComponent<Renderer>().bounds.size;
        float spacingX = brickSize.x + 0.1f; // le das 0.1 extra para que no se toquen
        float spacingY = brickSize.y + 0.1f;

        Vector2 startPosition = new(-((columns - 1) * spacingX) / 2f, 4f);

        allBricks = new List<Brick>();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 spawnPos = startPosition + new Vector2(x * spacingX, -y * spacingY);
                GameObject brickGO = GameObject.Instantiate(brickPrefab, spawnPos, Quaternion.identity);
                Brick brick = new ();
                brick.Initialize(brickSize, brickGO.transform);
                allBricks.Add(brick);
            }
        }

        Debug.Log($"Bricks instanciados: {allBricks.Count}");
    }

}