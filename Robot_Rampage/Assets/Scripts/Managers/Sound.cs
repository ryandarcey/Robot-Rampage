using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.5f, 2f)]
    public float pitch = 1f;

    public bool isLooping = false;

    [HideInInspector]
    public AudioSource audioSource;
}
