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

	public int roundNumber = 1;
	DateTime startTime;

	public static LogManager instance;

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
			string columnHeaders = "seconds_since_start,round_number,action";  // TODO: figure out what columns we want
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

		string line = timeSinceStartString + "," + roundNumber.ToString() + "," + log;

		// write new line to file
		using (StreamWriter stream = File.AppendText(path))
		{
			stream.WriteLine(line);
		}
	}
}
