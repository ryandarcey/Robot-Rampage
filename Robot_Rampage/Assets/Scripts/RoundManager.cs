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
	public TextMeshProUGUI timeText;
	public TextMeshProUGUI winText;

	public int roundNumber = 1;
	private int shotsFired = 0;
	private int shotsHit = 0;

	private bool isRoundOver = false;
	private float timeSinceRoundOver = 0f;

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
		if(timeSinceRoundOver > 3)
		{
			EndRound();
		}
		
		
		if (time <= 0 && !isRoundOver)
		{
			EndRound();
		}
		time -= Time.deltaTime;

		timeRounded = System.Math.Round(time, 2);
		timeText.text = "Time: " + timeRounded.ToString();

		if (Input.GetButtonDown("EndRound"))
		{
			EndRound();
		}
	}

	public void ShotGun()
	{
		shotsFired++;
	}

	public void ShotHit()
	{
		shotsHit++;
	}

	public void EndRound()
	{ 
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	public void WinRound()
	{
		isRoundOver = true;
		winText.SetText("You Win! :)");
		FindObjectOfType<AudioManager>().PlaySound("level clear");
		FindObjectOfType<LogManager>().writeLog("level won");
	}

	public void RoundLose()
	{
		isRoundOver = true;
		winText.SetText("You Lose :(");
		FindObjectOfType<LogManager>().writeLog("level lost");
	}

}

