using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UIElements;

[Serializable]
public class AssetsGroupData
{
    public string name = "";
    public List<AssetReferenceGameObject> prefabs = new();
    public List<AssetReference> background = new();
}

public class AdressableInstantiator
{
    [SerializeField]
    private List<AssetsGroupData> assetReferences = new List<AssetsGroupData>();
    public Dictionary<string, GameObject> loadedAssets;
    public Dictionary<string, Sprite> loadedBackground;
    private int currentGroupIndex;
    private List<AsyncOperationHandle> loadedHandles = new List<AsyncOperationHandle>();
    public bool useRemoteAssets = true;
    public String localURL = "http://localhost:3000/";
    public String cloudURL = "https://myserver.com/";

    UpdateManager updateManager;
    LevelController level;

    public void Initialize(List<AssetsGroupData> adressablessList, UpdateManager up, LevelController level)
    {
        assetReferences = adressablessList;

        if (useRemoteAssets)
        {
            Addressables.ResourceManager.InternalIdTransformFunc +=
            ChangeAssetUrlToPrivateServer;
        }
        loadedAssets = new Dictionary<string, GameObject>();
        loadedBackground = new Dictionary<string, Sprite>();
        updateManager = up;
        this.level = level;
        currentGroupIndex = -1;
        LoadGroupLevels(level.CountLevels);
    }

    protected string ChangeAssetUrlToPrivateServer(IResourceLocation location)
    {
        String assetURL = location.InternalId;
        if (location.InternalId.IndexOf(localURL) != -1)
        {
            assetURL = location.InternalId.Replace(localURL, cloudURL);
        }

        return location.InternalId;
    }

    public void LoadGroupLevels(int level)
    {
        int groupIndex = (level - 1) / 5; // Ejemplo: niveles 1-5 grupo 0, 6-10 grupo 1, etc.

        if (groupIndex == currentGroupIndex)
        {
            return;
        }

        UnloadAssets();
        currentGroupIndex = groupIndex;

        updateManager.CoRoutineStart(LoadAssetsCoroutine(groupIndex));
    }

    private IEnumerator LoadAssetsCoroutine(int groupIndex)
    {
        loadedHandles.Clear();
        loadedAssets.Clear();
        loadedBackground.Clear();

        int assetsToLoad = assetReferences[groupIndex].prefabs.Count + assetReferences[groupIndex].background.Count;
        int assetsLoaded = 0;

        foreach (AssetReference assetReference in assetReferences[groupIndex].prefabs)
        {
            AsyncOperationHandle<GameObject> handle = assetReference.LoadAssetAsync<GameObject>();
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                loadedHandles.Add(handle);
                string assetName = handle.Result.name.Split(' ')[0];
                loadedAssets.Add(assetName, handle.Result);
                assetsLoaded++;
            }
        }

        foreach (AssetReference assetReference in assetReferences[groupIndex].background)
        {
            AsyncOperationHandle<Sprite> handle = assetReference.LoadAssetAsync<Sprite>();
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                loadedHandles.Add(handle);
                string assetName = handle.Result.name.Split(' ')[0];
                loadedBackground.Add(assetName, handle.Result);
                assetsLoaded++;
            }
        }

        if (assetsLoaded == assetsToLoad)
        {
            Debug.Log("Assets cargados: " + assetsLoaded);
            Time.timeScale = 0f;
            level.Start(updateManager, this);
            Time.timeScale = 1f;
        }
    }

    public void UnloadAssets()
    {
        foreach (AsyncOperationHandle handle in loadedHandles)
        {
            Addressables.Release(handle);
        }
        loadedHandles.Clear();
        loadedAssets.Clear();
        loadedBackground.Clear();
    }

    public GameObject GetInstancePrefabs(string assetName)
    {
        if (loadedAssets.ContainsKey(assetName))
        {
            return loadedAssets[assetName];
        }

        return null;
    }

    public Sprite GetInstanceSprite(string assetName)
    {
        if (loadedBackground.ContainsKey(assetName))
        {
            return loadedBackground[assetName];
        }

        return null;
    }
}

