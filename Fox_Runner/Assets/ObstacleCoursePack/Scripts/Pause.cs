using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public void Resume()
    {
        gameObject.SetActive(false);
        CharacterControls.Paused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void OpenMenu()
    {
        SceneManager.LoadScene(0);
    }
}
