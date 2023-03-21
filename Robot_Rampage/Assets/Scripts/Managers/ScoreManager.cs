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
        scoreText.text = "Score: " + score.ToString();
    }

	// Add points to the total score. Includes negative values
	public void addPoints(int points)
	{
		score += points;
	}

	public int getScore()
	{
		return score;
	}
}
