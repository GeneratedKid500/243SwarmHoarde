using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class Weapons : ScriptableObject
{
    [Header("Weapon Information")]
    public string weaponName;
    public GameObject modelPrefab;
    public int baseDamage;

    [Header("Attack Animations")]
    public string lightAttack;
    public string attack1;
    public string attack2;
    public string attack3;
    public string sprintAttack;
    public string sprintAttack2;

}
