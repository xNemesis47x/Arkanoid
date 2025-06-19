public class UIModel
{
    private LevelController levelController;
    
    public int CountLevels { get; private set; }
    public int Lives { get; private set; }
    public int PointsPlayer { get; private set; }
    public int PaddleHits { get; private set; }
    public int bricksAmount { get; private set; }

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

    public void UpdatePaddleHits()
    {
        PaddleHits = levelController.CurrentPaddle.PaddleHits;
    }

    public void UpdateBricksAmount()
    {
        this.bricksAmount = levelController.BrickManager.GetActiveBricks();
    }

}
