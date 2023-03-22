using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class RoundManager : MonoBehaviour
{
	// Starting time for player
	public const float roundTime = 180f;
	public float time = roundTime;
	double timeRounded;

	// UI related values
	public TextMeshProUGUI timeText;
	public TextMeshProUGUI winText;

	private int roundNumber = 0;

	// Round Ending related values
	private bool isRoundOver = false;
	private float timeSinceRoundOver = 0f;

	// Pause related values
    public bool isPaused = false;
    private float previousTimeScale = 1f;

    // TODO: stats we want to log each round:
    //		- number of shots fired
    //		- number of shots hit
    //		- number of hits/damage taken				<--
    //		- number of enemies killed / damage dealt
    //		- number of each enemy type that spawned
    //		- time taken/left to finish round			<--
    //		- types of pickups picked up
    //		- number / layout of rooms
    //		- 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(isRoundOver)
		{
			timeSinceRoundOver += Time.deltaTime;
		}

		// 
		if(timeSinceRoundOver > 3)
		{
			EndRound();
		}
		
		// 
		if (time <= 0 && !isRoundOver)
		{
			EndRound();
		}

        // Calculate current round time and display it in UI
        time -= Time.deltaTime;
        timeRounded = System.Math.Round(time, 2);
		timeText.text = "Time: " + timeRounded.ToString();

		// End the round immediately
		if (Input.GetButtonDown("EndRound"))
		{
			EndRound();
		}

		// Toggle on and off the pause functionality
        if (Input.GetKeyDown("p"))
        {
            TogglePause();
        }
    }

	// Exit the game
	public void EndRound()
	{ 
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

    // Begin ending the game and display lose message
    public void WinRound()
	{
		isRoundOver = true;
		winText.SetText("You Win! :)");
		FindObjectOfType<AudioManager>().PlaySound("level clear");
		FindObjectOfType<LogManager>().writeLog("level won");
	}

	// Begin ending the game and display lose message
	public void RoundLose()
	{
		isRoundOver = true;
		winText.SetText("You Lose :(");
		FindObjectOfType<LogManager>().writeLog("level lost");
	}

	// Pause or un-pause the game
    public void TogglePause()
    {
		// If the timescale is over 0, then the game is currently running and will be paused
        if (Time.timeScale > 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;     // maybe?
                                            //pauseLabel.enabled = true;	// UI element

			// Log the end of the round
            FindObjectOfType<LogManager>().writeLog("End Round");
            FindObjectOfType<LogManager>().writeLog("Round Score: " + FindObjectOfType<ScoreManager>().getScore().ToString());

            isPaused = true;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = previousTimeScale;
            AudioListener.pause = false;    // maybe?
                                            //pauseLabel.enabled = true;	// UI element

            // Log the end of the round
            FindObjectOfType<SettingsManager>().changeSettings();

            isPaused = false;
        }
    }

    /*public void setRoundNumber(int num)
    {
        roundNumber = num;
    }*/

    public void increaseRoundNumber()
    {
        roundNumber++;
    }

    public int getRoundNumber()
    {
        return roundNumber;
    }

}

