using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoaderController
{
    //puede ser como una pantalla de carga
    //private GameObject loader;

    AdressableInstantiator adressable;

    public void Initialize(AdressableInstantiator AddIns)
    {
        adressable = AddIns;
        adressable.SubscribeOnLoadComplete(OnAssetsLoadComplete);
    }

    private void OnAssetsLoadComplete()
    {
        HideLoader();
    }

    private void HideLoader()
    {
        //Esto es para getear la instancia de los prefabs y desde aca asignarselos al objeto (script), es decir, todos los prefabs del UpdateManager se van a ir
        Debug.Log("HideLoader");
        //loader.SetActive(false);
        // Ejemplo de instanciación de un objeto
        //GameObject instance = adressable.GetInstance("Ball");
    }
}