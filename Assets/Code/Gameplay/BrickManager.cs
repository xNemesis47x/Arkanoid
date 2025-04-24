using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickManager
{
    public static BrickManager Instance = new BrickManager();

    private GameObject brickPrefab;
    private Transform brickContainer;

    public List<Brick> AllBricks { get; private set; } = new List<Brick>();
    public Queue<GameObject> InactiveBrick { get; private set; }
    public Queue<Brick> InactiveBricksLogic { get; private set; }


    public void Initialize(UpdateManager currentUM)
    {
        InactiveBricksLogic = new Queue<Brick>();
        InactiveBrick = new Queue<GameObject>();
        brickContainer = currentUM.BrickContainer;
        brickPrefab = currentUM.BrickPrefab;
        InitializeBricks(currentUM);
    }

    public void InitializeBricks(UpdateManager currentUM)
    {
        RecycleBricks();

        int columns = 10;
        int rows = 5;

        Vector2 brickSize = brickPrefab.GetComponent<Renderer>().bounds.size;
        float spacingX = brickSize.x + 0.1f; // le das 0.1 extra para que no se toquen
        float spacingY = brickSize.y + 0.1f;

        Vector2 startPosition = new Vector2(-((columns - 1) * spacingX) / 2f, 4f);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 spawnPos = startPosition + new Vector2(x * spacingX, -y * spacingY);
                GameObject brickGO = GetBrick(spawnPos);

                Brick brick = GetLogic();
                bool containsPowerUp = Random.value < 0.1f; // 10% chance
                brick.Initialize(brickSize, brickGO.transform, this, currentUM, containsPowerUp);
                AllBricks.Add(brick);
            }
        }
    }

    private GameObject GetBrick(Vector2 spawnPos)
    {
        if (InactiveBrick.Count > 0)
        {
            GameObject brick = InactiveBrick.Dequeue();
            brick.transform.position = spawnPos;
            brick.SetActive(true);
            return brick;
        }

        return GameObject.Instantiate(brickPrefab, spawnPos, Quaternion.identity, brickContainer);
    }

    private void RecycleBricks()
    {
        foreach (Brick brick in AllBricks)
        {
            brick.Disable();
            InactiveBrick.Enqueue(brick.BrickObject);
            InactiveBricksLogic.Enqueue(brick); 
        }

        AllBricks.Clear();
    }

    private Brick GetLogic()
    {
        if (InactiveBricksLogic.Count > 0)
        {
            return InactiveBricksLogic.Dequeue();
        }
        else
        {
            return new Brick();
        }
    }

    public int GetActiveBricks()
    {
        return AllBricks.Count(brick => brick.IsActive);
    }
}