using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    AnimatorManager animatorManager;
    CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;
    PlayerAttacker playerAttacker;

    public bool isInteracting;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAttacker = GetComponent<PlayerAttacker>();
    }

    private void Update()
    {
        inputManager.rollFlag = false;

        if (!isInteracting && playerAttacker.weaponManager.isWeaponLoaded())
        {
            playerAttacker.weaponManager.LoadWeapon(false);
        }

        inputManager.HandleInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleCamera();

        isInteracting = animatorManager.animator.GetBool("isInteracting");
        playerLocomotion.isJumping = animatorManager.animator.GetBool("isJumping");

        animatorManager.animator.SetBool("isGrounded", playerLocomotion.isGrounded);
        animatorManager.animator.SetFloat("airTime", playerLocomotion.inAirTimer);
        animatorManager.animator.SetFloat("idleTime", animatorManager.idleTime);

        inputManager.rt_Input = false;
        inputManager.rb_Input = false;
    }
}
