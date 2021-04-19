using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpToSpawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = Vector3.zero;
    }
}
