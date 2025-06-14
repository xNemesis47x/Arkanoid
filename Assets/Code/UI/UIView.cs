using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIView
{
    private Image menuScreen;
    private Image winScreen;
    private Image definetelyWinScreen;
    private Image defeatScreen;
    private Image pauseScreen;
    private VideoPlayer videoController;

    private TMP_Text livesText;
    private TMP_Text levelsText;
    private TMP_Text pointsText;

    public UIView(TMP_Text lives, TMP_Text levels, TMP_Text points, Image menu, Image win, Image definetelyWin, Image defeat, Image pause, VideoPlayer splashVideo)
    {
        menuScreen = menu;
        winScreen = win;
        definetelyWinScreen = definetelyWin;
        defeatScreen = defeat;
        pauseScreen = pause;
        videoController = splashVideo;
        livesText = lives;
        levelsText = levels;
        pointsText = points;
    }

    public void ShowLives(int lives)
    {
        livesText.text = $"{lives}";
    }

    public void ShowLevels(int level)
    {
        levelsText.text = $"Level: {level}";
    }

    public void ShowPoints(int points)
    {
        pointsText.text = $"Points: {points}";
    }

    public void ShowSplashScreen(bool isActive)
    {
        videoController.gameObject.SetActive(isActive);
        if (isActive)
        {
            videoController.Play();
        }
        else
        {
            videoController.Stop();
        }
    }

    public void ShowMenuScreen(bool isActive)
    {
        menuScreen.gameObject.SetActive(isActive);
    }

    public void ShowWinScreen(bool isActive)
    {
        winScreen.gameObject.SetActive(isActive);
    }

    public void ShowDefinetelyWinScreen(bool isActive)
    {
        definetelyWinScreen.gameObject.SetActive(isActive);
    }

    public void ShowDefeatScreen(bool isActive)
    {
        defeatScreen.gameObject.SetActive(isActive);
    }

    public void ShowPauseScreen(bool isActive)
    {
        pauseScreen.gameObject.SetActive(isActive);
    }

}
