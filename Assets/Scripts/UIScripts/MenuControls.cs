using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControls : MonoBehaviour {

	public GameObject instructions;

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

	public void InstructionsClicked() {
		instructions.SetActive(true);
	}

	public void CloseInstructions() {
		instructions.SetActive(false);
	}
}
