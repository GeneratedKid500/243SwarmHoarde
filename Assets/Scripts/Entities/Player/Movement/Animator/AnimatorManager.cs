using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class AnimatorManager : EntityAnimatorHandler
{
    InputManager inputManager;
    PlayerManager playerManager;
    PlayerLocomotion playerLocomotion;
    PlayerStats playerStats;
    PauseMenu pause;
    public float idleTime;

    int horizontal;
    int vertical;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        inputManager = GetComponentInParent<InputManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        playerStats = GetComponentInParent<PlayerStats>();
        //pause = GetComponentInChildren<PauseMenu>();

        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalM, float verticalM, bool sprinting)
    {
        #region Animation Snapping
        //float snapH;
        //if (horizontalM > 0 && horizontalM < 0.55f)
        //{
        //    snapH = 0.5f;
        //}
        //else if (horizontalM >= 0.55f)
        //{
        //    snapH = 1f;
        //}
        //else if (horizontalM < 0 && horizontalM > -0.55f)
        //{
        //    snapH = -0.5f;
        //}
        //else if (horizontalM <= 0.55f)
        //{
        //    snapH = -1;
        //}
        //else
        //{
        //    snapH = 0;
        //}

        //float snapV;
        //if (verticalM > 0 && verticalM < 0.55f)
        //{
        //    snapV = 0.5f;
        //}
        //else if (verticalM >= 0.55f)
        //{
        //    snapV = 1f;
        //}
        //else if (verticalM < 0 && verticalM > -0.55f)
        //{
        //    snapV = -0.5f;
        //}
        //else if (verticalM <= 0.55f)
        //{
        //    snapV = -1;
        //}
        //else
        //{
        //    snapV = 0;
        //}
        #endregion

        if (sprinting)
            verticalM = 2;

        #region Idle Animation
        if (horizontalM == 0 && verticalM == 0)
        {
            if (idleTime < 23)
                idleTime += Time.deltaTime;
            else
                idleTime = 0;
        }
        else
            idleTime = 0;
        #endregion //

        animator.SetFloat(horizontal, horizontalM, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, verticalM, 0.1f, Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        if (SceneManager.GetActiveScene().name != "Win")
        {
            if (playerManager.isInteracting == false || playerStats.collecting)
            {
                return;
            }
        }

        if (SceneManager.GetActiveScene().name != "Win")
            playerLocomotion.rb.drag = 0;
        else
            GetComponentInParent<Rigidbody>().drag = 0;
        Vector3 deltaPos = animator.deltaPosition;
        deltaPos.y = 0;
        Vector3 vel = deltaPos / Time.deltaTime;
        if (SceneManager.GetActiveScene().name != "Win" && Time.timeScale > 0 && !playerStats.collecting)
        {
            if (SceneManager.GetActiveScene().name != "Win")
                playerLocomotion.rb.velocity = vel;
            else
                GetComponentInParent<Rigidbody>().velocity = vel;
        }
    }
}
