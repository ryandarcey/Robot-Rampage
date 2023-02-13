using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	// in-level music
	public AudioClip track1, track2, track3, track4, track5;
	
	private ArrayList musicSources;
	private int trackPlaying;

	public static MusicManager instance;
	// singleton
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
		musicSources = new ArrayList();
		// have 5 in-level music tracks
		for(int i = 0; i < 5; i++)
		{
			musicSources.Add(gameObject.AddComponent<AudioSource>());
		}

		trackPlaying = 1;	// start with first track (can change)
	}
}
