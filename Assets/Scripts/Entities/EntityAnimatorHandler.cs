using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimatorHandler : MonoBehaviour
{
    public Animator animator;

    public void PlayTargetAnimation(string animation, bool interactLocked)
    {
        animator.applyRootMotion = interactLocked;
        animator.SetBool("isInteracting", interactLocked);
        animator.CrossFade(animation, 0.2f);
    }
}
