using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;

    Transform cameraObject;
    public Rigidbody rb;

    Vector3 moveDirection;

    [Header ("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    ///public float walkSpeed = 1.5f;
    public float movementSpeed = 6f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 10f;

    [Header("Jumping Attributes")]
    public float gravityIntensity = -15;
    public float jumpHeight = 3;

    [Header("Falling Attributes")]
    public float inAirTimer;
    public float leapingVelocity = 3f;
    public float fallingVelocity = 33f;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        inputManager = GetComponent<InputManager>();

        rb = GetComponent<Rigidbody>();

        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isInteracting)
            return;

        if (isJumping)
            return;

        HandleMovement();
        HandleRotation();
        HandleRolling();
    }

    private void HandleMovement()
    {

        moveDirection = cameraObject.forward * inputManager.vertical;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        switch (isSprinting) 
        {
            case true:
                moveDirection = moveDirection * sprintSpeed;
                break;

            default:
                moveDirection = moveDirection * movementSpeed;
                break;
        }

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.vertical;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontal;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleRolling()
    {
        if (playerManager.isInteracting)
            return;

        if (inputManager.rollFlag)
        {
            moveDirection = cameraObject.forward * inputManager.vertical;
            moveDirection += cameraObject.right * inputManager.horizontal;

            if (inputManager.moveAmount > 0)
            {
                animatorManager.PlayTargetAnimation("Rolling", true);
                animatorManager.animator.SetBool("isRolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = rollRotation;
            }
            else
            {
                if (animatorManager.animator.GetBool("isGrounded"))
                    animatorManager.PlayTargetAnimation("Backstep", true);
            }
        }
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y = raycastOrigin.y + rayCastHeightOffset;

        if (!isGrounded && rb.velocity.y <= 0)
        {
            if (!playerManager.isInteracting && !animatorManager.animator.GetBool("isRolling"))
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * (leapingVelocity * 2));
            rb.AddForce(-Vector3.up * (fallingVelocity * 25) * inAirTimer);
        }


        if (Physics.SphereCast(raycastOrigin, 0.15f, -Vector3.up, out hit, 1f, groundLayer))
        {
            if (!isGrounded)
            {
                if (!animatorManager.animator.GetBool("isRolling"))
                {
                    if (inAirTimer < 1)
                        animatorManager.PlayTargetAnimation("Land", true);
                    else
                        animatorManager.PlayTargetAnimation("HardLand", true);
                }
            }

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void HandleJump()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpVel = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVel = moveDirection;
            playerVel.y = jumpVel;

            rb.velocity = playerVel;
        }
    }

    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(.2f);
    }
}
