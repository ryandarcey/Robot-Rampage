using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leguar.TotalJSON;
using System.IO;
using System;

public class SettingsManager : MonoBehaviour
{
    public CameraChange cameraChange;
    public GameObject playerArmature;

    float enemyMovementSpeed = 1;

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
        // Change player attack accuracy by adjusting the horizontal range
        playerArmature.GetComponent<PlayerAttack>().horizontalRange = json.GetFloat("horizontalRange");

        // Change enemy movement speed given the multiplier provided
        enemyMovementSpeed = json.GetFloat("enemyMovementSpeed");

        // TODO
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
