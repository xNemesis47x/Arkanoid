using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private static UpdateManager instance;
    public static UpdateManager Instance => instance;

    private readonly List<IUpdatable> updatables = new List<IUpdatable>();
    private List<IUpdatable> toAdd = new List<IUpdatable>();
    private List<IUpdatable> toRemove = new List<IUpdatable>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // persiste entre escenas si querés

        Debug.Log("UpdateManager inicializado");
    }

    // Este método reemplaza el Update global

    private void LateUpdate()
    {
        float deltaTime = Time.deltaTime;

        // Agregar nuevos
        if (toAdd.Count > 0)
        {
            foreach (var updatable in toAdd)
            {
                if (updatable != null && !updatables.Contains(updatable))
                    updatables.Add(updatable);
            }
            toAdd.Clear();
        }

        // Ejecutar CustomUpdate y remover los null automáticamente
        for (int i = updatables.Count - 1; i >= 0; i--)
        {
            var obj = updatables[i];
            if (obj == null)
            {
                updatables.RemoveAt(i); // remover si está destruido
            }
            else
            {
                obj.CustomUpdate(deltaTime);
            }
        }

        // Limpiar los que se pidieron quitar explícitamente
        if (toRemove.Count > 0)
        {
            foreach (var obj in toRemove)
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
}
