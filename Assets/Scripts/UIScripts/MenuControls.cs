using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour {
	
	public void StartClicked() {
		SceneManager.LoadScene(1);
	}

	public void MainMenuClicked() {
		SceneManager.LoadScene(0); 
	}

	public void QuitClicked() {
		Application.Quit();
	}

	public void ResumeClicked() {
		GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CheckPause();
	}
}
