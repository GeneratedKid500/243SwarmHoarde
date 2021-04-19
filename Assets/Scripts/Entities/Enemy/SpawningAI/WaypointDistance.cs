using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDistance : MonoBehaviour
{
    public GameObject player;
    public float distanceFromPlayer;

    void Start()
    {
        transform.parent = GameObject.FindGameObjectWithTag("WaypointManager").transform;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
    }
}
