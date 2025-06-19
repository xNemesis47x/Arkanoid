using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    [Header("Imagenes")]
    [SerializeField] private Image menuScreen;
    [SerializeField] private Image winScreen;
    [SerializeField] private Image definetelyWinScreen;
    [SerializeField] private Image defeatScreen;
    [SerializeField] private Image pauseScreen;
    [SerializeField] private VideoPlayer videoController;
    [SerializeField] private Animator lifeAnimator;


    [Header("Textos")]
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text levelsText;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text paddleHitsText;
    [SerializeField] private TMP_Text bricksAmountText;

    [SerializeField] private UpdateManager updateManager;
    private UIModel currentModel;
    private UIView currentView;
    private UIController currentController;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentModel = new UIModel(updateManager.LevelController);
        currentView = new UIView(livesText, levelsText, pointsText, paddleHitsText, bricksAmountText, menuScreen, winScreen, definetelyWinScreen, defeatScreen, pauseScreen, videoController, lifeAnimator);

        currentController = new UIController(currentModel, currentView);
    }

    public void Menu()
    {
        currentController.Menu();
    }

    public void SplashScreen()
    {
        currentController.SplashScreen();
        StartCoroutine(SplashCoroutine());
    }

    public void Game()
    {
        currentController.Gameplay();
    }

    public void ShowWin()
    {
        currentController.PlayerWin();
    }

    public void ShowDefinetelyWin()
    {
        currentController.PlayerDefintelyWin();
    }

    public void ShowDefeat()
    {
        currentController.PlayerDefeat();
    }

    public void ShowPause()
    {
        currentController.PlayerPause();
        updateManager.pauseAnimation.Play("PauseIn");
    }

    public void CountPaddleHits()
    {
        currentController.AddPaddleHit();
    }

    public void BricksAmount()
    {
        currentController.BricksAmount();
    }

    public void LoseLife()
    {
        currentController.LoseLife();
    }

    public void AddPoints()
    {
        currentController.AddPoints();
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator SplashCoroutine()
    {
        float timeout = 1f;
        float elapsed = 0f;
        while (!videoController.isPlaying && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        while (videoController.isPlaying)
        {
            yield return null;
        }

        currentController.Menu();
    }
}
