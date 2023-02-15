using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    void Start()
    {
        // Set cursor to be visible and unlocked
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

		//FindObjectOfType<AudioManager>().PlayMusic("menu music");
	}

    public void Play()
    {
        Debug.Log("Play Game");
		
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		
        SceneManager.LoadScene("RoomGenerationTest", LoadSceneMode.Single);
		
        //FindObjectOfType<AudioManager>().ChangeLevelMusic();
	}

    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
