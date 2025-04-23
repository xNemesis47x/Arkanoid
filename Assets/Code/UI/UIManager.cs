using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Imagenes")]
    [SerializeField] Image menuScreen;
    [SerializeField] Image winScreen;
    [SerializeField] Image defeatScreen;
    [SerializeField] Image pauseScreen;

    [Header("Textos")]
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text levelsText;
    [SerializeField] TMP_Text pointsText;

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
            // Setup como antes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentModel = new UIModel(updateManager.LevelController);
        currentView = new UIView(livesText, levelsText, pointsText, menuScreen, winScreen, defeatScreen, pauseScreen);

        currentController = new UIController(currentModel, currentView);
    }

    public void Menu()
    {
        currentController.Menu();
    }

    public void Game()
    {
        currentController.Gameplay();
    }

    public void ShowWin()
    {
        currentController.PlayerWin();
    }

    public void ShowDefeat()
    {
        currentController.PlayerDefeat();
    }

    public void ShowPause()
    {
        currentController.PlayerPause();
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
}
