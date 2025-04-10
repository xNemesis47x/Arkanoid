using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform ballSpawnPoint;

    public GameObject paddlePrefab;
    public Transform paddleSpawnPoint;

    public GameObject brickPrefab;

    LevelController levelController = new LevelController();

    private static UpdateManager instance;
    public static UpdateManager Instance => instance;

    private readonly List<IUpdatable> updatables = new List<IUpdatable>();
    private List<IUpdatable> toAdd = new List<IUpdatable>();
    private List<IUpdatable> toRemove = new List<IUpdatable>();

    private void Awake()
    {
        //Se crea el manager
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // persiste entre escenas si quer�s

        Debug.Log("UpdateManager inicializado");
    }

    private void Start()
    {
        levelController.Start();
    }

    // Este m�todo reemplaza el Update global
    private void LateUpdate()
    {

        float deltaTime = Time.deltaTime;

        // Agregar nuevos
        if (toAdd.Count > 0)
        {
            foreach (IUpdatable updatable in toAdd)
            {
                if (updatable != null && !updatables.Contains(updatable))
                    updatables.Add(updatable);
            }
            toAdd.Clear();
        }

        // Ejecutar CustomUpdate y remover los null autom�ticamente
        for (int i = updatables.Count - 1; i >= 0; i--)
        {
            IUpdatable obj = updatables[i];
            if (obj == null)
            {
                updatables.RemoveAt(i); // remover si est� destruido
            }
            else
            {
                obj.CustomUpdate(deltaTime);
            }
        }

        // Limpiar los que se pidieron quitar expl�citamente
        if (toRemove.Count > 0)
        {
            foreach (IUpdatable obj in toRemove)
            {
                updatables.Remove(obj);
            }
            toRemove.Clear();
        }
    }

    // Registrar un objeto que implementa IUpdatable
    public void Register(IUpdatable updatable)
    {
        if (!toAdd.Contains(updatable))
        {
            toAdd.Add(updatable);
        }
    }

    // Quitar un objeto
    public void Unregister(IUpdatable updatable)
    {
        if (!toRemove.Contains(updatable))
        {
            toRemove.Add(updatable);
        }
    }

    public void ClearAll()
    {
        updatables.Clear();
        toAdd.Clear();
        toRemove.Clear();
    }

    public void DestoyThis(Object obj)
    {
        Destroy(obj);
    }
}
