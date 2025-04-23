using UnityEngine;

public class UIModel
{
    public int CountLevels { get; private set; }
    public int Lives { get; private set; }
    public int PointsPlayer { get; private set; }

    LevelController levelController;

    public UIModel(LevelController currentLevelController)
    {
        levelController = currentLevelController;
    }

    public void UpdatePoints()
    {
        PointsPlayer = levelController.GetPoints();
    }

    public void UpdateLevels()
    {
        CountLevels = levelController.GetLevels();
    }

    public void UpdateLives()
    {
        Lives = levelController.GetLives();
    }

}
