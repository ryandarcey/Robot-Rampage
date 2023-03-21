using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

using Leguar.TotalJSON;
using System.IO;
using System;
using Unity.VisualScripting;

public class SettingsManager : MonoBehaviour
{
    public CameraChange cameraChange;
    public GameObject playerArmature;
    public GameObject roundManagerGO;
    private RoundManager roundManager;

    float enemyMovementSpeed = 1;

    int animationOverrideValue = 0;

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
        //playerArmature.GetComponent<PlayerAttack>().horizontalRange = json.GetFloat("horizontalRange");

        // Change enemy movement speed given the multiplier provided
        enemyMovementSpeed = json.GetFloat("enemyMovementSpeed");

        // Set frame rate
        Application.targetFrameRate = json.GetInt("targetFPS");

        // Set current round number
        roundManager.roundNumber = json.GetInt("roundNumber");

        // Toggle animations on and off
        animationOverrideValue = json.GetInt("animationsOn");

        // Change accuracy of the player and enemy given the difficulty. 0 = easy, 1 = hard
        if (json.GetInt("difficulty") == 1)
        {
            playerArmature.GetComponent<PlayerAttack>().horizontalRange = 5;
            setEnemyDifficulty(2);
        }
        else
        {
            playerArmature.GetComponent<PlayerAttack>().horizontalRange = 15;
            setEnemyDifficulty(15);
        }

        // to change shader, should just be material.setFloat or something?

        // Update round manager
        FindObjectOfType<LogManager>().updateParameters(json.GetString("cameraMode"), json.GetInt("difficulty"), json.GetInt("animationsOn"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            Start();
        }
    }

    public float getEnemyMovementSpeed()
    {
        return enemyMovementSpeed;
    }

    public int getAnimationValue()
    {
        return animationOverrideValue;
    }

    void setEnemyDifficulty(int range)
    {
        GameObject[] GOArray = SceneManager.GetActiveScene().GetRootGameObjects();// GetComponents<GameObject>();

        for (int i = 0; i < GOArray.Length; i++)
        {
            GameObject go = GOArray[i];
            if (go.layer == 6 && go.GetComponent<EnemyAction>() != null)
            {
                go.GetComponent<EnemyAction>().horizontalRange = range;
            }
        }
    }
}
