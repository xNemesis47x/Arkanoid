using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeGame()
    {
        Debug.Log("GameManager: InitializeGame llamado");
        // Pod�s poner l�gica para inicializar variables, vidas, score, etc.
    }
}