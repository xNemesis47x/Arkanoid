using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BrickManager
{
    public static BrickManager Instance = new BrickManager();

    public GameObject brickPrefab;
    private Transform brickContainer;

    public List<Brick> AllBricks { get; private set; } = new List<Brick>();
    public Queue<GameObject> InactiveBrick { get; private set; }
    public Queue<Brick> InactiveBricksLogic { get; private set; }

    private AdressableInstantiator currentAdressable;

    public Dictionary<int, GameObject> BrickPrefabsByLife { get; private set; } = new();

    public void Initialize(UpdateManager currentUM, AdressableInstantiator adressable)
    {
        InactiveBricksLogic = new Queue<Brick>();
        InactiveBrick = new Queue<GameObject>();
        brickContainer = currentUM.BrickContainer;
        currentAdressable = adressable;

        BrickPrefabsByLife[1] = adressable.GetInstancePrefabs("Brick_Life1");
        BrickPrefabsByLife[2] = adressable.GetInstancePrefabs("Brick_Life2");
        BrickPrefabsByLife[3] = adressable.GetInstancePrefabs("Brick_Life3");
        BrickPrefabsByLife[4] = adressable.GetInstancePrefabs("Brick_Life4");

        brickPrefab = BrickPrefabsByLife[4]; // usar el más fuerte para tamaño base

        List<Vector2Int> layout = GenerateRandomBrickPositions(20, 10, 5);
        InitializeBricks(currentUM, layout);
    }

    public void InitializeBricks(UpdateManager currentUM, List<Vector2Int> layout)
    {
        RecycleBricks();

        Vector2 brickSize = brickPrefab.GetComponent<Renderer>().bounds.size;
        float spacingX = brickSize.x + 0.1f; // le das 0.1 extra para que no se toquen
        float spacingY = brickSize.y + 0.1f;

        Vector2 startPosition = new Vector2(-(10 - 1) * spacingX / 2f, 4f);

        foreach (Vector2Int pos in layout)
        {
            Vector2 spawnPos = startPosition + new Vector2(pos.x * spacingX, -pos.y * spacingY);
            GameObject brickGO = GetBrick(spawnPos);

            Brick brick = GetLogic();
            bool containsPowerUp = Random.value < 0.1f;
            brick.Initialize(brickSize, brickGO.transform, this, currentUM, currentAdressable, containsPowerUp);
            AllBricks.Add(brick);
        }
    }

    List<Vector2Int> GenerateRandomBrickPositions(int count, int maxColumns, int maxRows)
    {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

        while (positions.Count < count)
        {
            int x = Random.Range(0, maxColumns);
            int y = Random.Range(0, maxRows);
            positions.Add(new Vector2Int(x, y));
        }

        return positions.ToList();
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