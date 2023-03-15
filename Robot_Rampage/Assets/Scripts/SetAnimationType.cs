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
        overrider.SetAnimations(controllers[index]);
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle on and off the pause functionality
        if (Input.GetKeyDown("o"))
        {
            index = (index + 1) % 2;
            Debug.Log("Index = " + index);
            SetAnim();
        }
    }
}
