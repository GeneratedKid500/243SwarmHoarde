using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public int hpLevel = 10;
    public float hpCurrent;
    public int hpMax;

    public int SetMaxHealth()
    {
        hpMax = hpLevel * 10;
        return hpMax;
    }



}
