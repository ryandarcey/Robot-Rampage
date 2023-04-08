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

    // Texture related values
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material columnMaterial;
    [SerializeField] private Texture wallTexHigh;
    [SerializeField] private Texture wallTexMed;
    [SerializeField] private Texture wallTexLow;

	[SerializeField] private Material groundMaterial;
	[SerializeField] private Texture groundTexHigh;
	[SerializeField] private Texture groundTexMed;
	[SerializeField] private Texture groundTexLow;

    private string currTextureQuality;

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
        //animationOverrideValue = json.GetInt("animationsOff");
        
        // Based on animation value, turn on or off the particle effects. 
/*        if(animationOverrideValue == 0)
        {
            setParticles = true;
        }
        else
        {
            setParticles = false;
        }
        setParticlesState();*/

        // Set textures based on value
        switch(json.GetInt("textures"))
        {
            case 0:
                wallMaterial.SetTexture("_MainTex", wallTexLow);
				columnMaterial.SetTexture("_MainTex", wallTexLow);
				groundMaterial.SetTexture("_MainTex", groundTexLow);
                currTextureQuality = "Low";
				break;
            case 1:
                wallMaterial.SetTexture("_MainTex", wallTexMed);
				columnMaterial.SetTexture("_MainTex", wallTexMed);
				groundMaterial.SetTexture("_MainTex", groundTexMed);
                currTextureQuality = "Medium";
				break;
            case 2:
                wallMaterial.SetTexture("_MainTex", wallTexHigh);
				columnMaterial.SetTexture("_MainTex", wallTexHigh);
				groundMaterial.SetTexture("_MainTex", groundTexHigh);
                currTextureQuality = "High";
				break;
            default:
				wallMaterial.SetTexture("_MainTex", wallTexMed);
				columnMaterial.SetTexture("_MainTex", wallTexMed);
				groundMaterial.SetTexture("_MainTex", groundTexMed);
                currTextureQuality = "Medium";
				break;
        }

        // Change accuracy of the player and enemy given the difficulty.
        switch(json.GetInt("difficulty"))
        {
            // Easy
            case 0:
                playerArmature.GetComponent<PlayerAttack>().horizontalRange = 15;
                setEnemyDifficulty(8);
                break;
            // Medium
            case 1:
                playerArmature.GetComponent<PlayerAttack>().horizontalRange = 10;
                setEnemyDifficulty(4);
                break;
            // Hard
            case 2:
                playerArmature.GetComponent<PlayerAttack>().horizontalRange = 6;
                setEnemyDifficulty(1);
                break;
        }

        // Update round manager
        FindObjectOfType<LogManager>().updateParameters(json.GetString("cameraMode"), json.GetInt("difficulty"), currTextureQuality);
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
