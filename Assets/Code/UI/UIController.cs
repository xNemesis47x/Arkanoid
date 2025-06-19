public class UIController
{
    private UIModel model;
    private UIView view;

    public UIController(UIModel currentModel, UIView currentView)
    {
        model = currentModel;
        view = currentView;
    }

    public void SplashScreen()
    {
        view.ShowSplashScreen(true);
        view.ShowMenuScreen(false);
    }

    public void Menu()
    {
        view.ShowMenuScreen(true);
        view.ShowWinScreen(false);
        view.ShowDefinetelyWinScreen(false);
        view.ShowDefeatScreen(false);
        view.ShowPauseScreen(false);
        view.ShowSplashScreen(false);
    }

    public void Gameplay()
    {
        view.ShowMenuScreen(false);
        view.ShowWinScreen(false);
        view.ShowDefinetelyWinScreen(false);
        view.ShowDefeatScreen(false);
        view.ShowPauseScreen(false);
        model.UpdateLevels();
        model.UpdateLives();
        model.UpdatePoints();
        model.UpdatePaddleHits();
        model.UpdateBricksAmount();

        view.ShowLevels(model.CountLevels);
        view.ShowPoints(model.PointsPlayer);
        view.ShowLives(model.Lives);
        view.ShowPaddleHits(model.PaddleHits);
        view.ShowBricksAmount(model.bricksAmount);
    }

    public void AddPaddleHit()
    {
        model.UpdatePaddleHits();
        view.ShowPaddleHits(model.PaddleHits);
    }

    public void BricksAmount()
    {
        model.UpdateBricksAmount();
        view.ShowBricksAmount(model.bricksAmount);
    }

    public void AddPoints()
    {
        model.UpdatePoints();
        view.ShowPoints(model.PointsPlayer);
    }

    public void LoseLife()
    {
        model.UpdateLives();
        view.ShowLives(model.Lives);
    }

    public void PlayerDefeat()
    {
        view.ShowDefeatScreen(true);
    }

    public void PlayerWin()
    {
        view.ShowWinScreen(true);
    }

    public void PlayerDefintelyWin()
    {
        view.ShowDefinetelyWinScreen(true);
    }

    public void PlayerPause()
    {
        view.ShowPauseScreen(true);
    }
}

