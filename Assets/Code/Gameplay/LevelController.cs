using System.Collections.Generic;
using UnityEngine;

public class LevelController
{
    [Header("Referencias")]
    private GameObject paddlePrefab;
    private Transform paddleSpawnPoint;

    private AdressableInstantiator adressable;
    private UpdateManager updateManager;

    private BackgroundLogic background;

    public PaddleController CurrentPaddle { get; private set; }

    public int CountLevels { get; set; }
    public int CountPoints { get; set; }
    public List<ILevel> Levels { get; private set; } = new List<ILevel>(); 

    public void Start(UpdateManager currentUM, AdressableInstantiator currentAdress)
    {
        CountLevels = 1;
        CountPoints = 0;
        updateManager = currentUM;
        adressable = currentAdress;
        paddlePrefab = adressable.GetInstancePrefabs("paddlePrefab");
        paddleSpawnPoint = updateManager.PaddleSpawnPoint;
        background = new BackgroundLogic(adressable, updateManager.firstSpriteRenderer, updateManager.secondSpriteRenderer, updateManager.thirdSpriteRenderer);
        Initialize();
        InitializeLevels();
        BrickManager.Instance.Initialize(currentUM, currentAdress);
        currentUM.OnRestartGame += RestartLevel;
        currentUM.OnNextLevel += NextLevel;
    }

    public void Initialize()
    {
        if (paddlePrefab != null && paddleSpawnPoint != null && CurrentPaddle == null)
        {
            GameObject paddleGO = GameObject.Instantiate(paddlePrefab, paddleSpawnPoint.position, paddleSpawnPoint.rotation);
            Renderer paddleRenderer = paddleGO.GetComponent<Renderer>();

            CurrentPaddle = new PaddleController();
            CurrentPaddle.Initialize(paddleRenderer, paddleGO.transform, updateManager, adressable);

            CurrentPaddle.OnMovePaddle += background.ParallaxEffect;
        }
    }

    private void RestartLevel()
    {
        CountLevels = 1;
        CountPoints = 0;
        CurrentPaddle.Lives = 3;
        BrickManager.Instance.Initialize(updateManager, adressable);
    }

    private void InitializeLevels()
    {
        Levels.Add(new Level1());
        Levels.Add(new Level2());
        Levels.Add(new Level2());
        Levels.Add(new Level2());
        Levels.Add(new Level2());
        Levels.Add(new Level2());
        Levels.Add(new Level2());
        Levels.Add(new Level2());
        Levels.Add(new Level2());
        Levels.Add(new Level2());
    }


    public void NextLevel()
    {
        CountLevels++;
        int index = CountLevels - 2;
        
        if (index <= Levels.Count - 1)
        {
            adressable.LoadGroupLevels(CountLevels);
            BrickManager.Instance.InitializeBricks(updateManager, Levels[index].Layout());
        }
        else
        {
            Debug.LogWarning("No hay mas niveles pa");
        }
        
    }

    
}