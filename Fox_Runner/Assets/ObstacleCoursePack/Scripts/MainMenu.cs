using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public static int SceneCounter = 1;
    public static float startTime;
    public static float t;
    public static float minutes;
    public static float seconds;

    public static float lastsceneM = 0;
    public static float lastsceneS = 0;

    public static float lastM;
    public static float lastS;
    public static float secondsHelper;

    public static string lastRun;

    public TextMeshProUGUI record;
    public static string bestGame;

    public static bool speedrun = true;


    private void Start()
    {
        bestGame = PlayerPrefs.GetString("rekord");
        record.text = bestGame;
    }

    public void PlayGame()
    {
        SceneCounter = 1;
        CharacterControls.Paused = false;
        speedrun = false;
        SceneManager.LoadScene(1);
    }

    public void SpeedRun()
    {
        SceneCounter = 1;
        CharacterControls.Paused = false;
        CharacterControls.finished = false;
        speedrun = true;
        t = 0;
        minutes = 0;
        seconds = 0;
        lastsceneM = 0;
        lastsceneS = 0;
        secondsHelper = 0;
        SceneManager.LoadScene(1);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void OpenMenu()
    {
        if (speedrun == true)
        {
            SceneCounter = 1;
            t = 0;
            minutes = 0;
            seconds = 0;
            lastsceneM = 0;
            lastsceneS = 0;
            secondsHelper = 0;
            speedrun = false;
            SceneManager.LoadScene(0);
        }
        if (speedrun == false)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void LoadLastGame()
    {
        CharacterControls.Paused = false;
        SceneManager.LoadScene(SceneCounter);
    }
    public static void LoadNextMap()
    {
        SceneCounter++;
        CharacterControls.Paused = false;
        lastsceneS = seconds;
        lastsceneM = minutes;
        secondsHelper += t;
        SceneManager.LoadScene(SceneCounter);
    }

    public static void SetString(string KeyName, string Value)
    {
        PlayerPrefs.SetString(KeyName, Value);
        bestGame = PlayerPrefs.GetString("rekord");
    }

}
