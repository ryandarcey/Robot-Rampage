using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

// Audio Manager heavily inspired by Brackey's
//  https://www.youtube.com/watch?v=6OT43pvUyfY 

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;
    public Sound[] music;
    public int NUM_LEVEL_TRACKS = 5;

    //private bool isLoopingLevelTracks = false;
    
    private int musicTrackPlaying = -1;
    private float timeSinceSwitchingTracks = 0f;
    public float TIME_TO_PLAY_MUSIC = 10f;
    public const float TIME_TO_FADE = 4f;

    string levelName = "";
	public string ROOM_LEVEL_NAME = "RoomGenerationTest";
	public string MENU_LEVEL_NAME = "MainMenu";

	public static AudioManager instance;

    void Awake()
    {
        // singleton AudioManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // already have AudioManager in scene, don't need another one
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);  // AudioManager persists between scenes

        // initialize sound effects
        foreach (Sound sound in sounds)
        {
            InitSound(sound);
        }

        // initialize music
        foreach (Sound sound in music)
        {
            InitSound(sound);
        }

        levelName = SceneManager.GetActiveScene().name;

        if (levelName == MENU_LEVEL_NAME)
        {
			timeSinceSwitchingTracks = TIME_TO_PLAY_MUSIC + 1f;
			PlayMusic("menu music", true, 1f);
        }
        else if (levelName == ROOM_LEVEL_NAME)
        {
            PlayMusic("cyberpunk arcade", true, 1f);
        }
    }

	void Update()
	{        
        levelName = SceneManager.GetActiveScene().name;

        if (levelName == MENU_LEVEL_NAME && musicTrackPlaying != 5)
        {
			timeSinceSwitchingTracks = TIME_TO_PLAY_MUSIC + 1f;
			PlayMusic("menu music", false, 1f);
            
            for(int i = 0; i < NUM_LEVEL_TRACKS; i++)
            {
                music[i].audioSource.Stop();
            }
        }
        
        else if (levelName == ROOM_LEVEL_NAME)
        {
            if (timeSinceSwitchingTracks > TIME_TO_PLAY_MUSIC)
			{
				ChangeLevelMusic();
				timeSinceSwitchingTracks = 0f;
			}
			else
			{
				timeSinceSwitchingTracks += Time.deltaTime;
			}
		}
	}

	private void InitSound(Sound sound)
    {
		sound.audioSource = gameObject.AddComponent<AudioSource>();
		sound.audioSource.clip = sound.audioClip;

		sound.audioSource.volume = sound.volume;
		sound.audioSource.pitch = sound.pitch;

		sound.audioSource.loop = sound.isLooping;
	}

    public void PlaySound(string soundName)
    {
        soundName = soundName.ToLower();
        Sound sound = Array.Find(sounds, s => s.name == soundName);
        if (sound == null)
        {
            //Debug.LogWarning("Sound: " + soundName + " not found.");
            return;
        }

		sound.audioSource.Play();
    }

    public void StopSound(string soundName)
    {
        soundName = soundName.ToLower();
        Sound sound = Array.Find(sounds, s => s.name == soundName);
        if (sound == null)
        {
            //Debug.LogWarning("Sound: " + soundName + " not found.");
            return;
        }

        sound.audioSource.Stop();
    }

    public void PlayMusic(string musicName, bool isFirstCall=false, float timeToFade=TIME_TO_FADE)
    {
        musicName = musicName.ToLower();
        Sound sound = Array.Find(music, s => s.name == musicName);
        if (sound == null)
        {
            //Debug.LogWarning("Music: " + musicName + " not found.");
            return;
        }

        // fade in/fade to music
        StopAllCoroutines();
        StartCoroutine(FadeMusicToNewTrack(sound, isFirstCall, timeToFade));
        musicTrackPlaying = Array.FindIndex(music, s => s.name == musicName);
    }

    public void ChangeLevelMusic()
    {
        StopAllCoroutines();

        Sound nextTrack = music[(musicTrackPlaying + 1) % NUM_LEVEL_TRACKS];
        if (musicTrackPlaying >= 5)
        {
            // if was playing menu/end music
            StartCoroutine(FadeMusicToNewTrack(nextTrack, false, 1f));
        }
        else
        {
            StartCoroutine(FadeMusicToNewTrack(nextTrack));
        }

        musicTrackPlaying = (musicTrackPlaying + 1) % NUM_LEVEL_TRACKS;
    }

    private IEnumerator FadeMusicToNewTrack(Sound nextTrack, bool isFirstCall=false, float timeToFade=TIME_TO_FADE)
    {
        float timeElapsed = 0f;

        Sound previousTrack;

        if (isFirstCall)
        {
			// not playing anything currently
			previousTrack = new Sound
			{
				audioSource = gameObject.AddComponent<AudioSource>()
			};
		}
        else
        {
            // already playing something -- get what's already playing
			previousTrack = music[musicTrackPlaying];
		}
		
        nextTrack.audioSource.Play();

        while (timeElapsed < timeToFade)
        {
            previousTrack.audioSource.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            nextTrack.audioSource.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        previousTrack.audioSource.Stop();
        //Debug.Log("Now playing: " + nextTrack.name);
    }
}

