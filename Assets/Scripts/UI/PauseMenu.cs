using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerStats playerStats;

    public CanvasGroup pauseMenu;

    bool pauseInput = false;
    public bool paused = false;

    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        playerControls = GetComponentInParent<InputManager>().playerControls;
    }

    void Update()
    {
        if (!paused)
        {
            pauseMenu.alpha = 0;
            if (!playerStats.collecting)
            {
                playerControls.PlayerActions.Pause.performed += i => pauseInput = true;
            }


        }
        else
        {
            pauseMenu.alpha = 1;
            if (!playerStats.collecting)
            {
                Time.timeScale = 0;
            }

            playerControls.PlayerActions.Continue.performed += i => pauseInput = true;

            playerControls.PlayerActions.Exit.performed += i => Application.Quit();
        }

        if (pauseInput)
        {
            paused = !paused;
            pauseInput = false;
            if (Time.timeScale < 1)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }

        }

    }
}
