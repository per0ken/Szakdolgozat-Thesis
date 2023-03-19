using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            MainMenu.LoadNextMap();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu.OpenMenu();
        }
    }
}
