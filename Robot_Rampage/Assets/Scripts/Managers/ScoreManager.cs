using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Text;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

	// Score used throughout the game
	[SerializeField] private int score;
    
	// UI related values
    public TextMeshProUGUI scoreText;
	private float timeSincePointsAdded;
    private float timeSinceTimerDecrease;
    private bool noAction = false;

    public static ScoreManager instance;

	void Awake()
	{
        // singleton ScoreManager
        if (instance == null)
		{
			instance = this;
		}
		else
		{
            // already have ScoreManager in scene, don't need another one
            Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);  // ScoreManager persists between scenes
    }

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
        // Update time since any points were added
		if (timeSincePointsAdded >= 0) {
            timeSincePointsAdded += Time.deltaTime;
            // Change color to default white after enough time passed
            if (timeSincePointsAdded > .5f)
            {
                scoreText.color = Color.white;
				//timeSincePointsAdded = -1f;
            }

			// Start deducting points once enough time passes
			if (timeSincePointsAdded > 5f)
			{
				noAction = true;
			}
        }

		// Decrease time if no action has been occurring and enough time has passed
		if (noAction)
		{
			timeSinceTimerDecrease += Time.deltaTime;
			if (timeSinceTimerDecrease > 2f)
			{
				timePassed();
			}
        }


        scoreText.text = "Score: " + score.ToString();
    }

	void timePassed()
	{
		// Decrease points
		score -= 10;

		// Reset timer for decreasing points
        timeSinceTimerDecrease = 0f;
    }

	// Add points to the total score. Includes negative values
	public void addPoints(int points)
	{
		// Add points
		score += points;

		// If points were negative, change text color to red for a second. Otherwise, change to green
		if (points < 0)
		{
			scoreText.color = Color.red;
		}
		else
		{
			scoreText.color = Color.green;
		}

		// Reset time since the points were added
		timeSincePointsAdded = 0f;

		// Reset values related to deducting points when no action occurs
		timeSinceTimerDecrease = 0f;
		noAction = false;

	}

	public int getScore()
	{
		return score;
	}
}
