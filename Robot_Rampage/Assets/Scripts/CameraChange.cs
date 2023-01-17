using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject playerArmature;
    private ThirdPersonController thirdPersonController;

    public GameObject firstPersonCamera;    // 0
    public GameObject thirdPersonCamera;    // 1
    public GameObject overheadCamera;       // 2
    public int cameraMode = 1;

    // Player Mesh for Third Person and Gun for First Person
    public GameObject playerModel;
    public MeshRenderer playerRenderer;
    //public GameObject firstPersonGun;

    void Start()
    {
        thirdPersonController = playerArmature.GetComponent<ThirdPersonController>();
        CamChange();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Camera"))
        { 
            cameraMode = (cameraMode + 1) % 3;
			Debug.Log("camera button down, camera mode  " + cameraMode);
			StartCoroutine(CamChange());
		}
    }

    IEnumerator CamChange()
    {
        yield return new WaitForSeconds(0.01f);

        if(cameraMode == 0)
        {
            firstPersonCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);
            overheadCamera.SetActive(false);

            //playerModel.SetActive(false);
            playerModel.GetComponent<SkinnedMeshRenderer>().enabled = false;
            //playerRenderer.enabled = false;
            //firstPersonGun.SetActive(true);
            thirdPersonController.LockCameraPosition = false;
        }
        else if (cameraMode == 1)
        {
            firstPersonCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);
			overheadCamera.SetActive(false);

			//playerModel.SetActive(true);
			playerModel.GetComponent<SkinnedMeshRenderer>().enabled = true;
			//playerRenderer.enabled = true;
			//firstPersonGun.SetActive(false);
			thirdPersonController.LockCameraPosition = false;
		}
        else if (cameraMode == 2)
        {
			firstPersonCamera.SetActive(false);
			thirdPersonCamera.SetActive(false);
			overheadCamera.SetActive(true);

			//playerModel.SetActive(true);
			playerModel.GetComponent<SkinnedMeshRenderer>().enabled = true;
			//playerRenderer.enabled = true;
			//firstPersonGun.SetActive(false);
			thirdPersonController.LockCameraPosition = true;
		}
    }
}
