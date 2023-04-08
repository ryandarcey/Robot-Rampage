using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationType : MonoBehaviour
{
    private int index = 0;

    [SerializeField] private AnimationOverrider overrider;
    [SerializeField] private AnimatorOverrideController[] controllers;

    // Given the index, change the animations to the new controller
    public void SetAnim()
    {
        index = FindObjectOfType<SettingsManager>().getAnimationValue();
        overrider.SetAnimations(controllers[index]);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the animation value from settingsManager has changed. If so, call SetAnim()
        if (FindObjectOfType<SettingsManager>().getAnimationValue() != index)
        {
            SetAnim();
        }
    }
}
