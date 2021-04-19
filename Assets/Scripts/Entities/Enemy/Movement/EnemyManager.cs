using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    EnemyMovement enemyLocomotion;

    public bool isPerformingAction;

    [Header("A.I settings")]
    public float detectRadius = 20;
    public float maxDetectAngle = 55;
    public float minDetectAngle = -55;

    void Awake()
    {
        enemyLocomotion = GetComponent<EnemyMovement>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        HandleCurrent();
    }

    private void HandleCurrent()
    {
        enemyLocomotion.HandleMovement();
    }
}
