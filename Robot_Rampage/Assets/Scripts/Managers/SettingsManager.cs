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

    float enemyMovementSpeed = 1f;
    //float enemyProjectileSpeed = 40f;
    int targetFPS = 60;

    // Particle related values
    bool setParticles;
    GameObject[] particleObjects;

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
        changeSettings();
    }

    // Update is called once per frame
    void Update()
    {
/*        if (Input.GetKeyDown("l"))
        {
            Start();
        }*/
    }

    public void changeSettings()
    {
        // get relevant game objects
        if (cameraChange == null)
        {
            // should probably make sure to grab things programatically
            // just in case
            Debug.Log("CAMERA CHANGER IS NULL");
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

        // Change the camera type between "firstPerson", "thirdPerson", or "overhead"
        cameraChange.SetCameraMode(json.GetString("cameraMode"));

        // Set frame rate
        Application.targetFrameRate = targetFPS;

        // Set current round number
        FindObjectOfType<RoundManager>().increaseRoundNumber();

        // Toggle animations on and off. 1 if off, 0 is on. Corresponds to animation override controller index
        animationOverrideValue = json.GetInt("animationsOff");
        
        // Based on animation value, turn on or off the particle effects. 
        if(animationOverrideValue == 0)
        {
            setParticles = true;
        }
        else
        {
            setParticles = false;
        }
        setParticlesState();

        // Change accuracy of the player and enemy given the difficulty.
        switch(json.GetInt("difficulty"))
        {
            // Easy
            case 0:
                playerArmature.GetComponent<PlayerAttack>().horizontalRange = 15;
                setEnemyDifficulty(15);
                break;
            // Medium
            case 1:
                playerArmature.GetComponent<PlayerAttack>().horizontalRange = 10;
                setEnemyDifficulty(10);
                break;
            // Hard
            case 2:
                playerArmature.GetComponent<PlayerAttack>().horizontalRange = 5;
                setEnemyDifficulty(2);
                break;
        }

        // Update round manager
        FindObjectOfType<LogManager>().updateParameters(json.GetString("cameraMode"), json.GetInt("difficulty"), json.GetInt("animationsOff"));
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

    void setParticlesState()
    {
        // Get all particle effect objects
        particleObjects = GameObject.FindGameObjectsWithTag("Particles");

        // Iterate through each particle effect and set current active state
        foreach(GameObject particle in particleObjects)
        {
            particle.GetComponent<Renderer>().enabled = setParticles;
        }
    }
}
