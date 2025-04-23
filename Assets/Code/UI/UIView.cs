using TMPro;
using UnityEngine.UI;

public class UIView  
{
    Image menuScreen;
    Image winScreen;
    Image defeatScreen;
    Image pauseScreen;

    TMP_Text livesText;
    TMP_Text levelsText;
    TMP_Text pointsText;
    
    public UIView(TMP_Text lives, TMP_Text levels, TMP_Text points, Image menu, Image win, Image defeat, Image pause)
    {
        menuScreen = menu;
        winScreen = win;
        defeatScreen = defeat;
        pauseScreen = pause;
        menuScreen.gameObject.SetActive(true);
        livesText = lives;
        levelsText = levels;
        pointsText = points;
    }

    public void ShowLives(int lives)
    {
        livesText.text = $"Lives: {lives}";
    }

    public void ShowLevels(int level)
    {
        levelsText.text = $"Level: {level}";
    }

    public void ShowPoints(int points)
    {
        pointsText.text = $"Points: {points}";
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
