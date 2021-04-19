using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : EntityAnimatorHandler
{
    EnemyLocomotion enemyLocomotion;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        enemyLocomotion = GetComponent<EnemyLocomotion>();
    }
}
