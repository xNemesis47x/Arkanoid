public class UIModel
{
    private LevelController levelController;
    
    public int CountLevels { get; private set; }
    public int Lives { get; private set; }
    public int PointsPlayer { get; private set; }

    public UIModel(LevelController currentLevelController)
    {
        levelController = currentLevelController;
    }

    public void UpdatePoints()
    {
        PointsPlayer = levelController.CountPoints;
    }

    public void UpdateLevels()
    {
        CountLevels = levelController.CountLevels;
    }

    public void UpdateLives()
    {
        Lives = levelController.CurrentPaddle.Lives;
    }

}
