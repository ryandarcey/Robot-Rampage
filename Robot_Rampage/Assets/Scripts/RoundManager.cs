using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.SceneView;

public class RoundManager : MonoBehaviour
{
    public bool isRoundOver = false;

	// Starting time for player
	public const float roundTime = 180f;
	public float time = roundTime;
	double timeRounded;
	public TextMeshProUGUI timeText;

	public int roundNumber = 1;
	private int shotsFired = 0;
	private int shotsHit = 0;

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
		if (time <= 0)
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
        FindObjectOfType<LogManager>().writeLog("Shot hit");
        shotsHit++;
	}

	public void EndRound()
	{
        /*string now = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
		string filename = now + "_round-" + roundNumber.ToString();

		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RobotRampage_RoundLogs\" + filename + ".txt";

		if (!File.Exists(path))
		{
			//Debug.Log("file at  " + path + "  does not exist");
			path = @".\RobotRampage_RoundLogs\" + filename + ".txt";
		}

		// write stats and stuff to log file
		using (StreamWriter sw = File.CreateText(path))
		{
			sw.WriteLine("New file created: {0}", DateTime.Now.ToString());
			sw.WriteLine("Author: Robot Rampage");
			float t = (float)Math.Round((roundTime - time), 2);
			sw.WriteLine("Seconds taken:	" + t.ToString());
			sw.WriteLine("shots fired:	" + shotsFired.ToString());
			sw.WriteLine("shots hit:	" + shotsHit.ToString());
			sw.WriteLine("Done! ");
		}*/

        FindObjectOfType<LogManager>().writeLog("end round");

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}

