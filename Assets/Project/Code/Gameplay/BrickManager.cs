using System.Collections.Generic;
using UnityEngine;

public class BrickManager 
{
    public static BrickManager Instance = new ();
    private GameObject brickPrefab; 
    private GameObject brickContainer; 

    [Header("Bricks")]
    private List<Brick> allBricks = new List<Brick>();
    public List<Brick> Bricks => allBricks;

    public void Initialize()
    {
        brickContainer = UpdateManager.Instance.brickContainer;
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
                GameObject brickGO = GameObject.Instantiate(brickPrefab, spawnPos, Quaternion.identity, brickContainer.transform);
                Brick brick = new ();
                bool containsPowerUp = Random.value < 0.1f; // 10% chance
                brick.Initialize(brickSize, brickGO.transform, containsPowerUp);
                allBricks.Add(brick);
            }
        }

        Debug.Log($"Bricks instanciados: {allBricks.Count}");
    }

}