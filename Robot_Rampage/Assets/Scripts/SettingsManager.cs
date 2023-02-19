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
    public GameObject roundManagerGO;
    private RoundManager roundManager;

    float enemyMovementSpeed = 1;

    public static SettingsManager instance;

    void Awake()
    {
        // singleton SettingsManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // already have SettingsManager in scene, don't need another one
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);  // SettingsManager persists between scenes
    }

    // Start is called before the first frame update
    void Start()
    {
        getSettings();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("p"))
        {
            getSettings();
        }

    }

    public float getEnemyMovementSpeed()
    {
        return enemyMovementSpeed;
    }

    void getSettings()
    {
        // get relevant game objects
        if (cameraChange == null)
        {
            // should probably make sure to grab things programatically
            // just in case
            Debug.Log("CAMERA CHANGER IS NULL");
        }
        // TODO
        if (roundManager == null)
        {
            roundManager = roundManagerGO.GetComponent<RoundManager>();
        }

        // grab settings .txt file as string
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\testing_game_settings.txt";
        if (!File.Exists(path))
        {
            //Debug.Log("file at  " + path + "  does not exist");
            path = @".\testing_game_settings.txt";
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

        Application.targetFrameRate = json.GetInt("targetFPS");

        roundManager.roundNumber = json.GetInt("roundNumber");

        // to change shader, should just be material.setFloat or something?
    }
}
