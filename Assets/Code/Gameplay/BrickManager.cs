using System.Collections.Generic;
using UnityEngine;

public class BrickManager 
{
    public static BrickManager Instance = new BrickManager();

    private GameObject brickPrefab; 
    private Transform brickContainer;

    [Header("Bricks")]
    private List<Brick> allBricks = new List<Brick>();
    public List<Brick> Bricks => allBricks;

    public Queue<GameObject> InactiveBrick { get; private set; }

    public void Initialize(UpdateManager currentUM)
    {
        InactiveBrick = new Queue<GameObject>();
        brickContainer = currentUM.brickContainer;
        brickPrefab = currentUM.brickPrefab;
        InitializeBricks(currentUM);
    }

    public void RemoveBrick(Brick brick)
    {
        if (allBricks.Contains(brick))
        {
            allBricks.Remove(brick);
        }
    }

    public void InitializeBricks(UpdateManager currentUM)
    {
        int columns = 10;
        int rows = 5;

        Vector2 brickSize = brickPrefab.GetComponent<Renderer>().bounds.size;
        float spacingX = brickSize.x + 0.1f; // le das 0.1 extra para que no se toquen
        float spacingY = brickSize.y + 0.1f;

        Vector2 startPosition = new Vector2(-((columns - 1) * spacingX) / 2f, 4f);

        allBricks = new List<Brick>();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 spawnPos = startPosition + new Vector2(x * spacingX, -y * spacingY);
                GameObject brickGO = GetBrick(spawnPos);

                Brick brick = new Brick();
                bool containsPowerUp = Random.value < 0.1f; // 10% chance
                brick.Initialize(brickSize, brickGO.transform, this, currentUM, containsPowerUp);
                allBricks.Add(brick);
            }
        }

        Debug.Log($"Bricks instanciados: {allBricks.Count}");
    }

    public GameObject GetBrick(Vector2 spawnPos)
    {
        if(InactiveBrick.Count > 0)
        {
            foreach (GameObject brick in InactiveBrick)
            {
                if (!brick.activeInHierarchy)
                {
                    brick.SetActive(true);
                    return brick;
                }
            }
        }

        return GameObject.Instantiate(brickPrefab, spawnPos, Quaternion.identity, brickContainer);
    }

}