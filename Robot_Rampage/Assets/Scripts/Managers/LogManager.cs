using Leguar.TotalJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Text;
using UnityEngine;

public class LogManager : MonoBehaviour
{
	string path;

	public int roundNumber = 0;
	DateTime startTime;

	public static LogManager instance;

	// Game parameter related values
	string currentCamera;
	string currentDifficulty;
	string currentAnimation;

	void Awake()
	{
		// singleton LogManager
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			// already have LogManager in scene, don't need another one
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);  // LogManager persists between scenes
	}

	// Start is called before the first frame update
	void Start()
	{
		string now = string.Format("{0:yyyy-MM-dd_hh-mm-ss-fff-tt}", DateTime.Now);
		startTime = DateTime.Now;

		string filename = "session_" + now;

		path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RobotRampage_RoundLogs\";

		if (!Directory.Exists(path))
		{
			//Debug.Log("file at  " + path + "  does not exist");
			path = @".\RobotRampage_RoundLogs\" + filename + ".csv";
		}
		else
		{
			path += filename + ".csv";
		}
		
		Debug.Log(path);

		StreamWriter sw = File.CreateText(path);    // creates file
		sw.Close();

		using (StreamWriter stream = File.AppendText(path))
		{
			string columnHeaders = "seconds_since_start,round_number,camera,difficulty,animation,action";  // TODO: figure out what columns we want
			stream.WriteLine(columnHeaders);
			Debug.Log(columnHeaders);
		}

		Debug.Log("LogManager Start() called");
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void writeLog(string log)
	{
		// string builder? / construct string
		//string now = string.Format("{0:hh:mm:ss.fff}", DateTime.Now);   // time of day
		TimeSpan timeSinceStart = DateTime.Now - startTime;
		string timeSinceStartString = timeSinceStart.TotalSeconds.ToString();
		roundNumber = FindObjectOfType<RoundManager>().getRoundNumber();

        string line = timeSinceStartString + "," + roundNumber.ToString() + "," + currentCamera + "," + currentDifficulty + "," + currentAnimation + "," + log;

		// write new line to file
		using (StreamWriter stream = File.AppendText(path))
		{
			stream.WriteLine(line);
		}
	}

	public void updateParameters(string camera, int difficulty, int animations)
	{
        // Set camera mode
        currentCamera = camera;

		// Set difficulty mode
		switch (difficulty)
		{
			case 0:
				currentDifficulty = "easy";
				break;
			case 1:
                currentDifficulty = "hard";
                break;
			default:
				break;
        }

        // Set animation mode
        switch (animations)
        {
            case 0:
                currentAnimation = "on";
                break;
            case 1:
                currentDifficulty = "off";
                break;
            default:
                break;
        }


        writeLog("New Round Started");
	}
}
