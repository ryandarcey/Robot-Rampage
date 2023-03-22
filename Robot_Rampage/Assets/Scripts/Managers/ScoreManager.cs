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
        // Update time since any points were added. Change color to default white after enough time passed
		if (timeSincePointsAdded >= 0) {
            timeSincePointsAdded += Time.deltaTime;
            if (timeSincePointsAdded > .5f)
            {
                scoreText.color = Color.white;
				timeSincePointsAdded = -1f;
            }
        }

        scoreText.text = "Score: " + score.ToString();
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

		timeSincePointsAdded = 0f;
	}

	public int getScore()
	{
		return score;
	}
}
