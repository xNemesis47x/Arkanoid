using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    private bool isInitialized = false;

    public void StartGame()
    {
        if (!isInitialized)
        {
            StartCoroutine(WaitForManagerAndStart());
        }

        // Limpiar el UpdateManager antes de recargar la escena
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.ClearAll();
        }

        SceneManager.LoadScene("SampleScene");

        Debug.Log("Bot�n presionado: StartGame llamado");
    }

    private IEnumerator WaitForManagerAndStart()
    {
        // Ac� pod�s esperar si necesit�s que alg�n manager termine de iniciar
        yield return new WaitForSeconds(0.1f);
        isInitialized = true;
    }
}