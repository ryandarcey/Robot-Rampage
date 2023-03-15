using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    // Boolean for whether or not animations are on
    public bool animationsOn = true;

    // Animation overrider related values
    [SerializeField] private AnimationOverrider overrider;
    [SerializeField] private AnimatorOverrideController[] controllers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle on and off the pause functionality
        if (Input.GetKeyDown("o"))
        {
            animationsOn = !animationsOn;
        }
    }

    // Given the index, change the animations to the new controller
    public void SetAnim(int index)
    {
        overrider.SetAnimations(controllers[index]);
    }
}
