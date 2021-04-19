using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    EnemyAnimatorManager animatorManager;

    void Start()
    {
        animatorManager = GetComponent<EnemyAnimatorManager>();

        hpMax = SetMaxHealth();
        hpCurrent = hpMax;
    }

    public void TakeDamage(int damage)
    {
        hpCurrent -= damage;

        if (hpCurrent > 0)
        {
            animatorManager.animator.Play("Flying Back Death");
        }
        else
        {
            //die after x amount of seconds
            Destroy(this.gameObject, 2f);
            animatorManager.animator.Play("Flying Back Death");
            GetComponentInParent<EnemySpawning>().RemoveFromList(this.gameObject);
        }
    }
}
