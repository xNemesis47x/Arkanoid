using UnityEngine;

public class UIController
{
    UIModel model;
    UIView view;

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
        view.ShowDefeatScreen(false);
        view.ShowPauseScreen(false);
        view.ShowSplashScreen(false);
    }

    public void Gameplay()
    {
        view.ShowMenuScreen(false);
        view.ShowWinScreen(false);
        view.ShowDefeatScreen(false);
        view.ShowPauseScreen(false);
        model.UpdateLevels();
        model.UpdateLives();
        model.UpdatePoints();

        view.ShowLevels(model.CountLevels);
        view.ShowPoints(model.PointsPlayer);
        view.ShowLives(model.Lives);
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

    public void PlayerPause()
    {
        view.ShowPauseScreen(true);
    }
}

