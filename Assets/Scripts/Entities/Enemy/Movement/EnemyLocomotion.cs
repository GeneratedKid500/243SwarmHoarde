using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : MonoBehaviour
{
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimator;

    Rigidbody rb;
    NavMeshAgent navMeshAgent;

    [Header("Entity Detection")]
    public EntityStats currentTarget;
    public LayerMask detectionLayer;

    public float targetDistance;
    public float stoppingDistance = 1f;

    public float rotationSpeed = 15;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimator = GetComponent<EnemyAnimatorManager>();


        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public void HandleSightDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            EntityStats entityStats = colliders[i].transform.GetComponent<EntityStats>();

            if (entityStats != null)
            {
                Vector3 targetDirection = entityStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyManager.minDetectAngle && viewableAngle < enemyManager.maxDetectAngle)
                {
                    currentTarget = entityStats;
                }
            }
        }
    }

    public void HandleMoveToTarget()
    {
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        targetDistance = Vector3.Distance(currentTarget.transform.position, transform.position);

        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        
        //if performing action, stop moving
        if (enemyManager.isPerformingAction)
        {
            enemyAnimator.animator.SetFloat("VelocityZ", 0, 0.1f, Time.deltaTime);
            //navMeshAgent.enabled = false;
        }
        else
        {
            //if (!navMeshAgent.enabled)  
            //    navMeshAgent.enabled = true;

            if (targetDistance > stoppingDistance && targetDistance < enemyManager.detectRadius / 2)
            {
                enemyAnimator.animator.SetFloat("VelocityZ", 0.6f, 0.1f, Time.deltaTime);
            }
            else if (targetDistance > enemyManager.detectRadius / 2)
            {
                enemyAnimator.animator.SetFloat("VelocityZ", 1f, 0.1f, Time.deltaTime);
            }
        }

        RotateToTarget();

        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void RotateToTarget()
    {
        //rotate manually if performing action
        if (enemyManager.isPerformingAction)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
                direction = transform.forward;

            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed / Time.deltaTime);
        }
        //using navmesh
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
            Vector3 targetVel = rb.velocity;

            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(currentTarget.transform.position);

            rb.velocity = targetVel;
            transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
        }
    }
}
