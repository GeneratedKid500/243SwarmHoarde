using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCollide : MonoBehaviour
{
    PlayerStats ps;

    private void Start()
    {
        ps = GetComponentInParent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider collided)
    {
        switch (collided.tag)
        {
            case "Enemy":
                collided.GetComponent<EnemyStats>().TakeDamage(10);
                ps.TakeDamage(-0.5f);
                break;
        }
    }
}
