using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static int SceneCounter = 1;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void OpenMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLastGame()
    {
        SceneManager.LoadScene(SceneCounter);
    }
    public static void LoadNextMap()
    {
        SceneCounter++;
        SceneManager.LoadScene(SceneCounter);
    }
}
