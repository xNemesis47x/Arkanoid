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

    public int CountLevels { get; set; } = 0;
    public int CountPoints { get; set; } = 0;
    public List<ILevel> Levels { get; private set; } = new List<ILevel>();

    public void Start(UpdateManager currentUM, AdressableInstantiator currentAdress)
    {
        updateManager = currentUM;
        adressable = currentAdress;
        paddleSpawnPoint = updateManager.PaddleSpawnPoint;
        background = new BackgroundLogic(adressable, updateManager.firstSpriteRenderer, updateManager.secondSpriteRenderer, updateManager.thirdSpriteRenderer);
        Initialize();
        BrickManager.Instance.Initialize(currentUM, currentAdress);
        currentUM.OnRestartGame -= RestartLevel;
        currentUM.OnRestartGame += RestartLevel;
        currentUM.OnNextLevel -= NextLevel;
        currentUM.OnNextLevel += NextLevel;
    }

    public void Initialize()
    {
        paddlePrefab = adressable.GetInstancePrefabs("paddlePrefab");
        if (paddlePrefab != null && paddleSpawnPoint != null)
        {
            if (CurrentPaddle == null)
            {
                CurrentPaddle = new PaddleController();
                CurrentPaddle.OnMovePaddle += background.ParallaxEffect;
            }
            else
                CurrentPaddle.DestroyReference();

            GameObject paddleGO = GameObject.Instantiate(paddlePrefab, paddleSpawnPoint.position, paddleSpawnPoint.rotation);
            Renderer paddleRenderer = paddleGO.GetComponent<Renderer>();

            CurrentPaddle.Initialize(paddleRenderer, paddleGO.transform, updateManager, adressable);
        }
    }

    private void RestartLevel()
    {
        CountLevels = 1;
        CountPoints = 0;
        CurrentPaddle.Lives = 3;
        adressable.LoadGroupLevels(CountLevels);
        BrickManager.Instance.Initialize(updateManager, adressable);
    }

    public void InitializeLevels()
    {
        Levels.Add(new Level1());
        Levels.Add(new Level2());
        Levels.Add(new Level3());
        Levels.Add(new Level4());
        Levels.Add(new Level6());
        Levels.Add(new Level7());
        Levels.Add(new Level8());
        Levels.Add(new Level5());
        Levels.Add(new Level2());
        Levels.Add(new Level8());

    }


    public void NextLevel()
    {
        CountLevels++;

        if (CountLevels <= Levels.Count - 1)
        {
            adressable.LoadGroupLevels(CountLevels);
            BrickManager.Instance.InitializeBricks(updateManager, Levels[CountLevels].Layout());
        }
        else
        {
            Debug.LogWarning("No hay mas niveles pa");
        }

    }


}