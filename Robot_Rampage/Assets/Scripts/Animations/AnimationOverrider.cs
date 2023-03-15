using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOverrider : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimations(AnimatorOverrideController overrideController)
    {
        animator.runtimeAnimatorController = overrideController;
    }
}
