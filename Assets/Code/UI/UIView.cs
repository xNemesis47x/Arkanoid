using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIView
{
    Image menuScreen;
    Image winScreen;
    Image defeatScreen;
    Image pauseScreen;
    RawImage splashScreen;
    VideoPlayer videoController;

    TMP_Text livesText;
    TMP_Text levelsText;
    TMP_Text pointsText;

    public UIView(TMP_Text lives, TMP_Text levels, TMP_Text points, Image menu, Image win, Image defeat, Image pause, VideoPlayer splashVideo)
    {
        menuScreen = menu;
        winScreen = win;
        defeatScreen = defeat;
        pauseScreen = pause;
        videoController = splashVideo;
        livesText = lives;
        levelsText = levels;
        pointsText = points;
        //this.splashScreen = splashScreen;
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

    public void ShowDefeatScreen(bool isActive)
    {
        defeatScreen.gameObject.SetActive(isActive);
    }

    public void ShowPauseScreen(bool isActive)
    {
        pauseScreen.gameObject.SetActive(isActive);
    }

}
