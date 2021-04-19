using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    public WeaponManager weaponManager;

    Weapons weapon;

    bool attack1, attack2, attack3;

    private void Start()
    {
        weaponManager = GetComponentInChildren<WeaponManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        weapon = GetComponentInChildren<WeaponManager>().weapon;
    }


    public void HandleLightAttack()
    {
        weaponManager.LoadWeapon(true);
        animatorManager.PlayTargetAnimation(weapon.lightAttack, true);
    }

    public void HandleHeavyAttack()
    {
        Debug.Log(weapon.attack1);
        weaponManager.LoadWeapon(true);

        if (!animatorManager.animator.GetCurrentAnimatorStateInfo(1).IsName(weapon.attack1) &&
            !animatorManager.animator.GetCurrentAnimatorStateInfo(1).IsName(weapon.attack2) &&
            !animatorManager.animator.GetCurrentAnimatorStateInfo(1).IsName(weapon.attack3))
        {
            if (animatorManager.animator.GetCurrentAnimatorStateInfo(1).IsName(weapon.attack1))
                return;

            animatorManager.PlayTargetAnimation(weapon.attack1, true);
        }
        else if (animatorManager.animator.GetCurrentAnimatorStateInfo(1).IsName(weapon.attack1))
        {
            animatorManager.animator.SetBool("attack2", true);
        }
        else if (animatorManager.animator.GetCurrentAnimatorStateInfo(1).IsName(weapon.attack2))
        {
            animatorManager.animator.SetBool("attack3", true);
        }
    }

    public void HandleSprintAttack()
    {
        weaponManager.LoadWeapon(true);

        if (Random.Range(0, 2) == 0)
            animatorManager.PlayTargetAnimation(weapon.sprintAttack, true);
        else
            animatorManager.PlayTargetAnimation(weapon.sprintAttack2, true);
    }


}
