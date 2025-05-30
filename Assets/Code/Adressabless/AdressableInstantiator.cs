using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

[Serializable]
public class Aasd
{
    public string name = "";
    public List<AssetReferenceGameObject> refe = new();
}

public class AdressableInstantiator
{
    [SerializeField]
    private List<Aasd> assetReferences = new List<Aasd>();
    public Dictionary<string, GameObject> loadedAssets;
    public event Action OnLoadComplete;
    public bool useRemoteAssets = true;
    public String localURL = "http://localhost:3000/";
    public String cloudURL = "https://myserver.com/";

    LoaderController loaderController = new LoaderController();

    public void Initialize(List<Aasd> adressablessList, UpdateManager up)
    {
        assetReferences = adressablessList;

        loaderController.Initialize(this);

        if (useRemoteAssets)
        {
            Addressables.ResourceManager.InternalIdTransformFunc +=
            ChangeAssetUrlToPrivateServer;
        }
        loadedAssets = new Dictionary<string, GameObject>();
        LoadAssets(up);
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

    private void LoadAssets(UpdateManager up)
    {
        up.CoRoutineStart(LoadAssetsCoroutine());
    }

    private IEnumerator LoadAssetsCoroutine()
    {
        Debug.Log("CoRoutine");
        int assetsToLoad = assetReferences[(int)(1 / 6)].refe.Count;
        int assetsLoaded = 0;

        foreach (AssetReference assetReference in assetReferences[(int)(1 / 6)].refe)
        {
            AsyncOperationHandle<GameObject> handle =
           assetReference.LoadAssetAsync<GameObject>();
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                String assetName = handle.Result.name.Split(" ")[0];
                loadedAssets.Add(assetName, handle.Result);
                assetsLoaded++;
            }
        }

        if (assetsLoaded == assetsToLoad)
        {
            OnLoadComplete?.Invoke();
        }
    }

    public void SubscribeOnLoadComplete(Action callback)
    {
        OnLoadComplete += callback;
    }

    public GameObject GetInstance(string assetName)
    {
        if (loadedAssets.ContainsKey(assetName))
        {
            return GameObject.Instantiate(loadedAssets[assetName]);
        }
        Debug.LogError($"Asset '{assetName}' not found.");
        return null;
    }
}

