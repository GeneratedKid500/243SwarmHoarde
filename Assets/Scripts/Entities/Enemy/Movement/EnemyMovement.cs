using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public EntityStats currentTarget;
    public LayerMask detectionLayer;

    public float targetDistance;
    public float stoppingDistance = 0.5f;
    public float speed = 3;
    public float rotationSpeed = 15;

    EnemyStats enemyStats;
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimator;

    NavMeshAgent navMeshAgent;

    GameObject[] allWaypoints;

    bool travelling;

    void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimator = GetComponent<EnemyAnimatorManager>();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

    void Start()
    {
        currentTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.speed = speed;
    }

    public void HandleMovement()
    {
        enemyAnimator.animator.SetBool("Attack", false);
        if (Sight() && currentTarget != null && enemyStats.hpCurrent > 0)
        {
            targetDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
            RotateToTarget(currentTarget.gameObject);
            travelling = false;

            if (targetDistance > stoppingDistance && !enemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
            {
                navMeshAgent.SetDestination(currentTarget.transform.position);
                if (targetDistance > stoppingDistance && targetDistance < enemyManager.detectRadius / 2 && !(enemyAnimator.animator.GetFloat("VelocityZ") >= 1f))
                {
                    enemyAnimator.animator.SetFloat("VelocityZ", 0.6f, 0.1f, Time.fixedDeltaTime);
                    speed = 4f;
                }
                else if (targetDistance > enemyManager.detectRadius / 2)
                {
                    enemyAnimator.animator.SetFloat("VelocityZ", 1.1f, 0.1f, Time.fixedDeltaTime);
                    speed = 5f;
                }
            }
            else
            {
                enemyAnimator.animator.SetFloat("VelocityZ", 0f, 0.1f, Time.fixedDeltaTime);
                AttackingRange();
            }
        }
        else
        {
            if (!travelling) SetDestination();
            enemyAnimator.animator.SetFloat("VelocityZ", 0.3f, 0.1f, Time.deltaTime);
            speed = 1;
        }

        if (travelling && navMeshAgent.remainingDistance < 1f)
        {
            SetDestination();
        }
    }

    private void SetDestination()
    {
        travelling = false;
        navMeshAgent.SetDestination(allWaypoints[Random.Range(0, allWaypoints.Length)].transform.position);
        travelling = true;
    }

    private bool Sight()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            EntityStats entityStats = colliders[i].transform.GetComponent<EntityStats>();

            if (entityStats != null)
            {
                LocalAlert(entityStats);
                Vector3 targetDirection = entityStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyManager.minDetectAngle && viewableAngle < enemyManager.maxDetectAngle)
                {
                    currentTarget = entityStats;
                    return true;
                }
            }
        }
        if (currentTarget != null && currentTarget.tag == "Player")
        {
            return true;
        }
        else return false;
    }
    private void RotateToTarget(GameObject target)
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        float angleToTarget = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Slerp(Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z),
                                              Quaternion.Euler(transform.eulerAngles.x, angleToTarget, transform.eulerAngles.z), rotationSpeed * Time.deltaTime);
    }

    private void AttackingRange()
    {
        if (Physics.Raycast(transform.position, (currentTarget.transform.position - transform.position), out RaycastHit rayHit, stoppingDistance))
        {
            speed = 0;
            navMeshAgent.SetDestination(transform.position);
            Debug.Log("Hit Directional");

            if (!enemyAnimator.animator.GetBool("Attack") && enemyStats.hpCurrent > 0)
            {
                enemyAnimator.animator.SetBool("Attack", true);
                enemyAnimator.animator.Play("Attacking");
            }
        }
    }

    private void LocalAlert(EntityStats es)
    {
        if (currentTarget != null && currentTarget.tag == "Player")
        {
            switch (es.tag)
            {
                case "Enemy":
                    es.GetComponent<EnemyMovement>().currentTarget = currentTarget;
                    break;

                default:
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = new Color(0, 1, 0, 0.5f);
        //Gizmos.DrawSphere(transform.position, enemyManager.detectRadius);
    }
}
