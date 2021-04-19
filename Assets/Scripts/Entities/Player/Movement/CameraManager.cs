using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    public Transform target;
    public Transform cameraPivot;
    public Transform cameraTransform;

    [Header ("Camera Follow")]
    public float camFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2f;
    public float cameraPivotSpeed = 2f;

    Vector3 cameraFollowVel = Vector3.zero;
    private Vector3 cameraVectorPos;

    [Header ("Camera Rotation")]
    public float lookAngle; //up down
    public float pivotAngle; //left right
    public float minPivotAngle = -35;
    public float maxPivotAngle = 35;

    private float defaultPos;

    [Header ("Camera Collision")]
    public float cameraCollisionRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;
    public LayerMask collisionLayers;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.parent = null;

        inputManager = FindObjectOfType<InputManager>();

        target = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;

        defaultPos = cameraTransform.localPosition.z;
    }

    public void HandleCamera()
    {
        //if (!(Time.timeScale < 1))
        //{
            FollowTarget();
            RotateCamera();
            HandleCameraCollisions();
        //}


        Cursor.visible = false;
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, target.position, ref cameraFollowVel, camFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        lookAngle = lookAngle + (inputManager.cameraHorizontal * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraVertical * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);


        Vector3 rot = Vector3.zero;
        rot.y = lookAngle;
        Quaternion targetRot = Quaternion.Euler(rot);
        transform.rotation = targetRot;

        rot = Vector3.zero;
        rot.x = pivotAngle;
        targetRot = Quaternion.Euler(rot);
        cameraPivot.localRotation = targetRot;
    }

    private void HandleCameraCollisions()
    {
        float targetPos = defaultPos;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPos), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPos =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPos) < minCollisionOffset)
        {
            targetPos = targetPos - minCollisionOffset;
        }

        cameraVectorPos.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPos, 0.2f);
        cameraTransform.localPosition = cameraVectorPos;
    }
}
