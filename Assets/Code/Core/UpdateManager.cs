using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform ballContainer;

    public GameObject paddlePrefab;
    public Transform paddleSpawnPoint;

    public GameObject brickPrefab;
    public Transform brickContainer;

    public GameObject powerUpPrefab;

    LevelController levelController = new LevelController();

    private UpdateManager Instance;

    private List<IUpdatable> updatables = new List<IUpdatable>();

    private void Awake()
    {
        //Se crea el manager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // persiste entre escenas si querés

        Debug.Log("UpdateManager inicializado");
    }

    private void Start()
    {
        levelController.Start(paddlePrefab, paddleSpawnPoint, Instance);
    }

    // Este método reemplaza el Update global
    private void Update()
    {
        float deltaTime = Time.deltaTime;

        // Ejecutar CustomUpdate y remover los null automáticamente
        for (int i = updatables.Count - 1; i >= 0; i--)
        {
            IUpdatable obj = updatables[i];
            if (obj == null)
            {
                updatables.RemoveAt(i); // remover si está destruido
            }
            else
            {
                obj.CustomUpdate(deltaTime);
            }
        }

//no darle bola a esto
# if DEBUG_ON
        //Debug.Log("");
#endif

    }

    // Registrar un objeto que implementa IUpdatable
    public void Register(IUpdatable updatable)
    {
        if (!updatables.Contains(updatable) && updatable != null)
        {
            updatables.Add(updatable);
        }
    }

    // Quitar un objeto
    public void Unregister(IUpdatable updatable)
    {
        if (updatables.Contains(updatable) && updatable != null)
        {
            updatables.Remove(updatable);
        }
    }

    public PaddleController GetPaddle()
    {
        return levelController.CurrentPaddle;
    }
}
