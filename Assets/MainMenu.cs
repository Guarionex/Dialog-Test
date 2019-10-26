using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
	{
		//this will call the next scene, you can set this on File>BuildSettings, 
		//add this on position 0 and on position 1 the first scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame ()
 	{
 		Debug.Log ("QUIT!");
  		Application.Quit();
 	}
  
}
	

