using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject firstPersonCamera;
    public GameObject thirdPersonCamera;
    public bool isFirstPerson;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Camera"))
        {
            isFirstPerson = !isFirstPerson;
        }
        StartCoroutine(CamChange());
    }

    IEnumerator CamChange()
    {
        yield return new WaitForSeconds(0.01f);

        if(isFirstPerson)
        {
            firstPersonCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);
        }
        else
        {
            firstPersonCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);
        }
    }
}
