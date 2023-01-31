using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leguar.TotalJSON;
using System.IO;
using System;
using UnityEngine.Device;

public class SettingsManager : MonoBehaviour
{    
    public CameraChange cameraChange;
<<<<<<< Updated upstream:Robot_Rampage/Assets/SettingsManager.cs
    public GameObject playerArmature;

    float enemyMovementSpeed = 1;
=======
    public Material shaderMaterial;
>>>>>>> Stashed changes:Robot_Rampage/Assets/Scripts/SettingsManager.cs

    // Start is called before the first frame update
    void Start()
    {
		// get relevant game objects
		if (cameraChange == null)
        {
            // should probably make sure to grab things programatically
            // just in case
            Debug.Log("CAMERA CHANGER IS NULL");
        }
        // TODO

        // grab settings .txt file as string
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\testing_game_settings.txt";
        if (!File.Exists(path))
        {
            Debug.Log("file at  " + path + "  does not exist");
        }

        // get JSON object as a string
        string fileContents = File.ReadAllText(path);

        // use parser to turn into JSON object
        JSON json = JSON.ParseString(fileContents);

        // call methods to set parameters

        // Change the camera type between "firstPerson", "thirdPerson", or "overhead"
        cameraChange.SetCameraMode(json.GetString("cameraMode"));
<<<<<<< Updated upstream:Robot_Rampage/Assets/SettingsManager.cs
        // Change player attack accuracy by adjusting the horizontal range
        playerArmature.GetComponent<PlayerAttack>().horizontalRange = json.GetFloat("horizontalRange");

        // Change enemy movement speed given the multiplier provided
        enemyMovementSpeed = json.GetFloat("enemyMovementSpeed");

        // TODO
=======
		// TODO

		UnityEngine.Application.targetFrameRate = json.GetInt("targetFPS");

        /*shaderMaterial.SetInteger("Resolution", json.GetInt("shaderResolution"));
        shaderMaterial.SetInteger("Pixel Width", json.GetInt("shaderPixelWidth"));
        shaderMaterial.SetInteger("Pixel Height", json.GetInt("shaderPixelHeight"));
        Debug.Log(shaderMaterial.GetInteger("Resolution"));*/
>>>>>>> Stashed changes:Robot_Rampage/Assets/Scripts/SettingsManager.cs
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getEnemyMovementSpeed()
    {
        return enemyMovementSpeed;
    }
}
