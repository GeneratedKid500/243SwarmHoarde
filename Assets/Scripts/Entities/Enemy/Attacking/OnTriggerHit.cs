using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerHit : MonoBehaviour
{
    int damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("hit " + other.name);
            other.transform.GetComponent<PlayerStats>().TakeDamage(damage);
        }
    }
}
