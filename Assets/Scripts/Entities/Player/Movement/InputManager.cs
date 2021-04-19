using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    PlayerAttacker playerAttacker;
    AnimatorManager animatorManager;

    [Header("Movement")]
    public Vector2 movementInput;
    public float horizontal;
    public float vertical;
    public float moveAmount;

    [Header("Camera")]
    public Vector2 cameraInput;
    public float cameraHorizontal;
    public float cameraVertical;

    [Header("Actions")]
    public bool sprintInput;
    public bool rollInput;
    public bool rollFlag;
    public bool jumpInput;
    public bool rb_Input;
    public bool rt_Input;


    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAttacker = GetComponent<PlayerAttacker>();
    }

    public void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            //movement inputs
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            //camera inputs
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            //sprint
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            //jump
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
        }

        playerControls.Enable();
    }

    public void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleInputs()
    {
        HandleMovementInput();

        HandleJumpingInput();

        HandleAttackInput();
    }

    private void HandleMovementInput()
    {
        vertical = movementInput.y;
        horizontal = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);

        cameraVertical = cameraInput.y;
        cameraHorizontal = cameraInput.x;

        HandleRollInput();
        HandleSprintingInput();
    }

    private void HandleRollInput()
    {
        rollInput = playerControls.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

        if (rollInput && !rollFlag)
        {
            rollFlag = true;
        }
    }

    private void HandleSprintingInput()
    {
        if (sprintInput && moveAmount >= 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    public void HandleJumpingInput()
    {
        if (jumpInput && !animatorManager.animator.GetBool("isInteracting"))
        {
            jumpInput = false;
            playerLocomotion.HandleJump();
        }
        else jumpInput = false;
    }

    public void HandleAttackInput()
    {
        //light attack
        playerControls.PlayerActions.RB.performed += i => rb_Input = true;

        //general attack
        playerControls.PlayerActions.RT.performed += i => rt_Input = true;

        if (animatorManager.animator.GetBool("isGrounded"))
        {
            if (!animatorManager.animator.GetBool("isInteracting"))
            {
                if (rb_Input)
                {
                    playerAttacker.HandleLightAttack();
                }
            }

            if (rt_Input && !animatorManager.animator.GetCurrentAnimatorStateInfo(1).IsName("Finding") &&
                !animatorManager.animator.GetCurrentAnimatorStateInfo(1).IsName("Power Up") &&
                !animatorManager.animator.GetCurrentAnimatorStateInfo(1).IsName("Greater Power Up"))
            {
                switch (playerLocomotion.isSprinting) 
                {
                    case true:
                        if (!animatorManager.animator.GetBool("isInteracting"))
                            playerAttacker.HandleSprintAttack();
                        break;
                    default:
                        playerAttacker.HandleHeavyAttack();
                        break;
                }
            }
        }
    }
}
