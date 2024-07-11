using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // M�todo para el bot�n de Play
    public void PlayGame()
    {
        // Asume que la pantalla de Map es la escena con el �ndice 1
        SceneManager.LoadScene("Map");
    }

    // M�todo para el bot�n de Quit
    public void QuitGame()
    {
        // Sale del juego. No funciona en el editor de Unity, pero funcionar� en una build.
        Application.Quit();

        // Para verificar que el bot�n funciona en el editor, puedes usar Debug.Log:
        Debug.Log("Game is exiting");
    }
}