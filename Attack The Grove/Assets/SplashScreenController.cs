using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    public float splashDuration = 3.0f; // Duraci�n del splash screen

    void Start()
    {
        // Inicia la corrutina para cargar la siguiente escena despu�s del tiempo especificado
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    IEnumerator LoadNextSceneAfterDelay()
    {
        // Espera el tiempo especificado
        yield return new WaitForSeconds(splashDuration);

        // Carga la siguiente escena (asume que la siguiente escena est� en el �ndice 1)
        SceneManager.LoadScene(1);
    }
}
