using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public PlayerControls playerControls;

    bool go, quit;

    void Start()
    {
        playerControls = new PlayerControls();
    }

    void Update()
    {
        Time.timeScale = 1;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Load");
            if (SceneManager.GetActiveScene().name == "Start")
                SceneManager.LoadScene("Level");
            else if (SceneManager.GetActiveScene().name == "Win" || SceneManager.GetActiveScene().name == "Lose")
                Application.Quit();
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
